using FinFit.ViewModel;
using Microsoft.Maui.Controls;

namespace FinFit
{
    public partial class LoginPage : ContentPage
    {
        private LoginViewModel _viewModel;

        public LoginPage()
        {
            InitializeComponent();

            // Check if the user is already logged in
            string userUID = Preferences.Get("user_uid", null);

            if (!string.IsNullOrEmpty(userUID))
            {
                // User is already logged in, redirect to HomePage
                Shell.Current.GoToAsync("//HomePage");
            }
        }

        public LoginPage(LoginViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        public async void OnLoginClicked(object sender, EventArgs e)
        {
            var email = usernameEntry.Text;
            var password = passwordEntry.Text;

            bool isSuccess = await _viewModel.Login(email, password);

            if (isSuccess)
            {
                // Display success message and navigate to HomePage
                await DisplayAlert("Success", "Login successful!", "OK");
                await Shell.Current.GoToAsync("//HomePage");
            }
            else
            {
                // Display failure message based on the reason
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    await DisplayAlert("Error", "Please enter both email and password.", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "Login failed. Incorrect email or password.", "OK");
                }
            }
        }

        private async void OnSignUpClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Sign Up", "Do you want to create a new account?", "Yes", "No");
            if (answer)
            {
                // Navigate to sign up page if user confirms
                await Shell.Current.GoToAsync("//CreateAccountPage");
            }
        }

    }
}
