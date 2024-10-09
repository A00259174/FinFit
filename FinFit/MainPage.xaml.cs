using Microsoft.Maui.Controls;

namespace FinFit
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnBudgetTrackingClicked(object sender, EventArgs e)
        {
            // Navigate to the Budget Tracking page
            await Navigation.PushAsync(new BudgetTrackingPage());
        }

        private async void OnGoalSettingClicked(object sender, EventArgs e)
        {
            // Navigate to the Goal Setting page
            await Navigation.PushAsync(new GoalSettingPage());
        }

        private async void OnSavingChallengeClicked(object sender, EventArgs e)
        {
            // Navigate to the Saving Challenge page
            await Navigation.PushAsync(new SavingChallengePage());
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            // Implement logout functionality
            await Navigation.PopToRootAsync(); // or navigate back to the login page
        }
    }
}
