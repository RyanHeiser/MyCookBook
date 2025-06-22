using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace MyCookBook.WPF.Converters
{
    public class PercentageConverter : MarkupExtension, IValueConverter
    {
        private static PercentageConverter? _instance;

        #region IValueConverter Members

        /// <summary>
        /// Converts the value of an element to a percentage of itself.
        /// </summary>
        /// <param name="value">The value being converted</param>
        /// <param name="targetType"></param>
        /// <param name="percentage">The percentage the value is multiplied by</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object percentage, CultureInfo culture)
        {
            return System.Convert.ToDouble(value) * System.Convert.ToDouble(percentage);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new PercentageConverter());
        }
    }
}
