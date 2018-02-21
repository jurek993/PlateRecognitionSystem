using PlateRecognitionSystem.Model;
using PlateRecognitionSystem.NeutralNetwork.Layers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace PlateRecognitionSystem.NeutralNetwork
{
    [Serializable]
    public class NeuralNetwork : AbstractHelperClass
    {
        public int MaximumIteration { get; set; } = 10000;
        public MainViewModel ViewModel { get;set; }
        private GlobalSettingsModel _settingsModel;
        private Dictionary<string, double[]> _trainingSet;
        private IBackPropagation _neuralNet;
        private Dictionary<string, double[]> _distractionTrainingSet;
        private RecognizeModel _recognizeModel = new RecognizeModel();

        public NeuralNetwork(IBackPropagation IBackPro,  GlobalSettingsModel settingsModel)
        {
            _neuralNet = IBackPro;
            _trainingSet = settingsModel.TrainingSet;
            _distractionTrainingSet = settingsModel.DistractionTrainingSet;
            _neuralNet.InitializeNetwork(_trainingSet);
            _settingsModel = settingsModel;
        }
        public NeuralNetwork()
        {

        }

        public bool Train()
        {
            double currentError = 0;
            int currentIteration = 0;
            NeuralEventArgs Args = new NeuralEventArgs();

            do
            {
                currentError = 0;
                Random r = new Random();
                foreach (KeyValuePair < string, double[]> pattern in _distractionTrainingSet.OrderBy(x => r.Next()))
                {
                    _neuralNet.ForwardPropagate(pattern.Value, pattern.Key[0].ToString());
                    _neuralNet.BackPropagate();
                    currentError += _neuralNet.GetError();
                }
                currentIteration++;

                if (currentIteration % 5 == 0)
                {
                    ViewModel.CurrentError = currentError;
                    ViewModel.CurrentIteration = currentIteration;
                }
                if (currentIteration % 10 == 0)
                {
                    List<string> toRemoved = new List<string>();
                    bool isOver = true;
                    foreach (var samplePattern in _settingsModel.SampleTrainingSet)
                    {
                        Recognize(samplePattern.Value, _recognizeModel);
                        if (samplePattern.Key[0].Equals(_recognizeModel.MatchedHigh[0]) && _recognizeModel.OutputHightValue >= 0.1)
                        {
                            toRemoved.Add(samplePattern.Key);
                           // _trainingSet.Remove(samplePattern.Key);
                        }
                        else
                        {
                           // currentError = 0.1;
                            
                            isOver = false;
                            //isOver = true;
                        }
                    }
                    if (isOver)
                    {
                        foreach (var patternToremove in toRemoved)
                        {
                            _settingsModel.SampleTrainingSet.Remove(patternToremove);
                        }
                    }

                }

            } while (currentError > ViewModel.MaximumError && currentIteration < MaximumIteration);

            if (Math.Abs(currentError) < 0.001 && currentIteration != 0)
            {
                ViewModel.CurrentError = currentError;
                ViewModel.CurrentIteration = currentIteration;
            }

            if (currentIteration >= MaximumIteration)
                return false;//Training Not Successful

            return true;
        }

        public void Recognize(double[] Input, RecognizeModel recognizeModel)
        {
            _neuralNet.Recognize(Input,  recognizeModel);
        }

        public void SaveNetwork(string path)
        {
            FileStream fileStream = new FileStream(path, FileMode.Create);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fileStream, _neuralNet);
            fileStream.Close();
        }

        public void LoadNetwork(string path)
        {
            FileStream fileStream = new FileStream(path, FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            _neuralNet = (IBackPropagation)binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
        }
    }
}
