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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlateRecognitionSystem.Initialize
{

    //TODO: sprawdz pesa coding style ;) 

    public class GlobalSettings
    {
        public GlobalSettingsModel SettingsModel { get; set; }


        //private int _averageImageHeight = 0;
        //private int _averageImageWidth = 0;
        //private int _numOfPatterns = 0;
        //private Dictionary<string, double[]> _trainingSet = null;
        //public string Path;

        public GlobalSettings() 
        {
            SettingsModel = new GlobalSettingsModel();
            NameValueCollection appSettings = ConfigurationManager.AppSettings;
            SettingsModel.PatternPath = System.IO.Path.GetFullPath(appSettings["PatternsDirectory"]);
        }

        public void InitializeSettings()
        {
            //textBoxState.AppendText("Initializing Settings..");

            try
            {
                //comboBoxLayers.SelectedIndex = (Int16.Parse(AppSettings["NumOfLayers"]) - 1);

                //textBoxMaxError.Text = AppSettings["MaxError"];

                string[] images = Directory.GetFiles(SettingsModel.PatternPath, "*.bmp");
                SettingsModel.NumberOfPatterns = images.Length;

                SettingsModel.AverageImageHeight = 0;
                SettingsModel.AverageImageWidth = 0;

                foreach (string image in images)
                {
                    Bitmap temp = new Bitmap(image);
                    SettingsModel.AverageImageHeight += temp.Height;
                    SettingsModel.AverageImageWidth += temp.Width;
                    temp.Dispose();
                }
                SettingsModel.AverageImageHeight /= SettingsModel.NumberOfPatterns;
                SettingsModel.AverageImageWidth /= SettingsModel.NumberOfPatterns;

                int networkInput = SettingsModel.AverageImageWidth * SettingsModel.AverageImageHeight;

                // textBoxInputUnit.Text = ((int)((double)(networkInput + NumOfPatterns) * .33)).ToString();
                // textBoxHiddenUnit.Text = ((int)((double)(networkInput + NumOfPatterns) * .11)).ToString();
                // textBoxOutputUnit.Text = NumOfPatterns.ToString();
            }
            catch (Exception ex)
            {
                //TODO: dodać logi
                MessageBox.Show("Error Initializing Settings: " + ex.Message, "Error");
            }
            //  textBoxState.AppendText("Done!\r\n");
        }

        public void GenerateTrainingSet()
        {
            // textBoxState.AppendText("Generating Training Set..");

            string[] patterns = Directory.GetFiles(SettingsModel.PatternPath, "*.bmp");

           SettingsModel.TrainingSet = new Dictionary<string, double[]>(patterns.Length);
            foreach (string pattern in patterns)
            {
                Bitmap temp = new Bitmap(pattern);
                SettingsModel.TrainingSet.Add(System.IO.Path.GetFileNameWithoutExtension(pattern),
                    ImageProcessing.ToMatrix(temp, SettingsModel.AverageImageHeight, SettingsModel.AverageImageWidth));
                temp.Dispose();
            }
            // textBoxState.AppendText("Done!\r\n");
        }

    }
}
