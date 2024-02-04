using M9AWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Diagnostics;
using M9AWPF.Constants;
using M9AWPF.Constants;

namespace M9AWPF.Model
{
    internal class ConfigManager
    {
        // static string targetConfigFilepath;
        private static readonly ConfigObject configObject;
        private static List<BoxedMAATask> boxedMAATasks;

        /// <summary>
        /// 按路径初始化
        /// </summary>
        /// <exception cref="FileNotFoundException">default-config.json找不到时抛出</exception>
        /// <exception cref="FileLoadException">default-config.json读取失败时抛出</exception>
        static ConfigManager()
        {
            Console.Out.WriteLine("Reading json config...");

            // M9A Release默认不带config.json文件，会导致报错
            // 考虑通过自带的default-config.json作为初始配置文件
            // 先检查default-config.json文件是否存在
            if (!File.Exists(Configurations.M9AConfigDefault))
            {
                throw new FileNotFoundException("File not Found: default-config.json");
            }

            // 如果不带config.json，则复制default-config.json
            if (!File.Exists(Configurations.M9AConfig))
            {
                Console.Out.WriteLine("M9A config file not found");
                File.Copy(sourceFileName: Configurations.M9AConfigDefault,
                    destFileName: Configurations.M9AConfig, overwrite: true);
            }

            StreamReader sr = File.OpenText(Configurations.M9AConfig);
            string jsonString = sr.ReadToEnd();
            // 如果json文件读取失败，则报错退出
            configObject = JsonSerializer.Deserialize<ConfigObject>(jsonString) ??
                           throw new FileLoadException("Failed to read config.json");
            sr.Close();
        }

        /// <summary>
        /// 保存到原文件
        /// </summary>
        public static void SaveConfig()
        {
            List<MAATask> maaTasks = new List<MAATask>();
            foreach (var task in boxedMAATasks)
            {
                maaTasks.Add(MAATask.ConvertionFrom(task));
            }

            configObject.tasks = maaTasks;
            string jsonString = JsonSerializer.Serialize(configObject);
            jsonString = jsonString.Replace("null", "{}");
            File.WriteAllText(Configurations.M9AConfig, jsonString, new UTF8Encoding(false));
        }

        public static void ChangeADBAddress(string address)
        {
            configObject.adb_address = address;
        }

        /// <summary>
        /// 自动属性，Clent和int互转
        /// </summary>
        public static Configurations.Client Client
        {
            get { return (Configurations.Client)configObject.client_type; }
            set
            {
                switch (value)
                {
                    case Configurations.Client.Official:
                        configObject.client_type = 1;
                        break;
                    case Configurations.Client.Bilibili:
                        configObject.client_type = 2;
                        break;
                }
            }
        }

        public static string ADBPath //修改ADB路径
        {
            get { return configObject.adb; }
            set { configObject.adb = value; }
        }

        public static int ADBPort //修改端口
        {
            get
            {
                int temp = configObject.adb_address.IndexOf(":") + 1;
                return int.Parse(configObject.adb_address[temp..]);
            }
            set { configObject.adb_address = $"127.0.0.1:{value}"; }
        }

        public static List<BoxedMAATask> GetAllMAATasks() //获取所有的已包装的对象
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
