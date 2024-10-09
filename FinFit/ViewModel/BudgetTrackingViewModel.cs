using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace FinFit.ViewModel
{
    public class BudgetTrackingViewModel : INotifyPropertyChanged
    {
        private decimal _totalExpenses;
        private decimal _income;
        private string _result;
        private string _motivationalQuote;
        private bool _isLoading;
        private readonly FirebaseService _firebaseService;
        private List<Expense> _expenses;
        private List<BudgetEntry> _budgetHistory;

        public BudgetTrackingViewModel(FirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
            _expenses = new List<Expense>();
            _budgetHistory = new List<BudgetEntry>();
        }

        public decimal TotalExpenses
        {
            get => _totalExpenses;
            set
            {
                _totalExpenses = value;
                OnPropertyChanged();
            }
        }

        public decimal Income
        {
            get => _income;
            set
            {
                _income = value;
                OnPropertyChanged();
            }
        }

        public string Result
        {
            get => _result;
            set
            {
                _result = value;
                OnPropertyChanged();
            }
        }

        public string MotivationalQuote
        {
            get => _motivationalQuote;
            set
            {
                _motivationalQuote = value;
                OnPropertyChanged();
            }
        }

        public List<Expense> Expenses
        {
            get => _expenses;
            set
            {
                _expenses = value;
                OnPropertyChanged();
            }
        }

        public List<BudgetEntry> BudgetHistory
        {
            get => _budgetHistory;
            set
            {
                _budgetHistory = value;
                OnPropertyChanged();
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        // Add expense locally for displaying in the list
        public void AddExpenseLocally(string type, decimal amount)
        {
            var expense = new Expense { Type = type, Amount = amount };

            // Add the expense to the Expenses list
            _expenses.Add(expense);

            // Manually re-assign the list to trigger UI updates
            Expenses = new List<Expense>(_expenses);

            TotalExpenses += amount;

            // Notify UI to update the list
            OnPropertyChanged(nameof(Expenses)); // Trigger the update in the UI
            OnPropertyChanged(nameof(TotalExpenses));
        }

        // Calculate and store the budget data in Firebase
        public async Task CalculateAndStoreBudgetData()
        {
            decimal remaining = Income - TotalExpenses;

            if (remaining >= 0)
            {
                Result = $"Amount Saved: {remaining}";
                MotivationalQuote = GetRandomMotivationalQuote();
            }
            else
            {
                Result = $"You have exceeded your budget by: {Math.Abs(remaining)}";
                MotivationalQuote = "Consider these tips to start saving money:\n1. Track your expenses.\n2. Set a budget.\n3. Avoid impulse buying.";
            }

            // Store the budget data and expenses in Firebase
            var uid = Preferences.Get("user_uid", null);
            if (!string.IsNullOrEmpty(uid))
            {
                var entry = new BudgetEntry
                {
                    Date = DateTime.Now.ToString("g"),
                    Income = Income,
                    TotalExpenses = TotalExpenses,
                    Expenses = new List<Expense>(_expenses)
                };

                await _firebaseService.AddBudgetEntry(uid, entry);

                // Reload the budget history after storing new data
                await LoadBudgetHistory();
            }

            // Notify the UI to update
            OnPropertyChanged(nameof(Result));
        }

        // Load the budget history from Firebase
        public async Task LoadBudgetHistory()
        {
            IsLoading = true;

            var uid = Preferences.Get("user_uid", null);
            if (!string.IsNullOrEmpty(uid))
            {
                try
                {
                    var history = await _firebaseService.GetBudgetHistory(uid);

                    if (history != null && history.Count > 0)
                    {
                        // BudgetEntry.Result will be automatically computed, no need to assign it manually
                        BudgetHistory = new List<BudgetEntry>(history);
                        BudgetHistory.Sort((x, y) => DateTime.Parse(y.Date).CompareTo(DateTime.Parse(x.Date)));  // Sort by latest date
                    }
                    else
                    {
                        Result = "No data found.";
                    }
                }
                catch (Exception ex)
                {
                    Result = $"Failed to load data: {ex.Message}";
                }
            }

            IsLoading = false;
            OnPropertyChanged(nameof(Result));
            OnPropertyChanged(nameof(BudgetHistory));
        }


        private string GetRandomMotivationalQuote()
        {
            List<string> quotes = new List<string>
            {
                "Don't watch the clock; do what it does. Keep going.",
                "The secret of getting ahead is getting started.",
                "It does not matter how slowly you go as long as you do not stop.",
                "Success is not the key to happiness. Happiness is the key to success.",
                "The best time to plant a tree was 20 years ago. The second best time is now."
            };

            Random random = new Random();
            int index = random.Next(quotes.Count);
            return quotes[index];
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    public class Expense
    {
        public string Type { get; set; }
        public decimal Amount { get; set; }
    }

    public class BudgetEntry
    {
        public string Date { get; set; }
        public decimal Income { get; set; }
        public decimal TotalExpenses { get; set; }
        public List<Expense> Expenses { get; set; }

        // Computed Result message based on income and expenses
        public string Result
        {
            get
            {
                return TotalExpenses > Income
                    ? $"You exceeded your budget by: {TotalExpenses - Income}"
                    : $"Amount saved: {Income - TotalExpenses}";
            }
        }
    }
}
