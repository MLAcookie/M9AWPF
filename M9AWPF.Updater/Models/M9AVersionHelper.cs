using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using M9AWPF.Updater.JsonSerializeObject;
using M9AWPF.Core.Constants;
using M9AWPF.Core.M9AModels;

namespace M9AWPF.Updater.Models
{
    public class M9AVersionHelper
    {
        //const string M9AAllReleaseAPI = "https://api.github.com/repos/MaaXYZ/M9A/releases";
        //通过GithubAPI获取最新的release
        const string M9ALatestReleaseAPI =
            "https://api.github.com/repos/MaaXYZ/M9A/releases/latest";

        static HttpClient client = new HttpClient();

        static GitHubReleaseObject latestRelease = null!;

        //表示是否已经下载了
        public static bool HasDownloaded = false;

        static string? proxy = null;

        public static string? Proxy
        {
            get { return proxy; }
            set
            {
                proxy = value;
                HttpClient.DefaultProxy = new WebProxy(proxy);
            }
        }

        /// <summary>
        /// 获取最新release的版本号
        /// </summary>
        public static string LatestReleaseVersion
        {
            get { return latestRelease.tag_name; }
        }

        public M9AVersionHelper()
        {
            //加入浏览器UA，防止github阻止访问
            client.DefaultRequestHeaders.Add(
                "User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:123.0) Gecko/20100101 Firefox/123.0"
            );
            Init();
        }

        /// <summary>
        /// 初始化内置的Release对象，防止多次使用API后被Github限制
        /// </summary>
        private static void Init()
        {
            //新建http请求
            var request = new HttpRequestMessage
            {
                Method = new HttpMethod("GET"),
                RequestUri = new Uri(M9ALatestReleaseAPI),
            };
            HttpResponseMessage response = client.SendAsync(request).Result;
            if (response.IsSuccessStatusCode)
            {
                //接受json，反序列化
                string jsonResponse = response.Content.ReadAsStringAsync().Result;
                latestRelease = JsonSerializer.Deserialize<GitHubReleaseObject>(jsonResponse)!;
            }
            else
            {
                //如果错误就在控制台输出错误代码（暂时）
                Console.WriteLine("Error: " + response.StatusCode);
            }
        }

        /// <summary>
        /// 判断当前使用的M9A是否为最新版
        /// </summary>
        /// <returns></returns>
        public static bool IsLatestVersion()
        {
            return latestRelease?.tag_name == ConfigInterface.M9AVersion;
        }

        /// <summary>
        /// 获取最新的M9ARelease，下载到temp目录中
        /// </summary>
        public static async Task GetLatestM9ARelase()
        {
            //文件夹路径检查
            if (!Directory.Exists(ConfKeys.TempDownload))
            {
                Directory.CreateDirectory(ConfKeys.TempDownload);
            }

            if (!Directory.Exists(ConfKeys.TempLatest))
            {
                Directory.CreateDirectory(ConfKeys.TempLatest);
            }

            foreach (var asset in latestRelease.assets)
            {
                if (asset.name.Contains("win-x86_64"))
                {
                    string downloadFilePath = Path.Combine(ConfKeys.TempDownload, asset.name);
                    //有同名文件的话不下载
                    if (!File.Exists(downloadFilePath))
                    {
                        Stream latestReleaseStream = await client.GetStreamAsync(
                            asset.browser_download_url
                        );
                        FileStream saveFile = new(downloadFilePath, FileMode.CreateNew);
                        await latestReleaseStream.CopyToAsync(saveFile);
                        saveFile.Close();
                    }

                    CleanPath(ConfKeys.TempLatest);
                    await Task.Run(
                        () => ZipFile.ExtractToDirectory(downloadFilePath, ConfKeys.TempLatest)
                    );
                }
            }
        }

        public static void CloseUpdate(object? sender, CancelEventArgs e)
        {
            //检查有无下载，下载过就执行更新
            if (HasDownloaded)
            {
                UpdateM9A();
            }
        }

        private static void UpdateM9A()
        {
            CleanPath(ConfKeys.TempConfigBackup);
            //复制config文件备份
            CopyTo(ConfKeys.M9AConfig, ConfKeys.TempConfigBackup);
            CopyTo(ConfKeys.M9AToolkit, ConfKeys.TempConfigBackup);
            //清空M9A-Bin
            Directory.Delete(ConfKeys.M9ARoot, true);
            //将latest的文件移动到M9A-Bin中
            Directory.Move(ConfKeys.TempLatest, ConfKeys.M9ARoot);
            //恢复config备份
            Directory.CreateDirectory(Path.Combine(ConfKeys.M9ARoot, "config"));
            MoveTo(
                Path.Combine(ConfKeys.TempConfigBackup, "maa_pi_config.json"),
                Path.Combine(ConfKeys.M9ARoot, "config")
            );
            MoveTo(
                Path.Combine(ConfKeys.TempConfigBackup, "maa_toolkit.json"),
                Path.Combine(ConfKeys.M9ARoot, "config")
            );
        }

        private static void CleanPath(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            Directory.CreateDirectory(path);
        }

        private static void CopyTo(string sourceFile, string path)
        {
            string fileName = Path.GetFileName(sourceFile);
            File.Copy(sourceFile, Path.Combine(path, fileName));
        }

        private static void MoveTo(string sourceFile, string path)
        {
            string fileName = Path.GetFileName(sourceFile);
            File.Move(sourceFile, Path.Combine(path, fileName));
        }
    }
}
