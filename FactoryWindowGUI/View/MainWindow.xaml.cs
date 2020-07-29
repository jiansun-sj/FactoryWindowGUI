// ==================================================
// 文件名：MainWindow.xaml.cs
// 创建时间：2020/03/04 16:25
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/05/11 16:25
// 修改人：jians
// ==================================================

using FactoryWindowGUI.ViewModel;

//using FactoryWindowGUI.Config.Models;

namespace FactoryWindowGUI.View
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindowViewModel MainWindowVm { get; set; } = new MainWindowViewModel();
    }
}