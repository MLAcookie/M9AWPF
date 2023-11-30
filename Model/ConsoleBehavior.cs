using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M9AWPF.Model
{
    internal class ConsoleBehavior
    {
        const string M9A_PATH = @".\M9A_CLI.exe";
        readonly ProcessStartInfo m9aStartInfo = new()
        {
            FileName = M9A_PATH,
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
