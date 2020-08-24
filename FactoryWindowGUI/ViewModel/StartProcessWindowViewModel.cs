// ==================================================
// 文件名：StartProcessWindowViewModel.cs
// 创建时间：2020/04/30 13:39
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/07/29 13:39
// 修改人：jians
// ==================================================

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using FactoryWindowGUI.Annotations;
using FactoryWindowGUI.Model;
using FactoryWindowGUI.Util;
using RosemaryThemes.Wpf.BaseClass;

namespace FactoryWindowGUI.ViewModel
{
    public class ParameterContainer : INotifyPropertyChanged
    {
        private string _containerKey;
        private string _containerValue;
        private int _id;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string ContainerKey
        {
            get => _containerKey;
            set
            {
                _containerKey = value;
                OnPropertyChanged(nameof(ContainerKey));
            }
        }

        public string ContainerValue
        {
            get => _containerValue;
            set
            {
                _containerValue = value;
                OnPropertyChanged(nameof(ContainerValue));
            }
        }

        public bool HasValue => !string.IsNullOrEmpty(ContainerValue);


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class StartProcessWindowViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ParameterContainer> _containers = new ObservableCollection<ParameterContainer>();

        private ProcessUtil _processUtil;

        private ObservableCollection<string> _resources = new ObservableCollection<string>
        {
            "TestMachineA",
            "TestMachineB",
            "TestMachineC",
            "TestMachineD"
        };

        public ObservableCollection<ParameterContainer> Containers
        {
            get => _containers;
            set
            {
                _containers = value;
                OnPropertyChanged(nameof(Containers));
            }
        }

        public string ProcessName { get; set; }

        public ObservableCollection<string> Resources
        {
            get => _resources;
            set
            {
                _resources = value;
                OnPropertyChanged(nameof(Resources));
            }
        }

        public ICommand StartProcessCommand => new RelayCommand(StartProcess);

        public event PropertyChangedEventHandler PropertyChanged;

        private void StartProcess(object obj)
        {
            if (Containers.Any(a => !a.HasValue))
            {
                MessageBox.Show("绑定的设备名不能为空。");
                return;
            }

            _processUtil.StartProcess(ProcessName, Containers.ToDictionary(a => a.ContainerKey, b => b.ContainerValue),
                new Dictionary<string, string>());
        }

        public void SetProcessAttribute(ProcessSearchResultModel selectedProcess, List<string> resources,
            ProcessUtil processUtil)
        {
            Resources = new ObservableCollection<string>(resources);

            _processUtil = processUtil;

            var count = 0;

            Containers = new ObservableCollection<ParameterContainer>(selectedProcess.ContainerNames.Select(a =>
                new ParameterContainer
                {
                    Id = ++count,
                    ContainerKey = a,
                    ContainerValue = ""
                }));

            ProcessName = selectedProcess.ProcessName;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}