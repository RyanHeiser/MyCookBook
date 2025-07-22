using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Xps.Packaging;
using System.Xml;

namespace MyCookBook.WPF.Converters
{
    public class FlowToFixedDocumentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            FlowDocument flowDocument = (FlowDocument)value;
            DocumentPaginator paginator = ((IDocumentPaginatorSource)flowDocument).DocumentPaginator;

            Package package = Package.Open(new MemoryStream(), FileMode.Create, FileAccess.ReadWrite);


            Uri packageUri = new Uri("pack://temp.xps");
            PackageStore.RemovePackage(packageUri);
            PackageStore.AddPackage(packageUri, package);
            XpsDocument xps = new XpsDocument(package, CompressionOption.NotCompressed, packageUri.ToString());
            XpsDocument.CreateXpsDocumentWriter(xps).Write(paginator);
            FixedDocument doc = xps.GetFixedDocumentSequence().References[0].GetDocument(true);
            return doc;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private FlowDocument CloneFlowDocument(FlowDocument original)
        {
            string xaml = XamlWriter.Save(original);
            using (var stringReader = new StringReader(xaml))
            {
                using (var xmlReader = XmlReader.Create(stringReader))
                {
                    return (FlowDocument)XamlReader.Load(xmlReader);
                }
            }
        }
    }
}
