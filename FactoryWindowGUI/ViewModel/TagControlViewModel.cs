// ==================================================
// 文件名：TagControlViewModel.cs
// 创建时间：2020/04/12 13:39
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/07/29 13:39
// 修改人：jians
// ==================================================

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using DevExpress.Xpf.Grid;
using FactoryWindowGUI.Annotations;
using FactoryWindowGUI.Model;
using FactoryWindowGUI.Util;
using FactoryWindowGUI.View;
using log4net;
using RosemaryThemes.Wpf.BaseClass;

namespace FactoryWindowGUI.ViewModel
{
    public sealed class TagControlViewModel : INotifyPropertyChanged
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(TagControlViewModel));

        private readonly DispatcherTimer _dataSourceTimer = new DispatcherTimer
            {Interval = new TimeSpan(0, 0, 0, 1)};


        private readonly Timer _tagQueryTimer = new Timer {Interval = 1000, AutoReset = true};

        private bool _autoRefresh;
        private bool _autoRefreshDataSource;

        private ObservableCollection<MachineDataSourceModel> _machineDataSourceList =
            new ObservableCollection<MachineDataSourceModel>();

        //machine combobox item source
        private ObservableCollection<MachineNameModel> _machineList;

        //machine combobox selected item
        private MachineNameModel _selectedMachineName;

        private TagSearchResultModel _selectedTag;

        //tag combobox selected item
        private TagNameModel _selectedTagName;

        //tag combobox item source
        private ObservableCollection<TagNameModel> _tagList = new ObservableCollection<TagNameModel>();

        public MachineUtil MachineUtil = new MachineUtil();

        public TagControlView TagControlView;


        public TagControlViewModel()
        {
            _tagQueryTimer.Elapsed += AutoRefreshTagQuery;
            _dataSourceTimer.Tick += GetDataSourceModels;
        }

        public ObservableCollection<MachineDataSourceModel> MachineDataSourceList
        {
            get => _machineDataSourceList;
            set
            {
                _machineDataSourceList = value;
                OnPropertyChanged(nameof(MachineDataSourceList));
            }
        }

        //刷新数据源通讯状态
        public ICommand RefreshDataSourceCommand => new RelayCommand(RefreshDataSource);

        public bool AutoRefreshDataSource
        {
            get => _autoRefreshDataSource;
            set
            {
                _autoRefreshDataSource = value;

                if (_autoRefreshDataSource)
                    _dataSourceTimer.Start();
                else
                    _dataSourceTimer.Stop();
                OnPropertyChanged(nameof(AutoRefreshDataSource));
            }
        }

        public TagSearchResultModel SelectedTag
        {
            get => _selectedTag;
            set
            {
                _selectedTag = value;
                OnPropertyChanged(nameof(SelectedTag));
            }
        }

        public ObservableCollection<MachineNameModel> MachineList
        {
            get => _machineList;
            set
            {
                _machineList = value;
                OnPropertyChanged(nameof(MachineList));
            }
        }

        public ObservableCollection<TagNameModel> TagList
        {
            get => _tagList;
            set
            {
                _tagList = value;
                OnPropertyChanged(nameof(TagList));
            }
        }

        public bool AutoRefresh
        {
            get => _autoRefresh;
            set
            {
                _autoRefresh = value;

                _tagQueryTimer.Enabled = _autoRefresh;

                OnPropertyChanged(nameof(AutoRefresh));
            }
        }

        public MachineNameModel SelectedMachineName
        {
            get => _selectedMachineName;
            set
            {
                _selectedMachineName = value;

                //重新获取tag列表
                if (_selectedMachineName != null && !string.IsNullOrEmpty(_selectedMachineName.MachineName))
                {
                    var tagList = MachineUtil.GetTagList(_selectedMachineName.MachineName);

                    if (tagList != null)
                    {
                        _tagList = new ObservableCollection<TagNameModel> {new TagNameModel {Id = 0, TagName = "全部"}};
                        for (var i = 0; i < tagList.Count; i++)
                            _tagList.Add(new TagNameModel {Id = i + 1, TagName = tagList[i]});
                    }
                }

                if (PropertyChanged == null) return;
                OnPropertyChanged(nameof(SelectedMachineName));
                OnPropertyChanged(nameof(TagList));
            }
        }

        public TagNameModel SelectedTagName
        {
            get => _selectedTagName;
            set
            {
                _selectedTagName = value;
                OnPropertyChanged(nameof(SelectedTagName));
            }
        }

        //-------Tag-------------------------------------------------------------------------------
        public ObservableCollection<TagSearchResultModel> QueryResultList { get; set; } =
            new ObservableCollection<TagSearchResultModel>();

        //查询Tag功能
        public ICommand QueryTagCommand =>
            new RelayCommand(QueryMachineTagAsync, QueryTagCommandCanExecute);

        //刷新Machine资源列表
        public ICommand RefreshMachineListCommand => new RelayCommand(RefreshMachineList);

        //清除查询结果
        public ICommand ClearButtonCommand => new RelayCommand(ClearTagSearchResult);

        //从查存列表中移除选中的Tag
        public ICommand RemoveSelectedTagCommand => new RelayCommand(RemoveSelectedTag);

        //提交修改Tag值操作
        public ICommand SubmitTagChangeCommand =>
            new RelayCommand(SubmitTagChange, SubmitTagChangeCommandCanExecute);

        public event PropertyChangedEventHandler PropertyChanged;

        private void QueryMachineTagAsync(object obj)
        {
            if (obj is GridControl gridControl) gridControl.Dispatcher.Invoke(QueryMachineTag);
        }

        public void RefreshDataSource(object o)
        {
            MachineDataSourceList.Clear();

            GetDataSourceModels(null, null);
        }

        public void GetDataSourceModels(object sender, EventArgs eventArgs)
        {
            var machineDataSourceConn = MachineUtil.GetMachineDataSourceConn();

            if (machineDataSourceConn == null) return;

            foreach (var dataSourceConnectionModel in machineDataSourceConn)
            {
                var machineDataSourceModel = MachineDataSourceList.FirstOrDefault(a =>
                    a.MachineName == dataSourceConnectionModel.MachineName &&
                    a.DataSource == dataSourceConnectionModel.Name);

                if (machineDataSourceModel == null)
                    MachineDataSourceList.Add(new MachineDataSourceModel
                    {
                        Id = MachineDataSourceList.Count + 1,
                        MachineName = dataSourceConnectionModel.MachineName,
                        DataSource = dataSourceConnectionModel.Name,
                        DataSourceType = dataSourceConnectionModel.Type,
                        LinkState = dataSourceConnectionModel.Status
                    });

                else
                    machineDataSourceModel.LinkState = dataSourceConnectionModel.Status;
            }
        }

        private ObservableCollection<MachineNameModel> GetMachineList()
        {
            var machineList = MachineUtil.GetMachineList();

            _machineList = new ObservableCollection<MachineNameModel>();
            if (machineList == null) return _machineList;
            for (var i = 0; i < machineList.Count; i++)
                _machineList.Add(new MachineNameModel {Id = i + 1, MachineName = machineList[i]});

            return _machineList;
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void QueryMachineTag()
        {
            try
            {
                if (SelectedTagName.Id == 0)
                {
                    var queryList = new List<string[]>();
                    foreach (var item in TagList)
                    {
                        if (item.TagName == "全部") continue;
                        string[] singleQueryList = {SelectedMachineName.MachineName, item.TagName, "", "",""};
                        queryList.Add(singleQueryList);
                    }

                    var tagAllValue = MachineUtil.SearchTagValue(queryList);

                    if (tagAllValue == null) return;
                    foreach (var t in tagAllValue)
                        QueryResultList.Add(new TagSearchResultModel
                        {
                            Id = QueryResultList.Count + 1,
                            MachineName = t[0],
                            TagName = t[1],
                            TagValue = t[2],
                            IsReadOnly = t[3] != "Read",
                            TagType = t[4] ?? ""
                        });
                }
                else
                {
                    var queryList = new List<string[]>();

                    string[] singleQueryList = {SelectedMachineName.MachineName, SelectedTagName.TagName, "", "",""};
                    queryList.Add(singleQueryList);

                    var tagValue = MachineUtil.SearchTagValue(queryList);
                    //when tag value is not null, add current TagSearchResultModel to corresponding list in view model
                    if (tagValue != null)
                        QueryResultList.Add(new TagSearchResultModel
                        {
                            Id = QueryResultList.Count + 1,
                            MachineName = tagValue[0][0],
                            TagName = tagValue[0][1],
                            TagValue = tagValue[0][2],
                            IsReadOnly = tagValue[0][3] != "Read",
                            TagType = tagValue[0][4] ?? ""
                        });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("QueryButton_Click-Execute异常：" + ex.Message);
                Log.Error("QueryButton_Click-Execute异常：" + ex.Message);
            }
        }

        public void RefreshMachineList(object o)
        {
            MachineList = GetMachineList();

            if (TagList.Count > 0) TagList.Clear();

            QueryResultList.Clear();
        }

        private bool QueryTagCommandCanExecute(object arg)
        {
            if (SelectedMachineName != null && !string.IsNullOrEmpty(SelectedMachineName.MachineName))
                return SelectedTagName != null && !string.IsNullOrEmpty(SelectedTagName.TagName);

            return false;
        }

        private void ClearTagSearchResult(object obj)
        {
            QueryResultList.Clear();
        }

        private void AutoRefreshTagQuery(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (QueryResultList.Count <= 0) return;

            var queryList = new List<string[]>();

            foreach (var item in QueryResultList)
            {
                string[] singleQueryList = {item.MachineName, item.TagName, "", "",""};
                queryList.Add(singleQueryList);
            }

            queryList = MachineUtil.SearchTagValue(queryList);

            if (queryList == null) return;

            for (var j = 0; j < QueryResultList.Count; j++)
                QueryResultList[j].TagValue = queryList[j][2];
        }

        private void RemoveSelectedTag(object obj)
        {
            if (obj == null || !(obj is TagSearchResultModel tagSearchResultModel)) return;

            if (QueryResultList.Contains(tagSearchResultModel))
                QueryResultList.Remove(tagSearchResultModel);
        }

        private void SubmitTagChange(object obj)
        {
            try
            {
                var writeList = new List<string[]>();

                foreach (var item in QueryResultList)
                {
                    if (!item.IsChecked) continue;

                    if (string.IsNullOrEmpty(item.ModifyValue))
                        //MessageBox.Show("异常：输入为空");
                        return;

                    string[] singleQueryList = {item.MachineName, item.TagName, item.ModifyValue};
                    writeList.Add(singleQueryList);
                }

                MachineUtil.WriteTagValue(writeList);
            }
            catch (Exception ex)
            {
                MessageBox.Show("SubmitButton_Click - Execute异常：" + ex.Message);
                Log.Error("SubmitButton_Click-Execute异常：" + ex.Message);
            }
        }

        private bool SubmitTagChangeCommandCanExecute(object arg)
        {
            return QueryResultList.Any(item => item.IsChecked);
        }
    }
}