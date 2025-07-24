using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using System.Xml;

namespace MyCookBook.WPF.Converters
{
    public class FlowToFixedDocumentConverter : IValueConverter
    {
        private string? _tempXpsPath;
        private XpsDocument? _xpsDocument;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            FlowDocument flowDocument = (FlowDocument)value;
            DocumentPaginator paginator = ((IDocumentPaginatorSource)flowDocument).DocumentPaginator;

            // Create a temp file
            _tempXpsPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xps");

            // Write paginator to temp file
            using (var xpsDocWrite = new XpsDocument(_tempXpsPath, FileAccess.ReadWrite))
            {
                XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(xpsDocWrite);
                writer.Write(paginator);
            }

            // Reopen the file in read mode and return document
            _xpsDocument = new XpsDocument(_tempXpsPath, FileAccess.Read);
            FixedDocument fixedDoc = _xpsDocument.GetFixedDocumentSequence().References[0].GetDocument(false);
            return fixedDoc;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
