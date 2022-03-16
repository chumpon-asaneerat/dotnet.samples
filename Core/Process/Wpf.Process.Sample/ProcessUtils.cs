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

    public class NProcess : ICloneable
    {
        #region Static (readonly)

        /// <summary>
        /// Gets Current Process.
        /// </summary>
        public static readonly System.Diagnostics.Process Current = System.Diagnostics.Process.GetCurrentProcess();

        #endregion

        #region Constructor and Destructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public NProcess() : base() { }
        /// <summary>
        /// Constructor (internal).
        /// </summary>
        /// <param name="process">The System.Diagnostics.Process instance.</param>
        private NProcess(System.Diagnostics.Process process) : this() 
        {
            Empty();
            if (null != process)
            {
                // ProcessName
                bool error = false;
                this.Accessible = true;

                try { this.ProcessName = process.ProcessName; }
                catch (Exception ex)
                {
                    error = true;
                    this.ErrorMessage = "Error access [ProcessName] : " + ex.Message;
                    this.ProcessName = string.Empty;
                    this.Accessible = false;
                }
                if (error) return;
                // Id
                try { this.Id = process.Id; }
                catch (Exception ex)
                {
                    error = true;
                    this.ErrorMessage = "Error access [Id] : " + ex.Message;
                    this.Id = 0;
                    this.Accessible = false;
                }
                if (error) return;
                // Responding
                try { this.Responding = process.Responding; }
                catch (Exception ex)
                {
                    error = true;
                    this.ErrorMessage = "Error access [Responding] : " + ex.Message;
                    this.Responding = false;
                    this.Accessible = false;
                }
                if (error) return;
                // Has Exited
                try { this.HasExited = process.HasExited; }
                catch (Exception ex)
                {
                    error = true;
                    this.ErrorMessage = "Error access [HasExited] : " + ex.Message;
                    this.HasExited = true;
                    this.Accessible = false;
                }
                if (error) return;
                // Exit Code
                try { this.ExitCode = (!process.HasExited) ? -1 : process.ExitCode; }
                catch (Exception ex)
                {
                    error = true;
                    this.ErrorMessage = "Error access [ExitCode] : " + ex.Message;
                    this.ExitCode = -1;
                    this.Accessible = false;
                }
                if (error) return;

                // StartTime
                try { this.StartTime = new DateTime?(process.StartTime); }
                catch (Exception ex)
                {
                    error = true;
                    this.ErrorMessage = "Error access [StartTime] : " + ex.Message;
                    this.StartTime = new DateTime?();
                }
                if (error) return;
                // ExitTime
                try { this.ExitTime = (!process.HasExited) ? new DateTime?() : new DateTime?(process.ExitTime); }
                catch (Exception ex)
                {
                    error = true;
                    this.ErrorMessage = "Error access [ExitTime] : " + ex.Message;
                    this.ExitTime = new DateTime?();
                }
                if (error) return;

                // Handle
                try { this.Handle = (!process.HasExited) ? process.Handle : IntPtr.Zero; }
                catch (Exception ex)
                {
                    error = true;
                    this.ErrorMessage = "Error access [Handle] : " + ex.Message;
                    this.Handle = IntPtr.Zero;
                    this.Accessible = false;
                }
                if (error) return;
                // HandleCount
                try { this.HandleCount = process.HandleCount; }
                catch (Exception ex)
                {
                    error = true;
                    this.ErrorMessage = "Error access [HandleCount] : " + ex.Message;
                    this.HandleCount = 0;
                }
                if (error) return;
                // MainWindowHandle
                try { this.MainWindowHandle = (!process.HasExited) ? process.MainWindowHandle : IntPtr.Zero; }
                catch (Exception ex)
                {
                    error = true;
                    this.ErrorMessage = "Error access [MainWindowHandle] : " + ex.Message;
                    this.MainWindowHandle = IntPtr.Zero;
                    this.Accessible = false;
                }
                if (error) return;
                // MainWindowTitle
                try { this.MainWindowTitle = (!process.HasExited) ? process.MainWindowTitle : string.Empty; }
                catch (Exception ex)
                {
                    error = true;
                    this.ErrorMessage = "Error access [MainWindowTitle] : " + ex.Message;
                    this.MainWindowTitle = string.Empty;
                    this.Accessible = false;
                }
                if (error) return;
                this.Accessible = true;
            }
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~NProcess()
        {

        }

        #endregion

        #region Private Methods

        private void Empty()
        {
            this.ProcessName = string.Empty;
            this.Id = -1;

            this.Responding = false;
            this.HasExited = true;
            this.ExitCode = -1;

            this.StartTime = new DateTime?();
            this.ExitTime = new DateTime?();

            this.Handle = IntPtr.Zero;
            this.HandleCount = 0;

            this.MainWindowHandle = IntPtr.Zero;
            this.MainWindowTitle = string.Empty;

            this.ErrorMessage = string.Empty;

            this.Accessible = false;
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// GetHashCode.
        /// </summary>
        /// <returns>Returns hashcode.</returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
        /// <summary>
        /// Equals.
        /// </summary>
        /// <param name="obj">The conpare object instance.</param>
        /// <returns>Returns true if compare object is same as current object instance.</returns>
        public override bool Equals(object obj)
        {
            if (null == obj)
                return false;
            if (obj.GetType() != typeof(NProcess))
                return false;
            if ((obj as NProcess).Id != this.Id)
                return false;
            return true;
        }

        #endregion

        #region Interface Implements

        /// <summary>
        /// Clone.
        /// </summary>
        /// <returns>Returns Clone Instance of current object.</returns>
        public object Clone()
        {
            NProcess inst = new NProcess();
            inst.Copy(this);
            return inst;
        }
        /// <summary>
        /// Copy all properties from source to current instance.
        /// </summary>
        /// <param name="source"></param>
        public void Copy(NProcess source)
        {
            if (null == source)
            {
                Empty();
            }
            else
            {
                this.ProcessName = source.ProcessName;
                this.Id = source.Id;

                this.Responding = source.Responding;
                this.HasExited = source.HasExited;
                this.ExitCode = source.ExitCode;

                this.StartTime = source.StartTime;
                this.ExitTime = source.ExitTime;

                this.Handle = source.Handle;
                this.HandleCount = source.HandleCount;

                this.MainWindowHandle = source.MainWindowHandle;
                this.MainWindowTitle = source.MainWindowTitle;

                this.Accessible = false;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets process name.
        /// </summary>
        public string ProcessName { get; internal set; }
        /// <summary>
        /// Gets process id.
        /// </summary>
        public int Id { get; internal set; }
        /// <summary>
        /// Checks is process is still responding.
        /// </summary>
        public bool Responding { get; internal set; }
        /// <summary>
        /// Checks is process has exited.
        /// </summary>
        public bool HasExited { get; internal set; }
        /// <summary>
        /// Gets process exit code.
        /// </summary>
        public int ExitCode { get; internal set; }
        /// <summary>
        /// Gets process start time.
        /// </summary>
        public DateTime? StartTime { get; internal set; }
        /// <summary>
        /// Gets process exit time.
        /// </summary>
        public DateTime? ExitTime { get; internal set; }

        /// <summary>
        /// Gets process handle.
        /// </summary>
        public IntPtr Handle { get; internal set; }
        /// <summary>
        /// Gets process handle count.
        /// </summary>
        public int HandleCount { get; internal set; }
        /// <summary>
        /// Gets Main Window Handle.
        /// </summary>
        public IntPtr MainWindowHandle { get; internal set; }
        /// <summary>
        /// Gets Main Window Title.
        /// </summary>
        public string MainWindowTitle { get; internal set; }
        /// <summary>
        /// Checks is current process.
        /// </summary>
        public bool IsCurrent
        {
            get
            {
                lock (this)
                {
                    return (null != Current) ? this.Id == Current.Id : false;
                }
            }
        }
        /// <summary>
        /// Gets is current process symbol in string.
        /// </summary>
        public string IsCurrentSymbol { get { return (IsCurrent) ? "#" : string.Empty; } }
        /// <summary>
        /// Checks is handle accessible.
        /// </summary>
        public bool Accessible { get; internal set; }
        /// <summary>
        /// Gets Error message (when get information from System.Diagnostics.Process).
        /// </summary>
        public string ErrorMessage { get; internal set; }

        #endregion

        #region Static Methods

        /// <summary>
        /// Create.
        /// </summary>
        /// <param name="process">The System.Diagnostics.Process process instance.</param>
        /// <returns>Rerurns new instance of NProcess.</returns>
        public static NProcess Create(System.Diagnostics.Process process)
        {
            return new NProcess(process);
        }

        #endregion
    }

    public static class ProcessUtils
    {
        public static NProcess Create(this System.Diagnostics.Process process)
        {
            return NProcess.Create(process);
        }
        public static System.Diagnostics.Process Current { get { return NProcess.Current; } }

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
