using MOSAD0.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MOSAD0.Models
{
    public class TodoItem:INotifyPropertyChanged
    {
        private string _id;
        private bool _selected;
        private string _title;
        private string _description;
        private DateTime _date;
        private ImageSource _img;
        private double _imgSize;
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public bool selected { get { return this._selected; } set { this._selected = value; NotifyPropertyChanged("selected"); } }
        public string title { get { return this._title; } set { this._title = value == null ? "unknown" : value; ; NotifyPropertyChanged("title"); } }
        public string description { get { return this._description; } set { this._description = value == null ? "unknown" : value; NotifyPropertyChanged("description"); } }
        public ImageSource img { get { return this._img; } set { _img = value;  NotifyPropertyChanged("img"); } }
        public double imgSize { get { return this._imgSize; } set { this._imgSize = value == 0 ? 100 : value; NotifyPropertyChanged("imgSize"); } }
        public string id { get { return _id; } }
        //public bool completed { get { return this.completed; } set { this.completed = value; NotifyPropertyChanged("completed"); } }
        public DateTime date { get { return this._date; } set { this._date = value == null ? DateTime.Now : value; NotifyPropertyChanged("date"); } }
       
        public TodoItem(string _title,string _description, DateTime _date,double _imgSize, ImageSource _img)
        {
            this._id = Guid.NewGuid().ToString();
            this.title = _title;
            this.description = _description;
            this.date = _date;
            this.imgSize = _imgSize;
            this.img=_img;
            //this.completed = false;
        }
        
        public async void transferAsync(myItem item)
        {
            this._id = item.id;
            title = item.title;
            description = item.description;
            imgSize = item.imgSize;
            date = item.date;
            selected = item.selected;
            img = await DataService.GetProfilePictureAsync(_id);
        }
        public TodoItem(myItem item)
        {
            transferAsync(item);
        }
    }
    public class myItem
    {
        public string id { get; set; }
        public string title { get; set;  }
        public string description { get; set; }
        public double imgSize { get; set; }
        public DateTime date { get; set; }

        public bool selected { get; set; }

        [Newtonsoft.Json.JsonConstructor]
        public myItem(string id,string title, string description, double imgSize,DateTime date,bool selected)
        {
            this.id = id;
            this.title = title;
            this.description = description;
            this.date = date;
            this.imgSize = imgSize;
            this.selected = selected;
        }
        public myItem(TodoItem item)
        {
            id = item.id;
            title = item.title;
            description = item.description;
            imgSize = item.imgSize;
            date = item.date;
            selected = item.selected;
        }
    }
}
