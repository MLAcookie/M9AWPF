using M9AWPF.ViewModel;
using System;
using System.Collections.Generic;
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

namespace M9AWPF.View
{
    /// <summary>
    /// EasyUI.xaml 的交互逻辑
    /// </summary>
    using VM = M9AWPF.ViewModel.EasyUIViewModel;
    public partial class EasyUI : UserControl
    {
        public EasyUI()
        {
            InitializeComponent();
        }

        private void Delete_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as VM).DeleteTask(((sender as MenuItem).DataContext as BoxedMAATask), TaskList_ItemControl.Items);
        }

        private void MoveDown_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as VM).ItemMoveDown(((sender as MenuItem).DataContext as BoxedMAATask), TaskList_ItemControl.Items);
        }

        private void MoveUp_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as VM).ItemMoveUp(((sender as MenuItem).DataContext as BoxedMAATask), TaskList_ItemControl.Items);
        }
    }
}
