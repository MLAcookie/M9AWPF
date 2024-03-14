using System.Diagnostics;
using M9AWPF.App.Constants;

namespace M9AWPF.App.Model;

public class ConsoleBehavior
{
	static readonly string M9APath = ConfKeys.M9ABin;
	private readonly Process _m9A = new Process
    {
		StartInfo = new ProcessStartInfo
		{
			FileName = M9APath,
			RedirectStandardInput = true,
			RedirectStandardOutput = false,
			CreateNoWindow = false,
			UseShellExecute = false,
			Arguments = "-d", // 使MAA不进行交互，直接运行
		}
	};

	public void Start()
	{
		_m9A.Start();
		_m9A.WaitForExit();
		_m9A.Close();
	}
}
