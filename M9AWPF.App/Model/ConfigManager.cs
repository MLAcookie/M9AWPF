using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using M9AWPF.App.Constants;

namespace M9AWPF.App.Model;

/// <summary>
/// 管理全局config的静态类，提供读取和写入maa的config的功能
/// </summary>
public static class ConfigManager
{
	/// <summary>
	/// config 路径
	/// </summary>
	private static readonly string Path = ConfKeys.M9AConfig;

	/// <summary>
	/// config对象（用于序列化和反序列化）
	/// </summary>
	private static readonly M9AConfigObject ConfigObject = new M9AConfigObject();

	/// <summary>
	/// 按路径初始化
	/// </summary>
	static ConfigManager()
	{
		// 不存在目录则创建目录
		var dir = System.IO.Path.GetDirectoryName(Path)!;
		if (!Directory.Exists(dir))
		{
			Directory.CreateDirectory(dir);
		}

		// 存在文件则打开，没有文件则创建文件
		if (File.Exists(Path))
		{
			StreamReader sr = File.OpenText(Path);
			string jsonString = sr.ReadToEnd();
			ConfigObject = JsonSerializer.Deserialize<M9AConfigObject>(jsonString)!;
			sr.Close();
		}
		else
		{
			// 创建一个新文件
			var sr = File.Open(Path, FileMode.CreateNew);
			sr.Close();

			// 将json object默认内容写入文件
			var jsonString = JsonSerializer.Serialize(ConfigObject, new JsonSerializerOptions()
			{
				Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
			});
			File.WriteAllText(Path, jsonString, new UTF8Encoding(false));
		}
	}

	/// <summary>
	/// 将当前config json object写入到配置文件中
	/// </summary>
	public static void SaveConfig()
	{
		var jsonString = JsonSerializer.Serialize(ConfigObject, new JsonSerializerOptions()
		{
			Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
		});
		File.WriteAllText(Path, jsonString, new UTF8Encoding(false));
	}

	public static string AdbAddress
	{
		get { return ConfigObject.controller.Address; }
		set { ConfigObject.controller.Address = value; }
	}

	public static string Client
	{
		get { return ConfigObject.Resource; }
		set { ConfigObject.Resource = value; }
	}

	public static string AdbPath // 修改ADB路径
	{
		get { return ConfigObject.controller.AdbPath; }
		set { ConfigObject.controller.AdbPath = value; }
	}

	/// <summary>
	/// 端口。当端口内容无效时返回-1
	/// </summary>
	public static int AdbPort
	{
		get
		{
			var addr = AdbAddress;
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
			if (AdbAddress == null || AdbAddress == string.Empty) AdbAddress = $"127.0.0.1:{value}";

			// adb address存在值但找不到分隔符，则添加到最后
			int idx = AdbAddress.IndexOf(":");
			if (idx == -1) AdbAddress = $"{AdbAddress}:{value}";

			// 满足所有符合的要求
			AdbAddress = $"{AdbAddress[..idx]}:{value}";
		}
	}

	/// <summary>
	/// 获取所有的任务
	/// </summary>
	public static M9AConfigObject.Task[] AllMaaTasks
	{
		get { return ConfigObject.task.ToArray(); }
		set { ConfigObject.task = value.ToList(); }
	}
}
