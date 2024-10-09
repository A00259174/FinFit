using FinFit.ViewModel;
using Microsoft.Maui.Controls;
using System;

namespace FinFit
{
    public partial class BudgetTrackingPage : ContentPage
    {
        private BudgetTrackingViewModel _viewModel;

        public BudgetTrackingPage()
        {
            InitializeComponent();

            // Resolve BudgetTrackingViewModel using the App's DI container
            _viewModel = App.Current.Services.GetService<BudgetTrackingViewModel>();

            // Set the BindingContext for the XAML page
            BindingContext = _viewModel;

            // Call the method to load budget history when the screen loads
            LoadBudgetHistoryOnStart();
        }

        private async void LoadBudgetHistoryOnStart()
        {
            // Load the budget history from Firebase when the screen loads
            await _viewModel.LoadBudgetHistory();
        }

        private void OnAddExpenseClicked(object sender, EventArgs e)
        {
            if (decimal.TryParse(ExpenseAmountEntry.Text, out decimal expenseAmount) &&
                ExpenseTypePicker.SelectedItem != null)
            {
                string expenseType = ExpenseTypePicker.SelectedItem.ToString();

                // Add the expense via ViewModel
                _viewModel.AddExpenseLocally(expenseType, expenseAmount);

                // Clear the Expense Amount Entry
                ExpenseAmountEntry.Text = string.Empty;
            }
            else
            {
                DisplayAlert("Invalid Input", "Please enter a valid expense amount and select an expense type.", "OK");
            }
        }

        private async void OnCalculateClicked(object sender, EventArgs e)
        {
            if (int.TryParse(IncomeEntry.Text, out int income))
            {
                _viewModel.Income = income;

                // Calculate savings and store everything in Firebase
                await _viewModel.CalculateAndStoreBudgetData();
            }
            else
            {
                DisplayAlert("Invalid Input", "Please enter a valid income amount.", "OK");
            }
        }
    }
}
