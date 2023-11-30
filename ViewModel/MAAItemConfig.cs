using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M9AWPF.ViewModel
{
    [Serializable]
    public class Show
    {
        public string ConfigName { get; set; }
        public string UIName { get; set; }
        public List<string> Stages { get; set; }
    }
    [Serializable]
    public class TaskType
    {
        public Dictionary<string, Show>? FromShowNameGetShow;
        public List<string>? AllShowConfigName;
        public List<string>? AllShowUIName;

        public string ConfigName { get; set; }
        public string UIName { get; set; }
        public bool IsCombat { get; set; }
        public List<Show>? Shows { get; set; }

        internal void Init()
        {
            if (Shows is not null)
            {
                FromShowNameGetShow = new Dictionary<string, Show>();
                AllShowConfigName = new List<string>();
                AllShowUIName = new List<string>();
                foreach (var s in Shows)
                {
                    FromShowNameGetShow.Add(s.ConfigName, s);
                    AllShowConfigName.Add(s.ConfigName);
                    AllShowUIName.Add(s.UIName);
                }
            }
        }
    }
    [Serializable]
    public class MAAItemConfig
    {
        public Dictionary<string, TaskType> FromTypeNameGetType;
        public List<string> AllTypeConfigName;
        public List<string> AllTypeUIName;

        public List<TaskType> Types { get; set; }
        public string MAAVersion { get; set; } = "Beta_0.0.1";

        public void Init()
        {
            FromTypeNameGetType = new Dictionary<string, TaskType>();
            AllTypeConfigName = new List<string>();
            AllTypeUIName = new List<string>();
            foreach (var t in Types)
            {
                FromTypeNameGetType.Add(t.ConfigName, t);
                AllTypeConfigName.Add(t.ConfigName);
                AllTypeUIName.Add(t.UIName);
                t.Init();
            }
        }
    }
}
