using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAA1999WPF.ViewModel
{
    [Serializable]
    public class Show
    {
        public string Type { get; set; }
        public string ConfigName { get; set; }
        public string UIName { get; set; }
        public List<string> Stages { get; set; }
    }
    [Serializable]
    public class TaskType
    {
        public string ConfigName { get; set; }
        public string UIName { get; set; }
        public bool IsCombat { get; set; }
    }
    [Serializable]
    public class MAAItemConfig
    {
        public List<TaskType> Types { get; set; }
        public List<Show> Shows { get; set; }
        public string Version { get; set; } = "0.01";
    }
}
