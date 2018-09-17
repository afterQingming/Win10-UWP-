using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOSAD1.Services;
using MOSAD1.Models;

namespace MOSAD1.ViewModels
{
    class WeatherViewModel: INotifyPropertyChanged
    {
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private Now _nowWeather; 
        public Now nowWeather { get {return _nowWeather; }set { this._nowWeather = value; NotifyPropertyChanged("nowWeather"); } }
        private Suggestion _suggestion;
        public Suggestion suggestion { get {return this._suggestion; } set {this._suggestion=value; NotifyPropertyChanged("suggestion"); } }
        private List<Daily_forecast> _daily_forecast;
        public List<Daily_forecast> daily_forecast { get { return this._daily_forecast; } set {this._daily_forecast=value; NotifyPropertyChanged("daily_forecast"); } }
        private Basic _basic;
        public Basic basic { get { return this._basic; } set { this._basic = value; NotifyPropertyChanged("basic"); } }
        private ExpressInfoModel _expressInfoModel;
        public ExpressInfoModel expressInfoModel { get { return this._expressInfoModel; } set { this._expressInfoModel = value; NotifyPropertyChanged("expressInfoModel"); } }
        public void update(RootObject rootObject)
        {
            nowWeather = rootObject.HeWeather5[0].now;
            suggestion= rootObject.HeWeather5[0].suggestion;
            daily_forecast= rootObject.HeWeather5[0].daily_forecast;
            basic = rootObject.HeWeather5[0].basic;
        }

        
    }
    class ExpressViewModel : INotifyPropertyChanged
    {
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private ExpressInfoModel _expressInfoModel;
        public ExpressInfoModel expressInfoModel { get { return this._expressInfoModel; } set { this._expressInfoModel = value; NotifyPropertyChanged("expressInfoModel"); } }
        public void update(root root)
        {
            expressInfoModel= root.result;
        }
    }
}
