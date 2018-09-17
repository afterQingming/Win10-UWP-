using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MOSAD0.ViewModels
{
    class TodoItemViewModel
    {
        private Models.TodoItem _selectedItem=null;
        private ObservableCollection<Models.TodoItem> allItems = new ObservableCollection<Models.TodoItem>();
        public ObservableCollection<Models.TodoItem> AllItems { get { return allItems; } }
        public TodoItemViewModel() { 
          
        }
        public void AddTodoItem(string title, string description,DateTimeOffset date,double _imgSize, ImageSource _img)
        {
            this.AllItems.Add(new Models.TodoItem(title, description,date,_imgSize,_img));
        }
        public void RemoveTodoItem()
        {
            this.AllItems.Remove(this._selectedItem);
            this._selectedItem = null;
        }
        public void UpdateTodoItem(string title, string description,DateTime date, double _imgSize, ImageSource _img)
        {
            if (this._selectedItem != null)
            {
                this._selectedItem.title = title;
                this._selectedItem.description = description;
                this._selectedItem.date = date;
                this._selectedItem.img = _img;
                this._selectedItem.imgSize = _imgSize;
            }
            //
            this._selectedItem = null;
        }
        public void DeleteSelectedItem()
        {
            for(int i = 0; i < AllItems.Count; i++)
            {
                if (AllItems[i].selected == true)
                {
                    AllItems.RemoveAt(i);
                    i--;
                }
            }
        }
        public Models.TodoItem selectedItem { get { return this._selectedItem; } set { this._selectedItem = value; } }
    }
    
}