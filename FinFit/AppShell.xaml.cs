using Microsoft.Maui.Storage;

namespace FinFit
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        // Use OnAppearing to safely navigate after Shell initialization
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Check if the user is already logged in
            string userUID = Preferences.Get("user_uid", null);

            if (!string.IsNullOrEmpty(userUID))
            {
                // User is logged in, navigate to HomePage directly
                Shell.Current.GoToAsync("//HomePage");
            }
            else
            {
                // User is not logged in, navigate to LoginPage
                Shell.Current.GoToAsync("//LoginPage");
            }
        }
    }
}
