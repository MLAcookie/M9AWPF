using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAA1999WPF.Model
{
    [Serializable]
    class Show
    {
        public string Type { get; set; } = "Combat";
        public string ConfigName { get; set; } = "mm";
        public string UIName { get; set; } = "mm123";
        public List<string> Stages { get; set; } = new List<string>()
        {
            "0001",
        };
    }
    [Serializable]
    class Type
    {
        public string ConfigName { get; set; } = "emmmm";
        public string UIName { get; set; } = "emmmm123";
    }
    [Serializable]
    class MAAItemConfig
    {
        public List<Type> Types { get; set; } = new List<Type>()
        {
            new Type(),
        };
        public List<Show> Shows { get; set; } = new List<Show>()
        {
            new Show(),
        };
        public string Version { get; set; } = "0.01";
    }
}
