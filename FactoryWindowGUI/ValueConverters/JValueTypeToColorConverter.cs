﻿// ==================================================
// 文件名：JValueTypeToColorConverter.cs
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
using System.Windows.Media;
using Newtonsoft.Json.Linq;

namespace FactoryWindowGUI.ValueConverters
{
    public sealed class JValueTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is JValue jValue)
                switch (jValue.Type)
                {
                    case JTokenType.String:
                        return new BrushConverter().ConvertFrom("#4e9a06");
                    case JTokenType.Float:
                    case JTokenType.Integer:
                        return new BrushConverter().ConvertFrom("#ad7fa8");
                    case JTokenType.Boolean:
                        return new BrushConverter().ConvertFrom("#c4a000");
                    case JTokenType.Null:
                        return new SolidColorBrush(Colors.OrangeRed);
                }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException(GetType().Name + " can only be used for one way conversion.");
        }
    }
}