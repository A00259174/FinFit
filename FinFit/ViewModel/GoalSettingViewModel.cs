using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using FinFit;

namespace FinFit.ViewModel
{
    public class GoalSettingViewModel : INotifyPropertyChanged
    {
        private readonly FirebaseService _firebaseService;
        private string _newGoal;
        private string _historyText;
        private ObservableCollection<GoalEntry> _goalsList;

        public GoalSettingViewModel(FirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
            GoalsList = new ObservableCollection<GoalEntry>();
        }

        public string NewGoal
        {
            get => _newGoal;
            set
            {
                _newGoal = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<GoalEntry> GoalsList
        {
            get => _goalsList;
            set
            {
                _goalsList = value;
                OnPropertyChanged();
            }
        }

        public string HistoryText
        {
            get => _historyText;
            set
            {
                _historyText = value;
                OnPropertyChanged();
            }
        }

        public async Task AddGoalAsync()
        {
            if (string.IsNullOrWhiteSpace(NewGoal))
            {
                // Display alert if goal is empty
                await App.Current.MainPage.DisplayAlert("Error", "Please enter a valid goal.", "OK");
                return;
            }

            // Prompt for additional goal details (savings, goal amount)
            string savingsInput = await App.Current.MainPage.DisplayPromptAsync("Savings Input", $"Enter your current savings for '{NewGoal}':", "OK", "Cancel", placeholder: "0");
            if (decimal.TryParse(savingsInput, out decimal currentSavings))
            {
                string goalAmountInput = await App.Current.MainPage.DisplayPromptAsync("Goal Amount", $"Enter the total amount needed for '{NewGoal}':", "OK", "Cancel", placeholder: "0");
                if (decimal.TryParse(goalAmountInput, out decimal goalAmount))
                {
                    // Calculate amount needed
                    decimal amountNeeded = goalAmount - currentSavings;

                    string dailySavingsInput = await App.Current.MainPage.DisplayPromptAsync("Daily Savings Input", "How much do you plan to save each day?", "OK", "Cancel", placeholder: "0");
                    if (decimal.TryParse(dailySavingsInput, out decimal dailySavings) && dailySavings > 0)
                    {
                        int totalDaysNeeded = (int)(amountNeeded / dailySavings);
                        string plan = $"Plan: Save ${dailySavings} daily for {totalDaysNeeded} day(s)";

                        // Create the GoalEntry
                        var newGoalEntry = new GoalEntry
                        {
                            GoalName = NewGoal,
                            CurrentSavings = currentSavings,
                            GoalAmount = goalAmount,
                            DateCreated = DateTime.Now,
                            Plan = plan,
                            Result = amountNeeded > 0 ? $"Amount Needed: ${amountNeeded}" : "Goal Achieved!"
                        };

                        // Save to Firebase
                        var uid = Preferences.Get("user_uid", null);
                        if (!string.IsNullOrEmpty(uid))
                        {
                            await _firebaseService.AddGoal(uid, newGoalEntry);
                        }

                        // Add the goal to the local list
                        GoalsList.Add(newGoalEntry);
                    }
                }
            }
        }

        public async Task LoadGoals()
        {
            // Load goals from Firebase
            var uid = Preferences.Get("user_uid", null);
            if (!string.IsNullOrEmpty(uid))
            {
                var goalsFromFirebase = await _firebaseService.GetGoals(uid);
                GoalsList = new ObservableCollection<GoalEntry>(goalsFromFirebase);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
