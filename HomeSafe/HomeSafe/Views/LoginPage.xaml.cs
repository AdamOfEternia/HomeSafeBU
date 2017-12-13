using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HomeSafe.Models;

namespace HomeSafe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        async private void LoginBtnClicked(object sender, EventArgs e)
        {
            User user = new User
            {
                Username = userName.Text,
                Password = userPwd.Text
            };

            var isValid = IsUserValid(user);
            if (!isValid)
            {
                messageLabel.Text = "User verification failed - please retry.";
                userPwd.Text = string.Empty;
            }
            else
            {
                App.IsUserLoggedIn = true;
                Navigation.InsertPageBefore(new CompanyPage(), this);
                await Navigation.PopAsync();
            }
        }

        private bool IsUserValid(User user)
        {
            return (user.Username == "DemoUser" && user.Password == "demo123");
        }
    }
}