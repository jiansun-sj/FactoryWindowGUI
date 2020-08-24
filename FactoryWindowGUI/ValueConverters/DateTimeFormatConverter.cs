// ==================================================
// 文件名：TickStringToDateTimeConverter.cs
// 创建时间：2020/06/11 13:39
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/07/29 13:39
// 修改人：jians
// ==================================================

using System;
using System.Globalization;
using System.Windows.Data;

namespace FactoryWindowGUI.ValueConverters
{
    internal class DateTimeFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value is DateTime dateTime)
                    return dateTime.ToString("G");

                return value;
            }
            catch (Exception e)
            {
                Console.WriteLine($@"Tick转换DateTime失败，value：[{value}]");
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException(GetType().Name + " can only be used for one way conversion.");
        }
    }
}