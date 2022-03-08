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

        /*
        private static void KillProcessAndChildrens(int pid)
        {
            ManagementObjectSearcher processSearcher = new ManagementObjectSearcher
              ("Select * From Win32_Process Where ParentProcessID=" + pid);
            ManagementObjectCollection processCollection = processSearcher.Get();

            // We must kill child processes first!
            if (processCollection != null)
            {
                foreach (ManagementObject mo in processCollection)
                {
                    KillProcessAndChildrens(Convert.ToInt32(mo["ProcessID"])); //kill child processes(also kills childrens of childrens etc.)
                }
            }

            // Then kill parents.
            try
            {
                Process proc = Process.GetProcessById(pid);
                if (!proc.HasExited) proc.Kill();
            }
            catch (ArgumentException)
            {
                // Process already exited.
            }
        }
        */
    }
}
