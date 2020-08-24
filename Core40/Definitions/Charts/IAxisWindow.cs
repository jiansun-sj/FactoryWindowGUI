// ==================================================
// 文件名：IAxisWindow.cs
// 创建时间：2020/05/25 13:37
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/07/29 13:37
// 修改人：jians
// ==================================================

using System.Collections.Generic;

namespace LiveCharts
{
    /// <summary>
    /// </summary>
    public interface IAxisWindow
    {
        /// <summary>
        ///     Gets the minimum reserved space for separators
        /// </summary>
        double MinimumSeparatorWidth { get; }

        /// <summary>
        ///     Determines whether a dateTime is a header
        /// </summary>
        bool IsHeader(double x);

        /// <summary>
        ///     Gets or sets a function to determine whether a dateTime is a separator
        /// </summary>
        bool IsSeparator(double x);

        /// <summary>
        ///     Gets or sets a function to format the label for the axis
        /// </summary>
        string FormatAxisLabel(double x);

        bool TryGetSeparatorIndices(IEnumerable<double> indices, int maximumSeparatorcount,
            out IEnumerable<double> separatorIndices);
    }
}