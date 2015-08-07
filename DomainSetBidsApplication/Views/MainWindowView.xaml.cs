using System;
using System.Threading.Tasks;
using System.Windows;
using AviaTicketsWpfApplication.Models;
using DomainSetBidsApplication.Utils;
using DomainSetBidsApplication.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace DomainSetBidsApplication.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : MetroWindow
    {
        private bool _shutdown;

        public MainWindowView()
        {
            InitializeComponent();

            Closing += async (s, e) => { e.Cancel = !_shutdown; await CloseHandleAsync(); };

            mainFrame.NavigationService.Navigate(DataContext);
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            Messenger.Default.Register<PageMessage>(this, PageMessageHandler);
            Messenger.Default.Register<bool>(this, MessageToken.PAGE_GO_BACK, NavigationBackHandler);
        }

        private void PageMessageHandler(PageMessage msg)
        {
            var uriPage = ViewModelLocator.GetPathPage(msg.TypeViewModel);
            mainFrame.NavigationService.Navigate(uriPage, msg.Parametrs);
        }

        private void NavigationBackHandler(bool isBack)
        {
            if (isBack)
                mainFrame.NavigationService.GoBack();

            mainFrame.NavigationService.GoForward();
        }

        private async Task CloseHandleAsync()
        {
            if (_shutdown) return;

            var settings = new MetroDialogSettings()
            {
                AffirmativeButtonText = Properties.Resources.Close,
                NegativeButtonText = Properties.Resources.Cancel,
                AnimateShow = true,
                AnimateHide = false
            };

            var result = await this.ShowMessageAsync(
                Properties.Resources.TitleDlgExitApp,
                Properties.Resources.QuestionExitApp,
                MessageDialogStyle.AffirmativeAndNegative, settings);

            if (result == MessageDialogResult.Affirmative)
            {
                ViewModelLocator.Cleanup();

                _shutdown = true;
                Application.Current.Shutdown();
            }
        }
    }
}
