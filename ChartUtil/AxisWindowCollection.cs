// ==================================================
// 文件名：AxisWindowCollection.cs
// 创建时间：2020/05/25 13:39
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/07/29 13:39
// 修改人：jians
// ==================================================

using System.Collections.Generic;
using LiveCharts;
using LiveCharts.Helpers;

namespace FactoryWindowGUI.ChartUtil
{
    /// <summary>
    /// </summary>
    public class AxisWindowCollection : NoisyCollection<AxisWindow>
    {
        public AxisWindowCollection()
        {
            NoisyCollectionChanged += OnNoisyCollectionChanged;
        }

        private void OnNoisyCollectionChanged(IEnumerable<AxisWindow> oldItems, IEnumerable<AxisWindow> newItems)
        {
        }
    }
}