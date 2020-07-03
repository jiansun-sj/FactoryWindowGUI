// ==================================================
// 文件名：WorkFlowChartXmlConfigUtil.cs
// 创建时间：2020/01/04 16:25
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/05/11 16:25
// 修改人：jians
// ==================================================

using System;
using System.Xml;

namespace FactoryWindowGUI.Util
{
    public class WorkFlowChartXmlConfigUtil
    {
        private const int BeginX = 440;
        private const int BeginY = 40;

        private const int SizeX = 180;
        private const int SizeY = 50;
        private const int SpaceHeight = 80;

        private const int TurnSpace = 20;
        private readonly XmlDocument _xmlDoc = new XmlDocument();

        private int _turn1Count = 1;
        private int _turn3Count = 1;

        public void AddNewStepNode(short stepIndex,string index, string position, string background, string shape, string content,
            string itemKind)
        {
            try
            {
                _xmlDoc.Load("WorkFlowChartConfig.xml");
                var mainRoot = _xmlDoc.SelectSingleNode("/XtraSerializer/Items/Item1/Children");
                var childRoot = _xmlDoc.CreateElement("Item" + index);
                childRoot.SetAttribute("Tag", stepIndex.ToString());
                childRoot.SetAttribute("Position", position);
                childRoot.SetAttribute("Size", SizeX + "," + SizeY);
                childRoot.SetAttribute("Background", background);
                childRoot.SetAttribute("Shape", shape);
                childRoot.SetAttribute("Content", content);
                childRoot.SetAttribute("ItemKind", itemKind);
                childRoot.SetAttribute("FontWeight", "Bold");
                childRoot.SetAttribute("FontSize", "16");
                mainRoot?.AppendChild(childRoot);
                _xmlDoc.Save("WorkFlowChartConfig.xml");
            }
            catch (Exception)
            {
                // ignored
            }
        }
        //auto calculate step block position

        public string GetStepPosition(int index)
        {
            var positionX = BeginX.ToString();
            var positionY = (BeginY + SpaceHeight * (index - 1)).ToString();
            return positionX + "," + positionY;
        }

        public string GetLineTurn1(int index)
        {
            _turn1Count += 1;
            var positionX = (BeginX + SizeX + TurnSpace * (_turn1Count / 2)).ToString();
            var positionY = (BeginY + SpaceHeight * index + SizeY / 2).ToString();
            return positionX + "," + positionY;
        }

        public string GetLineTurn3(int index)
        {
            _turn3Count += 1;
            var positionX = (BeginX - TurnSpace * (_turn3Count / 2)).ToString();
            var positionY = (BeginY + SpaceHeight * index + SizeY / 2).ToString();
            return positionX + "," + positionY;
        }

        public void DeleteAllItem()
        {
            try
            {
                _turn1Count = 1;
                _turn3Count = 1;
                _xmlDoc.Load("WorkFlowChartConfig.xml");
                var mainRoot = _xmlDoc.SelectSingleNode("/XtraSerializer/Items/Item1/Children");
                mainRoot?.RemoveAll();
                _xmlDoc.Save("WorkFlowChartConfig.xml");
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void AddLineNode(string index, string beginItemPointIndex, string endItemPointIndex, string itemKind,
            string beginItem, string endItem, string points)
        {
            _xmlDoc.Load("WorkFlowChartConfig.xml");
            var mainRoot = _xmlDoc.SelectSingleNode("/XtraSerializer/Items/Item1/Children");
            var childRoot = _xmlDoc.CreateElement("Item" + index);
            childRoot.SetAttribute("BeginItemPointIndex", beginItemPointIndex);
            childRoot.SetAttribute("EndItemPointIndex", endItemPointIndex);
            childRoot.SetAttribute("Points", points);
            childRoot.SetAttribute("ItemKind", itemKind);
            childRoot.SetAttribute("BeginItem", beginItem);
            childRoot.SetAttribute("EndItem", endItem);
            //childRoot.SetAttribute("BeginPoint", BeginPoint);
            //childRoot.SetAttribute("EndPoint", EndPoint);
            mainRoot?.AppendChild(childRoot);
            _xmlDoc.Save("WorkFlowChartConfig.xml");
        }

        public void VaryCurrentStepBackground(short currentStepId)
        {
            try
            {
                _xmlDoc.Load("WorkFlowChartConfig.xml");
                var mainRoot = _xmlDoc.SelectSingleNode("/XtraSerializer/Items/Item1/Children");
                
                if (mainRoot != null)
                {
                    foreach (XmlElement childNode in mainRoot.ChildNodes)
                    {
                        if (!childNode.HasAttribute("StepIndex"))
                        {
                            continue;
                        }
                        
                        var stepIndex = short.Parse(childNode.GetAttribute("StepIndex"));

                        childNode.SetAttribute("Background", stepIndex == currentStepId ? "#FF44EB44" : "#FF5B9BD5");
                    }
                }

                _xmlDoc.Save("WorkFlowChartConfig.xml");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}