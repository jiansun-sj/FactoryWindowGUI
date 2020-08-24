// ==================================================
// 文件名：JArrayLengthConverter.cs
// 创建时间：2020/03/13 16:13
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/07/29 16:13
// 修改人：jians
// ==================================================

using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace FactoryWindowGUI.ValueConverters
{
    public sealed class JArrayLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (!(value is JToken jToken))
                    throw new Exception("Wrong type for this converter");

                switch (jToken.Type)
                {
                    case JTokenType.Array:
                        var arrayLen = jToken.Children().Count();
                        return $"[{arrayLen}]";
                    case JTokenType.Property:
                        var propertyArrayLen = jToken.Children().FirstOrDefault()?.Children().Count();
                        return $"[ {propertyArrayLen} ]";
                    default:
                        throw new Exception("Type should be JProperty or JArray");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($@"{e}");
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException(GetType().Name + " can only be used for one way conversion.");
        }
    }
}