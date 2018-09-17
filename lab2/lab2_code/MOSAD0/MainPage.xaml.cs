using Windows.UI.Xaml;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;
using System.Diagnostics;
using Windows.Storage;
using System.Collections.Generic;
using Newtonsoft.Json;
using MOSAD0.Services;
using Windows.ApplicationModel.DataTransfer;
using System.IO;
// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace MOSAD0
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>

    public sealed partial class MainPage : Page
    {
        private ViewModels.TodoItemViewModel ViewModel = ViewModels.TodoItemViewModel.GetInstance();
        public MainPage()
        {
            this.InitializeComponent();
            
            this.SizeChanged += (s, e) =>
            {
                var state = "Wide";
                if (e.NewSize.Width > 000 && e.NewSize.Width <= 600)
                {
                    state = "Narrow";
                    if (ViewModel.selectedItem == null&&PageView.Content==null)
                    {
                        state += "PanelView";
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
                    state += "PanelView";
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
            PageView.Navigate(typeof(NewPage));
            ViewModel.selectedItem = null;
            VisualStateChange();
        }
        private void editPage()
        {
            //Frame rootFrame = Window.Current.Content as Frame;
 
            PageView.Navigate(typeof(EditPage));
            VisualStateChange();
        }
        private void detailPage()
        {
            //Frame rootFrame = Window.Current.Content as Frame;

            PageView.Navigate(typeof(DetailPage));
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
            DataService.ReadFileAsync();

            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    Windows.UI.Core.AppViewBackButtonVisibility.Collapsed;
            if (e.NavigationMode != NavigationMode.New)
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("selected"))
                {
                    foreach (var item in ViewModel.AllItems)
                    {
                        if (item.id == (string)ApplicationData.Current.LocalSettings.Values["selected"])
                        {
                            ViewModel.selectedItem = item;
                        }
                    }

                }
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("pageNavigationState"))
                {
                    PageView.SetNavigationState((string)ApplicationData.Current.LocalSettings.Values["pageNavigationState"]);

                }
                ApplicationData.Current.LocalSettings.Values.Remove("selected");
                ApplicationData.Current.LocalSettings.Values.Remove("pageNavigationState");

            }

        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            bool suspending = ((App)App.Current).issuspend;
            if (suspending)
            {
                
                ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
                if(PageView != null&&PageView.Content!=null)
                {
                    ApplicationData.Current.LocalSettings.Values["pageNavigationState"] = PageView.GetNavigationState();
                    
                }
                
                if (ViewModel.selectedItem != null)
                {
                    ApplicationData.Current.LocalSettings.Values["selected"] = ViewModel.selectedItem.id;
                }
                
            }
            base.OnNavigatedFrom(e);
        }
        private async void saveAsync()
        {
            List<string> L = new List<string>();
            var allitems = ViewModel.AllItems;
            foreach (var item in allitems)
            {
                Models.myItem myitem = new Models.myItem(item);
                Debug.WriteLine(item);
                L.Add(JsonConvert.SerializeObject(myitem));

            }
            StorageFolder pictureFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("ProfilePictures");
            var file = await pictureFolder.CreateFileAsync("file", CreationCollisionOption.ReplaceExisting);

            string str = JsonConvert.SerializeObject(L);
            byte[] content = System.Text.Encoding.ASCII.GetBytes(str);

            using (var stream = await file.OpenStreamForWriteAsync())
            {
                await stream.WriteAsync(content,0,content.Length);
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var data = (sender as FrameworkElement).DataContext;
            var item = ItemListView.ContainerFromItem(data) as ListViewItem;
            ViewModel.selectedItem = item.Content as Models.TodoItem;
            DataService.Delete(ViewModel.selectedItem.id);
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
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var data =(sender as FrameworkElement).DataContext;
            var item = ItemListView.ContainerFromItem(data) as ListViewItem;
            DataService.Complete((item.Content as Models.TodoItem).id,(sender as CheckBox).IsChecked.Value);
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
                    VisualStateManager.GoToState(this, "NarrowPanelView", true);
                    

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
                
            }
            else
            {
                DataService.Delete(ViewModel.selectedItem.id);
                PageView.Content = null;
                ViewModel.selectedItem = null;
                VisualStateChange();
            }
            MyGoBack();
        }

        private async void BackgroundChangeButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            background.ImageSource = await DataService.ImgSelectorAsync();
        }

        private void ShareButton_Click(object sender, RoutedEventArgs e)
        {

            dynamic ori = e.OriginalSource;
            ViewModel.selectedItem = (Models.TodoItem)ori.DataContext;
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += DataTransferManager_DataRequested;
            DataTransferManager.ShowShareUI();
        }
        async void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            var deferral = args.Request.GetDeferral();
            StorageFolder pictureFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("ProfilePictures");
            StorageFile pictureFile = await pictureFolder.GetFileAsync(ViewModel.selectedItem.id + ".jpg");
            
            request.Data.Properties.Title = ViewModel.selectedItem.title;
            string str = ViewModel.selectedItem.description;
            request.Data.SetText(str);
            request.Data.SetStorageItems(new List<StorageFile> { pictureFile });
            deferral.Complete();

        }

        private async void SearchButton_clicked(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            string alert = DatabaseService.GetInstance().Query(args.QueryText);
            ContentDialog dialog = new ContentDialog()
            {
                Title = "消息提示",
                Content = alert,
                PrimaryButtonText = "确定",
                FullSizeDesired = false,
            };
            await dialog.ShowAsync();
        }

        
    }
}
