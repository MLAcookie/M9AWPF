using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using M9AWPF.Core.M9AModels;
using M9AWPF.Updater.Models;

namespace M9AWPF.Updater.ViewModels;

public class UpdaterMainWindowViewModel : ObservableObject
{
    M9AVersionHelper m9AVersionHelper = new M9AVersionHelper();


    public static string M9AVersion
    {
        get { return $"M9A Version: {ConfigInterface.M9AVersion}"; }
    }

    public static Visibility IsM9ANotLatest
    {
        get { return M9AVersionHelper.IsLatestVersion() ? Visibility.Collapsed : Visibility.Visible; }
    }

    public static string M9ALatestVerion
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

    async void UpdateM9A()
    {
        IsDownloading = true;
        await M9AVersionHelper.GetLatestM9ARelase();
        IsDownloading = false;
        M9AVersionHelper.HasDownloaded = true;
    }

    public RelayCommand UpdateM9ACommand { get; set; }

    public UpdaterMainWindowViewModel()
    {
        UpdateM9ACommand = new RelayCommand(UpdateM9A);
    }
}
