// ==================================================
// 文件名：MeasureModel.cs
// 创建时间：2020/05/25 13:39
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/07/29 13:39
// 修改人：jians
// ==================================================

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FactoryWindowGUI.Annotations;

namespace FactoryWindowGUI.ViewModel
{
    public sealed class MeasureModel : INotifyPropertyChanged
    {
        private DateTime _dateTime;
        private double _value;

        public DateTime DateTime
        {
            get => _dateTime;
            set
            {
                _dateTime = value;
                OnPropertyChanged(nameof(DateTime));
            }
        }

        public double Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}