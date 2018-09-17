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
        private ViewModels.TodoItemViewModel ViewModel = null;
        public DetailPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.ViewModel = (ViewModels.TodoItemViewModel)e.Parameter;
            Frame rootFrame = Window.Current.Content as Frame;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    Windows.UI.Core.AppViewBackButtonVisibility.Visible;
        }

        private void EditButtonClicked(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = this.Parent as Frame;
            rootFrame.Navigate(typeof(EditPage),ViewModel);
        }
        private void MyGoBack()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            MainPage ma = rootFrame.Content as MainPage;
            ma.MyGoBack();
        }
    }
}
