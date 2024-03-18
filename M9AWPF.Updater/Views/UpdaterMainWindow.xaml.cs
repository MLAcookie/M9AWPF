using System.Windows;
using M9AWPF.Updater.Models;

namespace M9AWPF.Updater.Views
{
    /// <summary>
    /// Interaction logic for UpdaterMainWindow.xaml
    /// </summary>
    public partial class UpdaterMainWindow : Window
    {
        public UpdaterMainWindow()
        {
            InitializeComponent();
            //往关闭事件添加
            this.Closing += M9AVersionHelper.CloseUpdate;
        }
    }
}
