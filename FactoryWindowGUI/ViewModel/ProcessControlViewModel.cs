// ==================================================
// 文件名：ProcessControlViewModel.cs
// 创建时间：2020/04/30 16:26
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/05/11 16:26
// 修改人：jians
// ==================================================

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Arthas.Controls.Metro;
using DevExpress.Xpf.Diagram;
using FactoryWindowGUI.Annotations;
using FactoryWindowGUI.ICommandImpl;
using FactoryWindowGUI.Model;
using FactoryWindowGUI.Util;
using FactoryWindowGUI.View;
using log4net;
using ProcessControlService.Contracts.ProcessData;

namespace FactoryWindowGUI.ViewModel
{
    public class ProcessControlViewModel : INotifyPropertyChanged
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ProcessControlViewModel));

        private bool _processTabIsSelected;

        public bool ProcessTabIsSelected
        {
            get => _processTabIsSelected;
            set
            {
                _processTabIsSelected = value;

                _flowChartShown = value != true;
            }
        }
        
        private readonly DispatcherTimer _autoRefreshProcessTimer =
            new DispatcherTimer {Interval = new TimeSpan(0, 0, 0, 0,400)};

        private readonly object _processLocker = new object();

        private bool _flowChartShown;
        
        private readonly ProcessUtil _processUtil = new ProcessUtil();

        public DiagramControl WorkFlowChartControl { get; set; }

        //process的属性

        //refresh switch button view model
        private bool _processAutoRefresh;

        private ObservableCollection<ProcessInstanceModel> _processInstanceModels =
            new ObservableCollection<ProcessInstanceModel>();

        private ObservableCollection<ProcessInstanceRecord> _processInstanceRecords =
            new ObservableCollection<ProcessInstanceRecord>();

        private int _recordCounts = 10;

        //Process combobox item source
        private ProcessInstanceModel _selectedInstanceModel = new ProcessInstanceModel();

        private ProcessSearchResultModel _selectedProcess;

        private ProcessInstanceRecord _selectedProcessRecord = new ProcessInstanceRecord();

        private StartProcessWindow _startProcessWindow /*=new StartProcessWindow()*/;

        private List<string> _staticResources = new List<string>();

        public ProcessControlViewModel()
        {
            _autoRefreshProcessTimer.Tick += AutoFreshProcess;
        }

        public List<int> RecordCountSelection { get; set; } = new List<int> {10, 20, 50, 100};

        public int RecordCounts
        {
            get => _recordCounts;
            set
            {
                _recordCounts = value;
                OnPropertyChanged(nameof(RecordCounts));
            }
        }

        public ICommand AutoRefreshProcessCommand =>
            new RelayCommandImplementation(AutoFreshChecked, AutoRefreshProcessCanExecute);

        public ICommand RefreshProcessCommand =>
            new RelayCommandImplementation(RefreshProcess);

        public ICommand QueryProcessRecordCommand =>
            new RelayCommandImplementation(QueryProcessRecord);

        public ObservableCollection<ProcessInstanceRecord> ProcessInstanceRecords
        {
            get => _processInstanceRecords;
            set
            {
                _processInstanceRecords = value;
                OnPropertyChanged(nameof(ProcessInstanceRecords));
            }
        }

        public ProcessInstanceRecord SelectedProcessRecord
        {
            get => _selectedProcessRecord;
            set
            {
                _selectedProcessRecord = value;
                OnPropertyChanged(nameof(SelectedProcessRecord));
                OnPropertyChanged(nameof(ProcessExceptions));
            }
        }

        [NotNull]
        public List<ProcessException> ProcessExceptions
        {
            get
            {
                var i = 0;
                if (SelectedProcessRecord == null) return new List<ProcessException>();
                var list = SelectedProcessRecord?.Messages
                    .Select(a => new ProcessException {Id = ++i, Exception = a.Description})
                    .ToList();
                return list;
            }
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
            }
        }

        //------- Process--------------------------------------------------------------------------
        public ProcessInstanceModel SelectedInstanceModel
        {
            get => _selectedInstanceModel;
            set
            {
                _selectedInstanceModel = value;
                OnPropertyChanged(nameof(SelectedInstanceModel));
            }
        }

        public bool ProcessAutoRefresh
        {
            get => _processAutoRefresh;
            set
            {
                _processAutoRefresh = value;
                OnPropertyChanged(nameof(ProcessAutoRefresh));
            }
        }

        // Process 停止
        public ICommand StopProcessInstanceCommand =>
            new RelayCommandImplementation(StopProcessInstance, StopProcessInstanceCanExecute);
        
        public ICommand ShowFlowChartCommand =>
            new RelayCommandImplementation(ShowFlowChart);

        private static readonly WorkFlowChartXmlConfigUtil XmlUtil = new WorkFlowChartXmlConfigUtil();

        public string SelectedProcessName
        {
            get => _selectedProcessName;
            set
            {
                _selectedProcessName = value;
                OnPropertyChanged(nameof(SelectedProcessName));
            }
        }

        private Dictionary<short, List<string>> _stepInfo = new Dictionary<short, List<string>>();
        private string _selectedProcessName;

        /// <summary>
        /// 当前步骤与上次系统刷新不一致时，高亮当前步骤。
        /// </summary>
        /// <param name="currentStepId"></param>
        private void VaryFlowChartCurrentStep(short currentStepId)
        {
            if (!_flowChartShown)
            {
                return;
            }

            var diagramItemCollection = WorkFlowChartControl.Items;

            foreach (var diagramItem in diagramItemCollection)
            {
                if (diagramItem is DiagramShape)
                {
                    diagramItem.Background = diagramItem.Tag.ToString()==currentStepId.ToString() ? new SolidColorBrush(Color.FromArgb(0xFF,0x44,0xEB,0x44)) : new SolidColorBrush(Color.FromArgb(0xFF,0x5B,0x9B,0xD5));
                }
            }
        }
        
        private void ShowFlowChart(object obj)
        { 
            try
            {
                if (!(obj is MetroTabItem processWorkFlowControl))
                {
                    return;
                }
                
                processWorkFlowControl.IsSelected = true;

                _flowChartShown = true;

                SelectedProcessName = SelectedProcess.ProcessName;

                _stepInfo = _processUtil.GetWorkFlowChartInfo(SelectedProcessName);
                
                XmlUtil.DeleteAllItem();

                //var currentStep = _processUtil.GetCurrentStep(_processName);

                var stepIndex = new Dictionary<short, int>();

                var hanover = false;
                var index = 0;
                foreach (var item in _stepInfo)
                {
                    //添加Step
                    stepIndex.Add(item.Key, index); //ItemID从1开始,但是线连接的Item从0开始算
                    index += 1;
                    XmlUtil.AddNewStepNode(item.Key,index.ToString(), XmlUtil.GetStepPosition(index),
                        /*item.Key == currentStep ? "#FF44EB44" :*/ "#FF5B9BD5",
                        "BasicShapes.RoundedRectangle", item.Value[0], "DiagramShape");
                    for (var i = 1; i < item.Value.Count; i++)
                        if (item.Value[i] == "-1")
                            hanover = true;
                }

                if (hanover)
                {
                    stepIndex.Add(-1, index);
                    index += 1;
                    XmlUtil.AddNewStepNode(-1,index.ToString(), XmlUtil.GetStepPosition(index),
                        /*-1 == currentStep ? "#FF44EB44" :*/ "#FF5B9BD5", "BasicShapes.RoundedRectangle", "结束",
                        "DiagramShape");
                }

                foreach (var item in _stepInfo)
                    //添加线
                    for (var i = 1; i < item.Value.Count; i++)
                    {
                        index += 1;
                        var nextStepId = Convert.ToInt16(item.Value[i]);

                        var stepIndexNextStepId = stepIndex[nextStepId];
                        var stepIndexStepId = stepIndex[item.Key];

                        if (stepIndexNextStepId == stepIndexStepId + 1)
                            XmlUtil.AddLineNode(index.ToString(), "2", "0", "DiagramConnector",
                                stepIndexStepId.ToString(), stepIndexNextStepId.ToString(), "(Empty)");
                        else if (stepIndexNextStepId > stepIndexStepId)
                            XmlUtil.AddLineNode(index.ToString(), "1", "1", "DiagramConnector",
                                stepIndexStepId.ToString(), stepIndexNextStepId.ToString(),
                                XmlUtil.GetLineTurn1(stepIndexStepId) + " " +
                                XmlUtil.GetLineTurn1(stepIndexNextStepId));
                        else
                            XmlUtil.AddLineNode(index.ToString(), "3", "3", "DiagramConnector",
                                stepIndexStepId.ToString(), stepIndexNextStepId.ToString(),
                                XmlUtil.GetLineTurn3(stepIndexStepId) + " " +
                                XmlUtil.GetLineTurn3(stepIndexNextStepId));
                    }

                WorkFlowChartControl.LoadDocument("WorkFlowChartConfig.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show("打开流程图异常：" + ex.Message);
            }

        }

        public ICommand StartProcessCommand => new RelayCommandImplementation(StartProcess);

        public ObservableCollection<ProcessSearchResultModel> ProcessInfoModels { get; set; } =
            new ObservableCollection<ProcessSearchResultModel>();

        public ProcessSearchResultModel SelectedProcess
        {
            get => _selectedProcess;
            set
            {
                _selectedProcess = value;
                OnPropertyChanged(nameof(SelectedProcess));
            }
        }

        public ObservableCollection<ProcessInstanceModel> ProcessInstanceModels
        {
            get => _processInstanceModels;
            set
            {
                _processInstanceModels = value;
                OnPropertyChanged(nameof(ProcessInstanceModels));
            }
        }

        public List<string> SearDateTimes { get; set; } = new List<string>
        {
            DateTime.Today.ToString("yyyy-M-d"),
            DateTime.Today.AddDays(-1).ToString("yyyy-M-d"),
            DateTime.Today.AddDays(-2).ToString("yyyy-M-d")
        };

        public string SearchDate { get; set; } = DateTime.Today.ToString("yyyy-M-d");

        public event PropertyChangedEventHandler PropertyChanged;

        private void StartProcess(object obj)
        {
            if (SelectedProcess == null) return;

            if (_selectedProcess.ContainerNames.Count <= 0)
            {
                Task.Run(() =>
                {
                    _processUtil.StartProcess(_selectedProcess.ProcessName, new Dictionary<string, string>(),
                        new Dictionary<string, string>());
                });
            }

            /*_startProcessWindow = new StartProcessWindow();
            _startProcessWindow.InitializeProcessAttribute(_selectedProcess, _staticResources, _processUtil);
            _startProcessWindow.Show();*/
        }

        private void QueryProcessRecord(object obj)
        {
            ProcessInstanceRecords.Clear();

            ProcessInstanceRecords =
                new ObservableCollection<ProcessInstanceRecord>(
                    _processUtil.ReadProcessRecords(SelectedProcess?.ProcessName, RecordCounts,
                        Convert.ToDateTime(SearchDate)));
        }

        private void AutoFreshChecked(object obj)
        {
            try
            {
                if (ProcessAutoRefresh)
                    _autoRefreshProcessTimer.Start();
                else
                    _autoRefreshProcessTimer.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ProcessAutoRefreshButton_Click-Execute异常：" + ex.Message);
                Log.Error("ProcessAutoRefreshButton_Click-Execute异常：" + ex.Message);
            }
        }
        
        private void StopAutoFreshProcess()
        {
            try
            {
                ProcessAutoRefresh = false; 
                _autoRefreshProcessTimer.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ProcessAutoRefreshButton_Click-Execute异常：" + ex.Message);
                Log.Error("ProcessAutoRefreshButton_Click-Execute异常：" + ex.Message);
            }
        }
        
        private void AutoFreshProcess(object sender, EventArgs eventArgs)
        {
            QueryProcess();
        }

        private bool AutoRefreshProcessCanExecute(object arg)
        {
            return ProcessInfoModels.Count > 0;
        }

        private void RefreshProcess(object obj)
        {
            ProcessInfoModels.Clear();

            QueryProcess();

            _staticResources = _processUtil.GetStaticResources();
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void QueryProcess()
        {
            try
            {
                lock (_processLocker)
                {
                    var processInfoModels = _processUtil.GetProcessInfoModels();

                    if (processInfoModels == null)
                    {
                        ProcessInfoModels.Clear();
                        ProcessInstanceModels.Clear();
                        WorkFlowChartControl.DeleteItems(WorkFlowChartControl.Items);
                        
                        StopAutoFreshProcess();

                        MessageBox.Show($"从服务器获取Process信息出错或超时，已中止实时刷新Process信息。");
                        return;
                    }

                    foreach (var processInfoModel in processInfoModels)
                    {
                        if (ProcessInfoModels.All(a => a.ProcessName != processInfoModel.ProcessName))
                        {
                            var processSearchResultModel = new ProcessSearchResultModel
                            {
                                Index = ProcessInfoModels.Count + 1, ProcessName = processInfoModel.ProcessName,
                                RunningInstancesNumber = processInfoModel.RunningInstanceNumber,
                                ContainerNames = processInfoModel.ContainerNames,
                                TotalRunningTimes = processInfoModel.TotalRunningTimes,
                                BreakingTimes = processInfoModel.BreakCounts
                            };

                            ProcessInfoModels.Add(processSearchResultModel);

                            continue;
                        }

                        var searchResultModel =
                            ProcessInfoModels.FirstOrDefault(a => a.ProcessName == processInfoModel.ProcessName);

                        Debug.Assert(searchResultModel != null, nameof(searchResultModel) + " != null");
                        searchResultModel.RunningInstancesNumber = processInfoModel.RunningInstanceNumber;
                        searchResultModel.TotalRunningTimes = processInfoModel.TotalRunningTimes;
                        searchResultModel.BreakingTimes = processInfoModel.BreakCounts;
                    }

                    QueryProcessInstance();
                }
            }
            catch (Exception ex)
            {
                Log.Error("ProcessQueryButton_Click-Execute异常：" + ex.Message);
            }
        }

        public void QueryProcessInstance()
        {
            try
            {
                if (SelectedProcess == null) return;

                if (string.IsNullOrEmpty(SelectedProcess.ProcessName))
                    return;

                var processInstanceInfoModels = _processUtil.GetInstanceInfoModels(SelectedProcess.ProcessName);

                if (processInstanceInfoModels == null)
                {
                    ProcessInstanceModels.Clear();
                    WorkFlowChartControl.DeleteItems(WorkFlowChartControl.Items);
                    return;
                }

                if (processInstanceInfoModels.Count==0)
                {
                    ProcessInstanceModels.Clear();
                    VaryFlowChartCurrentStep(-99);
                    return;
                }

                var instanceModelsPid = ProcessInstanceModels
                    .Where(a => processInstanceInfoModels.All(b => b.Index != a.Pid)).Select(a => a.Pid).ToList();

                foreach (var pid in instanceModelsPid)
                    ProcessInstanceModels.Remove(ProcessInstanceModels.FirstOrDefault(a => a.Pid == pid));

                foreach (var processInstanceInfoModel in processInstanceInfoModels)
                {
                    var instanceParameterModels = new List<InstanceParameterModel>();

                    //填充过程实例参数
                    var index = 0;

                    foreach (var a in processInstanceInfoModel.ProcessInstanceParameters)
                    {
                        index++;
                        instanceParameterModels.Add(new InstanceParameterModel
                        {
                            Index = index,
                            Name = a.Name,
                            ValueInString = a.ValueInString,
                            Type = a.Type,
                            Key = a.Key
                        });
                    }

                    var processInstanceModel =
                        ProcessInstanceModels.FirstOrDefault(a => a.Pid == processInstanceInfoModel.Index);

                    if (_selectedInstanceModel!=null&&_flowChartShown&&processInstanceInfoModel.Index==_selectedInstanceModel.Pid)
                    {
                        VaryFlowChartCurrentStep(processInstanceInfoModel.CurrentStepId);
                    }
                    
                    if (processInstanceModel != null)
                    {
                        processInstanceModel.CurrentStep = processInstanceInfoModel.CurrentStep;

                        processInstanceModel.Container = processInstanceInfoModel.Container;

                        processInstanceModel.UpdateParameters(instanceParameterModels);

                        continue;
                    }

                    var instanceModel = new ProcessInstanceModel
                    {
                        Pid = processInstanceInfoModel.Index,
                        CurrentStep = processInstanceInfoModel.CurrentStep,
                        CurrentStepId = processInstanceInfoModel.CurrentStepId,
                        Container = processInstanceInfoModel.Container
                    };
                    
                    instanceModel.InitParameters(instanceParameterModels);

                    ProcessInstanceModels.Add(instanceModel);
                }

                if (_selectedInstanceModel==null&&ProcessInstanceModels!=null&&ProcessInstanceModels.Count>0)
                {
                    SelectedInstanceModel = ProcessInstanceModels[0];
                }

                
            }
            catch (Exception e)
            {
                Log.Error($"查询ProcessInstance失败，异常为:[{e.Message}]");
            }
        }

        private void StopProcessInstance(object obj)
        {
            try
            {
                if (!string.IsNullOrEmpty(SelectedInstanceModel?.Pid))
                    _processUtil.StopProcessInstance(SelectedInstanceModel?.Pid);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ResourceQueryButton_Click-Execute异常：" + ex.Message);
                Log.Error("ResourceQueryButton_Click-Execute异常：" + ex.Message);
            }
        }

        private bool StopProcessInstanceCanExecute(object arg)
        {
            return !string.IsNullOrEmpty(SelectedInstanceModel?.Pid);
        }
    }
}