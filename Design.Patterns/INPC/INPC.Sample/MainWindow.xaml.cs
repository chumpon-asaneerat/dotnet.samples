﻿#region Using

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

    #region INotifyPropertyChanged Extensions

    // use INotifyPropertyChanged Extensions
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

    #endregion
}
