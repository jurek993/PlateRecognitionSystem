using PlateRecognitionSystem.Extension;
using PlateRecognitionSystem.Image;
using PlateRecognitionSystem.Initialize;
using PlateRecognitionSystem.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PlateRecognitionSystem.NeutralNetwork.NeuronComponents
{
    public class InitializeRecognition
    {
        private RecognizeModel _recognizeModel;
        private InitializeNeutralNetwork _initializeNetwork;
        private MainViewModel _viewModel;
        private GlobalSettings _settings;

        public InitializeRecognition(MainViewModel viewModel, InitializeNeutralNetwork initializeNeutralNetwork, GlobalSettings globalSettings)
        {
            _viewModel = viewModel;
            _settings = globalSettings;
            _initializeNetwork = initializeNeutralNetwork;
        }
        public void Recognize(BitmapImage character)
        {
            StartRecognize(character.ToBitmap());
        }

        public void Recognize(ObservableCollection<ImageSource> characters)
        {
            foreach (var character in characters)
            {
                if(character!= null)
                {
                    BitmapImage bitmapImageCharacter = character as BitmapImage;
                    StartRecognize(bitmapImageCharacter.ToBitmap());
                }
                else
                {
                    _viewModel.LogTextBox += "\n";
                }
            }
        }

        private void StartRecognize(Bitmap character)
        {
            _recognizeModel = new RecognizeModel { MatchedHigh = "?", MatchedLow = "?" };
            double[] input = ImageProcessing.ToMatrix(character,
            _settings.SettingsModel.AverageImageHeight, _settings.SettingsModel.AverageImageWidth);
            _initializeNetwork.NeuralNetwork.Recognize(input, _recognizeModel);
            _viewModel.MatchedHightValue = _recognizeModel.MatchedHigh;
            _viewModel.MatchedLowValue = _recognizeModel.MatchedLow;
            _viewModel.MatchedHightPercent = Convert.ToInt32((100 * _recognizeModel.OutputHightValue)).ToString() + " %";
            _viewModel.MatchedLowPercent = Convert.ToInt32((100 * _recognizeModel.OutputLowValue)).ToString() + " %";
            _viewModel.LogTextBox += String.Format("Rozpoznano literę {0} - {1}, Drugie miejsce litera {2} - {3}\n",
                _viewModel.MatchedHightValue, _viewModel.MatchedHightPercent, _viewModel.MatchedLowValue, _viewModel.MatchedLowPercent);
        }
    }
}
