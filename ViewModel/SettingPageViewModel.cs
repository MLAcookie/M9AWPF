using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MAA1999WPF.Model;

namespace MAA1999WPF.ViewModel
{
    public partial class SettingPageViewModel : ObservableObject
    {
        const string path = @".\config.json";
        //const string path = @"D:\OtherProject\MAA1999-win-x86_64-v0.6.0\config.json";
        readonly ConfigManager configManager = new(path);
        [ObservableProperty]
        List<BoxedMAATask> allMAATasks = ConfigManager.GetAllMAATasks();
        [ObservableProperty]
        List<string> allStages;
        [ObservableProperty]
        public static List<string> showsName;
        [ObservableProperty]
        public static List<string> stagesName;
        public TaskType selectedTaskType;
        public Show selevtedShow;
        public static int Client
        {
            get
            {
                return (int)ConfigManager.Client - 1;
            }
            set
            {
                ConfigManager.Client = (Client)(value + 1);
            }
        }
        public static string ADBPath
        {
            get
            {
                return ConfigManager.ADBPath;
            }
            set
            {
                ConfigManager.ADBPath = value;
            }
        }
        public int ADBPort
        {
            get
            {
                return ConfigManager.ADBPort;
            }
            set
            {
                ConfigManager.ADBPort = value;
            }
        }
        public void AddTask(string maaTaskType, bool allIn, bool candy24H, string enterShow, string targetStage, bool isDifficulty, int replayTimes, string taskName)
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
            if (BoxedMAATask.TypeIsCombat[maaTaskType])
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
