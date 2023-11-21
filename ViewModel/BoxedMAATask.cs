using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        public static readonly Dictionary<string, bool> TypeWhichIsCombat;

        public string name;
        public TaskType type;
        [ObservableProperty]
        public bool allIn;
        [ObservableProperty]
        public bool eatCandyWithin24H;
        public Show? show;
        public string? stage;
        public int setReplaysTimes;
        public string difficulty = "StageDifficulty_None";

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
            }
        }
        public static List<string> AllTypes
        {
            get
            {
                return ItemConfig.AllTypeUIName;
            }
        }
        public List<string>? AllShows
        {
            get
            {
                return type.AllShowUIName;
            }
        }
        public List<string>? AllStages
        {
            get
            {
                return show?.Stages;
            }
        }
        public int TypeIndex
        {
            get
            {
                return ItemConfig.Types.IndexOf(type);
            }
            set
            {
                type = ItemConfig.Types[value];
                OnPropertyChanged(nameof(AllShows));
                OnPropertyChanged(nameof(ShowIndex));
                OnPropertyChanged(nameof(IsCombat));
            }
        }
        public int ShowIndex
        {
            get
            {
                return (type.Shows is null) ? 0 : type.Shows.IndexOf(show);
            }
            set
            {
                if (value >= 0)
                {
                    show = type.Shows?[value];
                }
                OnPropertyChanged(nameof(AllStages));
                OnPropertyChanged(nameof(StageIndex));
            }
        }
        public int StageIndex
        {
            get
            {
                return (show is null) ? 0 : show.Stages.IndexOf(stage);
            }
            set
            {
                stage = show!.Stages[value - 1];
            }
        }
        public int SetReplaysTimes
        {
            get
            {
                return setReplaysTimes;
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
                setReplaysTimes = value;
                OnPropertyChanged(nameof(SetReplaysTimes));
            }
        }
        public bool IsDifficaulty
        {
            get
            {
                if (difficulty == "StageDifficulty_Hard")
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
                    difficulty = "StageDifficulty_Hard";
                }
                else
                {
                    difficulty = "StageDifficulty_None";
                }
                OnPropertyChanged(nameof(IsDifficaulty));
            }
        }
        public bool IsCombat
        {
            get
            {
                return type.IsCombat;
            }
        }

        static BoxedMAATask()
        {
            StreamReader sr = File.OpenText(@".\ItemsConfig.json");
            ItemConfig = JsonSerializer.Deserialize<MAAItemConfig>(sr.ReadToEnd());
            sr.Close();
            ItemConfig!.Init();
            TypeWhichIsCombat = new Dictionary<string, bool>();
            foreach (var t in ItemConfig.Types)
            {
                TypeWhichIsCombat.Add(t.ConfigName, t.IsCombat);
            }
        }
        public static BoxedMAATask ConvertionFrom(MAATask maaTask)//标准对象转为包装的对象
        {
            BoxedMAATask box = new()
            {
                name = maaTask.name,
                type = ItemConfig.FromTypeNameGetType![maaTask.type],
            };
            if (TypeWhichIsCombat[maaTask.type])
            {
                box.allIn = maaTask.param.diff_task!.AllIn.enabled;
                box.eatCandyWithin24H = maaTask.param.diff_task!.EatCandyWithin24H.enabled;
                box.show = box.type.FromShowNameGetShow![maaTask.param.diff_task.EnterTheShow.next];
                box.stage = maaTask.param.diff_task.TargetStageName.text;
                box.setReplaysTimes = int.Parse(maaTask.param.diff_task!.SetReplaysTimes.text);
                box.difficulty = maaTask.param.diff_task.StageDifficulty.next;
            }
            else
            {
                box.allIn = false;
                box.eatCandyWithin24H = false;
                box.show = null;
                box.stage = null;
                box.setReplaysTimes = 1;
                box.difficulty = "StageDifficulty_None";
            }
            return box;
        }
    }
}
