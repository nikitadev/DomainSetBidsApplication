using System.Collections.Generic;
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
            var listBox = (sender as ListBox);
            e.CanExecute = listBox.SelectedItems != null;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var listBox = sender as ListBox;

            var elements = new List<FrameworkElement>();
            foreach (var items in listBox.SelectedItems)
            {
                // Get the ListBoxItem 
                var listItem = listBox.ItemContainerGenerator.ContainerFromIndex(listBox.SelectedIndex) as ListBoxItem;

                // Retrieve the first child which is assumed to be a framework element
                var element = VisualTreeHelper.GetChild(listItem, 0) as FrameworkElement;

                elements.Add(element);

                // now perform the copy
                elements.ToClipboard();
            }
        }
    }
}
