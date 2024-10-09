using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace FinFit.ViewModel
{
    public class SignUpViewModel : INotifyPropertyChanged
    {
        private string _name;
        private string _email;
        private string _password;
        private string _confirmpassword;

        private readonly FirebaseService _firebaseService;

        public SignUpViewModel(FirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string ConfirmPassword
        {
            get => _confirmpassword;
            set
            {
                _confirmpassword = value;
                OnPropertyChanged();
            }
        }

        public async Task SignUp(string name, string email, string password)
        {
            try
            {
                // Register the user and get the UID
                var uid = await _firebaseService.RegisterWithEmailPassword(email, password);

                if (!string.IsNullOrEmpty(uid))
                {
                    // Save user information to Realtime Database using UID
                    await _firebaseService.SaveUserToRealtimeDatabase(uid, name, email);

                    // Store the UID in Preferences for session management
                    Preferences.Set("user_uid", uid);

                    // Show success alert and navigate to home page directly
                    await Shell.Current.DisplayAlert("Success", "Account created successfully!", "OK");
                    await Shell.Current.GoToAsync("//HomePage");
                }
            }
            catch (Exception ex)
            {
                // Handle sign-up failure
                await Shell.Current.DisplayAlert("Error", $"Sign up failed: {ex.Message}", "OK");
            }
        }


    public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
