using System.Text;
using System.Windows;

namespace DomainSetBidsApplication.Utils
{
    internal static class FrameworkElementEx
    {
        /// <summary> 
        /// Copies a UI element to the clipboard as an image, and as text. 
        /// </summary> 
        /// <param name="element">The element to copy.</param>
        /// <remarks>Needs implement ToString method</remarks>
        public static void ToClipboard(this FrameworkElement element)
        {
            string tabbedText = element.DataContext.ToString();
            string csvText = element.DataContext.ToString();

            // data object to hold our different formats representing the element 
            var dataObject = new DataObject();

            dataObject.SetText(tabbedText);

            // Convert the CSV text to a UTF-8 byte stream before adding it to the container object.
            var bytes = Encoding.UTF8.GetBytes(csvText);
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
