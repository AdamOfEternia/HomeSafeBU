using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HomeSafe.Models
{
    public class Progress : INotifyPropertyChanged
    {
        public static string PROGRESS_LEFT_HOME = "Left Home";
        public static string PROGRESS_ARRIVED_AT_SITE = "Arrived at Site";
        public static string PROGRESS_LEFT_SITE = "Left Site";
        public static string PROGRESS_ARRIVED_HOME = "Left Arrived Home";
        public static string PROGRESS_CONTACTED_CONTROL_CENTRE = "** CONTACTED CONTROL CENTRE **";

        public event PropertyChangedEventHandler PropertyChanged;

        private string _date;
        private string _time;
        private string _status;
        private double _latitude;
        private double _longitude;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public string Date
        {
            get { return _date; }
            set
            {
                if (_date == value)
                {
                    return;
                }
                _date = value;
                OnPropertyChanged();
            }
        }

        public string Time
        {
            get { return _time; }
            set
            {
                if (_time == value)
                {
                    return;
                }
                _time = value;
                OnPropertyChanged();
            }
        }

        public string Status
        {
            get { return _status; }
            set
            {
                if (_status == value)
                {
                    return;
                }
                _status = value;
                OnPropertyChanged();
            }
        }

        public double Latitude
        {
            get { return _latitude; }
            set
            {
                if (_latitude == value)
                {
                    return;
                }
                _latitude = value;
                OnPropertyChanged();
            }
        }

        public double Longitude
        {
            get { return _longitude; }
            set
            {
                if (_longitude == value)
                {
                    return;
                }
                _longitude = value;
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}