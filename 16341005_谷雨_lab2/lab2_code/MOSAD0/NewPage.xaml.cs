using MOSAD0.Services;
using System;
using System.Diagnostics;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace MOSAD0
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NewPage : Page
    {
        private ViewModels.TodoItemViewModel ViewModel= ViewModels.TodoItemViewModel.GetInstance();

        public NewPage()
        {
            this.InitializeComponent();
        }
        private async void item_createAsync(object sender, RoutedEventArgs e)
        {
            if(title.Text == "")
            {
                ContentDialog dialog = new ContentDialog()
                {
                    Title = "消息提示",
                    Content = "标题内容不能为空",
                    PrimaryButtonText = "确定",
                    FullSizeDesired = false,
                };
                await dialog.ShowAsync();
            }
            else if (details.Text == "")
            {
                ContentDialog dialog = new ContentDialog()
                {
                    Title = "消息提示",
                    Content = "详情不能为空",
                    PrimaryButtonText = "确定",
                    FullSizeDesired = false,
                };
                await dialog.ShowAsync();
            }
            else if ( date.Date < DateTime.Now) {
                ContentDialog dialog = new ContentDialog()
                {
                    Title = "消息提示",
                    Content = "日期必须大于当前",
                    PrimaryButtonText = "确定",
                    FullSizeDesired = false,
                };
                await dialog.ShowAsync();
            }
            else
            {
                createSuccess();
                MyGoBack();
            }
        }
        private void MyGoBack()
        {

            Frame rootFrame = Window.Current.Content as Frame;
            MainPage ma = rootFrame.Content as MainPage;
            ma.MyGoBack();
        }
        private void createSuccess()
        {
            DataService.Insert(title.Text, details.Text, date.Date.DateTime,img.Width,img.Source);
        }
        private void create_cancel(object sender, RoutedEventArgs e)
        {
            clearContent();
        }
        private void clearContent()
        {
            title.Text="";
            details.Text = "";
            date.Date = DateTime.Now;
            PsetAsync();
        }
        private async void PsetAsync()
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/timg.jpg"));
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
            {
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                WriteableBitmap bmp = new WriteableBitmap((int)decoder.PixelWidth, (int)decoder.PixelHeight);
                await bmp.SetSourceAsync(stream);
                ImageSource t = bmp;
                img.Source = t;
                DataService.SaveBitmapToFileAsync(bmp, "temp");

            }
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    Windows.UI.Core.AppViewBackButtonVisibility.Visible;
            if (e.NavigationMode == NavigationMode.New)
            {
                PsetAsync();
            }
            else
            {
                Resume();
            }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            bool suspending = ((App)App.Current).issuspend;
            if (suspending)
            {
                saveTemporarily();   
            }
            base.OnNavigatedFrom(e);
        }
        private void saveTemporarily()
        {
            DataService.SaveTemp(title.Text, details.Text,img.Width,date.Date.DateTime,img.Source);
        }
        private void Resume()
        {
            string[] temp = new string[4];
            DataService.ResumeTemp(temp,img);
            title.Text = temp[0];
            details.Text = temp[1];
            slid.Value = Convert.ToDouble(temp[2]);
            date.Date = Convert.ToDateTime(temp[3]);
        }
        private async void AppBarButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            ImageSource imgsource= await DataService.ImgSelectorAsync();
            img.Source = imgsource;
        }
    }
}
