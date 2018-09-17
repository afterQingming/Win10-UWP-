using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace MOSAD0
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DetailPage : Page
    {
        private ViewModels.TodoItemViewModel ViewModel = ViewModels.TodoItemViewModel.GetInstance();
        public DetailPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    Windows.UI.Core.AppViewBackButtonVisibility.Visible;

            if (e.NavigationMode == NavigationMode.New)
            {
                ApplicationData.Current.LocalSettings.Values.Remove("detailpage");

            }
            else
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("detailpage"))
                {
                    var composite = ApplicationData.Current.LocalSettings.Values["detailpage"] as ApplicationDataCompositeValue;
                    title.Text = (string)composite["title"];
                    details.Text = (string)composite["details"];
                    date.Date = (DateTime)composite["date"];

                    ApplicationData.Current.LocalSettings.Values.Remove("detailpage");
                }
            }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            
            bool suspending = ((App)App.Current).issuspend;
            if (suspending)
            {
                ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
                composite["title"] = title.Text;
                composite["details"] = details.Text;
                composite["date"] = date.Date;
                ApplicationData.Current.LocalSettings.Values["detailpage"] = composite;
            }
            base.OnNavigatedFrom(e);
        }
        private void EditButtonClicked(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = this.Parent as Frame;
            rootFrame.Navigate(typeof(EditPage));
        }
        
    }
}
