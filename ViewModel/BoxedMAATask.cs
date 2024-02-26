using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using M9AWPF.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace M9AWPF.ViewModel;

/// <summary>
/// 封装的MAA Task
/// </summary>
public class BoxedMAATask
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
	public ConfigObject.Task ToMAATask()
	{
		var res = new ConfigObject.Task();
		res.name = Name;
		for (int i = 0; i < Options.Count; i++)
		{
			var option = new ConfigObject.Option()
			{
				name = Options[i],
				value = OptionVals[i],
			};
			res.option.Add(option);
		}
		return res;
	}
	public static BoxedMAATask FromMAATask(ConfigObject.Task task)
	{
		var res = new BoxedMAATask();
		res.Name = task.name;
		foreach (var item in task.option)
		{
			res.Options.Add(item.name);
			res.OptionVals.Add(item.value);
		}
		return res;
	}
}


