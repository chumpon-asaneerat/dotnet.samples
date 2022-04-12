#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
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
            txtTotal.DataContext = item;
        }

        private void cmdTest_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    #region INPCNet40
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
    #endregion

    #region INPCNet45
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
    #endregion

    #region INPCNet40LINQ
    /*
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
    */
    #endregion

    #region INPCNet40v2
    /*
    public class Item : INPCNet40v2
    {
        private int _total = 0;
        private string _value = string.Empty;
        public string Value
        {
            get { return _value; }
            set 
            {
                IfChanged(ref _value, value)
                    .Then((owner) =>
                    {
                        Console.WriteLine("Value changed. old: {0}, new {1}", owner.OldValue, owner.NewValue);
                    })
                    .Raise()
                    .Then((owner) =>
                    {
                        _total++;
                        Console.WriteLine("After Raise Event.", owner.OldValue, owner.NewValue);
                    });
                this.Raise(() => Total );
            }
        }

        public int Total { get { return _total; } set { }  }
    }
    */
    #endregion

    #region INotifyPropertyChanged Extensions v1

    // use INotifyPropertyChanged Extensions v1
    /*
    public class Item : System.ComponentModel.INotifyPropertyChanged
    {
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _value = string.Empty;
        public string Value
        {
            get { return _value; }
            set
            {
                if (this.SetPropertyAndNotify(PropertyChanged, ref _value, value))
                {
                    Console.WriteLine("Value changed.");
                }
                else
                {
                    Console.WriteLine("No change.");
                }
            }
        }
    }
    */
    #endregion

    #region INotifyPropertyChanged Extensions v2

    // use INotifyPropertyChanged Extensions v2
    /*
    public class Item : System.ComponentModel.INotifyPropertyChanged
    {
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _value = string.Empty;
        public string Value
        {
            get { return _value; }
            set
            {
                this.IfChanged(ref _value, value)
                    .Then((owner) => 
                    {
                        Console.WriteLine("Value changed. old: {0}, new {1}", owner.OldValue, owner.NewValue);
                    })
                    .Raise(PropertyChanged)
                    .Then((owner) => 
                    {
                        Console.WriteLine("After Raise Event.", owner.OldValue, owner.NewValue);
                    });

            }
        }
    }
    */
    #endregion

    #region INPCDMTv5

    public class Item : NInpc
    {
        private int _total = 0;
        private string _value = string.Empty;

        public string Value
        {
            get { return _value; }
            set 
            { 
                if (_value != value)
                {
                    _value = value;
                    ++_total;
                    Raise(() => Value, () => Total);
                }
            }
        }
        public int Total { get { return _total; } set { } }
    }

    #endregion
}
