using System;
using System.Collections.Generic;
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

namespace MAA1999WPF.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, UserControl> Pages = new Dictionary<string, UserControl>();
        public MainWindow()
        {
            InitializeComponent();
            Pages.Add("HomePage",new HomePage());
            Pages.Add("SettingPage",new SettingPage());
            MainPage.Content = Pages["HomePage"];
        }
        private void NavigationView_ItemInvoked(ModernWpf.Controls.NavigationView sender, ModernWpf.Controls.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                MainPage.Content = Pages["SettingPage"];
            }
            else
            {
                MainPage.Content = Pages[args.InvokedItemContainer.Tag.ToString()];
            }
        }
    }
}
