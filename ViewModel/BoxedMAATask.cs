using CommunityToolkit.Mvvm.ComponentModel;
using MAA1999WPF.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MAA1999WPF.ViewModel
{
    public partial class BoxedMAATask : ObservableObject//把标准对象包装，方便框架调用
    {
        public static readonly MAAItemConfig ItemConfig;
        public static readonly Dictionary<string, bool> TypeIsCombat;
        public static List<string> UITypeName { get; }
        static readonly List<string> ConfigTypeName;
        

        public string name;
        public string type;
        public Diff_Task diff_task;

        [ObservableProperty]
        public bool isCombat = false;
        public List<string>? Shows;
        static BoxedMAATask()
        {
            StreamReader sr = File.OpenText(@".\Test.json");
            ItemConfig = JsonSerializer.Deserialize<MAAItemConfig>(sr.ReadToEnd());
            sr.Close();
            TypeIsCombat = new Dictionary<string, bool>();
            UITypeName = new List<string>();
            ConfigTypeName = new List<string>();
            foreach (var t in ItemConfig.Types)
            {
                TypeIsCombat.Add(t.ConfigName, t.IsCombat);
                UITypeName.Add(t.UIName);
                ConfigTypeName.Add(t.ConfigName);
            }
        }
        public static BoxedMAATask ConvertionFrom(MAATask maaTask)//标准对象转为包装的对象
        {
            BoxedMAATask box = new BoxedMAATask();
            if (TypeIsCombat[maaTask.type])
            {
                box.diff_task = maaTask.param.diff_task;
                box.IsCombat = true;
            }
            else
            {
                box.diff_task = new Diff_Task
                {
                    AllIn = new Allin(),
                    EatCandyWithin24H = new Eatcandywithin24h(),
                    EnterTheShow = new Entertheshow(),
                    TargetStageName = new Targetstagename(),
                    SetReplaysTimes = new Setreplaystimes()
                };
                box.diff_task.SetReplaysTimes.text = "1";
                box.diff_task.StageDifficulty = new Stagedifficulty();
            }
            box.name = maaTask.name;
            box.type = maaTask.type;
            return box;
        }
        public string TaskName
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(nameof(TaskName));
                OnPropertyChanged(nameof(DisplayHeader));
            }
        }
        public string DisplayHeader
        {
            get
            {
                return $"{name} - ({type})";
            }
        }
        public int TaskTypeIndex
        {
            get
            {
                return ConfigTypeName.IndexOf(type);
            }
            set
            {
                type = ConfigTypeName[value];
                OnPropertyChanged(nameof(TaskType));
                OnPropertyChanged(nameof(DisplayHeader));
            }
        }
        public bool AllIn
        {
            get
            {
                return diff_task.AllIn.enabled;
            }
            set
            {
                diff_task.AllIn.enabled = value;
                OnPropertyChanged(nameof(AllIn));
            }
        }
        public bool EatCandyWithin24H
        {
            get
            {
                return diff_task.EatCandyWithin24H.enabled;
            }
            set
            {
                diff_task.EatCandyWithin24H.enabled = value;
                OnPropertyChanged(nameof(EatCandyWithin24H));
            }
        }
        public string EnterTheShow
        {
            get
            {
                return diff_task.EnterTheShow.next;
            }
            set
            {
                diff_task.EnterTheShow.next = value;
                OnPropertyChanged(nameof(EnterTheShow));
            }
        }
        public string TargetStageName
        {
            get
            {
                return diff_task.TargetStageName.text;
            }
            set
            {
                diff_task.TargetStageName.text = value;
                OnPropertyChanged(nameof(TargetStageName));
            }
        }
        public int SetReplaysTimes
        {
            get
            {
                return int.Parse(diff_task.SetReplaysTimes.text);
            }
            set
            {
                if (value > 4)
                {
                    value = 4;
                }
                else if (value < 1)
                {
                    value = 1;
                }
                diff_task.SetReplaysTimes.text = value.ToString();
                Trace.WriteLine(value.ToString());
                OnPropertyChanged(nameof(SetReplaysTimes));
            }
        }
        public bool IsDifficaulty
        {
            get
            {
                if (diff_task.StageDifficulty.next == "StageDifficulty_Hard")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value)
                {
                    diff_task.StageDifficulty.next = "StageDifficulty_Hard";
                }
                else
                {
                    diff_task.StageDifficulty.next = "StageDifficulty_None";
                }
                OnPropertyChanged(nameof(IsDifficaulty));
            }
        }
    }
}
