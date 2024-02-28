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
    /// 提供一个带标签的Combobox的模板
    /// </summary>
    public partial class OptionTemplate : UserControl
    {
        /// <summary>
        /// 选项的名字
        /// </summary>
        public string OptionName { get; set; }

        /// <summary>
        /// 所有可能的选项
        /// </summary>
        public string[] OptionValues { get; set; }

        public OptionTemplate(string optionName, string[] optionValues)
        {
            InitializeComponent();
            OptionName = optionName;
            OptionValues = optionValues;
            DataContext = this;
        }
    }
}
