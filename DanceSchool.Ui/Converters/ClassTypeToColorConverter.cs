using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using DanceSchool.Data.Enums;

namespace DanceSchool.Ui.Converters
{
    public class ClassTypeToColorConverter : IValueConverter
    {
        public static readonly ClassTypeToColorConverter Instance = new();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is ClassType classType)
            {
                return classType switch
                {
                    ClassType.Regular => Brush.Parse("#059669"), // Green
                    ClassType.Technical => Brush.Parse("#0284c7"), // Blue
                    ClassType.Trial => Brush.Parse("#dc2626"), // Red
                    ClassType.OpenForParents => Brush.Parse("#7c3aed"), // Purple
                    _ => Brush.Parse("#6b7280") // Gray
                };
            }
            
            return Brush.Parse("#6b7280");
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}