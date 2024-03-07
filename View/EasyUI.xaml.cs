using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using M9AWPF.Control;
using M9AWPF.Model;
using M9AWPF.ViewModel;

namespace M9AWPF.View
{
    /// <summary>
    /// EasyUI.xaml 的交互逻辑
    /// </summary>
    //using VM = M9AWPF.ViewModel.EasyUIViewModel;
    public partial class EasyUI : UserControl
    {
        public EasyUI()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 右键菜单删除某个item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var boxedMAATask = ((sender as MenuItem)!.DataContext as BoxedMAATask)!;
            var tasks = EasyUIViewModel.AllMAATasks.ToList();
            for (int i = 0; i < tasks.Count; i++)
            {
                if (tasks[i].Name == boxedMAATask.Name)
                {
                    tasks.RemoveAt(i);
                    break;
                }
            }

            var arr = tasks.ToArray();
            EasyUIViewModel.AllMAATasks = arr;
            var itemsControl = (FindName("TaskList_ItemControl") as ItemsControl)!;
            itemsControl.ItemsSource = arr;
        }

        /// <summary>
        /// 右键菜单下移某个item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveDown_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var boxedMAATask = ((sender as MenuItem)!.DataContext as BoxedMAATask)!;
            var tasks = EasyUIViewModel.AllMAATasks.ToList();
            for (int i = 0; i < tasks.Count; i++)
            {
                if (tasks[i].Name == boxedMAATask.Name)
                {
                    if (i == tasks.Count - 1)
                        return;
                    (tasks[i], tasks[i + 1]) = (tasks[i + 1], tasks[i]);
                    break;
                }
            }

            var arr = tasks.ToArray();
            EasyUIViewModel.AllMAATasks = arr;
            var itemsControl = (FindName("TaskList_ItemControl") as ItemsControl)!;
            itemsControl.ItemsSource = arr;
        }

        /// <summary>
        /// 右键菜单上移某个item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveUp_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var boxedMAATask = ((sender as MenuItem)!.DataContext as BoxedMAATask)!;
            var tasks = EasyUIViewModel.AllMAATasks.ToList();
            for (int i = 0; i < tasks.Count; i++)
            {
                if (tasks[i].Name == boxedMAATask.Name)
                {
                    if (i == 0)
                        return;
                    (tasks[i], tasks[i - 1]) = (tasks[i - 1], tasks[i]);
                    break;
                }
            }

            var arr = tasks.ToArray();
            EasyUIViewModel.AllMAATasks = arr;
            var itemsControl = (FindName("TaskList_ItemControl") as ItemsControl)!;
            itemsControl.ItemsSource = arr;
        }

        /// <summary>
        /// 选择不同的任务后呈现不同的选项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OnTaskNameSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            // 首先清空所有选项
            var stackPanel = (FindName("StackPanel_TaskSettings") as StackPanel)!;
            stackPanel?.Children.Clear();

            // 获取任务类型选择的combobox其所呈现的string（任务类型）
            var str = (sender as ComboBox)!.SelectedValue.ToString()!;

            // 获取该任务类型所对应的所有option的name
            var options = EasyUIViewModel.TaskMap2Option[str]!;
            if (options.Length == 0)
                return;

            // 一旦存在不为零的option，则准备向任务选项的panel里添加相应选择控件
            foreach (var option in options)
            {
                var panel = new StackPanel();
                panel.Orientation = Orientation.Horizontal;
                var opt = new OptionTemplate(option, EasyUIViewModel.OptionMap2Values[option]);
                stackPanel!.Children.Add(opt);
            }
        }

        /// <summary>
        /// 按下add按钮后任务列表呈现出对应的内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            // 找到目前的任务内容，构建任务对象
            var task = new BoxedMAATask();

            // 获取任务名称
            var combobox = (FindName("ComboBox_TaskName") as ComboBox)!;
            if (combobox.SelectedValue == null)
                return;
            task.Name = combobox.SelectedValue.ToString()!;

            // 获取任务选项
            var stackPanel = (FindName("StackPanel_TaskSettings") as StackPanel)!;
            foreach (OptionTemplate opt in stackPanel.Children)
            {
                if (opt.LocalComboBox.SelectedValue == null)
                    return;
                string optionName = opt.OptionName;
                string optionVal = opt.LocalComboBox.SelectedValue.ToString()!;

                task.Options.Add(optionName);
                task.OptionVals.Add(optionVal);
            }

            // 修改任务列表
            EasyUIViewModel.AppendTask(task);

            // 刷新任务列表
            var itemsControl = (FindName("TaskList_ItemControl") as ItemsControl)!;
            itemsControl.ItemsSource = EasyUIViewModel.AllMAATasks;
        }
    }
}
