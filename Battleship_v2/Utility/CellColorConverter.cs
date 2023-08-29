using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Battleship_v2.Utility
{
    public class CellColorConverter : IMultiValueConverter
    {
        object IMultiValueConverter.Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is DataGridCell cell && values[1] is DataRow row)
            {
                try
                {
                    string columnName = (string)cell.Column.Header;
                    int columnIndex = cell.Column.DisplayIndex;

                    if (columnIndex == 0)
                    {
                        cell.FontStyle = FontStyles.Italic;
                        cell.FontWeight = FontWeights.Bold;
                        cell.FontSize = 20;
                        return new SolidColorBrush(Colors.Yellow);
                    }

                    string content = row.Field<string>(columnName); // Header must be same as column name

                    if (content == "w")
                    {
                        return new SolidColorBrush(Colors.Blue);
                    }

                    if (content == "h")
                    {
                        return new SolidColorBrush(Colors.Red);
                    }

                    if (content == "m")
                    {
                        return new SolidColorBrush(Colors.LightGray);
                    }

                    if (content == "c" || content == "b" || content == "s" || content == "d" || content == "p")
                    {
                        return new SolidColorBrush(Colors.Gray);
                    }

                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    return new SolidColorBrush(Colors.Black); // Error! An Exception was thrown
                }
            }
            return new SolidColorBrush(Colors.Green); // Something else.
        }

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
