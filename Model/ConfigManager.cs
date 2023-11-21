using MAA1999WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Diagnostics;

namespace MAA1999WPF.Model
{
    //管理config的类，操纵的是从config文件反序列化得来的对象
    public enum Client
    {
        Official = 1,
        Bilibili = 2,
    }
    internal class ConfigManager
    {
        const string path= @".\config.json";
        static string targetConfigFilepath;
        static ConfigObject configObject;
        public static List<BoxedMAATask> boxedMAATasks;
        static ConfigManager()//按路径初始化

        {
            targetConfigFilepath = path;
            StreamReader sr = File.OpenText(path);
            string jsonString = sr.ReadToEnd();
            configObject = JsonSerializer.Deserialize<ConfigObject>(jsonString);
            sr.Close();
        }
        public static void SaveConfig()//保存到原文件
        {
            List<MAATask> maaTasks = new List<MAATask>();
            foreach (var task in boxedMAATasks)
            {
                maaTasks.Add(MAATask.ConvertionFrom(task));
            }
            configObject.tasks = maaTasks;
            string jsonString = JsonSerializer.Serialize(configObject);
            jsonString = jsonString.Replace("null", "{}");
            File.WriteAllText(targetConfigFilepath, jsonString, new UTF8Encoding(false));
        }
        public static void ChangeADBAddress(string address)
        {
            configObject.adb_address = address;
        }
        public static Client Client//自动属性，Clent和int互转
        {
            get
            {
                return (Client)configObject.client_type;
            }
            set
            {
                switch (value)
                {
                    case Client.Official:
                        configObject.client_type = 1;
                        break;
                    case Client.Bilibili:
                        configObject.client_type = 2;
                        break;
                }
            }
        }
        public static string ADBPath//修改ADB路径
        {
            get
            {
                return configObject.adb;
            }
            set
            {
                configObject.adb = value;
            }
        }
        public static int ADBPort//修改端口
        {
            get
            {
                int temp = configObject.adb_address.IndexOf(":") + 1;
                return int.Parse(configObject.adb_address[temp..]);
            }
            set
            {
                configObject.adb_address = $"127.0.0.1:{value}";
            }
        }
        public static List<BoxedMAATask> GetAllMAATasks()//获取所有的已包装的对象
        {
            List<BoxedMAATask> list = new List<BoxedMAATask>();
            foreach (var task in configObject.tasks)
            {
                list.Add(BoxedMAATask.ConvertionFrom(task));
            }
            boxedMAATasks = list;
            return list;
        }
    }
}
