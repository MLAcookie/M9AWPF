using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using M9AWPF.Model;

namespace M9AWPF.ViewModel;

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
            foreach (var t in ConfigInterface.task)
                res.Add(t.name);
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
            foreach (var item in ConfigInterface.task)
            {
                var li = new List<string>();
                foreach (var t in item.option)
                {
                    li.Add(t);
                }

                res.Add(item.name, li.ToArray());
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
        get { return ConfigInterface.resource.ToArray(); }
    }

    public string ADBPath
    {
        get { return ConfigManager.ADBPath; }
        set
        {
            if (ConfigManager.ADBPath == value)
            {
                return;
            }

            ConfigManager.ADBPath = value;
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
            ADBPath = openFileDialog.FileName;
        }
    }

    public static int ADBPort
    {
        get { return ConfigManager.ADBPort; }
        set { ConfigManager.ADBPort = value; }
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
    public static BoxedMAATask[] AllMAATasks
    {
        get
        {
            var res = new List<BoxedMAATask>();
            foreach (var item in ConfigManager.AllMAATasks)
            {
                var boxedMAATask = BoxedMAATask.FromMAATask(item);
                res.Add(boxedMAATask);
            }

            return res.ToArray();
        }

        set
        {
            var res = new List<M9AConfigObject.Task>();
            foreach (var item in value)
            {
                res.Add(item.ToMAATask());
            }

            ConfigManager.AllMAATasks = res.ToArray();
        }
    }

    /// <summary>
    /// append task to the tail of ALLMAATasks
    /// </summary>
    /// <param name="task"></param>
    public static void AppendTask(BoxedMAATask task)
    {
        var res = AllMAATasks.ToList();
        res.Add(task);
        AllMAATasks = res.ToArray();
    }

    /// <summary>
    /// 用于启动MAA CLI跑任务
    /// </summary>
    private readonly ConsoleBehavior consoleBehavior = new();

    // 一些命令操作
    public RelayCommand StartMAACommand { get; set; }

    public EasyUIViewModel()
    {
        StartMAACommand = new RelayCommand(StartMAA);
        SelectAdbFileCommand = new RelayCommand(SelectAdbFile);
    }

    async void StartMAA()
    {
        ConfigManager.SaveConfig();
        await Task.Run(() => consoleBehavior.Start());
    }
}
