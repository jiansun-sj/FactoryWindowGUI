// ==================================================
// 文件名：IOhlcPointView.cs
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

namespace LiveCharts.Definitions.Points
{
    /// <summary>
    /// </summary>
    /// <seealso cref="LiveCharts.Definitions.Points.IChartPointView" />
    public interface IOhlcPointView : IChartPointView
    {
        /// <summary>
        ///     Gets or sets the open.
        /// </summary>
        /// <value>
        ///     The open.
        /// </value>
        double Open { get; set; }

        /// <summary>
        ///     Gets or sets the high.
        /// </summary>
        /// <value>
        ///     The high.
        /// </value>
        double High { get; set; }

        /// <summary>
        ///     Gets or sets the close.
        /// </summary>
        /// <value>
        ///     The close.
        /// </value>
        double Close { get; set; }

        /// <summary>
        ///     Gets or sets the low.
        /// </summary>
        /// <value>
        ///     The low.
        /// </value>
        double Low { get; set; }

        /// <summary>
        ///     Gets or sets the width.
        /// </summary>
        /// <value>
        ///     The width.
        /// </value>
        double Width { get; set; }

        /// <summary>
        ///     Gets or sets the left.
        /// </summary>
        /// <value>
        ///     The left.
        /// </value>
        double Left { get; set; }

        /// <summary>
        ///     Gets or sets the start reference.
        /// </summary>
        /// <value>
        ///     The start reference.
        /// </value>
        double StartReference { get; set; }
    }
}