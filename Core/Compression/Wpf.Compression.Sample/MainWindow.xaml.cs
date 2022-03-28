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
// zip required
using System.IO.Compression;

#endregion

namespace Wpf.Compression.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        private void cmdCompress_Click(object sender, RoutedEventArgs e)
        {
            string path = string.Empty;
            string targetFile = string.Empty;
            using (var od = new System.Windows.Forms.FolderBrowserDialog())
            {
                od.RootFolder = Environment.SpecialFolder.Desktop;
                od.Description = "Select source folder";
                if (od.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    path = od.SelectedPath;
                }
            }

            if (string.IsNullOrWhiteSpace(path)) return;

            using(var fd = new System.Windows.Forms.SaveFileDialog())
            {
                fd.Title = "Enter target file name";
                fd.CheckPathExists = true;
                fd.CheckFileExists = false;
                fd.SupportMultiDottedExtensions = true;
                fd.Filter = "Zip Files (*.zip)|*.zip|All Files (*.*)|*.*";
                fd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    targetFile = fd.FileName;
                }
            }

            if (string.IsNullOrWhiteSpace(targetFile)) return;

            ZipFile.CreateFromDirectory(path, targetFile, CompressionLevel.Fastest, true);

            MessageBox.Show("Success");
            /*
            using (ZipArchive zip = ZipFile.Open("test.zip", ZipArchiveMode.Create))
            {
                zip.CreateEntryFromFile(@"c:\something.txt", "data/path/something.txt");
            }
            */
        }
    }
}
