using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAA1999WPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MAA1999WPF.ViewModel
{
    public partial class EasyUIViewModel : ObservableObject
    {

        readonly ConsoleBehavior consoleBehavior = new();
        TaskType type = BoxedMAATask.ItemConfig.Types[0];
        Show? show;
        string? stage;
        int setReplaysTimes = 1;

        [ObservableProperty]
        string newTaskName = "NewTask";
        [ObservableProperty]
        bool allIn = false;
        [ObservableProperty]
        bool eat24hCandy = false;
        [ObservableProperty]
        bool isDifficulty = false;
        [ObservableProperty]
        List<BoxedMAATask> allMAATasks = ConfigManager.GetAllMAATasks();
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
        public static int ADBPort
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
        public static List<string> AllTaskTypes
        {
            get
            {
                return BoxedMAATask.ItemConfig.AllTypeUIName;
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
        public bool IsCombat
        {
            get
            {
                return type.IsCombat;
            }
        }
        public int TypeIndex
        {
            get
            {
                return BoxedMAATask.ItemConfig.Types.IndexOf(type);
            }
            set
            {
                type = BoxedMAATask.ItemConfig.Types[value];
                OnPropertyChanged(nameof(IsCombat));
                OnPropertyChanged(nameof(AllShows));
                OnPropertyChanged(nameof(ShowIndex));
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
                if (value < 1)
                {
                    value = 1;
                }
                else if (value > 4)
                {
                    value = 4;
                }
                setReplaysTimes = value;
                OnPropertyChanged(nameof(SetReplaysTimes));
            }
        }
        public RelayCommand StartMAACommand { get; set; }
        public RelayCommand<ItemCollection> AddNewTaskCommand { get; set; }

        public EasyUIViewModel()
        {
            StartMAACommand = new RelayCommand(StartMAA);
            AddNewTaskCommand = new RelayCommand<ItemCollection>(AddNewTask);
        }
        async void StartMAA()
        {
            ConfigManager.SaveConfig();
            await Task.Run(() => consoleBehavior.Start());
        }
        void AddNewTask(ItemCollection items)
        {
            BoxedMAATask temp = new()
            {
                name = NewTaskName,
                type = type,
                AllIn = AllIn,
                EatCandyWithin24H = Eat24hCandy,
                show = show,
                stage = stage,
                difficulty = IsDifficulty ? "StageDifficulty_Hard" : "StageDifficulty_None",
                setReplaysTimes = setReplaysTimes,
            };
            AllMAATasks.Add(temp);
            OnPropertyChanged(nameof(AllMAATasks));
            items.Refresh();
        }
        public void DeleteTask(BoxedMAATask task, ItemCollection items)
        {
            AllMAATasks.Remove(task);
            items.Refresh();
        }
        public void ItemMoveDown(BoxedMAATask task, ItemCollection items)
        {
            int index = AllMAATasks.IndexOf(task);
            if (index == AllMAATasks.Count - 1)
            {
                return;
            }
            else
            {
                (AllMAATasks[index + 1], AllMAATasks[index]) = (AllMAATasks[index], AllMAATasks[index + 1]);
            }
            items.Refresh();
        }
        public void ItemMoveUp(BoxedMAATask task, ItemCollection items)
        {
            int index = AllMAATasks.IndexOf(task);
            if (index == 0)
            {
                return;
            }
            else
            {
                (AllMAATasks[index - 1], AllMAATasks[index]) = (AllMAATasks[index], AllMAATasks[index - 1]);
            }
            items.Refresh();
        }
    }
}
