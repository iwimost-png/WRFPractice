using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WRFPractice.Converters;

public class BooleanToVisibilityConverter : IValueConverter
{
    public bool Invert { get; set; } = false;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool flag = false;
        if (value is bool b) flag = b;

        if (Invert) flag = !flag;

        return flag ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Visibility vis)
        {
            bool flag = vis == Visibility.Visible;
            return Invert ? !flag : flag;
        }
        return false;
    }
}
