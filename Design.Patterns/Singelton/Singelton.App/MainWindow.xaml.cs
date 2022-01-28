#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        #region Loaded

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LocalDb.Instance.ShowMe();
            LocalDb.Instance.ShowMe();
            LocalDb.Instance.ShowMe();
        }

        #endregion
    }

    public class LocalDb : NSingelton<LocalDb>
    {
        private int iCnt = 0;

        protected LocalDb() : base() 
        {
            Console.WriteLine("Call constructor");
        }

        public void ShowMe()
        {
            iCnt++;
            Console.WriteLine("Call show me {0}", iCnt);
        }
    }
}
