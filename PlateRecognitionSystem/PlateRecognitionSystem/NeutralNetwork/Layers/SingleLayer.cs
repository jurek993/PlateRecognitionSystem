using PlateRecognitionSystem.NeutralNetwork.NeuronComponents;
using System;
using System.Collections.Generic;
using PlateRecognitionSystem.Model;

namespace PlateRecognitionSystem.NeutralNetwork.Layers
{
    [Serializable]
    public class SingleLayer<T> :  AbstractHelperClass<T>,  IBackPropagation<T> where T : IComparable<T>
    {
        private int _preInputNum;
        private int _outputNum;
        private FirstLayerInput[] _preInputLayer;
        private Output<T>[] _outputLayer;


        public SingleLayer(int preInputNum, int outputNum)
        {
            _preInputNum = preInputNum;
            _outputNum = outputNum;
            _preInputLayer = new FirstLayerInput[_preInputNum];
            _outputLayer = new Output<T>[_outputNum];
        }

        public void BackPropagate()
        {
            //Update The First Layer's Weights
            for (int j = 0; j < _outputNum; j++)
            {
                for (int i = 0; i < _preInputNum; i++)
                {
                    _preInputLayer[i].Weights[j] += LearningRate * (_outputLayer[j].Delta) * _preInputLayer[i].Value;
                } //TODO: dodać learningInput aby miało wpływ na uczenie
            }
        }



        public void ForwardPropagate(double[] pattern, T output)
        {
            int i, j;
            double total = 0.0;

            //Apply input to the network
            for (i = 0; i < _preInputNum; i++)
            {
                _preInputLayer[i].Value = pattern[i];
            }

            //Calculate The First(Output) Layer's Inputs, Outputs, Targets and Errors
            for (i = 0; i < _outputNum; i++)
            {
                total = 0.0;
                for (j = 0; j < _preInputNum; j++)
                {
                    total += _preInputLayer[j].Value * _preInputLayer[j].Weights[i];
                }
                _outputLayer[i].InputSum = total;
                _outputLayer[i].output = F(total);
                _outputLayer[i].Target = _outputLayer[i].Value.CompareTo(output) == 0 ? 1.0 : 0.0;
                _outputLayer[i].Delta = (_outputLayer[i].Target - _outputLayer[i].output) * (_outputLayer[i].output) * (1 - _outputLayer[i].output);
            }
        }


        public virtual double GetError() //dla pojedyńczej epok czyli erm erms
        {

        double total = 0.0;
            for (int j = 0; j<_outputNum; j++)
            {
                total += Math.Pow((_outputLayer[j].Target - _outputLayer[j].output), 2) / 2;
            }
            return total;
        }

        public void InitializeNetwork(Dictionary<T, double[]> TrainingSet)
        {
            Random rand = new Random();
            for (int i = 0; i < _preInputNum; i++)
            {
                _preInputLayer[i] = new FirstLayerInput();
                _preInputLayer[i].Weights = new double[_outputNum];
                for (int j = 0; j < _outputNum; j++)
                {
                    _preInputLayer[i].Weights[j] = 0.01 + ((double)rand.Next(0, 2) / 100);
                }
            }

            int k = 0;
            foreach (KeyValuePair<T, double[]> p in TrainingSet)
            {
                _outputLayer[k] = new Output<T>();
                _outputLayer[k++].Value = p.Key;
            }
        }

        public void Recognize(double[] Input, RecognizeModel<T> recognizeModel)
        {
            int i, j;
            double total = 0.0;
            double max = -1;

            //Apply Input to Network
            for (i = 0; i < _preInputNum; i++)
            {
                _preInputLayer[i].Value = Input[i];
            }

            //Find the [Two] Highest Outputs
            for (i = 0; i < _outputNum; i++)
            {
                total = 0.0;
                for (j = 0; j < _preInputNum; j++)
                {
                    total += _preInputLayer[j].Value * _preInputLayer[j].Weights[i];
                }
                _outputLayer[i].InputSum = total;
                _outputLayer[i].output = F(total);
                if (_outputLayer[i].output > max)
                {
                    recognizeModel.MatchedLow = recognizeModel.MatchedHigh;
                    recognizeModel.OutputLowValue = max;
                    max = _outputLayer[i].output;
                    recognizeModel.MatchedHigh = _outputLayer[i].Value;
                    recognizeModel.OutputHightValue = max;
                }
            }
        }
    }
}
