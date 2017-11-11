using Emgu.CV;
using PlateRecognitionSystem.Model;
using PlateRecognitionSystem.Plate;
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
using System.Windows.Shapes;

namespace PlateRecognitionSystem
{
    /// <summary>
    /// Interaction logic for PlateWindow.xaml
    /// </summary>
    public partial class PlateWindow : Window
    {
        public PlateWindow(MainViewModel viewModel, Mat mat)
        {
            PlateViewModel plateViewModel = new PlateViewModel
            {
                MainViewModel = viewModel,
                Mat = mat
            };
            DataContext = plateViewModel;
            InitializeComponent();
            ProcessingPlate processingPlate = new ProcessingPlate();
            processingPlate.DetectLicensePlate(plateViewModel);
        }
    }
}
