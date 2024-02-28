using System.IO;

namespace M9AWPF.Constants;

public static class ConfKeys
{
    // M9A文件夹
    private const string M9ARoot = "./M9A-Bin";

    // interface.json路径
    public static readonly string M9AInterface = Path.Combine(M9ARoot, "./interface.json");

    // M9A Config文件路径
    public static readonly string M9AConfig = Path.Combine(M9ARoot, "./config/maa_pi_config.json");

    // M9A可执行文件路径，适配v1.0.0+
    public static readonly string M9ABin = Path.Combine(M9ARoot, "MaaPiCli.exe");
}
