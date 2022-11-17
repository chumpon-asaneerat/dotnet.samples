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

namespace WpfShapeSample1
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
            InitShapes();
        }

        #endregion

        #region Private Methods

        private void AddShape(Shape shape)
        {
            if (null == shape) return;
            canvas.Children.Add(shape);
        }

        private void InitShapes()
        {
            // Add a Line Element
            var shape = new Line();
            shape.Stroke = Brushes.LightSteelBlue;
            shape.X1 = 1;
            shape.X2 = 50;
            shape.Y1 = 1;
            shape.Y2 = 50;
            shape.HorizontalAlignment = HorizontalAlignment.Left;
            shape.VerticalAlignment = VerticalAlignment.Center;
            shape.StrokeThickness = 2;

            AddShape(shape);
        }

        #endregion
    }
}
