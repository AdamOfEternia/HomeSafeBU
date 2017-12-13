using HomeSafe.Models;
using HomeSafe.Persistence;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeSafe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CompanyPage : ContentPage
    {
        private SQLiteAsyncConnection _connection;
        private ObservableCollection<Company> _companies;
        private bool _isDataLoaded = false;

        public CompanyPage()
        {
            InitializeComponent();

            _connection = DependencyService.Get<ISQLiteDb>().GetConnection();
        }

        async protected override void OnAppearing()
        {
            if (_isDataLoaded)
            {
                return;
            }

            await LoadCompanies();

            base.OnAppearing();
        }

        async private Task LoadCompanies()
        {
            await _connection.CreateTableAsync<Company>();

            var companies = await _connection.Table<Company>().ToListAsync();
            if (companies.Count == 0)
            {
                companies = new List<Company>
                {
                    new Company { Name = "Alto Construction Ltd", Location = "Sheffield" },
                    new Company { Name = "Total Rebuild", Location = "Rotherham" },
                    new Company { Name = "Terry Form and Sons", Location = "London"}
                };

                await _connection.InsertAllAsync(companies);
            }

            _companies = new ObservableCollection<Company>(companies);

            companyList.ItemsSource = _companies;

            _isDataLoaded = true;
        }

        async private void SignOutBtnClicked(object sender, EventArgs e)
        {
            App.IsUserLoggedIn = false;
            Navigation.InsertPageBefore(new LoginPage(), this);
            await Navigation.PopToRootAsync();
        }

        async private void SelectBtnClicked(object sender, SelectedItemChangedEventArgs e)
        {
            // because setting SelectItem to null triggers the event again we do this to stop the repeated event
            if (companyList.SelectedItem == null)
            {
                return;
            }

            var selectedCompany = e.SelectedItem as Company;

            // deselect the item in the list (which triggers another selection hence the null check above)
            companyList.SelectedItem = null;

            await Navigation.PushAsync(new ProgressPage(selectedCompany.Id));
        }
    }
}