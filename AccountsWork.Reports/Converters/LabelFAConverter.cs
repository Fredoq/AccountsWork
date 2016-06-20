using AccountsWork.Reports.Model;
using Syncfusion.UI.Xaml.Charts;
using System;
using System.Globalization;
using System.Windows.Data;

namespace AccountsWork.Reports.Converters
{
    public class LabelFAConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ChartAdornment pieAdornment = value as ChartAdornment;
            if (pieAdornment != null)
            {
                return string.Format((pieAdornment.Item as AccountFA).Capex + " : " + pieAdornment.YData.ToString() + " шт.");
            }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
