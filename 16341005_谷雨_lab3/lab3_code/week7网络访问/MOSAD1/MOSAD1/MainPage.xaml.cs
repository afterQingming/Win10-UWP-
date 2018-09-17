using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using MOSAD1.Services;
using MOSAD1.Models;
using MOSAD1.ViewModels;

using System.ComponentModel;
// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace MOSAD1
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private WeatherViewModel weatherViewModel=new WeatherViewModel();
        private ExpressViewModel expressViewModel = new ExpressViewModel();
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public MainPage()
        {
            this.InitializeComponent();
            WeatherQuery();
           
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async void WeatherQuery()
        {
            try
            {
                var datas = await WeatherService.GetWeather(WeatherQueryBox.Text);
                weatherViewModel.update(datas);
            }
            catch (Exception)
            {
                ContentDialog dialog = new ContentDialog()
                {
                    Title = "消息提示",
                    Content = "没有找到该城市",
                    PrimaryButtonText = "确定",
                    FullSizeDesired = false,
                };
                await dialog.ShowAsync();
            }
            
        }
        private void WeatherQueryButton_clicked(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            WeatherQuery();
        }

        private void ExpressQueryButton_clicked(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            ExpressQuery();
        }
        public async void ExpressQuery()
        {

            if (ExpressQueryBox.Text==""|| ((ComboBoxItem)select.SelectedItem).Content.ToString()=="")
            {
                ContentDialog dialog = new ContentDialog()
                {
                    Title = "消息提示",
                    Content = "不能为空",
                    PrimaryButtonText = "确定",
                    FullSizeDesired = false,
                };
                await dialog.ShowAsync();
            }
            else
            {
                try
                {
                    var tt = (ComboBoxItem)select.SelectedItem;
                    var temp = await ExpressQueryService.GetExpressInfo(tt.Content.ToString(), ExpressQueryBox.Text);
                    expressViewModel.update(temp);
                }
                catch (Exception)
                {
                    ContentDialog dialog = new ContentDialog()
                    {
                        Title = "消息提示",
                        Content = "没有找到该快递",
                        PrimaryButtonText = "确定",
                        FullSizeDesired = false,
                    };
                    await dialog.ShowAsync();
                }
            }
           
            
            
           
        }
    }
}
