// ==================================================
// 文件名：ProcessInstanceModel.cs
// 创建时间：2020/03/09 13:39
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
using FactoryWindowGUI.Annotations;

namespace FactoryWindowGUI.Model
{
    public class ProcessInstanceModel : INotifyPropertyChanged
    {
        private string _container;
        private string _currentStep;
        private short _currentStepId;

        private ObservableCollection<InstanceParameterModel> _instanceParameterModels =
            new ObservableCollection<InstanceParameterModel>();

        private string _pid;

        public string Pid
        {
            get => _pid;
            set
            {
                if (_pid == value) return;

                _pid = value;
                OnPropertyChanged(nameof(Pid));
            }
        }

        public string CurrentStep
        {
            get => _currentStep;
            set
            {
                if (_currentStep == value) return;

                _currentStep = value;
                OnPropertyChanged(nameof(CurrentStep));
            }
        }

        public short CurrentStepId
        {
            get => _currentStepId;
            set
            {
                if (_currentStepId == value)
                    return;
                _currentStepId = value;

                OnPropertyChanged(nameof(_currentStepId));
            }
        }

        public string Container
        {
            get => _container;
            set
            {
                if (_container == value) return;

                _container = value;
                OnPropertyChanged(nameof(Container));
            }
        }


        public ObservableCollection<InstanceParameterModel> InstanceParameterModels
        {
            get => _instanceParameterModels;
            set
            {
                _instanceParameterModels = value;
                OnPropertyChanged(nameof(InstanceParameterModels));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void InitParameters(List<InstanceParameterModel> instanceParameterModels)
        {
            InstanceParameterModels = new ObservableCollection<InstanceParameterModel>(instanceParameterModels);
        }

        public void UpdateParameters(List<InstanceParameterModel> instanceParameterModels)
        {
            foreach (var instanceParameterModel in instanceParameterModels)
            {
                var parameterModel = InstanceParameterModels.FirstOrDefault(a =>
                    a.Name == instanceParameterModel.Name && a.Key == instanceParameterModel.Key);

                if (parameterModel != null) parameterModel.ValueInString = instanceParameterModel.ValueInString;
            }
        }
    }
}