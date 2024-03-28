using System.Windows;
using HandyControl.Tools;

namespace M9AWPF.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //往关闭事件添加
            //this.Closing += M9AVersionHelper.CloseUpdate;
        }
    }
}
