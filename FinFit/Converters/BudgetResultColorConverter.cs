using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace FinFit.Converters
{
    public class BudgetResultColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is decimal income && values[1] is decimal totalExpenses)
            {
                return totalExpenses > income ? Colors.Red : Colors.Green;
            }

            return Colors.Black; // Fallback color
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}