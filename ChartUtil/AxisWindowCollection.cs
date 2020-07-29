using System.Collections.Generic;
using LiveCharts;
using LiveCharts.Helpers;

namespace FactoryWindowGUI.ChartUtil
{
    /// <summary>
    /// 
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