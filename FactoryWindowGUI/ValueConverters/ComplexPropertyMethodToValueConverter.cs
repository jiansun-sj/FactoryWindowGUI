// ==================================================
// 文件名：ComplexPropertyMethodToValueConverter.cs
// 创建时间：2020/03/13 13:39
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/07/29 13:39
// 修改人：jians
// ==================================================

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace FactoryWindowGUI.ValueConverters
{
    // This converter is only used by JProperty tokens whose Value is Array/Object
    internal class ComplexPropertyMethodToValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null || !(parameter is string methodName))
                    return null;
                var methodInfo = value.GetType().GetMethod(methodName, new Type[0]);
                if (methodInfo == null)
                    return null;
                var invocationResult = methodInfo.Invoke(value, new object[0]);
                var jTokens = (IEnumerable<JToken>) invocationResult;
                return jTokens.First().Children();
            }
            catch (Exception e)
            {
                MessageBox.Show($@"{e}");
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException(GetType().Name + " can only be used for one way conversion.");
        }
    }
}