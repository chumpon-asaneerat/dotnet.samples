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

    public class ProcessEx
    {
        //private static readonly System.Diagnostics.Process Current = System.Diagnostics.Process.GetCurrentProcess();

        public ProcessEx() : base() 
        {
            
        }

        internal ProcessEx(System.Diagnostics.Process process) : this()
        {
            if (null != process)
            {
                try { this.ProcessName = process.ProcessName; }
                catch (InvalidOperationException) { this.ProcessName = "Error InvalidOp"; }
                catch (Exception ex) 
                { 
                    this.ProcessName = "Error [ProcessName] - " + ex.Message; 
                }

                try { this.Id = process.Id; }
                catch (InvalidOperationException) { this.Id = 0; }
                catch (Exception ex) 
                { 
                    this.ProcessName = "Error [Id] - " + ex.Message;
                    this.Id = 0;
                }

                /*
                try { this.Handle = (!process.HasExited) ? process.Handle : IntPtr.Zero; }
                catch (InvalidOperationException) { this.Handle = IntPtr.Zero; }
                catch (Exception ex)
                {
                    this.ProcessName = "Error [Handle] - " + ex.Message;
                    this.Handle = IntPtr.Zero;
                }
                */

                try { this.HandleCount = process.HandleCount; }
                catch (InvalidOperationException) { this.HandleCount = 0; }
                catch (Exception ex)
                {
                    this.ProcessName = "Error [HandleCount] - " + ex.Message;
                    this.HandleCount = 0;
                }

                /*
                try { this.MainWindowHandle = (!process.HasExited) ? process.MainWindowHandle : IntPtr.Zero; }
                catch (InvalidOperationException) { this.MainWindowHandle = IntPtr.Zero; }
                catch (Exception ex)
                {
                    this.ProcessName = "Error [MainWindowHandle] - " + ex.Message;
                    this.MainWindowHandle = IntPtr.Zero;
                }
                */

                /*
                try { this.MainWindowTitle = (!process.HasExited) ? process.MainWindowTitle : string.Empty; }
                catch (InvalidOperationException) { this.MainWindowTitle = string.Empty; }
                catch (Exception ex)
                {
                    this.ProcessName = "Error [MainWindowTitle] - " + ex.Message;
                    this.MainWindowTitle = string.Empty;
                }
                */
            }
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (null == obj)
                return false;
            if (obj.GetType() != typeof(ProcessEx))
                return false;
            if ((obj as ProcessEx).Id != this.Id)
                return false;
            return true;
        }

        public void Assign(ProcessEx source)
        {
            if (null == source)
            {
                this.ProcessName = string.Empty;
                this.Id = -1;
                this.Handle = IntPtr.Zero;
                this.HandleCount = 0;

                this.MainWindowHandle = IntPtr.Zero;
                this.MainWindowTitle = string.Empty;
            }
            else
            {
                this.ProcessName = source.ProcessName;
                this.Id = source.Id;
                this.Handle = source.Handle;
                this.HandleCount = source.HandleCount;

                this.MainWindowHandle = source.MainWindowHandle;
                this.MainWindowTitle = source.MainWindowTitle;
            }
        }

        public string ProcessName { get; set; }
        public int Id { get; set; }
        public IntPtr Handle { get; set; }
        public int HandleCount { get; set; }

        public IntPtr MainWindowHandle { get; set; }
        public string MainWindowTitle { get; set; }

        public bool IsCurrent 
        { 
            get 
            {
                /*
                lock (this)
                {
                    return this.Id == ProcessEx.Current.Id;
                }
                */
                return true;
            } 
        }

        public string IsCurrentStr { get { return (this.IsCurrent) ? "#" : ""; } }

    }

    public static class ProcessUtils
    {
        public static ProcessEx Create(this System.Diagnostics.Process process)
        {
            ProcessEx inst = new ProcessEx(process);
            return inst;
        }


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
