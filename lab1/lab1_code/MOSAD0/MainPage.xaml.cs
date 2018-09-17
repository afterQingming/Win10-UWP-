using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.Storage.Streams;
using Windows.Storage.Pickers;
using Windows.Storage;
// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace MOSAD0
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    
    public sealed partial class MainPage : Page
    {
        private ViewModels.TodoItemViewModel ViewModel = new ViewModels.TodoItemViewModel();
        public MainPage()
        {
            this.InitializeComponent();
            ViewModel.AddTodoItem("示例", "22", DateTime.Now.AddDays(1),100,null);
            this.SizeChanged += (s, e) =>
            {
                var state = "Wide";
                if (e.NewSize.Width > 000 && e.NewSize.Width <= 600)
                {
                    state = "Narrow";
                    if (ViewModel.selectedItem == null&&PageView.Content==null)
                    {
                        state += "ItemListView";
                    }
                    else
                    {
                        state += "PageView";
                    }
                }
                else if (e.NewSize.Width > 600)
                {
                    if (ViewModel.selectedItem == null && PageView.Content == null)
                    {
                        state += "One";
                        PageView.Content = null;
                    }
                    else
                    {
                        state += "Both";
                    }
                    
                }
                
                VisualStateManager.GoToState(this, state, true);
            };
            var m = SystemNavigationManager.GetForCurrentView();
            m.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            m.BackRequested += M_BackRequested;
            //ImageBrush imageBrush = new ImageBrush();
            //imageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/background.jpg", UriKind.Absolute));
            //gd_backimage.Background = imageBrush;
        }
        public void VisualStateChange()
        {
            var state = "Wide";
            if (Window.Current.Bounds.Width > 000 && Window.Current.Bounds.Width <= 600)
            {
                state = "Narrow";
                if (PageView.Content==null)
                {
                    state += "ItemListView";
                }
                else
                {
                    state += "PageView";
                }
            }
            else if (Window.Current.Bounds.Width > 600)
            {
                if (PageView.Content == null)
                {
                    state += "One";
                }
                else
                {
                    state += "Both";
                }

            }
            VisualStateManager.GoToState(this, state, true);
        }
    

        private void newPage()
        {
            //Frame rootFrame = Window.Current.Content as Frame;
            PageView.Navigate(typeof(NewPage),ViewModel);
            ViewModel.selectedItem = null;
            VisualStateChange();
        }
        private void editPage()
        {
            //Frame rootFrame = Window.Current.Content as Frame;
 
            PageView.Navigate(typeof(EditPage), ViewModel);
            VisualStateChange();
        }
        private void detailPage()
        {
            //Frame rootFrame = Window.Current.Content as Frame;

            PageView.Navigate(typeof(DetailPage), ViewModel);
            VisualStateChange();
        }
        private void TodoItem_ItemCLicked(object sender,ItemClickEventArgs e)
        {
 
            
            dynamic clickedItem = e.ClickedItem;
            ViewModel.selectedItem = clickedItem;
            detailPage();
            //ViewModel.RemoveTodoItem(clickedItem.id);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    Windows.UI.Core.AppViewBackButtonVisibility.Collapsed;
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var data = (sender as FrameworkElement).DataContext;
            var item = ItemListView.ContainerFromItem(data) as ListViewItem;
            ViewModel.selectedItem = item.Content as Models.TodoItem;
            ViewModel.RemoveTodoItem();
            PageView.Content = null;
            ViewModel.selectedItem = null;
            VisualStateChange();
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            newPage();
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            var data = (sender as FrameworkElement).DataContext;
            var item = ItemListView.ContainerFromItem(data) as ListViewItem;
            ViewModel.selectedItem = item.Content as Models.TodoItem;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var data = (sender as FrameworkElement).DataContext;
            var item = ItemListView.ContainerFromItem(data) as ListViewItem;
            ViewModel.selectedItem = item.Content as Models.TodoItem;
            editPage();
        }
        private void M_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (e.Handled == true)
            {
                return;
            }
            MyGoBack();
            e.Handled = true;
        }
        public void MyGoBack()
        {
            
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    Windows.UI.Core.AppViewBackButtonVisibility.Collapsed;
            if (this.ActualWidth > 600)
            {

                ViewModel.selectedItem = null;
                PageView.Content = null;
                VisualStateManager.GoToState(this, "WideOne", true);
            }
            else//窄屏时如果显示PageView就切换回ItemListView  
            {

                if (PageView.Visibility == Visibility.Visible)
                {
                    ViewModel.selectedItem = null;
                    PageView.Content = null;
                    PageView.Visibility = Visibility.Collapsed;
                    ItemListView.Visibility = Visibility.Visible;
                    VisualStateManager.GoToState(this, "NarrowItemListView", true);
                    

                }
                else//窄屏时如果显示ItemListView就退出  
                {
                    App.Current.Exit();
                }
            }
        }

        private void DeleteSomeButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.selectedItem == null)
            {
                ViewModel.DeleteSelectedItem();
            }
            else
            {
                ViewModel.RemoveTodoItem();
                PageView.Content = null;
                ViewModel.selectedItem = null;
                VisualStateChange();
            }
        }

        private async void BackgroundChangeButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.List;  //设置文件的现实方式，这里选择的是图标  
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary; //设置打开时的默认路径，这里选择的是图片库  
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".gif");                      //添加可选择的文件类型，这个必须要设置  
            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();    //只能选择一个文件  

            if (file != null)
            {

                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(stream);
                    background.ImageSource = bitmapImage;
                }


                //to do something  
            }
        }
    }
}
