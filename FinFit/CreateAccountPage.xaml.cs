using FinFit.ViewModel;
using Microsoft.Maui.Controls;

namespace FinFit
{
    public partial class CreateAccountPage : ContentPage
    {
        public readonly SignUpViewModel signUpViewModel1;

        public CreateAccountPage()
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

        public CreateAccountPage(SignUpViewModel signUpViewModel)
        {
            InitializeComponent();
            signUpViewModel1 = signUpViewModel;
            BindingContext = signUpViewModel;
        }

        private async void OnSignUpClicked(object sender, EventArgs e)
        {
            string name = nameEntry.Text;
            string email = emailEntry.Text;
            string password = passwordEntry.Text;
            string confirmPassword = confirmPasswordEntry.Text;

            // Validate inputs
            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                password != confirmPassword)
            {
                await DisplayAlert("Error", "Please enter valid details.", "OK");
                return;
            }

            await signUpViewModel1.SignUp(name, email, password);
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            // Navigate to the login page
            await Shell.Current.GoToAsync($"///{nameof(LoginPage)}");
        }
    }
}
