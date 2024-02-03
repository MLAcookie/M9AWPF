using System.IO;

namespace M9AWPF.Constants;

public static class Configurations
{
    public const string M9ARoot = "./M9A-Bin";
    public static string M9AConfig = Path.Combine(M9ARoot, "config.json");
    public static string M9ABin = Path.Combine(M9ARoot, "M9A_CLI.exe");
}
