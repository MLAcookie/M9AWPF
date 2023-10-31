using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using MAA1999WPF.Model;
using MAA1999WPF.ViewModel;
using ModernWpf.Controls;

namespace MAA1999WPF.View
{
    /// <summary>
    /// SettingPage.xaml 的交互逻辑
    /// </summary>
    using VM = SettingPageViewModel;
    public partial class SettingPage : UserControl
    {
        public SettingPage()
        {
            InitializeComponent();
        }

        private void TaskList_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex >= 5)
            {
                ((sender as ComboBox).DataContext as BoxedMAATask).IsCombat = true;
            }
            else
            {
                ((sender as ComboBox).DataContext as BoxedMAATask).IsCombat = false;
            }
        }
        private void AddTask_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex >= 5)
            {
                AllIn_Grid.Visibility = Visibility.Visible;
                EatCandyWithin24H_Grid.Visibility = Visibility.Visible;
                EnterTheShow_Grid.Visibility = Visibility.Visible;
                TargetStageName_Grid.Visibility = Visibility.Visible;
                IsDifficaulty_Grid.Visibility = Visibility.Visible;
                SetReplaysTimes_Grid.Visibility = Visibility.Visible;
            }
            else
            {
                if (AllIn_Grid is not null)
                {
                    AllIn_Grid.Visibility = Visibility.Collapsed;
                    EatCandyWithin24H_Grid.Visibility = Visibility.Collapsed;
                    EnterTheShow_Grid.Visibility = Visibility.Collapsed;
                    TargetStageName_Grid.Visibility = Visibility.Collapsed;
                    IsDifficaulty_Grid.Visibility = Visibility.Collapsed;
                    SetReplaysTimes_Grid.Visibility = Visibility.Collapsed;
                }
            }
            NewTaskName_TextBox.Text = $"{((MAATaskType)((ComboBox)sender).SelectedIndex)}Task";
        }

        private void AddTask_Button_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as VM).AddTask((MAATaskType)AddTask_ComboBox.SelectedIndex,
                AllIn_CheckBox.IsChecked ?? false,
                EatCandyWithin24H_CheckBox.IsChecked ?? false,
                EnterTheShow_AutoSuggestBox.Text,
                TargetStageName_AutoSuggestBox.Text,
                IsDifficaulty_CheckBox.IsChecked ?? false,
                (int)SetReplaysTimes_NumberBox.Value,
                NewTaskName_TextBox.Text);
            AllTask_ItemsControl.Items.Refresh();
        }
        private void MoveUp_Button_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as VM).ItemMoveUp((sender as Button).DataContext as BoxedMAATask);
            AllTask_ItemsControl.Items.Refresh();
        }
        private void MoveDown_Button_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as VM).ItemMoveDown((sender as Button).DataContext as BoxedMAATask);
            AllTask_ItemsControl.Items.Refresh();
        }
        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as VM).ItemDelete((sender as Button).DataContext as BoxedMAATask);
            AllTask_ItemsControl.Items.Refresh();
        }
        private void ChooseShowNewTask_AutoSuggestBox_TextChanged(ModernWpf.Controls.AutoSuggestBox sender, ModernWpf.Controls.AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                sender.ItemsSource = VM.GetAllShow((MAATaskType)AddTask_ComboBox.SelectedIndex)
               .Where(t => t.Contains(sender.Text, StringComparison.OrdinalIgnoreCase))
               .ToList();
            }
        }
        private void ChooseShow_AutoSuggestBox_TextChanged(ModernWpf.Controls.AutoSuggestBox sender, ModernWpf.Controls.AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                sender.ItemsSource = VM.GetAllShow((sender.DataContext as BoxedMAATask).type)
               .Where(t => t.Contains(sender.Text, StringComparison.OrdinalIgnoreCase))
               .ToList();
            }
        }
        private void ChooseStageNewTask_AutoSuggestBox_TextChanged(ModernWpf.Controls.AutoSuggestBox sender, ModernWpf.Controls.AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                sender.ItemsSource = VM.GetAllStage(EnterTheShow_AutoSuggestBox.Text)
                    .Where(t => t.Contains(sender.Text, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }
        private void ChooseStage_AutoSuggestBox_TextChanged(ModernWpf.Controls.AutoSuggestBox sender, ModernWpf.Controls.AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                sender.ItemsSource = VM.GetAllStage((sender.DataContext as BoxedMAATask).EnterTheShow)
                    .Where(t => t.Contains(sender.Text, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }
    }
}
