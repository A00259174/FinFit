using System;
using Microsoft.Maui.Controls;

namespace FinFit
{
    public partial class YourChallengesPage : ContentPage
    {
        public YourChallengesPage(string challenge)
        {
            InitializeComponent();
            ChallengesLabel.Text = $"{challenge} started!";
            // Additional logic to manage challenges can go here.
        }
    }
}
