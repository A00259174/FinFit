using FinFit.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;

namespace FinFit
{
    public partial class HomePage : ContentPage
    {
        private HomeViewModel _viewModel;

        public HomePage()
        {
            InitializeComponent();

            // Resolve HomeViewModel using the App's DI container
            _viewModel = App.Current.Services.GetService<HomeViewModel>();

            // Set the BindingContext
            BindingContext = _viewModel;
        }

        private async void OnBudgetTrackingClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BudgetTrackingPage());
        }

        private async void OnGoalSettingClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GoalSettingPage());
        }

        private async void OnSavingChallengeClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SavingChallengePage());
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            Preferences.Remove("user_uid");
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
