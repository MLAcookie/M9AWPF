using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MaaPiAvaGui.Views;

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
