using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M9AWPF.Constants;

namespace M9AWPF.Model
{
    internal class ConsoleBehavior
    {
        // private string M9A_PATH = Configurations.M9ABin;

        readonly ProcessStartInfo m9aStartInfo = new()
        {
            FileName = Configurations.M9AConfig,
            RedirectStandardInput = true,
            RedirectStandardOutput = false,
            CreateNoWindow = false,
            UseShellExecute = false,
        };

        readonly Process m9a;

        public ConsoleBehavior()
        {
            m9a = new Process { StartInfo = m9aStartInfo };
        }

        public void Start()
        {
            m9a.Start();
            m9a.WaitForExit();
            m9a.Close();
        }
    }
}
