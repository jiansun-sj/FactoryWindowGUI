// ==================================================
// 文件名：IWindowAxisView.cs
// 创建时间：2020/05/25 13:37
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/07/29 13:37
// 修改人：jians
// ==================================================

namespace LiveCharts.Definitions.Charts
{
    /// <summary>
    /// </summary>
    public interface IWindowAxisView : IAxisView
    {
        void SetSelectedWindow(IAxisWindow window);
    }
}