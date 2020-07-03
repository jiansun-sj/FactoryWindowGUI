// ==================================================
// 文件名：ParametersResultModel.cs
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
    public class ParametersResultModel : INotifyPropertyChanged
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }


        //inherit inotify property changed
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}