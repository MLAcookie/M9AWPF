using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace M9AWPF.Model;

/// <summary>
/// 应用启动时读取interface文件，指示配置中可选的字段及其value
/// </summary>
public static class ConfigInterface
{
	private const string path = @"./M9A-Bin/interface.json";

	/// <summary>
	/// option及其所对应的能取的值
	/// </summary>
	public static readonly Dictionary<string, List<string>> option = new();

	/// <summary>
	/// 任务，每个任务由名称和一系列选项组成
	/// </summary>
	public class Task
	{
		/// <summary>
		/// 任务名
		/// </summary>
		public string name = string.Empty;

		/// <summary>
		/// 任务选项
		/// </summary>
		public List<string> option = new();
	}

	/// <summary>
	/// 指示任务，具体内容见Task
	/// </summary>
	public static readonly List<Task> task = new();

	/// <summary>
	/// 指示玩的是哪个服的游戏
	/// </summary>
	public static readonly List<string> resource = new();

	static ConfigInterface()
	{
		string jsonstring = File.ReadAllText(path);
		var json = JsonNode.Parse(jsonstring)!;

		// 获取所有options
		var option_obj = json["option"]!.AsObject();
		foreach (var item in option_obj)
		{
			List<string> vals = new();
			var arr = item.Value!["cases"]!.AsArray();
			foreach (var val in arr)
			{
				vals.Add(val!["name"]!.ToString());
			}

			option.Add(item.Key, vals);
		}

		// 获取所有task
		var tasks = json["task"]!.AsArray();
		foreach (var item in tasks)
		{
			var task_new = new Task()
			{
				name = item!["name"]!.ToString(),
			};

			var option = item!["option"];
			if (option != null)
			{
				var tmp = option.AsArray();
				foreach (var tmp_it in tmp)
				{
					if (tmp_it == null) continue;
					task_new.option.Add(tmp_it!.ToString());
				}
			}

			task.Add(task_new);
		}

		// 获取所有服务器
		var resources = json["resource"]!.AsArray()!;
		foreach (var item in resources)
		{
			resource.Add(item!["name"]!.ToString());
		}
	}
}

/// <summary>
/// 该类指示MAA config json文件内容
/// </summary>
[Serializable]
public class ConfigObject
{
	[Serializable]
	public class Controller
	{
		public string adb_path { get; set; } = string.Empty;
		public string address { get; set; } = string.Empty;
		public string name { get; set; } = "ADB 默认方式";
	}

	[Serializable]
	public class Option
	{
		public string name { get; set; } = string.Empty;
		public string value { get; set; } = string.Empty;
	}

	[Serializable]
	public class Task
	{
		public string name { get; set; } = string.Empty;
		public List<Option> option { get; set; } = new();
	}

	public Controller controller { get; set; } = new();
	public string resource { get; set; } = "官服";
	public List<Task> task { get; set; } = new(); // 初始化避免为null
}
