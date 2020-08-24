// ==================================================
// 文件名：SectionsCollection.cs
// 创建时间：2020/05/25 13:39
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/07/29 13:39
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
using LiveCharts.Helpers;

namespace FactoryWindowGUI.ChartUtil
{
    /// <summary>
    ///     The SectionsCollection class holds a collection of Axis.Sections
    /// </summary>
    public class SectionsCollection : NoisyCollection<AxisSection>
    {
        /// <summary>
        ///     Initializes a new instance of SectionsCollection instance
        /// </summary>
        public SectionsCollection()
        {
            NoisyCollectionChanged += OnNoisyCollectionChanged;
        }

        private static void OnNoisyCollectionChanged(IEnumerable<AxisSection> oldItems,
            IEnumerable<AxisSection> newItems)
        {
            if (oldItems == null) return;

            foreach (var oldSection in oldItems) oldSection.Remove();
        }
    }
}