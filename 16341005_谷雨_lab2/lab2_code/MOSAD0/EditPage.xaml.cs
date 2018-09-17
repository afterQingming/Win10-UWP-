using MOSAD0.Services;
using System;
using System.Diagnostics;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace MOSAD0
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class EditPage : Page
    {
        private ViewModels.TodoItemViewModel ViewModel = ViewModels.TodoItemViewModel.GetInstance();
        public EditPage()
        {
            this.InitializeComponent();

        }
        private async void ItemEditAsync(object sender, RoutedEventArgs e)
        {
            if (title.Text == "")
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
            else if (date.Date < DateTime.Now)
            {
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
                EditSuccess();
                MyGoBack();
            }
        }
        private void MyGoBack()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            MainPage ma = rootFrame.Content as MainPage;
            ma.MyGoBack();
        }
        private void EditSuccess()
        {
            DataService.Update(title.Text, details.Text, date.Date.DateTime, img.Width, img.Source);
        }
        private void EditCancel(object sender, RoutedEventArgs e)
        {
            title.Text = ViewModel.selectedItem.title;
            details.Text = ViewModel.selectedItem.description;
            date.Date = ViewModel.selectedItem.date;
            img.Source = ViewModel.selectedItem.img;
            img.Width = ViewModel.selectedItem.imgSize;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            Frame rootFrame = Window.Current.Content as Frame;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    Windows.UI.Core.AppViewBackButtonVisibility.Visible;
            loadAndImgCache();
            if (e.NavigationMode == NavigationMode.New)
            {
                
            }
            else
            {
                Resume();
            }
        }
        private void loadAndImgCache()
        {
            title.Text = ViewModel.selectedItem.title;
            details.Text = ViewModel.selectedItem.description;
            date.Date = ViewModel.selectedItem.date;
            img.Source = ViewModel.selectedItem.img;
            img.Width = ViewModel.selectedItem.imgSize;
            imgCache();
        }
        private void imgCache()
        {
            WriteableBitmap sor = img.Source as WriteableBitmap;
            if(sor!=null)
            DataService.SaveBitmapToFileAsync(sor, "temp");
            else
            {
                Debug.Write("??");
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
            DataService.SaveTemp(title.Text, details.Text, img.Width, date.Date.DateTime, img.Source);
        }
        private void Resume()
        {
            string[] temp = new string[4];
            DataService.ResumeTemp(temp, img);
            title.Text = temp[0];
            details.Text = temp[1];
            slid.Value = Convert.ToDouble(temp[2]);
            date.Date = Convert.ToDateTime(temp[3]);
        }
        private async void AppBarButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            img.Source = await DataService.ImgSelectorAsync();
        }
    }
}

