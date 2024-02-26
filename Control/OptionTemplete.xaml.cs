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

namespace M9AWPF.Control
{
    /// <summary>
    /// OptionTemplete.xaml 的交互逻辑
    /// </summary>
    public partial class OptionTemplete : UserControl
    {
        public string OptionName { get; set; }
        public string[] OptionValues { get; set; }
        public OptionTemplete(string optionName, string[] optionValues)
        {
            InitializeComponent();
            OptionName = optionName;
            OptionValues = optionValues;
            DataContext = this;
        }
    }
}
