using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FinFit.ViewModel
{
    public class SavingChallengeViewModel : INotifyPropertyChanged
    {
        private readonly FirebaseService _firebaseService;
        private decimal _totalNickelSavings;
        private decimal _totalHolidaySavings;
        private bool _isBusy;
        private List<SavingEntry> _savingsHistory;

        public SavingChallengeViewModel(FirebaseService firebaseService)
        {
            Debug.WriteLine("SavingChallengeViewModel: Constructor called");
            _firebaseService = firebaseService;
            SavingsHistory = new List<SavingEntry>();
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public decimal TotalNickelSavings
        {
            get => _totalNickelSavings;
            set
            {
                _totalNickelSavings = value;
                Debug.WriteLine($"TotalNickelSavings updated to {value}");
                OnPropertyChanged();
            }
        }

        public decimal TotalHolidaySavings
        {
            get => _totalHolidaySavings;
            set
            {
                _totalHolidaySavings = value;
                Debug.WriteLine($"TotalHolidaySavings updated to {value}");
                OnPropertyChanged();
            }
        }

        public List<SavingEntry> SavingsHistory
        {
            get => _savingsHistory;
            set
            {
                _savingsHistory = value;
                Debug.WriteLine($"SavingsHistory updated with {value.Count} entries");
                OnPropertyChanged();
            }
        }

        public async Task onSave(decimal amount)
        {
            Debug.WriteLine("Test");
        }

        public async Task AddNickelChallenge(decimal amount)
        {
            Debug.WriteLine($"Method AddNickelChallenge: Called with amount {amount}");
            try
            {
                var entry = new SavingEntry
                {
                    Amount = amount,
                    ChallengeType = "Daily Nickel Challenge",
                    Date = System.DateTime.Now.ToString("g")
                };

                Debug.WriteLine("AddNickelChallenge: Entry created");

                TotalNickelSavings += amount;
                Debug.WriteLine("AddNickelChallenge: TotalNickelSavings updated locally");

                await _firebaseService.AddSavingEntry(entry);
                Debug.WriteLine("AddNickelChallenge: Entry saved to Firebase");

                await LoadSavingHistory();
                Debug.WriteLine("AddNickelChallenge: Savings history reloaded");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"AddNickelChallenge: Error - {ex.Message}");
            }
        }

        public async Task AddHolidayChallenge(decimal amount)
        {
            try
            {
                Debug.WriteLine($"AddHolidayChallenge: Adding {amount} to holiday savings");

                var entry = new SavingEntry
                {
                    Amount = amount,
                    ChallengeType = "Holiday Challenge",
                    Date = System.DateTime.Now.ToString("g")
                };

                if (entry == null)
                {
                    Debug.WriteLine("AddHolidayChallenge: Entry object is null");
                    return;
                }

                // Check if entry properties are properly set
                Debug.WriteLine("AddHolidayChallenge: Looping through entry properties...");
                foreach (var prop in entry.GetType().GetProperties())
                {
                    Debug.WriteLine($"{prop.Name}: {prop.GetValue(entry, null)}");
                }

                // Update local savings
                TotalHolidaySavings += amount;
                Debug.WriteLine("AddHolidayChallenge: TotalHolidaySavings updated locally");

                // Save entry to Firebase
                await _firebaseService.AddSavingEntry(entry);
                Debug.WriteLine("AddHolidayChallenge: Entry saved to Firebase");

                // Reload savings history
                await LoadSavingHistory();
                Debug.WriteLine("AddHolidayChallenge: Savings history reloaded");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"AddHolidayChallenge: An error occurred - {ex.Message}");
            }
        }


        public async Task LoadSavingHistory()
        {
            Debug.WriteLine("LoadSavingHistory: Loading history from Firebase");

            var history = await _firebaseService.GetSavingHistory();

            if (history != null && history.Count > 0)
            {
                SavingsHistory = history;
                Debug.WriteLine($"LoadSavingHistory: Loaded {history.Count} entries from Firebase");

                // Debug each entry in history
                foreach (var entry in history)
                {
                    Debug.WriteLine($"Loaded entry - Amount: {entry.Amount}, Type: {entry.ChallengeType}, Date: {entry.Date}");
                }
            }
            else
            {
                Debug.WriteLine("LoadSavingHistory: No history found in Firebase");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
