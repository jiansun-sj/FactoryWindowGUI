// ==================================================
// 文件名：TagControlView.xaml.cs
// 创建时间：2020/04/12 16:25
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
    ///     TagControlView.xaml 的交互逻辑
    /// </summary>
    public partial class TagControlView
    {
        private bool _dataSourceFirstLoaded = true;
        private bool _firstLoaded = true;

        public TagControlView()
        {
            InitializeComponent();
            TagControlVm.TagControlView = this;
        }

        public TagControlViewModel TagControlVm { get; set; } = new TagControlViewModel();

        private void TagControlViewLoaded(object sender, RoutedEventArgs e)
        {
            if (!_firstLoaded) return;

            TagControlVm.RefreshMachineListCommand.Execute(null);

            _firstLoaded = false;
        }

        private void DataSourcePageLoaded(object sender, RoutedEventArgs e)
        {
            if (!_dataSourceFirstLoaded) return;
            DataSourceList.Dispatcher.Invoke(() =>
            {
                TagControlVm.RefreshDataSourceCommand.Execute(null);
                _dataSourceFirstLoaded = false;
            });
        }
    }
}