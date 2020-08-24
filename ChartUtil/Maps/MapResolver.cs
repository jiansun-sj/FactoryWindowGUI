﻿// ==================================================
// 文件名：MapResolver.cs
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
using System.IO;
using System.Xml;
using LiveCharts.Maps;

namespace FactoryWindowGUI.ChartUtil.Maps
{
    internal static class MapResolver
    {
        public static LvcMap Get(string file)
        {
            //file = Path.Combine(Directory.GetCurrentDirectory(), file);

            if (!File.Exists(file))
                throw new FileNotFoundException(string.Format("This file {0} was not found.", file));

            var svgMap = new LvcMap
            {
                DesiredHeight = 600,
                DesiredWidth = 800,
                Data = new List<MapData>()
            };

            using (var reader = XmlReader.Create(file, new XmlReaderSettings {IgnoreComments = true}))
            {
                while (reader.Read())
                {
                    if (reader.Name == "Height") svgMap.DesiredHeight = double.Parse(reader.ReadInnerXml());
                    if (reader.Name == "Width") svgMap.DesiredWidth = double.Parse(reader.ReadInnerXml());
                    if (reader.Name == "MapShape")
                    {
                        var p = new MapData
                        {
                            LvcMap = svgMap
                        };
                        reader.Read();
                        while (reader.NodeType != XmlNodeType.EndElement)
                        {
                            if (reader.NodeType != XmlNodeType.Element) reader.Read();
                            if (reader.Name == "Id") p.Id = reader.ReadInnerXml();
                            if (reader.Name == "Name") p.Name = reader.ReadInnerXml();
                            if (reader.Name == "Path") p.Data = reader.ReadInnerXml();
                        }

                        svgMap.Data.Add(p);
                    }
                }
            }

            return svgMap;
        }
    }
}