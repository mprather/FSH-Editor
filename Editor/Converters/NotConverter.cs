using System;
using System.Globalization;
using System.Windows.Data;

namespace Editor.Converters {

  public class NotConverter : IValueConverter {

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
      return !(bool)value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
      return !(bool)value;
    }
  }

}
