using System.Windows;
using DomainSetBidsApplication.Views;
using GalaSoft.MvvmLight.Threading;

namespace DomainSetBidsApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainView = new MainWindowView();
            mainView.Show();
        }
    }
}
