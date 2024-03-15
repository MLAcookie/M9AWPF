using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using M9AWPF.App.Model;
using Microsoft.Win32;

namespace M9AWPF.App.ViewModel;

public class EasyUIViewModel : INotifyPropertyChanged
{
    /// <summary>
    /// 指示有哪些任务选项
    /// </summary>
    public static string[] AllTaskTypes
    {
        get
        {
            var res = new List<string>();
            foreach (var t in ConfigInterface.Tasks)
                res.Add(t.Name);
            return res.ToArray();
        }
    }

    /// <summary>
    /// 指示task name对option name的映射，哪些task有哪些option
    /// </summary>
    public static Dictionary<string, string[]> TaskMap2Option
    {
        get
        {
            var res = new Dictionary<string, string[]>();
            foreach (var item in ConfigInterface.Tasks)
            {
                var li = new List<string>();
                foreach (var t in item.Option)
                {
                    li.Add(t);
                }

                res.Add(item.Name, li.ToArray());
            }

            return res;
        }
    }

    /// <summary>
    /// 指示所有选项及其所对应的可取值
    /// </summary>
    public static Dictionary<string, string[]> OptionMap2Values
    {
        get
        {
            var res = new Dictionary<string, string[]>();
            foreach (var item in ConfigInterface.option)
            {
                res.Add(item.Key, item.Value.ToArray());
            }

            return res;
        }
    }

    /// <summary>
    /// 显示有哪些服务器选项
    /// </summary>
    public static string[] AllResources
    {
        get { return ConfigInterface.Resource.ToArray(); }
    }

    public string AdbPath
    {
        get { return ConfigManager.AdbPath; }
        set
        {
            if (ConfigManager.AdbPath == value)
            {
                return;
            }

            ConfigManager.AdbPath = value;
            OnPropertyChanged();
        }
    }

    // 选择ADB路径Button相关
    public RelayCommand SelectAdbFileCommand { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void SelectAdbFile()
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        if (openFileDialog.ShowDialog() == true)
        {
            // TODO(KaronGH): 应该添加一个逻辑或限制，禁止用户选择非"adb.exe"的文件
            AdbPath = openFileDialog.FileName;
        }
    }

    public static int AdbPort
    {
        get { return ConfigManager.AdbPort; }
        set { ConfigManager.AdbPort = value; }
    }

    /// <summary>
    /// 服务器 “官服” “B服” 等
    /// </summary>
    public static string Client
    {
        get { return ConfigManager.Client; }
        set { ConfigManager.Client = value; }
    }

    /// <summary>
    /// 所有任务
    /// </summary>
    public static BoxedMaaTask[] AllMaaTasks
    {
        get
        {
            var res = new List<BoxedMaaTask>();
            foreach (var item in ConfigManager.AllMaaTasks)
            {
                var boxedMaaTask = BoxedMaaTask.FromMaaTask(item);
                res.Add(boxedMaaTask);
            }

            return res.ToArray();
        }

        set
        {
            var res = new List<M9AConfigObject.Task>();
            foreach (var item in value)
            {
                res.Add(item.ToMaaTask());
            }

            ConfigManager.AllMaaTasks = res.ToArray();
        }
    }

    /// <summary>
    /// append Tasks to the tail of AllMaaTasks
    /// </summary>
    /// <param name="task"></param>
    public static void AppendTask(BoxedMaaTask task)
    {
        var res = AllMaaTasks.ToList();
        res.Add(task);
        AllMaaTasks = res.ToArray();
    }

    /// <summary>
    /// 用于启动MAA CLI跑任务
    /// </summary>
    private readonly ConsoleBehavior _consoleBehavior = new ConsoleBehavior();

    // 一些命令操作
    public RelayCommand StartMaaCommand { get; set; }

    public EasyUIViewModel()
    {
        StartMaaCommand = new RelayCommand(StartMaa);
        SelectAdbFileCommand = new RelayCommand(SelectAdbFile);
    }

    async void StartMaa()
    {
        ConfigManager.SaveConfig();
        await Task.Run(() => _consoleBehavior.Start());
    }
}
