using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MAA1999WPF.Model;

namespace MAA1999WPF.ViewModel
{
    public enum MAATaskType
    {
        Close1999,  //关闭1999
        StartUp,    //启动游戏
        Wilderness, //收取荒原
        Awards,     //领取奖励
        Psychube,   //每日心相
        Combat,     //正常关卡
        ANightmareAtGreenLake,  //活动：绿湖噩梦(已结束)
        JourneytoMorPankh,      //活动：行至摩卢旁卡(已结束)
        ThePrisonerintheCave,   //活动：洞穴的囚徒
    }
    public partial class BoxedMAATask : ObservableObject//把标准对象包装，方便框架调用
    {
        public static BoxedMAATask ConvertionFrom(MAATask maaTask)//标准对象转为包装的对象
        {
            BoxedMAATask box = new BoxedMAATask();
            if (maaTask.type == MAATaskType.Combat.ToString()
                    || maaTask.type == MAATaskType.ANightmareAtGreenLake.ToString()
                    || maaTask.type == MAATaskType.JourneytoMorPankh.ToString()
                    || maaTask.type == MAATaskType.ThePrisonerintheCave.ToString())
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
            box.type = (MAATaskType)Enum.Parse(typeof(MAATaskType), maaTask.type);
            return box;
        }
        public string name;
        public MAATaskType type;
        public Diff_Task diff_task;
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
                OnPropertyChanged(nameof(StringType));
            }
        }
        public string StringType
        {
            get
            {
                return $"{name} - ({type})";
            }
        }
        public int TaskType
        {
            get
            {
                return (int)type;
            }
            set
            {
                type = (MAATaskType)value;
                OnPropertyChanged(nameof(TaskType));
                OnPropertyChanged(nameof(StringType));
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
        [ObservableProperty]
        public bool isCombat = false;
    }
    public partial class SettingPageViewModel : ObservableObject
    {
        const string path = @".\config.json";
        //const string path = @"D:\OtherProject\MAA1999-win-x86_64-v0.6.0\config.json";
        readonly ConfigManager configManager = new(path);
        [ObservableProperty]
        List<BoxedMAATask> allMAATasks;
        static readonly Dictionary<string, List<string>> allStage = new()
        {
            {"MainChapter_1",GenerateStageNumber(16)},//16
            {"MainChapter_2",GenerateStageNumber(15)},//15
            {"MainChapter_3",GenerateStageNumber(16)},//16
            {"MainChapter_4",GenerateStageNumber(23)},//23
            {"ResourceChapter_LP",GenerateStageNumber(6)},//06
            {"ResourceChapter_MA",GenerateStageNumber(6)},//06
            {"PromotionChapter_ME",GenerateStageNumber(6)},//06
            {"PromotionChapter_SL",GenerateStageNumber(6)},//06
            {"PromotionChapter_SS",GenerateStageNumber(6)},//06
            {"PromotionChapter_BW",GenerateStageNumber(6)},//06
            {"dummyThePrisonerintheCave",new List<string>(){ "dummy证明启示V" } },
        };
        static readonly List<string> allShow = new()
        {
            "MainChapter_1",
            "MainChapter_2",
            "MainChapter_3",
            "MainChapter_4",
            "ResourceChapter_LP",//尘埃
            "ResourceChapter_MA",//铸币
            "PromotionChapter_ME",//岩
            "PromotionChapter_SL",//星
            "PromotionChapter_SS",//林
            "PromotionChapter_BW",//兽
        };
        public SettingPageViewModel()
        {
            allMAATasks = configManager.GetAllMAATasks();
        }

        public int Client
        {
            get
            {
                return (int)configManager.Client - 1;
            }
            set
            {
                configManager.Client = (Client)(value + 1);
            }
        }
        public string ADBPath
        {
            get
            {
                return configManager.ADBPath;
            }
            set
            {
                configManager.ADBPath = value;
            }
        }
        public int ADBPort
        {
            get
            {
                return configManager.ADBPort;
            }
            set
            {
                configManager.ADBPort = value;
            }
        }
        public static List<string> GetAllShow(MAATaskType maaTaskType) => maaTaskType switch
        {
            MAATaskType.Combat => allShow,
            MAATaskType.ThePrisonerintheCave => new List<string>() { "dummyThePrisonerintheCave" },
            _ => new List<string>(),
        };
        public static List<string> GetAllStage(string chapterName)
        {
            if (allStage.ContainsKey(chapterName))
            {
                return allStage[chapterName];
            }
            else
            {
                return new List<string>();
            }
        }
        static List<string> GenerateStageNumber(int num)
        {
            List<string> result = new List<string>();
            for (int i = 1; i <= num; i++)
            {
                if (i < 10)
                {
                    result.Add($"0{i}");
                }
                else
                {
                    result.Add(i.ToString());
                }
            }
            return result;
        }
        public void AddTask(MAATaskType maaTaskType, bool allIn, bool candy24H, string enterShow, string targetStage, bool isDifficulty, int replayTimes, string taskName)
        {
            BoxedMAATask maaTask = new()
            {
                name = taskName,
                type = maaTaskType,
                diff_task = new Diff_Task
                {
                    AllIn = new Allin(),
                    EatCandyWithin24H = new Eatcandywithin24h(),
                    EnterTheShow = new Entertheshow(),
                    TargetStageName = new Targetstagename(),
                    SetReplaysTimes = new Setreplaystimes()
                }
            };
            maaTask.diff_task.SetReplaysTimes.text = "1";
            maaTask.diff_task.StageDifficulty = new Stagedifficulty();
            if (maaTaskType is MAATaskType.Combat)
            {
                maaTask.AllIn = allIn;
                maaTask.EatCandyWithin24H = candy24H;
                maaTask.EnterTheShow = enterShow;
                maaTask.TargetStageName = targetStage;
                maaTask.SetReplaysTimes = replayTimes;
                maaTask.IsDifficaulty = isDifficulty;
                maaTask.IsCombat = true;
            }
            AllMAATasks.Add(maaTask);
            OnPropertyChanged(nameof(AllMAATasks));
        }
        public void ItemMoveUp(BoxedMAATask maaTask)//上移任务
        {
            if (allMAATasks[0] == maaTask)
            {
                return;
            }
            int index = 0;
            while (true)
            {
                if (allMAATasks[index] == maaTask)
                {
                    break;
                }
                index++;
            }
            (allMAATasks[index - 1], allMAATasks[index]) = (allMAATasks[index], allMAATasks[index - 1]);
            OnPropertyChanged(nameof(AllMAATasks));
        }
        public void ItemMoveDown(BoxedMAATask maaTask)//下移任务
        {
            if (allMAATasks[allMAATasks.Count - 1] == maaTask)
            {
                return;
            }
            int index = 0;
            while (true)
            {
                if (allMAATasks[index] == maaTask)
                {
                    break;
                }
                index++;
            }
            (allMAATasks[index + 1], allMAATasks[index]) = (allMAATasks[index], allMAATasks[index + 1]);
            OnPropertyChanged(nameof(AllMAATasks));
        }
        public void ItemDelete(BoxedMAATask maaTask)//删除任务
        {
            int index = 0;
            while (true)
            {
                if (allMAATasks[index] == maaTask)
                {
                    break;
                }
                index++;
            }
            allMAATasks.RemoveAt(index);
            OnPropertyChanged(nameof(AllMAATasks));
        }
    }
}
