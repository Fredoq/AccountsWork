using AccountsWork.Reports.Model;
using Syncfusion.UI.Xaml.Charts;
using System;
using System.Globalization;
using System.Windows.Data;

namespace AccountsWork.Reports.Converters
{
    public class AmountLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ChartAdornment adornment = value as ChartAdornment;
            if (adornment != null)
            {
                var cultureInfo = new CultureInfo("ru-RU");
                var numberFormatInfo = (NumberFormatInfo)cultureInfo.NumberFormat.Clone();
                numberFormatInfo.CurrencySymbol = "₽";
                return string.Format("Сумма : " + adornment.YData.ToString("C", numberFormatInfo) + "\n" + "Кол-во ресторанов: " + (adornment.Item as MonthExp).StoreCount);
            }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
