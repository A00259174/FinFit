using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace FinFit.ViewModel
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private string _name;
        private readonly FirebaseService _firebaseService;

        // Default constructor for XAML binding, but it won't load data
        public HomeViewModel()
        {
        }

        // Constructor for Dependency Injection to inject FirebaseService
        public HomeViewModel(FirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
            if (_firebaseService != null)
            {
                LoadUserData();
            }
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

        // Load user data from Firebase Realtime Database
        private async Task LoadUserData()
        {
            // Get the UID stored in preferences
            var uid = Preferences.Get("user_uid", null);
            if (!string.IsNullOrEmpty(uid))
            {
                // Fetch user data by UID
                var user = await _firebaseService.GetUserByUID(uid);

                if (user != null)
                {
                    Name = user.Name; // Update the name property
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
