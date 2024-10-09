using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;

namespace FinFit
{
    public partial class App : Application
    {
        public static new App Current => (App)Application.Current;

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        public IServiceProvider Services { get; }

        public App(IServiceProvider services)
        {
            InitializeComponent();
            Services = services;

            MainPage = new AppShell();
        }
    }
}
