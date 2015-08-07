using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DomainSetBidsApplication.Utils;

namespace DomainSetBidsApplication.Views
{
    /// <summary>
    /// Interaction logic for MonitorView.xaml
    /// </summary>
    public partial class MonitorView : UserControl
    {
        public MonitorView()
        {
            InitializeComponent();
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (sender as ListView).SelectedItem != null;
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var listBox = sender as ListView;

            // Get the ListBoxItem 
            var listItem = listBox.ItemContainerGenerator.ContainerFromIndex(listBox.SelectedIndex) as ListViewItem;

            // Retrieve the first child which is assumed to be a framework element
            var element = VisualTreeHelper.GetChild(listItem, 0) as FrameworkElement;

            // now perform the copy
            element.ToClipboard();
        }
    }
}
