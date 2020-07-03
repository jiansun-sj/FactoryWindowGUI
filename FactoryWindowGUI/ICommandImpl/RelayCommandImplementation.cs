// ==================================================
// 文件名：RelayCommandImplementation.cs
// 创建时间：2020/03/03 16:26
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/05/11 16:26
// 修改人：jians
// ==================================================

using System;
using System.Windows.Input;

namespace FactoryWindowGUI.ICommandImpl
{
    /// <summary>
    ///     RelayCommand
    /// </summary>
    public class RelayCommandImplementation : ICommand
    {
        private readonly Func<object, bool> _canExecute;
        private readonly Action<object> _execute;

        public RelayCommandImplementation(Action<object> execute) : this(execute, null)
        {
        }

        public RelayCommandImplementation(Action<object> execute, Func<object, bool> canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));

            _canExecute = canExecute ?? (x => true);
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;

            remove => CommandManager.RequerySuggested -= value;
        }

        public void Refresh()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}