#region Using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

#endregion

namespace Wpf.Process.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Internal Variables

        private DispatcherTimer _timer = null;
        private ObservableCollection<ProcessEx> _items = new ObservableCollection<ProcessEx>();

        #endregion

        #region Loaded/Unloaded

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            grid.ItemsSource = _items;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(1000);
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            if (null != _timer)
            {
                _timer.Stop();
                _timer.Tick -= _timer_Tick;
            }
            _timer = null;
        }

        #endregion

        #region Button Handlers

        private void cmdFreeze60_Click(object sender, RoutedEventArgs e)
        {
            Thread.Sleep(TimeSpan.FromSeconds(60));
        }

        private void cmdFreezeForever_Click(object sender, RoutedEventArgs e)
        {
            Thread.Sleep(-1);
        }

        #endregion

        #region Timer Handler

        private void _timer_Tick(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            string processName = ProcessUtils.Current.ProcessName;
            var processes = ProcessUtils.GetProcesses(processName);
            int processCount = (null != processes) ? processes.Length : 0;

            TimeSpan ts = DateTime.Now - dt;

            txtProcessName.Text = processName;
            txtProcessCount.Text = processCount.ToString("n0");

            string execTime = string.Format("Execute time {0:n0} ms.", ts.TotalMilliseconds);
            txtExecTime.Text = execTime;

            string msg = string.Format("Updated {0:HH:mm:ss.fff}", DateTime.Now);
            txtLastUpdated.Text = msg;

            RefreshGrid(processes);
        }

        #endregion

        #region Private Methods

        public void RefreshGrid(System.Diagnostics.Process[] processes)
        {
            object lockObj = new object();

            grid.ItemsSource = null;

            List<ProcessEx> procs = new List<ProcessEx>();
            foreach (var proc in processes)
            {
                procs.Add(proc.Create());
            }

            grid.ItemsSource = procs;

            /*
            // Sync collections
            if (null != processes && null != _items)
            {
                foreach (var proc in procs)
                {
                    if (null != proc)
                    {
                        int idx = _items.IndexOf(proc);
                        if (idx == -1)
                        {
                            // not found so add new.
                            lock (lockObj)
                            {
                                _items.Add(proc);
                            }
                        }
                        else
                        {
                            // found update
                            lock (lockObj)
                            {
                                _items[idx].Assign(proc);
                            }
                        }
                    }
                }

                // Remove not exists
                List<ProcessEx> removes = new List<ProcessEx>();
                foreach (var proc in _items)
                {
                    int idx = procs.IndexOf(proc);
                    if (idx == -1) 
                    {
                        removes.Add(proc); // to remove list
                    }
                }
                if (null != removes && removes.Count > 0)
                {
                    foreach (var proc in removes) 
                    {
                        lock (lockObj)
                        {
                            _items.Remove(proc);
                        }
                    }
                }
            }
            */
        }

        #endregion
    }
}
