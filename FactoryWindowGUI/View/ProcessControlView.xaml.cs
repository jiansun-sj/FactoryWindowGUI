// ==================================================
// 文件名：ProcessControlView.xaml.cs
// 创建时间：2020/04/12 16:25
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/05/11 16:25
// 修改人：jians
// ==================================================

using System;
using System.Windows;
using System.Windows.Media.Animation;
using DevExpress.Xpf.Grid;
using FactoryWindowGUI.ViewModel;
using log4net;

namespace FactoryWindowGUI.View
{
    /// <summary>
    ///     ProcessControlView.xaml 的交互逻辑
    /// </summary>
    public partial class ProcessControlView
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ProcessControlView));

        private readonly DoubleAnimation _hideGridAnimation =
            new DoubleAnimation(1.0, 0.0, new Duration(new TimeSpan(0, 0, 0, 0, 200)));

        private readonly DoubleAnimation _showGridAnimation =
            new DoubleAnimation(0.0, 1.0, new Duration(new TimeSpan(0, 0, 0, 0, 200)));

        public ProcessControlView()
        {
            InitializeComponent();

            ProcessControlVm.WorkFlowChartControl = WorkFlowChartControl;
        }

        public ProcessControlViewModel ProcessControlVm { get; set; } = new ProcessControlViewModel();

        private void ChangeViewContent(object sender, EventArgs e)
        {
            try
            {
                _hideGridAnimation.Completed += (o, args) =>
                {
                    ProcessQueryGrid.Visibility = Visibility.Collapsed;
                    ProcessRecordGrid.Visibility = Visibility.Visible;
                    ProcessRecordGrid.BeginAnimation(OpacityProperty, _showGridAnimation);
                };

                ProcessQueryGrid.BeginAnimation(OpacityProperty, _hideGridAnimation);
            }
            catch (Exception exception)
            {
                Log.Error($"更改界面内容失败，异常为:{exception.Message}");
            }
        }
        
        private void InstanceListOnSelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            ProcessControlVm.QueryProcessInstance();
        }

        private void BackToProcessQueryViewButton_OnClick(object sender, EventArgs eventArgs)
        {
            ProcessRecordGrid.Visibility = Visibility.Collapsed;

            ProcessQueryGrid.Visibility = Visibility.Visible;

            ProcessControlVm.RecordCounts = 10;

            ProcessControlVm.SearchDate = DateTime.Today.ToString("yyyy-M-d");

            ProcessControlVm.ProcessInstanceRecords.Clear();
        }
    }
}