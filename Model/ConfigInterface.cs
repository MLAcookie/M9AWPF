using M9AWPF.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace M9AWPF.Model;

/// <summary>
/// 应用启动时读取interface文件，指示配置中可选的字段及其value
/// </summary>
public static class ConfigInterface
{
    private static readonly string path = ConfKeys.M9AInterface;

    /// <summary>
	/// option及其所对应的能取的值
	/// </summary>
	public static readonly Dictionary<string, List<string>> option = new();
    public static string M9AVersion { get; }

    /// <summary>
    /// M9A任务，每个任务由名称和一系列选项组成
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

        M9AVersion = json["version"]!.ToString();
    }
}