using HomeSafe.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace HomeSafe
{
    public partial class App : Application
    {
        public static bool IsUserLoggedIn { get; set; }

        public App()
        {
            Page p;

            if (!IsUserLoggedIn)
            {
                p = new LoginPage();
            }
            else
            {
                p = new CompanyPage();
            }

            MainPage = new NavigationPage(p)
            {
                BarBackgroundColor = Color.FromHex("#222222"),
                BarTextColor = Color.FromHex("#EEEEEE")
            };
        }

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
