// ==================================================
// 文件名：InstanceParameterModel.cs
// 创建时间：2020/03/09 13:39
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/07/29 13:39
// 修改人：jians
// ==================================================

using System.ComponentModel;
using System.Runtime.CompilerServices;
using FactoryWindowGUI.Annotations;

namespace FactoryWindowGUI.Model
{
    public class InstanceParameterModel : INotifyPropertyChanged
    {
        private int _index;
        private string _key;
        private string _name;
        private string _type;
        private string _valueInString;

        public int Index
        {
            get => _index;
            set
            {
                _index = value;
                OnPropertyChanged(nameof(Index));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;

                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string ValueInString
        {
            get => _valueInString;
            set
            {
                if (_valueInString == value) return;

                _valueInString = value;
                OnPropertyChanged(nameof(ValueInString));
            }
        }

        public string Type
        {
            get => _type;
            set
            {
                if (_type == value) return;

                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        public string Key
        {
            get => _key;
            set
            {
                if (_key == value) return;

                _key = value;
                OnPropertyChanged(nameof(Key));
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