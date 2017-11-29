using PlateRecognitionSystem.Image;
using PlateRecognitionSystem.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Windows;

namespace PlateRecognitionSystem.Initialize
{

    //TODO: sprawdz pesa coding style ;) 

    public class GlobalSettings
    {
        public GlobalSettingsModel SettingsModel { get; set; }
        private MainViewModel _viewModel;


        public GlobalSettings(MainViewModel viewModel)
        {
            _viewModel = viewModel;
            SettingsModel = new GlobalSettingsModel(_viewModel);
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            SettingsModel.PatternPath = System.IO.Path.GetFullPath(appSettings["PatternsDirectory"]);
            SettingsModel.DistractionsPatternPath = System.IO.Path.GetFullPath(appSettings["DistractionsPatternsDirectory"]);
            SettingsModel.TestSamplesPath = System.IO.Path.GetFullPath(appSettings["TestSamplesPatternsDirectory"]);
        }



        public void InitializeSettings()
        {
            _viewModel.LogTextBox += "\nInicjalizacja ustawień sieci";
            try
            {
                string[] images = Directory.GetFiles(SettingsModel.PatternPath, "*.bmp");
                string[] distractionImages = Directory.GetFiles(SettingsModel.DistractionsPatternPath, "*.bmp");
                SettingsModel.NumberOfPatterns = images.Length;
                _viewModel.NumberOfDistractionPatterns = distractionImages.Length;
                List<string[]> listOfImages = new List<string[]> { images, distractionImages };
                SettingsModel.AverageImageHeight = 0;
                SettingsModel.AverageImageWidth = 0;
                foreach (var imagesArray in listOfImages)
                {
                    foreach (string image in imagesArray)
                    {
                        Bitmap temp = new Bitmap(image);
                        SettingsModel.AverageImageHeight += temp.Height;
                        SettingsModel.AverageImageWidth += temp.Width;
                        temp.Dispose();
                    }
                }
                SettingsModel.AverageImageHeight /= (SettingsModel.NumberOfPatterns + distractionImages.Length);
                SettingsModel.AverageImageWidth /= (SettingsModel.NumberOfPatterns + distractionImages.Length);
                //int networkInput = SettingsModel.AverageImageWidth * SettingsModel.AverageImageHeight;

                // TODO: dodac do do UI textBoxInputUnit.Text = ((int)((double)(networkInput + NumOfPatterns) * .33)).ToString();
                // textBoxHiddenUnit.Text = ((int)((double)(networkInput + NumOfPatterns) * .11)).ToString();
                // textBoxOutputUnit.Text = NumOfPatterns.ToString();
            }
            catch (Exception ex)
            {
                //TODO: dodać logi
                MessageBox.Show("Error Initializing Settings: " + ex.Message, "Error");
            }
            _viewModel.LogTextBox += " - Skończone!\r\n";
        }

        public void GenerateTrainingSet()
        {
            _viewModel.LogTextBox += "\nPrzygotowanie elementów do nauki";
            string[] patterns = Directory.GetFiles(SettingsModel.PatternPath, "*.bmp");
            string[] distractionsPatterns = Directory.GetFiles(SettingsModel.DistractionsPatternPath, "*.bmp");
            string[] testPaterns = Directory.GetFiles(SettingsModel.TestSamplesPath, "*bmp");
            SettingsModel.TrainingSet = new Dictionary<string, double[]>(patterns.Length);
            SettingsModel.DistractionTrainingSet = new Dictionary<string, double[]>(distractionsPatterns.Length);
            SettingsModel.SampleTrainingSet = new Dictionary<string, double[]>(testPaterns.Length);
            foreach (string pattern in patterns)
            {
                AddImageToDictonary(pattern, SettingsModel.TrainingSet);
            }
            foreach (string pattern in distractionsPatterns)
            {
                AddImageToDictonary(pattern, SettingsModel.DistractionTrainingSet);
            }
            foreach(string pattern in testPaterns)
            {
                AddImageToDictonary(pattern, SettingsModel.SampleTrainingSet);
            }
            _viewModel.LogTextBox += " - Skończone!\r\n";
        }

        private void AddImageToDictonary(string pattern, Dictionary<string,double[]> dictionary) 
        {
            Bitmap temp = new Bitmap(pattern);
            dictionary.Add(Path.GetFileNameWithoutExtension(pattern),
                ImageProcessing.ToMatrix(temp, SettingsModel.AverageImageHeight, SettingsModel.AverageImageWidth));
            temp.Dispose();
        }
    }
}
