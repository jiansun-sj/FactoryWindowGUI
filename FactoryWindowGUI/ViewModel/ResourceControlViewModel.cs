// ==================================================
// 文件名：ResourceControlViewModel.cs
// 创建时间：2020/03/04 16:26
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/05/11 16:26
// 修改人：jians
// ==================================================

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using FactoryWindowGUI.Annotations;
using FactoryWindowGUI.ICommandImpl;
using FactoryWindowGUI.Model;
using FactoryWindowGUI.Util;
using log4net;
using Newtonsoft.Json;
using ProcessControlService.Contracts;

namespace FactoryWindowGUI.ViewModel
{
    public class ResourceControlViewModel : INotifyPropertyChanged
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ResourceControlViewModel));
        private ObservableCollection<ResourceNameModel> _resourceList = new ObservableCollection<ResourceNameModel>();

        private string _resourceServiceResult;
        private ResourceNameModel _selectedResourceName;

        //Service listbox selected item
        private ResourceServiceGuiModel _selectedServiceName;

        //Resource combobox item source
        private ObservableCollection<ResourceServiceGuiModel> _serviceList =
            new ObservableCollection<ResourceServiceGuiModel>();

        private ObservableCollection<ServiceParameterModel> _serviceParameterList;

        public ResourceUtil ResourceUtil = new ResourceUtil();

        public ResourceNameModel SelectedResourceName
        {
            get => _selectedResourceName;
            set
            {
                if (_selectedResourceName == value) return;

                _selectedResourceName = value;
                ParametersResultList.Clear();

                ResourceServiceResult = "已刷新";

                QueryResource();
                OnPropertyChanged(nameof(SelectedResourceName));
            }
        }

        public ObservableCollection<ResourceNameModel> ResourceList
        {
            get => _resourceList;
            set
            {
                _resourceList = value;
                OnPropertyChanged(nameof(ResourceList));
            }
        }

        public ResourceServiceGuiModel SelectedServiceName
        {
            get => _selectedServiceName;
            set
            {
                _selectedServiceName = value;

                if (_serviceList != null && _selectedServiceName != null && _selectedServiceName.Name != string.Empty)
                {
                    //获得当前的Service
                    var currentService = _selectedServiceName.Name;

                    ResourceServiceGuiModel guiModel = null;

                    //查找服务
                    foreach (var item in _serviceList)
                        if (item.Name == currentService)
                            guiModel = item;
                    //找到
                    if (guiModel != null)
                    {
                        ParametersResultList = new ObservableCollection<ParametersResultModel>();

                        if (guiModel.ServiceModel.Parameters != null)
                        {
                            var i = 1;
                            foreach (var item in guiModel.ServiceModel.Parameters)
                                ParametersResultList.Add(new ParametersResultModel
                                    {Id = i++, Name = item.Name, Type = item.Type, Value = item.Value});
                        }
                    }

                    OnPropertyChanged(nameof(ParametersResultList));
                }

                OnPropertyChanged(nameof(SelectedServiceName));
            }
        }

        public ObservableCollection<ResourceServiceGuiModel> ServiceList
        {
            get => _serviceList;
            set
            {
                _serviceList = value;
                OnPropertyChanged(nameof(ServiceList));
            }
        }

        public ObservableCollection<ServiceParameterModel> ServiceParameterList
        {
            get
            {
                if (_serviceList == null || _selectedServiceName == null || _selectedServiceName.Name == string.Empty)
                    return _serviceParameterList;
                //获得当前的Service
                var currentService = _selectedServiceName.Name;

                ResourceServiceGuiModel guiModel = null;

                //查找服务
                foreach (var item in _serviceList)
                    if (item.Name == currentService)
                        guiModel = item;
                //找到
                if (guiModel != null)
                    _serviceParameterList =
                        new ObservableCollection<ServiceParameterModel>(guiModel.ServiceModel.Parameters);

                return _serviceParameterList;
            }
            set
            {
                _serviceParameterList = value;
                OnPropertyChanged(nameof(ServiceParameterList));
            }
        }

        public ObservableCollection<ParametersResultModel> ParametersResultList { get; set; } =
            new ObservableCollection<ParametersResultModel>();

        public string ResourceServiceResult
        {
            get => _resourceServiceResult;
            set
            {
                _resourceServiceResult = value;

                OnPropertyChanged(nameof(ResourceServiceResult));

                RefreshTreeView?.Invoke(null, EventArgs.Empty);
            }
        }

        //public ICommand ResourceQueryCommand => new RelayCommandImplementation(QueryResource, QueryResourceCanExecute);

        public ICommand RefreshResourceListCommand => new RelayCommandImplementation(RefreshResourceList);

        public ICommand CallProcessServiceCommand =>
            new RelayCommandImplementation(CallProcessService, CallProcessServiceCanExecute);

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler RefreshTreeView;

        private void RefreshResourceList(object obj)
        {
            ResourceServiceResult = "已刷新";
            ServiceList.Clear();
            ParametersResultList.Clear();
            GetResourceList();
        }

        private void GetResourceList()
        {
            if (!ResourceUtil.Connected)
            {
                MessageBox.Show("连接到服务端失败，请检查与服务端的通信。");
                ResourceList.Clear();
                return;
            }

            var resourceList = ResourceUtil.GetResourceList();

            ResourceList.Clear();
            if (resourceList == null) return;

            for (var i = 0; i < resourceList.Count; i++)
                ResourceList.Add(new ResourceNameModel {Id = i + 1, ResourceName = resourceList[i]});
        }

        private void QueryResource()
        {
            try
            {
                if (SelectedResourceName == null) return;
                var selectedResourceName = SelectedResourceName.ResourceName;

                ServiceList.Clear();

                var resourceServiceGuiModels = ResourceUtil.GetServicesList(selectedResourceName);

                if (resourceServiceGuiModels == null)
                {
                    MessageBox.Show($"未查询到{selectedResourceName}提供的服务列表，请检查该资源是否完善了对应接口。");
                    return;
                }

                ServiceList =
                    new ObservableCollection<ResourceServiceGuiModel>(
                        resourceServiceGuiModels);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ResourceQueryButton_Click-Execute异常：" + ex.Message);
                Log.Error("ResourceQueryButton_Click-Execute异常：" + ex.Message);
            }
        }

        private void CallProcessService(object obj)
        {
            try
            {
                if (SelectedServiceName == null) return;
                var selectedResourceName = SelectedResourceName.ResourceName;
                var selectedServiceName = SelectedServiceName.Name;

                //获得服务需要的参数，参数模型为ResourceServiceModel，jiansun 2019-01-04
                var viewModelParametersResultList = ParametersResultList;

                var parameters = viewModelParametersResultList.Select(parametersResultModel => new ServiceParameterModel
                {
                    Name = parametersResultModel.Name,
                    Type = parametersResultModel.Type,
                    Value = parametersResultModel.Value
                }).ToList();

                ResourceServiceResult = string.Empty;

                var result = ResourceUtil.CallResourceService(selectedResourceName, selectedServiceName,
                    JsonConvert.SerializeObject(parameters));

                ResourceServiceResult = result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("CallProcessServiceButton_Click-Execute异常：" + ex.Message);
                Log.Error("CallProcessServiceButton_Click-Execute异常：" + ex.Message);
            }
        }

        private bool CallProcessServiceCanExecute(object arg)
        {
            return ResourceList != null && SelectedServiceName != null &&
                   !string.IsNullOrEmpty(SelectedServiceName.Name);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}