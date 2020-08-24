// ==================================================
// 文件名：ResourceControlView.xaml.cs
// 创建时间：2020/04/12 13:39
// 上海芸浦信息技术有限公司
// copyright@yumpoo
// ==================================================
// 最后修改于：2020/07/29 13:39
// 修改人：jians
// ==================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using FactoryWindowGUI.ViewModel;
using Newtonsoft.Json.Linq;

namespace FactoryWindowGUI.View
{
    /// <summary>
    ///     ResourceControlView.xaml 的交互逻辑
    /// </summary>
    public partial class ResourceControlView
    {
        private const GeneratorStatus Generated = GeneratorStatus.ContainersGenerated;
        private DispatcherTimer _timer;

        public ResourceControlView()
        {
            InitializeComponent();

            ResourceControlVm.RefreshTreeView += RefreshTreeView;
        }


        public ResourceControlViewModel ResourceControlVm { get; set; } = new ResourceControlViewModel();

        private void RefreshTreeView(object sender, EventArgs e)
        {
            Load(ResourceControlVm.ResourceServiceResult);
        }

        public void Load(string json)
        {
            JsonTreeView.ItemsSource = null;
            JsonTreeView.Items.Clear();

            var children = new List<JToken>();

            try
            {
                JToken token = null;

                try
                {
                    token = JToken.Parse(json);
                }
                catch (Exception)
                {
                    // ignored
                }

                if (token != null) children.Add(token);

                JsonTreeView.ItemsSource = children;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not open the JSON string:\r\n" + ex.Message);
            }
        }

        private void JValue_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount != 2)
                return;

            if (sender is TextBlock tb) Clipboard.SetText(tb.Text);
        }

        private void ExpandAll(object sender, RoutedEventArgs e)
        {
            ToggleItems(true);
        }

        private void CollapseAll(object sender, RoutedEventArgs e)
        {
            ToggleItems(false);
        }

        private void ToggleItems(bool isExpanded)
        {
            if (JsonTreeView.Items.IsEmpty)
                return;

            var prevCursor = Cursor;

            Cursor = Cursors.Wait;
            _timer = new DispatcherTimer(TimeSpan.FromMilliseconds(500), DispatcherPriority.Normal, delegate
            {
                ToggleItems(JsonTreeView, JsonTreeView.Items, isExpanded);

                _timer.Stop();
                Cursor = prevCursor;
            }, Application.Current.Dispatcher ?? throw new InvalidOperationException());
            _timer.Start();
        }

        private void ToggleItems(ItemsControl parentContainer, ItemCollection items, bool isExpanded)
        {
            var itemGen = parentContainer.ItemContainerGenerator;
            if (itemGen.Status == Generated)
                Recurse(items, isExpanded, itemGen);
            else
                itemGen.StatusChanged += delegate { Recurse(items, isExpanded, itemGen); };
        }

        private void Recurse(IEnumerable items, bool isExpanded, ItemContainerGenerator itemGen)
        {
            if (itemGen.Status != Generated)
                return;

            foreach (var item in items)
            {
                if (!(itemGen.ContainerFromItem(item) is TreeViewItem tvi))
                    continue;

                tvi.IsExpanded = isExpanded;
                ToggleItems(tvi, tvi.Items, isExpanded);
            }
        }
    }
}