using System;
using System.Windows.Data;

namespace Editor.Converters {

  public class EnumToBooleanConverter : IValueConverter {

    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      return value.Equals(parameter);
    }  // End of Convert

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
      return value.Equals(true) ? parameter : Binding.DoNothing;
    }  // End of ConvertBack

  }  // End of EnumToBooleanConverter class

}
