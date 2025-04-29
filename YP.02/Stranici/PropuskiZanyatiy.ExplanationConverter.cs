using System;
using System.Globalization;
using System.Windows.Data;

namespace YP._02.Stranici
{
    public partial class PropuskiZanyatiy
    {
        public class ExplanationConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return (value != null) ? "Есть объяснительная" : "Нет объяснительной";
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

    }
}


