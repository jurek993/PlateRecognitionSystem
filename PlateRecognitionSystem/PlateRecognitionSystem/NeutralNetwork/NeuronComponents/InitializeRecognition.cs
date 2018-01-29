using Emgu.CV;
using PlateRecognitionSystem.Extension;
using PlateRecognitionSystem.Image;
using PlateRecognitionSystem.Initialize;
using PlateRecognitionSystem.Model;
using System;
using System.Collections.Generic;
using PlateRecognitionSystem.Database;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using PlateRecognitionSystem.SignalRServer;

namespace PlateRecognitionSystem.NeutralNetwork.NeuronComponents
{
    public class InitializeRecognition
    {
        private RecognizeModel _recognizeModel;
        private InitializeNeutralNetwork _initializeNetwork;
        private MainViewModel _viewModel;
        private GlobalSettings _settings;
        private string _licencePlate = string.Empty;
        private string _loosedLicencePlate = string.Empty;
        private GarageDatabase _database;
        private PrepareDataForBoards _prepareDataForBoards;
        private SendDataToBoards _sendDataToBoards;

        public InitializeRecognition(MainViewModel viewModel, InitializeNeutralNetwork initializeNeutralNetwork, GlobalSettings globalSettings)
        {
            _viewModel = viewModel;
            _settings = globalSettings;
            _initializeNetwork = initializeNeutralNetwork;
            _database = new GarageDatabase(_viewModel);
        }
        public void Recognize(BitmapImage character)
        {
            StartRecognize(character.ToBitmap());
        }

        public bool Recognize(List<List<UMat>> charactersInPlates)
        {
            foreach (var charactersInSinglePlate in charactersInPlates)
            {
                foreach (var characters in charactersInSinglePlate)
                {
                    if (characters != null)
                    {
                        StartRecognize(characters.Bitmap);
                    }
                    else
                    {
                        _viewModel.LogTextBox += "\n";
                    }
                }
                _viewModel.LogTextBox += String.Format("\nRozpoznana tablica - {0}\n", _licencePlate);
                if(_licencePlate != string.Empty)
                {
                    var result = _database.CheckPermission(ref _licencePlate, _loosedLicencePlate);
                    if (result)
                    {
                        SingleVisit singleVisit = _database.SaveSingleVisit(_licencePlate);
                        _prepareDataForBoards = new PrepareDataForBoards(singleVisit);
                        _sendDataToBoards = new SendDataToBoards();
                        if(singleVisit.Vehicle.ExpirationDate != null && singleVisit.Vehicle.ExpirationDate >= DateTime.Now)
                        {
                            _viewModel.LogTextBox += String.Format("Abonament - Otwarcie szlabanu dla pojazdu o numerach {0}\n", _licencePlate);
                            var dataForBoards = _prepareDataForBoards.DataForSubscriptionViewModel();
                            _sendDataToBoards.SubscriberData(dataForBoards);
                            SendNormalMessageAfter10Second(dataForBoards.Boards.SingleOrDefault().FunctionName);
                        }
                        else
                        {
                            _viewModel.LogTextBox += String.Format("Wjazd jednorazowy - Otwarcie szlabanu dla pojazdu o numerach {0}\n", _licencePlate);
                            var  dataForBoards = _prepareDataForBoards.DataForGuestBoard();
                            _sendDataToBoards.GuestData(dataForBoards);
                            SendNormalMessageAfter10Second(dataForBoards.Boards.SingleOrDefault().FunctionName);
                        }
                        return true;
                    }
                }
                _licencePlate = string.Empty;
            }
            return false;
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
            _licencePlate += _viewModel.MatchedHightValue;
            _loosedLicencePlate += _viewModel.MatchedLowValue;
        }

        private void SendNormalMessageAfter10Second(string boardName)
        {
            System.Threading.Thread.Sleep(10000);
            var message = _prepareDataForBoards.DataForNormalMessage(boardName);
            _sendDataToBoards.NormalMessage(message);
        }
    }
}
