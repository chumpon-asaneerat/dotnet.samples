#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

#endregion

namespace Wpf.Process.Sample
{
    public class ProcessUtils
    {
        public static System.Diagnostics.Process Current
        {
            get { return System.Diagnostics.Process.GetCurrentProcess(); }
        }

        public static System.Diagnostics.Process[] GetProcesses(string processName)
        {
            if (string.IsNullOrWhiteSpace(processName))
                return null;
            return System.Diagnostics.Process.GetProcessesByName(processName);
        }
        public static System.Diagnostics.Process[] GetProcesses(System.Diagnostics.Process process)
        {
            if (null == process || string.IsNullOrWhiteSpace(process.ProcessName))
                return null;
            return System.Diagnostics.Process.GetProcessesByName(process.ProcessName);
        }
    }
}
