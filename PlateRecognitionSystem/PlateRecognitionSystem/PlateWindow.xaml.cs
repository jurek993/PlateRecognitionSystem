using Emgu.CV;
using PlateRecognitionSystem.Initialize;
using PlateRecognitionSystem.Model;
using PlateRecognitionSystem.NeutralNetwork;
using PlateRecognitionSystem.NeutralNetwork.NeuronComponents;
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
        private MainViewModel _model;
        private InitializeNeutralNetwork _initializeNeutralNetwork;
        private GlobalSettings _globalSettings;
        private Mat _mat;
        public bool IsRecognise { get; set; }
        public PlateWindow(MainViewModel viewModel, Mat mat, InitializeNeutralNetwork initializeNetwork, GlobalSettings settings)
        {
            _model = viewModel;
            _mat = mat;
            _initializeNeutralNetwork = initializeNetwork;
            _globalSettings = settings;
            RecognizePlate();
            InitializeComponent();
        }
        public PlateWindow()
        {
            IsRecognise = false;
        }
        public void RecognizePlate()
        {
            PlateViewModel plateViewModel = new PlateViewModel
            {
                MainViewModel = _model,
                Mat = _mat
            };
            plateViewModel.MainViewModel.LogTextBox = String.Empty;
            DataContext = plateViewModel;

            ProcessingPlate processingPlate = new ProcessingPlate();
            processingPlate.DetectLicensePlate(plateViewModel);
            InitializeRecognition initializeRecognition = new InitializeRecognition(plateViewModel.MainViewModel, _initializeNeutralNetwork, _globalSettings);
            IsRecognise =  initializeRecognition.Recognize(plateViewModel.filteredCharatersInSinglePlate);
        }
    }
}
