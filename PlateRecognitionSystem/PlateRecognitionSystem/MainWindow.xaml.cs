using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Microsoft.AspNet.SignalR;
using Microsoft.Win32;
using PlateRecognitionSystem.Extension;
using PlateRecognitionSystem.Image;
using PlateRecognitionSystem.Initialize;
using PlateRecognitionSystem.Model;
using PlateRecognitionSystem.NeutralNetwork;
using PlateRecognitionSystem.NeutralNetwork.NeuronComponents;
using PlateRecognitionSystem.Plate;
using PlateRecognitionSystem.SignalRServer;
using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WebcamControl;

namespace PlateRecognitionSystem
{
    public partial class MainWindow : Window
    {
        private GlobalSettings _settings = null;
        private delegate bool TrainingCallBack();
        private InitializeNeutralNetwork _initializeNetwork;
        public MainViewModel Model;
        public MainWindow()
        {
            InitializeComponent();
            GarageDBContext dBContext = new GarageDBContext();

            Model = new MainViewModel
            {
                LogTextBox = "Uruchomienie programu",
                MaximumError = Double.Parse(ConfigurationManager.AppSettings["DefaultMaximumError"]),
                LearningRate = Double.Parse(ConfigurationManager.AppSettings["DefaultLearningRate"]),
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
            if (numberOfLayers > 0)
            {
                _settings.SettingsModel.NumberOfLayers = numberOfLayers - 1;
            }
            else
            {
                _settings.SettingsModel.NumberOfLayers = numberOfLayers;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Model.LogTextBox += "\nRozpoczęcie nauki sieci";
            _initializeNetwork = new InitializeNeutralNetwork(_settings.SettingsModel, Model);
            TrainingNetwork trainingNetwork = new TrainingNetwork(_initializeNetwork.NeuralNetwork, Model);
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
                    Model.Image = new Bitmap(
                        new Bitmap(FileName)).ToBitmapImage();
                    Model.ImageLoaded = true;
                }
            }
        }

        private void RecognizeButton_Click(object sender, RoutedEventArgs e)
        {
            Model.LogTextBox = String.Empty;
            InitializeRecognition recognition = new InitializeRecognition(Model, _initializeNetwork, _settings);
            BitmapImage bitmapImage = Model.Image as BitmapImage;
            recognition.Recognize(bitmapImage);
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
                _initializeNetwork.NeuralNetwork = new NeuralNetwork();
                _initializeNetwork.NeuralNetwork.LoadNetwork(openFileDialog.FileName);
                Model.TrainingSuccess = true;
            }
        }

        private void RecognizePlateButton_Click(object sender, RoutedEventArgs e)
        {
            var bitmapImage = Model.Image as BitmapImage;
            Image<Bgr, Byte> imageCV = new Image<Bgr, byte>(bitmapImage.ToBitmap());
            Mat mat = imageCV.Mat;
            PlateWindow plateWindow = new PlateWindow(Model, mat, _initializeNetwork, _settings);
            plateWindow.Show();
        }

        private void RecognizePlateFromCameraButton_Copy_Click(object sender, RoutedEventArgs e)
        {
            CameraWindow cameraWindow = new CameraWindow(Model, _initializeNetwork, _settings);
            cameraWindow.Show();
        }
    }
}
