// ==================================================
// 文件名：SystemControlView.xaml.cs
// 创建时间：2020/03/04 16:25
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/05/11 16:25
// 修改人：jians
// ==================================================

using System.Windows;
using FactoryWindowGUI.ViewModel;

namespace FactoryWindowGUI.View
{
    /// <summary>
    ///     SystemControlView.xaml 的交互逻辑
    /// </summary>
    public partial class SystemControlView
    {
        public SystemControlView()
        {
            InitializeComponent();
        }

        public SystemControlViewModel SystemControlVm { get; set; } = new SystemControlViewModel();

        private void RefreshMemoryAndCpu(object sender, RoutedEventArgs e)
        {
            if (DataContext is SystemControlViewModel systemControlViewModel)
            {
                systemControlViewModel.GetProcessRamAndCpuUsage();
            }
        }
    }
}