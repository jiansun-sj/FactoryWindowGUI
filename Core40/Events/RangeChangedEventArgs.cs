// ==================================================
// 文件名：RangeChangedEventArgs.cs
// 创建时间：2020/05/25 13:36
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/07/29 13:36
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

namespace LiveCharts.Events
{
    /// <summary>
    /// </summary>
    public class RangeChangedEventArgs
    {
        /// <summary>
        ///     Gets the min limit difference compared with previous state
        /// </summary>
        public double LeftLimitChange { get; set; }

        /// <summary>
        ///     Gets the max limit difference compared with previous state
        /// </summary>
        public double RightLimitChange { get; set; }

        /// <summary>
        ///     Gets the current axis range
        /// </summary>
        public double Range { get; set; }

        /// <summary>
        ///     Gets the axis that fired the change
        /// </summary>
        public object Axis { get; set; }
    }
}