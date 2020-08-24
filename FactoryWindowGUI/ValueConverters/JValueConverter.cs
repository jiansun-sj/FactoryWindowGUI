// ==================================================
// 文件名：JValueConverter.cs
// 创建时间：2020/03/13 13:39
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/07/29 13:39
// 修改人：jians
// ==================================================

using System;
using System.Globalization;
using System.Windows.Data;
using Newtonsoft.Json.Linq;

namespace FactoryWindowGUI.ValueConverters
{
    public sealed class JValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is JValue val)
                switch (val.Type)
                {
                    case JTokenType.String:
                        return "\"" + val.Value + "\"";
                    case JTokenType.Null:
                        return "Null";
                }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException(GetType().Name + " can only be used for one way conversion.");
        }
    }
}