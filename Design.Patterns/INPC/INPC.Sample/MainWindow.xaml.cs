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

namespace INPC.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private Item item = new Item();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtSrc.DataContext = item;
            txtDst.DataContext = item;
        }
    }

    /*
    public class Item : INPCNet40
    {
        private string _value = string.Empty;
        public string Value
        {
            get { return _value; }
            set { SetField(ref _value, value, "Value"); }
        }
    }
    */
    /*
    public class Item : INPCNet45
    {
        private string _value = string.Empty;
        public string Value
        {
            get { return _value; }
            set { SetField(ref _value, value); }
        }
    }
    */
    public class Item : INPCNet40LINQ
    {
        private int _total = 0;
        private string _value = string.Empty;
        public string Value
        {
            get { return _value; }
            set 
            { 
                SetField(ref _value, value, 
                    () => Value, 
                    () => Total ); 
            }
        }

        public int Total { get { return _total; } }
    }
}
