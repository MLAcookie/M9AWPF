using M9AWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M9AWPF.Model
{
    //这里的所有代码都是用来json序列化的
    [Serializable]
    public class ConfigObject
    {
        public string adb { get; set; }
        public string adb_address { get; set; }
        public int client_type { get; set; }
        public bool debug { get; set; }
        public int screencap { get; set; }
        public List<MAATask> tasks { get; set; }
        public int touch { get; set; }
        public string version { get; set; } = "v0.2.0";
    }

    [Serializable]
    public class MAATask
    {
        public static MAATask ConvertionFrom(BoxedMAATask box)//从内部的编辑对象转换为标准对象
        {
            MAATask maaTask = new MAATask();
            maaTask.name = box.name;
            maaTask.type = box.type.ConfigName;
            if (box.IsCombat)
            {
                maaTask.param = new TaskParam
                {
                    diff_task = new Diff_Task
                    {
                        AllIn = new Allin
                        {
                            enabled = box.AllIn
                        },
                        EatCandyWithin24H = new Eatcandywithin24h
                        {
                            enabled = box.EatCandyWithin24H
                        },
                        EnterTheShow = new Entertheshow
                        {
                            next = box.show.ConfigName
                        },
                        SetReplaysTimes = new Setreplaystimes
                        {
                            text = box.setReplaysTimes.ToString()
                        },
                        StageDifficulty = new Stagedifficulty
                        {
                            next = box.difficulty
                        },
                        TargetStageName = new Targetstagename
                        {
                            text = box.stage
                        }
                    }
                };
            }
            return maaTask;
        }
        public string name { get; set; }
        public TaskParam param { get; set; }
        public string type { get; set; }
    }

    [Serializable]
    public class TaskParam
    {
        public Diff_Task? diff_task { get; set; }
    }

    [Serializable]
    public class Diff_Task
    {
        public Allin AllIn { get; set; }
        public Eatcandywithin24h EatCandyWithin24H { get; set; }
        public Entertheshow EnterTheShow { get; set; }
        public Setreplaystimes SetReplaysTimes { get; set; }
        public Stagedifficulty StageDifficulty { get; set; }
        public Targetstagename TargetStageName { get; set; }
    }

    [Serializable]
    public class Allin
    {
        public bool enabled { get; set; }
    }

    [Serializable]
    public class Eatcandywithin24h
    {
        public bool enabled { get; set; }
    }

    [Serializable]
    public class Entertheshow
    {
        public string next { get; set; }
    }

    [Serializable]
    public class Setreplaystimes
    {
        public string text { get; set; }
    }

    [Serializable]
    public class Stagedifficulty
    {
        public string next { get; set; }
    }

    [Serializable]
    public class Targetstagename
    {
        public string text { get; set; }
    }

}
