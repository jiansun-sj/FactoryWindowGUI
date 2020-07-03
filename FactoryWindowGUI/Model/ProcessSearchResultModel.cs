// ==================================================
// 文件名：ProcessSearchResultModel.cs
// 创建时间：2020/04/30 16:25
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/05/11 16:25
// 修改人：jians
// ==================================================

using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FactoryWindowGUI.Annotations;
using log4net;

namespace FactoryWindowGUI.Model
{
    /// <summary>
    ///     Process control view grid control model
    ///     Process  表
    /// </summary>
    public class ProcessSearchResultModel : INotifyPropertyChanged
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ProcessSearchResultModel));
        private long _breakingTimes;

        private string _conditionType;
        private string _flowChart;
        private int _index;
        private string _processName;
        private int _runningInstancesNumber;
        private long _totalRunningTimes;

        public int Index
        {
            get => _index;
            set
            {
                if (_index == value)
                    return;

                _index = value;
                OnPropertyChanged(nameof(Index));
            }
        }

        public int RunningInstancesNumber
        {
            get => _runningInstancesNumber;
            set
            {
                if (_runningInstancesNumber == value)
                    return;
                _runningInstancesNumber = value;
                OnPropertyChanged(nameof(RunningInstancesNumber));
            }
        }

        public long BreakingTimes
        {
            get => _breakingTimes;
            set
            {
                _breakingTimes = value;
                OnPropertyChanged(nameof(BreakingTimes));
            }
        }

        public List<string> ContainerNames { get; set; } = new List<string>();

        public long TotalRunningTimes
        {
            get => _totalRunningTimes;
            set
            {
                if (_totalRunningTimes == value)
                    return;

                _totalRunningTimes = value;
                OnPropertyChanged(nameof(TotalRunningTimes));
            }
        }

        public string ConditionType
        {
            get => _conditionType;
            set
            {
                if (_conditionType == value)
                    return;
                _conditionType = value;
                OnPropertyChanged(nameof(ConditionType));
            }
        }

        //process 名
        public string ProcessName
        {
            get => _processName;
            set
            {
                if (_processName == value)
                    return;
                _processName = value;
                OnPropertyChanged(nameof(ProcessName));
            }
        }

        // 流程图链接
        public string FlowChart
        {
            get => _flowChart;
            set
            {
                if (_flowChart == value)
                    return;
                _flowChart = value;
                OnPropertyChanged(nameof(FlowChart));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}