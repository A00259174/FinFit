using FinFit.ViewModel;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FinFit
{
    public class FirebaseService
    {
        private readonly FirebaseAuthClient _authClient;
        private readonly FirebaseClient _firebaseDatabaseClient;

        public FirebaseService(FirebaseAuthClient authClient)
        {
            _authClient = authClient;
            // Initialize Firebase Realtime Database
            _firebaseDatabaseClient = new FirebaseClient("https://finfitfb-default-rtdb.firebaseio.com/");  // Replace with your Firebase Realtime Database URL
        }

        // Login method to authenticate user
        public async Task<string> LoginWithEmailPassword(string email, string password)
        {
            try
            {
                var userCredential = await _authClient.SignInWithEmailAndPasswordAsync(email, password);
                return userCredential.User.Uid;  // Return the User UID after login
            }
            catch (Exception ex)
            {
                // Handle error
                return null;
            }
        }

        // Register method to create a new user
        public async Task<string> RegisterWithEmailPassword(string email, string password)
        {
            try
            {
                var userCredential = await _authClient.CreateUserWithEmailAndPasswordAsync(email, password);
                return userCredential.User.Uid;  // Return the User UID after successful registration
            }
            catch (Exception ex)
            {
                // Handle error
                return null;
            }
        }

        // Save user data to Firebase Realtime Database using UID
        public async Task SaveUserToRealtimeDatabase(string uid, string name, string email)
        {
            var user = new
            {
                Name = name,
                Email = email
            };

            // Use UID as the key to store the user data
            await _firebaseDatabaseClient
                .Child("Users")
                .Child(uid)  // Use UID instead of email as the node key
                .PutAsync(user);
        }

        public async Task<User> GetUserByUID(string uid)
        {
            // Fetch the user data from Firebase Realtime Database using the UID
            var user = await _firebaseDatabaseClient
                .Child("Users")
                .Child(uid)
                .OnceSingleAsync<User>();

            return user;
        }

        public async Task<List<BudgetEntry>> GetBudgetHistory(string uid)
        {
            var budgetEntries = await _firebaseDatabaseClient
                .Child("Users")
                .Child(uid)
                .Child("BudgetEntries")
                .OnceAsync<BudgetEntry>();

            // Convert Firebase data to a list of BudgetEntry objects
            return budgetEntries.Select(entry => entry.Object).ToList();
        }

        public async Task AddBudgetEntry(string uid, BudgetEntry entry)
        {
            await _firebaseDatabaseClient
                .Child("Users")
                .Child(uid)
                .Child("BudgetEntries")
                .PostAsync(entry);
        }

        // Add a new goal
        public async Task AddGoal(string uid, GoalEntry goalEntry)
        {
            await _firebaseDatabaseClient
                .Child("Users")
                .Child(uid)
                .Child("Goals")
                .PostAsync(goalEntry);
        }

        // Retrieve all goals for a user
        public async Task<List<GoalEntry>> GetGoals(string uid)
        {
            var goals = await _firebaseDatabaseClient
                .Child("Users")
                .Child(uid)
                .Child("Goals")
                .OnceAsync<GoalEntry>();

            return goals.Select(goal => goal.Object).ToList();
        }

        public async Task AddSavingEntry(SavingEntry entry)
        {
            Debug.WriteLine("AddSavingEntry: Saving entry to Firebase");

            var uid = Preferences.Get("user_uid", null);  // Get the current user's UID
            if (!string.IsNullOrEmpty(uid))
            {
                try
                {
                    Debug.WriteLine($"AddSavingEntry: UID is {uid}, Saving {entry.Amount} for {entry.ChallengeType}");
                    await _firebaseDatabaseClient
                        .Child("Users")
                        .Child(uid)
                        .Child("Savings")
                        .PostAsync(entry);
                    Debug.WriteLine("AddSavingEntry: Successfully saved entry to Firebase");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"AddSavingEntry: Error while saving entry: {ex.Message}");
                }
            }
            else
            {
                Debug.WriteLine("AddSavingEntry: UID is null or empty, cannot save entry");
            }
        }


        public async Task<List<SavingEntry>> GetSavingHistory()
        {
            Debug.WriteLine("GetSavingHistory: Preparing to fetch saving history");

            var uid = Preferences.Get("user_uid", null);
            if (!string.IsNullOrEmpty(uid))
            {
                try
                {
                    Debug.WriteLine($"GetSavingHistory: UID is {uid}, fetching savings history");

                    var entries = await _firebaseDatabaseClient
                        .Child("Users")
                        .Child(uid)
                        .Child("Savings")
                        .OnceAsync<SavingEntry>();

                    Debug.WriteLine($"GetSavingHistory: Fetched {entries.Count} entries from Firebase");

                    return entries.Select(e => e.Object).ToList();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"GetSavingHistory: Error while retrieving history: {ex.Message}");
                    return new List<SavingEntry>();
                }
            }
            else
            {
                Debug.WriteLine("GetSavingHistory: UID is null or empty, cannot retrieve history");
                return new List<SavingEntry>();
            }
        }
    }

    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class GoalEntry
    {
        public string GoalName { get; set; }
        public decimal CurrentSavings { get; set; }
        public decimal GoalAmount { get; set; }
        public DateTime DateCreated { get; set; }
        public string Plan { get; set; }  // Plan details (e.g., daily savings plan)
        public string Result { get; set; }  // Achieved or not, with breakdown
    }

    public class SavingEntry
    {
        public decimal Amount { get; set; }
        public string ChallengeType { get; set; }
        public string Date { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
