using System;
using Windows.UI.Xaml.Data;

namespace MovieMaker.Helpers
{
    public class DoubleToTimeSpan : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return new TimeSpan();
            }
            var b = TimeSpan.FromSeconds((double)value);
            return $"{b:hh\\:mm\\:ss}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return 0;
            }
            return ((TimeSpan)value).TotalSeconds;
        }
    }
}
