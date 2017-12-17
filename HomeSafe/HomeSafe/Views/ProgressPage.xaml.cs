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
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace HomeSafe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProgressPage : ContentPage
    {
        private SQLiteAsyncConnection _connection;
        private ObservableCollection<Progress> _progressUpdates;
        private bool _isDataLoaded = false;
        private int _companyId;
        private bool _recordLocation = true;

        public ProgressPage(int companyId)
        {
            InitializeComponent();

            if (!CrossGeolocator.IsSupported)
            {
                DisplayAlert("Warning!", "Geolocator is not supported on the current platform.  Location will not be recorded in your progress updates.", "Ok");
                _recordLocation = false;
            }

            _connection = DependencyService.Get<ISQLiteDb>().GetConnection();
            _companyId = companyId;
        }

        async protected override void OnAppearing()
        {
            if (_isDataLoaded)
            {
                return;
            }

            await LoadProgressUpdates();

            base.OnAppearing();
        }

        async private Task LoadProgressUpdates()
        {
            await _connection.CreateTableAsync<Progress>();

            var progressUpdates = await _connection.Table<Progress>().Where(status => status.CompanyId == _companyId).ToListAsync();

            _progressUpdates = new ObservableCollection<Progress>(progressUpdates);

            progressList.ItemsSource = _progressUpdates;

            _isDataLoaded = true;
        }

        async private void SignOutBtnClicked(object sender, EventArgs e)
        {
            App.IsUserLoggedIn = false;
            Navigation.InsertPageBefore(new LoginPage(), this);
            await Navigation.PopToRootAsync();
        }

        private void LeftHomeBtnClicked(object sender, EventArgs e)
        {
            UpdateProgress(Progress.PROGRESS_LEFT_HOME);
        }

        private void ArrivedSiteBtnClicked(object sender, EventArgs e)
        {
            UpdateProgress(Progress.PROGRESS_ARRIVED_AT_SITE);
        }

        private void LeftSiteBtnClicked(object sender, EventArgs e)
        {
            UpdateProgress(Progress.PROGRESS_LEFT_SITE);
        }

        private void ArrivedHomeBtnClicked(object sender, EventArgs e)
        {
            UpdateProgress(Progress.PROGRESS_ARRIVED_HOME);
        }

        private void SelectBtnClicked(object sender, SelectedItemChangedEventArgs e)
        {
            progressList.SelectedItem = null;
        }

        private void ControlCentreBtnClicked(object sender, EventArgs e)
        {
            UpdateProgress(Progress.PROGRESS_CONTACTED_CONTROL_CENTRE);
        }

        async private void UpdateProgress(string status)
        {
            DateTime now = DateTime.Now;
            string t = now.ToString(@"HH\:mm");
            string d = now.ToString("dd-MM-yyyy");
            double lng = 0d;
            double lat = 0d;
            Position position = null;

            if (_recordLocation)
            {
                try
                {
                    var locator = CrossGeolocator.Current;
                    locator.DesiredAccuracy = 50;

                    if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled)
                    {
                        await DisplayAlert("Warning!", "Location services are not enabled on your device or information is unavailable.  Please enable and try again later.", "Ok");
                        return;
                    }
                    else
                    {
                        position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);

                        lng = position.Longitude;
                        lat = position.Latitude;
                    }
                }
                catch (Exception e)
                {
                    await DisplayAlert("Warning!", "Location information is unavailable.  Please retry again later.", "Ok");
                    return;
                }
            }

            Progress progress = new Progress
            {
                Id = 0,
                Date = d,
                Time = t,
                Status = status,
                CompanyId = _companyId,
                Latitude = lat,
                Longitude = lng
            };

            await _connection.InsertAsync(progress);

            _progressUpdates.Add(progress);
        }

    }
}