using MAA1999WPF.ViewModel;
using MAA1999WPF.Model;
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
    public enum Client
    {
        Official = 1,
        Bilibili = 2,
    }
    internal class ConfigManager
    {
        static string targetConfigFilepath;
        static ConfigObject configObject;
        public static List<BoxedMAATask> boxedMAATasks;
        public ConfigManager(string targetConfigFilepath)
        {
            ConfigManager.targetConfigFilepath = targetConfigFilepath;
            StreamReader sr = File.OpenText(targetConfigFilepath);
            string jsonString = sr.ReadToEnd();
            configObject = JsonSerializer.Deserialize<ConfigObject>(jsonString);
            sr.Close();
        }
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
            File.WriteAllText(targetConfigFilepath, jsonString, new UTF8Encoding(false));
        }
        public void ChangeADBAddress(string address)
        {
            configObject.adb_address = address;
        }
        public Client Client
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
        public string ADBPath
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
        public int ADBPort
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
        public List<BoxedMAATask> GetAllMAATasks()
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
