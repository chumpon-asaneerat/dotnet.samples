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

namespace Wpf.Rest.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Loaded
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string baseUrl = @"http://localhost:8000/dmt-scw/api/v1";
            string apiUrl = @"version";
            int timeout = 1000 * 5;

            DateTime dt = DateTime.Now;
            Console.WriteLine("Call: {0}/{1}", baseUrl, apiUrl);
            var ret = NRestClient.Get(baseUrl, apiUrl, timeout);

            TimeSpan ts = DateTime.Now - dt;
            Console.WriteLine("Result: {0} time: {1:n3} s", ret, ts.TotalSeconds);
        }

        #endregion
    }
}
