namespace M9AWPF.Core.Constants;

public static class ConfKeys
{
    // M9A文件夹
    public const string M9ARoot = "./M9A-Bin";

    // interface.json路径
    public static readonly string M9AInterface = Path.Combine(M9ARoot, "./interface.json");

    // M9A Config文件路径
    public static readonly string M9AConfig = Path.Combine(M9ARoot, "./config/maa_pi_config.json");

    // M9A可执行文件路径，适配v1.0.0+
    public static readonly string M9ABin = Path.Combine(M9ARoot, "./MaaPiCli.exe");
    
    // maa_toolkit路径
    public static readonly string M9AToolkit = Path.Combine(M9ARoot, "./config/maa_toolkit.json");
    
    //下载临时存放文件夹
    public static readonly string TempDownload = "./temp";

    //临时存放解压文件
    public static readonly string TempLatest = Path.Combine(TempDownload, "latest");

    //临时备份config
    public static readonly string TempConfigBackup = Path.Combine(TempDownload, "config");
    
}
