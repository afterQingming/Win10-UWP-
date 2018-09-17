using MOSAD0.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MOSAD0.ViewModels
{
    class TodoItemViewModel
    {
        private static TodoItemViewModel todoItemViewModel;
        public static TodoItemViewModel GetInstance()
        {
            if (todoItemViewModel==null)
            {
                todoItemViewModel = new TodoItemViewModel();
            }
            return todoItemViewModel;
        }
        public Models.TodoItem selectedItem { get; set; }

        private ObservableCollection<Models.TodoItem> allItems = new ObservableCollection<Models.TodoItem>();
        public ObservableCollection<Models.TodoItem> AllItems { get { return allItems; } }

        public TodoItemViewModel() { }
        public void AddTodoItem(Models.TodoItem newModel)
        {
            this.AllItems.Add(newModel);
        }
        public void RemoveTodoItem()
        {
            this.AllItems.Remove(this.selectedItem);
            this.selectedItem = null;
        }
        public void UpdateTodoItem(string title, string description,DateTime date, double _imgSize, ImageSource _img)
        {
            if (this.selectedItem != null)
            {
                this.selectedItem.title = title;
                this.selectedItem.description = description;
                this.selectedItem.date = date;
                this.selectedItem.img = _img;
                this.selectedItem.imgSize = _imgSize;
            }
            this.selectedItem = null;
        }
    }
    
}