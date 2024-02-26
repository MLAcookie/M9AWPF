using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace M9AWPF.Model;


/// <summary>
/// 管理全局config的静态类，提供读取和写入maa的config的功能
/// </summary>
public static class ConfigManager
{
	/// <summary>
	/// config 路径
	/// </summary>
	private const string path = @"./M9A-Bin/config/maa_pi_config.json";

	/// <summary>
	/// config对象（用于序列化和反序列化）
	/// </summary>
	private static readonly ConfigObject configObject = new();



	/// <summary>
	/// 按路径初始化
	/// </summary>
	static ConfigManager()
	{
		// 不存在目录则创建目录
		var dir = System.IO.Path.GetDirectoryName(path)!;
		if (!Directory.Exists(dir))
		{
			Directory.CreateDirectory(dir);
		}

		// 存在文件则打开，没有文件则创建文件
		if (File.Exists(path))
		{
			StreamReader sr = File.OpenText(path);
			string jsonString = sr.ReadToEnd();
			configObject = JsonSerializer.Deserialize<ConfigObject>(jsonString)!;
			sr.Close();
		}
		else
		{
			// 创建一个新文件
			var sr = File.Open(path, FileMode.CreateNew);
			sr.Close();

			// 将json object默认内容写入文件
			var jsonString = JsonSerializer.Serialize(configObject, new JsonSerializerOptions()
			{
				Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
			});
			//jsonString = jsonString.Replace("null", "{}"); // 因为已经全部初始化，则不会出现null的情况
			File.WriteAllText(path, jsonString, new UTF8Encoding(false));
		}
	}

	/// <summary>
	/// 将当前config json object写入到配置文件中
	/// </summary>
	public static void SaveConfig()
	{
		var jsonString = JsonSerializer.Serialize(configObject, new JsonSerializerOptions()
		{
			Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
		});
		//jsonString = jsonString.Replace("null", "{}");
		File.WriteAllText(path, jsonString, new UTF8Encoding(false));
	}

	public static string ADBAddress
	{
		get { return configObject.controller.address; }
		set { configObject.controller.address = value; }
	}

	public static string Client
	{
		get { return configObject.resource; }
		set { configObject.resource = value; }
	}

	public static string ADBPath // 修改ADB路径
	{
		get { return configObject.controller.adb_path; }
		set { configObject.controller.adb_path = value; }
	}

	/// <summary>
	/// 端口。当端口内容无效时返回-1
	/// </summary>
	public static int ADBPort
	{
		get
		{
			var addr = ADBAddress;
			if (addr == null || addr == string.Empty) return -1; // 判断地址特殊情况

			int idx = addr.IndexOf(":");
			if (idx == -1) return -1; // 判断能否找到 : 这个分隔符

			var str_port = addr[(idx + 1)..];
			if (int.TryParse(str_port, out int res)) return res; // 判断分隔符后面的内容是否能够转换为数字
			else return -1;
		}
		set
		{
			// value不符合要求，走
			if (value <= 0 || value > 65535) return;

			// adb address为特殊情况，则重置IP
			if (ADBAddress == null || ADBAddress == string.Empty) ADBAddress = $"127.0.0.1:{value}";

			// adb address存在值但找不到分隔符，则添加到最后
			int idx = ADBAddress.IndexOf(":");
			if (idx == -1) ADBAddress = $"{ADBAddress}:{value}";

			// 满足所有符合的要求
			ADBAddress = $"{ADBAddress[..idx]}:{value}";
		}
	}

	/// <summary>
	/// 获取所有的任务
	/// </summary>
	public static ConfigObject.Task[] AllMAATasks
	{
		get { return configObject.task.ToArray(); }
		set { configObject.task = value.ToList(); }
	}
}
