// ==================================================
// 文件名：TagSearchResultModel.cs
// 创建时间：2020/01/04 16:25
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/05/11 16:25
// 修改人：jians
// ==================================================

using System.ComponentModel;

namespace FactoryWindowGUI.Model
{
    /// <summary>
    ///     Tag control view grid control model
    ///     tag 表
    /// </summary>
    public class TagSearchResultModel : INotifyPropertyChanged
    {
        private bool _isReadOnly;

        private string _tagValue;
        public int Id { get; set; }

        public string MachineName { get; set; }

        public string TagName { get; set; }

        public string TagValue
        {
            get => _tagValue;
            set
            {
                if (_tagValue == value) return;
                _tagValue = value;
                OnPropertyChanged("TagValue");
            }
        }

        public string TagType { get; set; }

        public string ModifyValue { get; set; }

        public bool IsChecked { get; set; }

        public bool IsReadOnly
        {
            get => _isReadOnly;
            set
            {
                _isReadOnly = value;
                if (!value) ModifyValue = "不可修改";
            }
        }

        //inherit inotify property changed
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}