using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Microsoft.Win32;
using PlateRecognitionSystem.Extension;
using PlateRecognitionSystem.Image;
using PlateRecognitionSystem.Initialize;
using PlateRecognitionSystem.Model;
using PlateRecognitionSystem.NeutralNetwork;
using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PlateRecognitionSystem
{
    public partial class MainWindow : Window
    {
        private GlobalSettings _settings = null;
        private delegate bool TrainingCallBack();
        private InitializeNeutralNetwork _initializeNetwork;
        private RecognizeModel<string> _recognizeModel;
        public ViewModel Model;
        public MainWindow()
        {
            InitializeComponent();
            Model = new ViewModel
            {
                LogTextBox = "Uruchomienie programu",
                MaximumError = Double.Parse(ConfigurationManager.AppSettings["DefaultMaximumError"]),
                LearningRate = Double.Parse(ConfigurationManager.AppSettings["LearningRate"]),
                TrainingSuccess = false,
                ImageLoaded = false
            };
            DataContext = Model;
            _settings = new GlobalSettings(Model);
            _settings.InitializeSettings();
            _settings.GenerateTrainingSet();
        }

        private void comboBoxLayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var numberOfLayers = int.Parse((comboBoxLayers.SelectedItem as ComboBoxItem).Content.ToString());
            if (numberOfLayers > 0){
                _settings.SettingsModel.NumberOfLayers = numberOfLayers - 1 ;
            } else
            {
                _settings.SettingsModel.NumberOfLayers = numberOfLayers;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Model.LogTextBox += "\nRozpoczęcie nauki sieci";
            _initializeNetwork = new InitializeNeutralNetwork(_settings.SettingsModel);
            TrainingNetwork<string> trainingNetwork = new TrainingNetwork<string>(_initializeNetwork.NeuralNetwork, Model);
            trainingNetwork.StartTraining();
        }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Bitmap Image(*.bmp)|*.bmp";

            if (openFileDialog.ShowDialog() == true)
            {
                string FileName = openFileDialog.FileName;
                if (Path.GetExtension(FileName) == ".bmp")
                {
                    LoadedImage.Source = new Bitmap(
                        new Bitmap(FileName), (int)LoadedImage.Width, (int)LoadedImage.Height).ToBitmapImage();
                    Model.ImageLoaded = true;
                }
            }
        }

        private void RecognizeButton_Click(object sender, RoutedEventArgs e)
        {
            _recognizeModel = new RecognizeModel<string> { MatchedHigh = "?", MatchedLow = "?" };
            var bitmapImage = LoadedImage.Source as BitmapImage;
            double[] input = ImageProcessing.ToMatrix(bitmapImage.ToBitmap(),
                _settings.SettingsModel.AverageImageHeight, _settings.SettingsModel.AverageImageWidth);
            _initializeNetwork.NeuralNetwork.Recognize(input, _recognizeModel);
            Model.MatchedHightValue = _recognizeModel.MatchedHigh;
            Model.MatchedLowValue = _recognizeModel.MatchedLow;
            Model.MatchedHightPercent = Convert.ToInt32((100 * _recognizeModel.OutputHightValue)).ToString() + " %";
            Model.MatchedLowPercent = Convert.ToInt32((100 * _recognizeModel.OutputLowValue)).ToString() + " %";
        }

        private void SaveNetwork_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Neural Network File(*.jur)|*.jur";
            if (saveFileDialog.ShowDialog() == true)
            {
                _initializeNetwork.NeuralNetwork.SaveNetwork(saveFileDialog.FileName);
            }
        }

        private void LoadNetwork_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Neural Network File(*.jur)|*.jur";
            if (openFileDialog.ShowDialog() == true)
            {
                _initializeNetwork = new InitializeNeutralNetwork();
                _initializeNetwork.NeuralNetwork = new NeuralNetwork<string>();
                _initializeNetwork.NeuralNetwork.LoadNetwork(openFileDialog.FileName);
                Model.TrainingSuccess = true;
            }
        }

        private void RecognizePlateButton_Click(object sender, RoutedEventArgs e)
        {
            var bitmapImage = LoadedImage.Source as BitmapImage;
            Image<Bgr, Byte> imageCV = new Image<Bgr, byte>(bitmapImage.ToBitmap()); 
            Mat mat = imageCV.Mat;
            PlateWindow plateWindow = new PlateWindow();
            plateWindow.Show();
        }


    }
}
