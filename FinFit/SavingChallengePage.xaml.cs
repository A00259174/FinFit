using FinFit.ViewModel;
using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace FinFit
{
    public partial class SavingChallengePage : ContentPage
    {
        private SavingChallengeViewModel _viewModel;

        public SavingChallengePage()
        {
            InitializeComponent();
            Debug.WriteLine("SavingChallengePage constructor called");

            // Ensure that the ViewModel is correctly fetched from the service provider
            _viewModel = App.Current.Services.GetService<SavingChallengeViewModel>();

            if (_viewModel == null)
            {
                Debug.WriteLine("ViewModel is null! Failed to retrieve ViewModel from service.");
            }
            else
            {
                Debug.WriteLine("ViewModel successfully retrieved.");
            }

            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Debug.WriteLine("OnAppearing: Loading saving history");
            await _viewModel.LoadSavingHistory();
            Debug.WriteLine("OnAppearing: Completed loading saving history");
        }

        private void OnNickelChallengeClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("OnNickelChallengeClicked: Clicked");
            NickelInputStack.IsVisible = true; // Show the input box for Nickel challenge
        }

        private async void OnSaveNickelChallengeClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("OnSaveNickelChallengeClicked: Saving Nickel Challenge");

            if (decimal.TryParse(NickelAmountEntry.Text, out decimal savedAmount))
            {
                try
                {
                    _viewModel.IsBusy = true;  // Show the loader

                    Debug.WriteLine($"OnSaveNickelChallengeClicked: Adding {savedAmount} to nickel savings");

                    // Call the actual method here
                    await _viewModel.AddNickelChallenge(savedAmount);
                    Debug.WriteLine("OnSaveNickelChallengeClicked: Nickel amount saved");

                    NickelInputStack.IsVisible = false; // Hide the input box after saving
                    NickelAmountEntry.Text = string.Empty; // Clear the input box

                    await _viewModel.LoadSavingHistory();
                    Debug.WriteLine("OnSaveNickelChallengeClicked: Loaded updated savings history");

                    await DisplayAlert("Success", "Nickel amount saved!", "OK");
                }
                finally
                {
                    _viewModel.IsBusy = false;  // Hide the loader
                }
            }
            else
            {
                Debug.WriteLine("OnSaveNickelChallengeClicked: Invalid input");
                await DisplayAlert("Invalid Input", "Please enter a valid amount.", "OK");
            }
            Debug.WriteLine("Done...");
        }

        private void OnHolidayChallengeClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("OnHolidayChallengeClicked: Clicked");
            HolidayInputStack.IsVisible = true; // Show the input box for Holiday challenge
        }

        private async void OnSaveHolidayChallengeClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("OnSaveHolidayChallengeClicked: Saving Holiday Challenge");

            if (decimal.TryParse(HolidayAmountEntry.Text, out decimal savedAmount))
            {
                Debug.WriteLine($"OnSaveHolidayChallengeClicked: Adding {savedAmount} to holiday savings");
                await _viewModel.AddHolidayChallenge(savedAmount);
                Debug.WriteLine("OnSaveHolidayChallengeClicked: Holiday amount saved");

                HolidayInputStack.IsVisible = false; // Hide the input box after saving
                HolidayAmountEntry.Text = string.Empty; // Clear the input box

                await _viewModel.LoadSavingHistory();
                Debug.WriteLine("OnSaveHolidayChallengeClicked: Loaded updated savings history");
                await DisplayAlert("Success", "Holiday amount saved!", "OK");
            }
            else
            {
                Debug.WriteLine("OnSaveHolidayChallengeClicked: Invalid input");
                await DisplayAlert("Invalid Input", "Please enter a valid amount.", "OK");
            }
        }
    }
}
