using AccountsWork.Reports.Model;
using Syncfusion.UI.Xaml.Charts;
using System;
using System.Globalization;
using System.Windows.Data;

namespace AccountsWork.Reports.Converters
{
    public class LabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ChartAdornment pieAdornment = value as ChartAdornment;
            if (pieAdornment != null)
            {
                var cultureInfo = new CultureInfo("ru-RU");
                var numberFormatInfo = (NumberFormatInfo)cultureInfo.NumberFormat.Clone();
                numberFormatInfo.CurrencySymbol = "₽";
                return string.Format((pieAdornment.Item as StatusSum).Status + " : " + pieAdornment.YData.ToString("C", numberFormatInfo));
            }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
