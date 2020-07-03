// ==================================================
// 文件名：SystemControlViewModel.cs
// 创建时间：2020/03/04 22:00
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/05/25 22:00
// 修改人：jians
// ==================================================

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using FactoryWindowGUI.Annotations;
using FactoryWindowGUI.ICommandImpl;
using FactoryWindowGUI.Util;
using LiveCharts;
using LiveCharts.Configurations;
using log4net;
using Timer = System.Timers.Timer;

namespace FactoryWindowGUI.ViewModel
{
    public sealed class SystemControlViewModel : INotifyPropertyChanged
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SystemControlViewModel));
        private readonly Timer _systemRefreshTimer = new Timer {Interval = 2000, AutoReset = true};

        private double _axisMax;
        private double _axisMin;
        private ChartValues<MeasureModel> _chartValues;

        //machine combobox  source

        //refresh switch button view model
        private bool _systemAutoRefresh;

        private static readonly ResourceUtil ResourceUtil=new ResourceUtil();


        private ChartValues<MeasureModel> _ramCounts=new ChartValues<MeasureModel>
        {
            /*Only for test*/
            new MeasureModel{DateTime = new DateTime(1,1,1,11,10,0),Value = 1},
            new MeasureModel{DateTime = new DateTime(1,1,1,11,30,0),Value = 5},
            new MeasureModel{DateTime = new DateTime(1,1,1,11,50,0),Value = 3},
            new MeasureModel{DateTime = new DateTime(1,1,1,12,0,0),Value = 2},
            new MeasureModel{DateTime = new DateTime(1,1,1,12,30,0),Value = 2},
        };

        private ChartValues<MeasureModel> _cpuCounts=new ChartValues<MeasureModel>
        {
            new MeasureModel{DateTime = new DateTime(1,1,1,11,10,0),Value = 10},
            new MeasureModel{DateTime = new DateTime(1,1,1,11,30,0),Value = 50},
            new MeasureModel{DateTime = new DateTime(1,1,1,11,50,0),Value = 30},
            new MeasureModel{DateTime = new DateTime(1,1,1,12,0,0),Value = 20},
            new MeasureModel{DateTime = new DateTime(1,1,1,12,30,0),Value = 20},
        };

        private DateTime _selectedDate=DateTime.Today;

        public SystemControlViewModel()
        {
            //_systemRefreshTimer.Elapsed += RefreshDataSource;

            var mapper = Mappers.Xy<MeasureModel>()
                .X(model => model.DateTime.Ticks) //use DateTime.Ticks as X
                .Y(model => model.Value); //use the value property as Y
            //lets save the mapper globally.
            Charting.For<MeasureModel>(mapper);

            //the values property will store our values array
            //ChartValues = new ChartValues<MeasureModel>();

            //lets set how to display the X Labels
            DateTimeFormatter = value => new DateTime((long) value).ToString("HH:mm");

            //AxisStep forces the distance between each separator in the X axis
            //AxisUnit forces lets the axis know that we are plotting seconds
            //this is not always necessary, but it can prevent wrong labeling
            AxisUnit = TimeSpan.TicksPerSecond;

            SetAxisLimits();
        }

        public ChartValues<MeasureModel> RamCounts
        {
            get => _ramCounts;
            set
            {
                _ramCounts = value;
                OnPropertyChanged(nameof(RamCounts));
            }
        }

        public ChartValues<MeasureModel> CpuCounts
        {
            get => _cpuCounts;
            set
            {
                _cpuCounts = value;
                OnPropertyChanged(nameof(CpuCounts));
            }
        }

        public Func<double, string> DateTimeFormatter { get; }

        public double AxisUnit { get; }

        public double AxisMax
        {
            get => _axisMax;
            set
            {
                _axisMax = value;
                OnPropertyChanged(nameof(AxisMax));
            }
        }

        public double AxisMin
        {
            get => _axisMin;
            set
            {
                _axisMin = value;
                OnPropertyChanged(nameof(AxisMin));
            }
        }

        public ICommand RedundancyChangeModeCommand =>
            new RelayCommandImplementation(ChangeRedundancyMode, ChangeRedundancyModeCanExecute);

        public ICommand RefreshMemoryAndCpuUsageCommand =>
            new RelayCommandImplementation(RefreshMemoryAndCpuUsage);

        private void RefreshMemoryAndCpuUsage(object obj)
        {
            GetProcessRamAndCpuUsage();
        }

        public bool SystemAutoRefresh
        {
            get => _systemAutoRefresh;
            set
            {
                _systemAutoRefresh = value;
                OnPropertyChanged(nameof(SystemAutoRefresh));
            }
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void GetProcessRamAndCpuUsage()
        {
            try
            {
                var ramAndCpuData = ResourceUtil.GetMemoryAndCpuData(SelectedDate);

                if (ramAndCpuData==null)
                    return;

                RamCounts.Clear();
                CpuCounts.Clear();

                foreach (var memoryAndCpuData in ramAndCpuData)
                {
                    var convertIndexToDateTime = ConvertIndexToDateTime(memoryAndCpuData.RecordTimeIndex);

                    if (convertIndexToDateTime==null)
                        continue;

                    var dateTime = (DateTime) convertIndexToDateTime;
                    
                    RamCounts.Add(new MeasureModel {DateTime = dateTime,Value = memoryAndCpuData.Memory});

                    CpuCounts.Add(new MeasureModel { DateTime = dateTime, Value = memoryAndCpuData.CpuUsage });
                }
            }
            catch (Exception e)
            {
                Log.Error($"查询系统使用内存和CPU失败，异常为：[{e.Message}]");
            }
        }

        private void SetAxisLimits()
        {
            AxisMax =TimeSpan.FromHours(24).Ticks /*now.Ticks + TimeSpan.FromMinutes(10).Ticks*/; // lets force the axis to be 1 second ahead
            AxisMin = 0; // and 8 seconds behind
        }

        private void RefreshDataSource(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            _systemRefreshTimer.Enabled = SystemAutoRefresh;
        }

        private void ChangeRedundancyMode(object obj)
        {
            try
            {
/*
                _adminUtil.ToggleRedundancyMode();
*/
            }
            catch (Exception ex)
            {
                MessageBox.Show("RedundancyToggleButton_click异常：" + ex.Message);
                Log.Error("RedundancyToggleButton_click异常：" + ex.Message);
            }
        }

        public DateTime? ConvertIndexToDateTime(int index)
        {
            try
            {
                if (index<0||index>143)
                    throw new ArgumentOutOfRangeException($"时间转换失败，时间转换序列范围为0～143.");

                return new DateTime(1, 1, 1, index*10/60, index*10%60, 0);
            }
            catch (Exception e)
            {
               Log.Error($"FW传递时间Index转化失败，异常为：[{e.Message}]");
               return null;
            }
        }

        private bool ChangeRedundancyModeCanExecute(object arg)
        {
/*
            return _adminUtil != null && SystemAutoRefresh;
*/
            return false;
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}