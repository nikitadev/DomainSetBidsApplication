using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
            CopyUIElementToClipboard(element);
        }

        /// <summary> 
        /// Copies a UI element to the clipboard as an image, and as text. 
        /// </summary> 
        /// <param name="element">The element to copy.</param> 
        public static void CopyUIElementToClipboard(FrameworkElement element)
        {
            string tabbedText = element.DataContext.ToString();
            string csvText = element.DataContext.ToString();

            // data object to hold our different formats representing the element 
            var dataObject = new DataObject();

            dataObject.SetText(tabbedText);

            // Convert the CSV text to a UTF-8 byte stream before adding it to the container object.
            var bytes = System.Text.Encoding.UTF8.GetBytes(csvText);
            var stream = new System.IO.MemoryStream(bytes);
            dataObject.SetData(DataFormats.CommaSeparatedValue, stream);

            // lets start with the text representation 
            // to make is easy we will just assume the object set as the DataContext has the ToString method overrideen and we use that as the text
            dataObject.SetData(DataFormats.CommaSeparatedValue, stream);

            // now place our object in the clipboard 
            Clipboard.SetDataObject(dataObject, true);
        }
    }
}
