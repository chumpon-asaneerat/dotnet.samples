#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

#endregion

namespace Singelton
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

        private DispatcherTimer timer1;
        private DispatcherTimer timer2;

        #region Loaded

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer1 = new DispatcherTimer();
            timer1.Interval = TimeSpan.FromMilliseconds(1000);
            timer1.Tick += Timer1_Tick;

            timer2 = new DispatcherTimer();
            timer2.Interval = TimeSpan.FromMilliseconds(1000);
            timer2.Tick += Timer2_Tick;

            timer1.Start();
            timer2.Start();
        }

        #endregion

        #region Timers

        private void Timer1_Tick(object sender, EventArgs e)
        {
            LocalDb.Instance.ShowMe();
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            LocalDb.Instance.ShowMe();
        }

        #endregion
    }

    public class LocalDb : NSingelton<LocalDb>
    {
        private int iCnt = 0;

        protected LocalDb() : base()
        {
            Console.WriteLine("Call Ctor.");
        }

        public void ShowMe()
        {
            iCnt++;
            Console.WriteLine("Call show me {0} at {1:HH:mm:ss.ffffff}", iCnt, DateTime.Now);
        }
    }
}
