// ==================================================
// 文件名：CartesianChart.cs
// 创建时间：2020/05/25 13:38
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/07/29 13:38
// 修改人：jians
// ==================================================

//Copyright(c) 2016 Alberto Rodriguez & LiveCharts Contributors

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.ComponentModel;
using System.Windows;
using FactoryWindowGUI.ChartUtil.Charts.Base;
using LiveCharts;
using LiveCharts.Charts;
using LiveCharts.Definitions.Charts;
using ChartUpdater = FactoryWindowGUI.ChartUtil.Components.ChartUpdater;

namespace FactoryWindowGUI.ChartUtil
{
    /// <summary>
    ///     The Cartesian chart can plot any series with x and y coordinates
    /// </summary>
    public class CartesianChart : Chart, ICartesianChart
    {
        /// <summary>
        ///     The visual elements property
        /// </summary>
        public static readonly DependencyProperty VisualElementsProperty = DependencyProperty.Register(
            "VisualElements", typeof(VisualElementsCollection), typeof(CartesianChart),
            new PropertyMetadata(default(VisualElementsCollection), OnVisualCollectionChanged));

        /// <summary>
        ///     Initializes a new instance of CartesianChart class
        /// </summary>
        public CartesianChart()
        {
            var freq = DisableAnimations ? TimeSpan.FromMilliseconds(10) : AnimationsSpeed;
            var updater = new ChartUpdater(freq);
            ChartCoreModel = new CartesianChartCore(this, updater);

            SetCurrentValue(SeriesProperty,
                DesignerProperties.GetIsInDesignMode(this)
                    ? GetDesignerModeCollection()
                    : new SeriesCollection());

            SetCurrentValue(VisualElementsProperty, new VisualElementsCollection());
        }

        /// <summary>
        ///     Gets or sets the collection of visual elements in the chart, a visual element display another UiElement in the
        ///     chart.
        /// </summary>
        public VisualElementsCollection VisualElements
        {
            get { return (VisualElementsCollection) GetValue(VisualElementsProperty); }
            set { SetValue(VisualElementsProperty, value); }
        }

        private static void OnVisualCollectionChanged(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var chart = (CartesianChart) dependencyObject;

            if (chart.VisualElements != null) chart.VisualElements.Chart = chart.Model;
        }
    }
}