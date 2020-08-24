// ==================================================
// 文件名：AxisWindows.cs
// 创建时间：2020/05/25 13:37
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/07/29 13:37
// 修改人：jians
// ==================================================

namespace LiveCharts
{
    /// <summary>
    /// </summary>
    public static class AxisWindows
    {
        /// <summary>
        /// </summary>
        public static AxisWindow EmptyWindow => new EmptyAxisWindow();

        /// <summary>
        /// </summary>
        public class EmptyAxisWindow : AxisWindow
        {
            /// <inheritdoc />
            public override double MinimumSeparatorWidth => 0;

            /// <inheritdoc />
            public override bool IsHeader(double x)
            {
                return false;
            }

            /// <inheritdoc />
            public override bool IsSeparator(double x)
            {
                return false;
            }

            /// <inheritdoc />
            public override string FormatAxisLabel(double x)
            {
                return "";
            }
        }
    }
}