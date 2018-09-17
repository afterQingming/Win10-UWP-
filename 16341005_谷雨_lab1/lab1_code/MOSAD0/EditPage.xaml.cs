using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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
        private ViewModels.TodoItemViewModel ViewModel = null;
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
            ViewModel.selectedItem.title = title.Text;
            ViewModel.selectedItem.description = details.Text;
            ViewModel.selectedItem.date = date.Date;
            ViewModel.selectedItem.img = img.Source;
            ViewModel.selectedItem.imgSize = img.Width;
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
            this.ViewModel = (ViewModels.TodoItemViewModel)e.Parameter;
            Frame rootFrame = Window.Current.Content as Frame;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    Windows.UI.Core.AppViewBackButtonVisibility.Visible;
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {

            img.Width = slid.Value + 100;
            img.Height = slid.Value + 100;
        }

        private async void AppBarButton_ClickAsync(object sender, RoutedEventArgs e)
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
                    img.Source = bitmapImage;
                }


                //to do something  
            }
        }
    }
}

