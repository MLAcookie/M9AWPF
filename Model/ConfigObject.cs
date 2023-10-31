using MAA1999WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAA1999WPF.Model
{
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
        public static MAATask ConvertionFrom(BoxedMAATask box)
        {
            MAATask maaTask = new MAATask();
            maaTask.name = box.name;
            maaTask.type = box.type.ToString();
            if (box.type is MAATaskType.Combat or MAATaskType.ANightmareAtGreenLake or MAATaskType.JourneytoMorPankh or MAATaskType.ThePrisonerintheCave)
            {
                maaTask.param = new TaskParam
                {
                    diff_task = box.diff_task
                };
            }
            return maaTask;
        }
        public string name { get; set; }
        public TaskParam param { get; set; }
        public string type { get; set; }
        public static void ChangeTaskName(MAATask task, string name)
        {
            task.name = name;
        }
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
