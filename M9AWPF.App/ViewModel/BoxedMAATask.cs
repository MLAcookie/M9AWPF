using System.Collections.Generic;
using M9AWPF.App.Model;
using M9AWPF.Core.M9AModels;

namespace M9AWPF.App.ViewModel;

/// <summary>
/// 封装的MAA Task
/// </summary>
public class BoxedMaaTask
{
    /// <summary>
    /// 任务名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 任务选项
    /// </summary>
    public List<string> Options { get; set; } = new();

    /// <summary>
    /// 任务选项对应的值
    /// </summary>
    public List<string> OptionVals { get; set; } = new();
    public M9AConfigObject.Task ToMaaTask()
    {
        var res = new M9AConfigObject.Task();
        res.Name = Name;
        for (int i = 0; i < Options.Count; i++)
        {
            var option = new M9AConfigObject.Option()
            {
                Name = Options[i],
                Value = OptionVals[i],
            };
            res.option.Add(option);
        }
        return res;
    }
    public static BoxedMaaTask FromMaaTask(M9AConfigObject.Task task)
    {
        var res = new BoxedMaaTask
        {
            Name = task.Name,
        };
        foreach (var item in task.option)
        {
            res.Options.Add(item.Name);
            res.OptionVals.Add(item.Value);
        }
        return res;
    }
}
