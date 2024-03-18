namespace M9AWPF.Core.M9AModels;

/// <summary>
/// 定义maa_pi_config.json的内容
/// </summary>
[Serializable]
public class M9AConfigObject
{
    [Serializable]
    public class Controller
    {
        public string AdbPath { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Name { get; set; } = "ADB 默认方式";
    }

    [Serializable]
    public class Option
    {
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }

    [Serializable]
    public class Task
    {
        public string Name { get; set; } = string.Empty;
        public List<Option> option { get; set; } = new List<Option>();
    }

    public Controller controller { get; set; } = new Controller();
    public string Resource { get; set; } = "官服";
    public List<Task> task { get; set; } = new List<Task>(); // 初始化避免为null
}
