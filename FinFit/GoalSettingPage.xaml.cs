using FinFit.ViewModel;
using Firebase.Auth;
using Microsoft.Maui.Controls;
using System;

namespace FinFit
{
    public partial class GoalSettingPage : ContentPage
    {
        private GoalSettingViewModel _viewModel;

        public GoalSettingPage()
        {
            InitializeComponent();

            // Initialize ViewModel
            _viewModel = new GoalSettingViewModel(new FirebaseService(App.Current.Services.GetService<FirebaseAuthClient>()));
            BindingContext = _viewModel;

            // Load the goals from Firebase when the screen loads
            LoadGoalsOnStart();
        }

        private async void LoadGoalsOnStart()
        {
            await _viewModel.LoadGoals();
        }

        private async void OnAddGoalClicked(object sender, EventArgs e)
        {
            await _viewModel.AddGoalAsync();
            GoalEntry.Text = string.Empty;  // Clear the entry after adding
        }
    }
}
