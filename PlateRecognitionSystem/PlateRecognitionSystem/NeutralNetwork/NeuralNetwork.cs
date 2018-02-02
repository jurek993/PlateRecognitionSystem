﻿using PlateRecognitionSystem.Model;
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
                foreach (KeyValuePair<string, double[]> pattern in _trainingSet)
                {
                    var dictonaryListForOneChar = _distractionTrainingSet.Where(x => x.Key[0].ToString() == pattern.Key[0].ToString()).ToList();
                    dictonaryListForOneChar.Add(pattern);
                    foreach (var dictonary in dictonaryListForOneChar)
                    {
                        _neuralNet.ForwardPropagate(dictonary.Value, pattern.Key);
                        _neuralNet.BackPropagate();
                        currentError += _neuralNet.GetError();
                    }
                }

                currentIteration++;

                if (currentIteration % 5 == 0)
                {
                    ViewModel.CurrentError = currentError;
                    ViewModel.CurrentIteration = currentIteration;
                }
                if (currentIteration % 50 == 0)
                {
                    List<string> removed = new List<string>();
                    bool isOver = true;
                    foreach (var samplePattern in _settingsModel.SampleTrainingSet)
                    {
                        Recognize(samplePattern.Value, _recognizeModel);
                        if (samplePattern.Key[0].Equals(_recognizeModel.MatchedHigh[0]) && _recognizeModel.OutputHightValue >= 0.9)
                        {
                            removed.Add(samplePattern.Key);
                            _trainingSet.Remove(samplePattern.Key);
                        }
                        else
                        {
                            isOver = false;
                        }
                    }
                    if (isOver)
                    {
                        foreach (var patternToremove in removed)
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
