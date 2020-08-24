// ==================================================
// 文件名：VisualElementsCollection.cs
// 创建时间：2020/05/25 13:37
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/07/29 13:37
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

using System.Collections.Generic;
using LiveCharts.Charts;
using LiveCharts.Definitions.Charts;
using LiveCharts.Helpers;

namespace LiveCharts
{
    /// <summary>
    ///     Defines a collection of items to be added in a cartesian chart
    /// </summary>
    public class VisualElementsCollection : NoisyCollection<ICartesianVisualElement>
    {
        /// <summary>
        ///     Initializes a new instance of VisualElementsCollection
        /// </summary>
        public VisualElementsCollection()
        {
            NoisyCollectionChanged += OnNoisyCollectionChanged;
        }

        /// <summary>
        ///     Gets or sets the chart.
        /// </summary>
        /// <value>
        ///     The chart.
        /// </value>
        public ChartCore Chart { get; set; }

        private void OnNoisyCollectionChanged(IEnumerable<ICartesianVisualElement> oldItems,
            IEnumerable<ICartesianVisualElement> newItems)
        {
            if (oldItems != null)
                foreach (var oltItem in oldItems)
                    oltItem.Remove(Chart);
            if (newItems != null)
                foreach (var newItem in newItems)
                    newItem.AddOrMove(Chart);
        }
    }
}