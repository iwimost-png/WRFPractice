using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using WRFPractice.Models;

namespace WRFPractice.Converters;

public class RoleToHorizontalAlignmentConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is MessageRole role)
        {
            return role == MessageRole.User ? HorizontalAlignment.Right : HorizontalAlignment.Left;
        }
        return HorizontalAlignment.Left;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

public class RoleToBubbleBackgroundConverter : IValueConverter
{
    private static readonly SolidColorBrush UserBrush = new(Color.FromRgb(37, 99, 235));       // #2563EB (Royal Blue)
    private static readonly SolidColorBrush AssistantBrush = new(Color.FromRgb(241, 245, 249)); // #F1F5F9 (Light Slate)
    private static readonly SolidColorBrush ErrorBrush = new(Color.FromRgb(254, 226, 226));     // #FEE2E2 (Light Red)

    static RoleToBubbleBackgroundConverter()
    {
        UserBrush.Freeze();
        AssistantBrush.Freeze();
        ErrorBrush.Freeze();
    }

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is MessageRole role)
        {
            return role == MessageRole.User ? UserBrush : AssistantBrush;
        }
        return AssistantBrush;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

public class RoleToForegroundConverter : IValueConverter
{
    private static readonly SolidColorBrush WhiteBrush = new(Colors.White);
    private static readonly SolidColorBrush DarkBrush = new(Color.FromRgb(15, 23, 42)); // #0F172A

    static RoleToForegroundConverter()
    {
        WhiteBrush.Freeze();
        DarkBrush.Freeze();
    }

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is MessageRole role)
        {
            return role == MessageRole.User ? WhiteBrush : DarkBrush;
        }
        return DarkBrush;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

public class RoleToAvatarConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is MessageRole role)
        {
            return role == MessageRole.User ? "👤" : "🤖";
        }
        return "🤖";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

public class RoleToSenderNameConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is MessageRole role)
        {
            return role == MessageRole.User ? "あなた" : "AI アシスタント";
        }
        return "システム";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

public class RoleToBorderBrushConverter : IValueConverter
{
    private static readonly SolidColorBrush UserBorder = new(Color.FromRgb(29, 78, 216));       // #1D4ED8 (Darker Blue)
    private static readonly SolidColorBrush AssistantBorder = new(Color.FromRgb(226, 232, 240)); // #E2E8F0 (BorderBrush)

    static RoleToBorderBrushConverter()
    {
        UserBorder.Freeze();
        AssistantBorder.Freeze();
    }

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is MessageRole role)
        {
            return role == MessageRole.User ? UserBorder : AssistantBorder;
        }
        return AssistantBorder;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}

