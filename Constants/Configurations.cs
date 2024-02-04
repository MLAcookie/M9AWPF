using System.IO;

namespace M9AWPF.Constants;

public static class Configurations
{
    public const string M9ARoot = "./M9A-Bin";
    public static readonly string M9AConfig = Path.Combine(M9ARoot, "config.json");
    public static readonly string M9AConfigDefault = Path.Combine("./Resource/default-config.json");
    public static readonly string M9ABin = Path.Combine(M9ARoot, "M9A_CLI.exe");

    /// <summary>
    /// 管理config的类，操纵的是从config文件反序列化得来的对象
    /// </summary>
    public enum Client
    {
        Official = 1,
        Bilibili = 2,
    }
}
