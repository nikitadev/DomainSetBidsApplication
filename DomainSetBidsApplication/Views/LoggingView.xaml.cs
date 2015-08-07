using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DomainSetBidsApplication.Utils;

namespace DomainSetBidsApplication.Views
{
    /// <summary>
    /// Interaction logic for LoggingView.xaml
    /// </summary>
    public partial class LoggingView : UserControl
    {
        public LoggingView()
        {
            InitializeComponent();
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (sender as ListBox).SelectedItem != null;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var listBox = sender as ListBox;

            // Get the ListBoxItem 
            var listItem = listBox.ItemContainerGenerator.ContainerFromIndex(listBox.SelectedIndex) as ListBoxItem;

            // Retrieve the first child which is assumed to be a framework element
            var element = VisualTreeHelper.GetChild(listItem, 0) as FrameworkElement;

            // now perform the copy
            element.ToClipboard();
        }
    }
}
