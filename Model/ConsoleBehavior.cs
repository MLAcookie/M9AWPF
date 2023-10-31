using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAA1999WPF.Model
{
    internal class ConsoleBehavior
    {
        const string MAA1999_PATH = @"D:\OtherProject\MAA1999-win-x86_64-v0.6.0\MAA1999_CLI.exe";
        readonly ProcessStartInfo maa1999StartInfo = new()
        {
            FileName = MAA1999_PATH,
            RedirectStandardInput = false,
            RedirectStandardOutput = false,
            CreateNoWindow = false,
            UseShellExecute = false,
        };
        readonly Process maa1999;
        public ConsoleBehavior()
        {
            maa1999 = new Process { StartInfo = maa1999StartInfo };
        }
        public void Start()
        {
            maa1999.Start();
            maa1999.WaitForExit();
            maa1999.Close();
        }
    }
}
