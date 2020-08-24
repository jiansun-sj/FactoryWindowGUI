// ==================================================
// 文件名：IDateAxisView.cs
// 创建时间：2020/05/25 13:37
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/07/29 13:37
// 修改人：jians
// ==================================================

using System;
using LiveCharts.Helpers;

namespace LiveCharts.Definitions.Charts
{
    /// <summary>
    /// </summary>
    public interface IDateAxisView : IWindowAxisView
    {
        /// <summary>
        ///     The datetime used for the first point to calculate relative date values
        /// </summary>
        DateTime InitialDateTime { get; set; }

        /// <summary>
        ///     Gets or sets the period used by the series in this axis
        /// </summary>
        PeriodUnits Period { get; set; }
    }
}