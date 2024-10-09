using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;


namespace FinFit.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {

        private string _email;
        private string _password;
        private string _loginResult;
        private readonly FirebaseService _firebaseService;
        public LoginViewModel(FirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
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

        public string LoginResult
        {
            get => _loginResult;
            set
            {
                _loginResult = value;
                OnPropertyChanged();
            }
        }

        public async Task<bool> Login(string email, string password)
        {
            try
            {
                var uid = await _firebaseService.LoginWithEmailPassword(email, password);

                if (!string.IsNullOrEmpty(uid))
                {
                    // Store the UID in Preferences for session management
                    Preferences.Set("user_uid", uid);

                    // Return true if login is successful
                    return true;
                }
                else
                {
                    // Invalid login credentials
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Handle login failure (for example, network errors or wrong password)
                return false;
            }
        }



    public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
