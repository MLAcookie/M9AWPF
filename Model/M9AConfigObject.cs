using System;
using System.Collections.Generic;

namespace M9AWPF.Model;

/// <summary>
/// 定义maa_pi_config.json的内容
/// </summary>
[Serializable]
public class M9AConfigObject
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
