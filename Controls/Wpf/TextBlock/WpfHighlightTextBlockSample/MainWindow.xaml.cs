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

namespace WpfHighlightTextBlockSample
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

            Setup();
        }

        #endregion

        #region Internal Class and Variable

        class Search : INotifyPropertyChanged
        {
            private string _filter = string.Empty;
            public string Filter 
            {
                get { return _filter; }
                set
                {
                    if (_filter != value)
                    {
                        _filter = value;
                        if (null != this.PropertyChanged)
                        {
                            this.PropertyChanged(this, new PropertyChangedEventArgs("Filter"));
                        }
                    }
                }
            }

            private string _description = string.Empty;
            public string Description
            {
                get { return _description; }
                set 
                {
                    if (_description != value)
                    {
                        _description = value;
                        if (null != this.PropertyChanged)
                        {
                            this.PropertyChanged(this, new PropertyChangedEventArgs("Description"));
                        }
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        private Search _search;

        #endregion

        #region Button Handlers

        private void cmdFilter_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        #region Private Methods

        private void Setup()
        {
            _search = new Search();
            _search.Filter = "is";
            // for converter
            //_search.Description = "Lorem Ipsum |~S~|is|~E~| simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";
            // for behavior
            _search.Description = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";

            this.DataContext = _search;
        }

        #endregion
    }
}
