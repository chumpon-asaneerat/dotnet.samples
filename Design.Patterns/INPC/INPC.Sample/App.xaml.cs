using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Management;

namespace INPC.Sample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

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
        public static int ProcessCount { get; set; }
        public static int Id { get; set; }

        public static IntPtr Handle { get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DateTime dt = DateTime.Now;
            // get all process with same process name.
            var process = Process.GetCurrentProcess();
            var processes = Process.GetProcessesByName(process.ProcessName);

            TimeSpan ts = DateTime.Now - dt;
            Console.WriteLine("Get process time: {0:n0} ms.", ts.TotalMilliseconds);

            if (null != processes)
            {
                ProcessCount = processes.Length;
            }

            if (null != process)
            {
                Id = process.Id;
                Handle = process.Handle;
            }

            var window = new MainWindow();
            window.Show();
        }
    }
}
