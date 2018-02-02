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
using PlateRecognitionSystem.Enums;

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
                            _viewModel.LogTextBox += String.Format("Abonament - Akcja dla pojazdu o numerach {0}\n", _licencePlate);
                            SubscriptionBoardViewModel dataForBoards = _prepareDataForBoards.DataForSubscriptionViewModel();
                            _sendDataToBoards.SubscriberData(dataForBoards);
                            var boardName = dataForBoards.Boards.SingleOrDefault().FunctionName; //TODO: na wyjeździe nie odświeża ilośc wolnych miejsc na wieździe 
                            _database.ChangeTheGarageOccupancy(boardName);
                            SendNormalMessageAfter10Second(boardName);
                        }
                        else
                        {
                            _viewModel.LogTextBox += String.Format("Klient jednorazowy - Akcja dla pojazdu o numerach {0}\n", _licencePlate);
                            var  dataForBoards = _prepareDataForBoards.DataForGuestBoard();
                            _sendDataToBoards.GuestData(dataForBoards);
                            var boardName = dataForBoards.Boards.SingleOrDefault().FunctionName;
                            _database.ChangeTheGarageOccupancy(boardName);
                            SendNormalMessageAfter10Second(boardName);
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
            if(_recognizeModel.MatchedHigh != "#")
            {
                _viewModel.MatchedHightValue = _recognizeModel.MatchedHigh;
                _viewModel.MatchedLowValue = _recognizeModel.MatchedLow;
                _viewModel.MatchedHightPercent = Convert.ToInt32((100 * _recognizeModel.OutputHightValue)).ToString() + " %";
                _viewModel.MatchedLowPercent = Convert.ToInt32((100 * _recognizeModel.OutputLowValue)).ToString() + " %";
                _viewModel.LogTextBox += String.Format("Rozpoznano literę {0} - {1}, Drugie miejsce litera {2} - {3}\n",
                    _viewModel.MatchedHightValue, _viewModel.MatchedHightPercent, _viewModel.MatchedLowValue, _viewModel.MatchedLowPercent);
                _licencePlate += _viewModel.MatchedHightValue;
                _loosedLicencePlate += _viewModel.MatchedLowValue;
            }
        }

        private void SendNormalMessageAfter10Second(TypeOfBoards boardName)
        {
            //it's not good solution. In real garage this block everething
            System.Threading.Thread.Sleep(10000);
            if(boardName == TypeOfBoards.ExitBoard) 
            {
                _sendDataToBoards.NormalMessage(_prepareDataForBoards.DataForNormalMessage(TypeOfBoards.EnterBoard));
            }
            var message = _prepareDataForBoards.DataForNormalMessage(boardName);
            _sendDataToBoards.NormalMessage(message);
        }
    }
}
