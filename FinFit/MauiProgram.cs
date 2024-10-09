using Firebase.Auth.Providers;
using Firebase.Auth;
using Firebase.Database;
using Microsoft.Extensions.Logging;
using FinFit.ViewModel;

namespace FinFit
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Register FirebaseAuthClient
            builder.Services.AddSingleton(services => new FirebaseAuthClient(new FirebaseAuthConfig()
            {
                ApiKey = "AIzaSyCVIpYT_Htt4ELBh0r79pBPTff-n3eFs3E",
                AuthDomain = "finfitfb.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                },
            }));

            // Register FirebaseService
            builder.Services.AddTransient<FirebaseService>();

            // Register pages and view models
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<CreateAccountPage>();

            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<SignUpViewModel>();

            builder.Services.AddTransient<FirebaseService>();
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<BudgetTrackingViewModel>();
            builder.Services.AddTransient<SavingChallengeViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Build and return the MauiApp
            var mauiApp = builder.Build();

            // Pass the service provider to the App
            return mauiApp;
        }
    }
}