// ==================================================
// 文件名：IGroupedStackedSeriesView.cs
// 创建时间：2020/05/25 13:37
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/07/29 13:37
// 修改人：jians
// ==================================================

namespace LiveCharts.Definitions.Series
{
    /// <summary>
    /// </summary>
    /// <seealso cref="LiveCharts.Definitions.Series.ISeriesView" />
    public interface IGroupedStackedSeriesView : ISeriesView
    {
        /// <summary>
        ///     Gets or sets the column grouping.
        /// </summary>
        /// <value>
        ///     The column grouping.
        /// </value>
        object Grouping { get; set; }
    }
}