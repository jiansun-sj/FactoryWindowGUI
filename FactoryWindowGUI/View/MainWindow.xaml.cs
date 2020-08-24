// ==================================================
// FactoryWindowGUI
// 文件名：MainWindow.xaml.cs
// 创建时间：2020/03/04 15:57
// ==================================================
// 最后修改于：2020/08/21 15:57
// 修改人：jians
// ==================================================

using System;
using System.Windows;
using System.Windows.Input;
using FactoryWindowGUI.ViewModel;

//using FactoryWindowGUI.Config.Models;

namespace FactoryWindowGUI.View
{
    public partial class MainWindow
    {
        public static Action MainWindowOnClosed;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindowViewModel MainWindowVm { get; set; } = new MainWindowViewModel();

        private void CloseButtonOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            MainWindowOnClosed?.Invoke();

            Environment.Exit(0);
        }

        private void MaximumButtonOnClick(object sender, RoutedEventArgs e)
        {
            ChangeWindowState();
        }

        private void ChangeWindowState()
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                    WindowState = WindowState.Maximized;
                    break;
                case WindowState.Minimized:
                    break;
                case WindowState.Maximized:
                    WindowState = WindowState.Normal;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HeaderOnDoubleClicked(object sender, MouseButtonEventArgs e)
        {
            ChangeWindowState();
        }

        private void MinimizeButtonOnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void HeaderOnLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}