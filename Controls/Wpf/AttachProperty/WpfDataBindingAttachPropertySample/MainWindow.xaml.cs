#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace WpfDataBindingAttachPropertySample
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
    }

    public class Example : INotifyPropertyChanged
    {
        private string _text = string.Empty;


        private void RaisePropertyChanged(string propertyName)
        {
            if (null != PropertyChanged) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Text
        {
            get { return _text;  }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    RaisePropertyChanged("Text");
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
