// ==================================================
// 文件名：MachineDataSourceModel.cs
// 创建时间：2020/01/04 13:39
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/07/29 13:39
// 修改人：jians
// ==================================================

using System.ComponentModel;

namespace FactoryWindowGUI.Model
{
    public class MachineDataSourceModel : INotifyPropertyChanged
    {
        private string _dataSource;
        private string _dataSourceType;
        private int _id;
        private string _linkState;
        private string _machineName;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string MachineName
        {
            get => _machineName;
            set
            {
                _machineName = value;
                OnPropertyChanged(nameof(MachineName));
            }
        }

        public string DataSource
        {
            get => _dataSource;
            set
            {
                _dataSource = value;
                OnPropertyChanged(nameof(DataSource));
            }
        }

        public string DataSourceType
        {
            get => _dataSourceType;
            set
            {
                _dataSourceType = value;
                OnPropertyChanged(nameof(DataSourceType));
            }
        }

        public string LinkState
        {
            get => _linkState;

            set
            {
                if (_linkState == value) return;
                _linkState = value;
                OnPropertyChanged("LinkState");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}