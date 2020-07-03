using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FactoryWindowGUI.Annotations;

namespace FactoryWindowGUI.ViewModel
{
    public sealed class MeasureModel:INotifyPropertyChanged
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
