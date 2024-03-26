using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using M9AWPF.JsonSerializeObject;
using M9AWPF.Model;

namespace M9AWPF.ViewModel;

public class EasyUIViewModel : ObservableObject
{
    M9AVersionHelper m9AVersionHelper = new();

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
    public static string ADBPath
    {
        get { return ConfigManager.ADBPath; }
        set { ConfigManager.ADBPath = value; }
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

    public static string M9AVersion
    {
        get { return $"M9A Version: {ConfigInterface.M9AVersion}"; }
    }
    public static Visibility IsM9ANotLatest
    {
        get
        {
            return M9AVersionHelper.IsLatestVersion() ? Visibility.Collapsed : Visibility.Visible;
        }
    }
    public static string M9ALatestVersion
    {
        get { return M9AVersionHelper.LatestReleaseVersion; }
    }

    // 用来指示正在下载
    bool isDownloading = false;
    public bool IsDownloading
    {
        get { return isDownloading; }
        set
        {
            isDownloading = value;
            OnPropertyChanged(nameof(IsDownloading));
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
    public RelayCommand StartM9ACommand { get; set; }
    public RelayCommand UpdateM9ACommand { get; set; }

    public EasyUIViewModel()
    {
        StartM9ACommand = new RelayCommand(StartM9A);
        UpdateM9ACommand = new RelayCommand(UpdateM9A);
    }

    async void StartM9A()
    {
        ConfigManager.SaveConfig();
        await Task.Run(() => consoleBehavior.Start());
    }

    async void UpdateM9A()
    {
        IsDownloading = true;
        await M9AVersionHelper.GetLatestM9ARelease();
        IsDownloading = false;
        M9AVersionHelper.HasDownloaded = true;
    }
}
