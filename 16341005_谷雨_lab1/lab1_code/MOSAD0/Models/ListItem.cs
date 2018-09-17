using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MOSAD0.Models
{
    class TodoItem:INotifyPropertyChanged
    {
        private string _id;
        private bool _selected;
        private string _title;
        private string _description;
        private DateTimeOffset _date;
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
        public ImageSource img { get { return this._img; } set { this._img = value==null? new BitmapImage(new Uri("ms-appx:///Assets/timg.jpg")):value; NotifyPropertyChanged("img"); } }
        public double imgSize { get { return this._imgSize; } set { this._imgSize = value == 0 ? 100 : value; NotifyPropertyChanged("imgSize"); } }
        public string id { get; }
        //public bool completed { get { return this.completed; } set { this.completed = value; NotifyPropertyChanged("completed"); } }
        public DateTimeOffset date { get { return this._date; } set { this._date = value == null ? DateTime.Now : value; NotifyPropertyChanged("date"); } }
       
        public TodoItem(string _title,string _description, DateTimeOffset _date,double _imgSize, ImageSource _img)
        {
            this.title = _title;
            this.description = _description;
            this.date = _date;
            this.imgSize = _imgSize;
            this.img=_img;
            //this.completed = false;
        }

    }
}
