// ==================================================
// 文件名：StartProcessWindow.xaml.cs
// 创建时间：2020/04/30 16:25
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/05/11 16:25
// 修改人：jians
// ==================================================

using System.Collections.Generic;
using FactoryWindowGUI.Model;
using FactoryWindowGUI.Util;
using FactoryWindowGUI.ViewModel;

namespace FactoryWindowGUI.View
{
    /// <summary>
    ///     StartProcessWindow.xaml 的交互逻辑
    /// </summary>
    public partial class StartProcessWindow
    {
        public StartProcessWindow()
        {
            InitializeComponent();
        }

        public StartProcessWindowViewModel StartProcessWindowVm { get; set; } = new StartProcessWindowViewModel();

        public void InitializeProcessAttribute(ProcessSearchResultModel selectedProcess, List<string> resources,
            ProcessUtil processUtil)
        {
            StartProcessWindowVm.SetProcessAttribute(selectedProcess, resources, processUtil);
        }
    }
}