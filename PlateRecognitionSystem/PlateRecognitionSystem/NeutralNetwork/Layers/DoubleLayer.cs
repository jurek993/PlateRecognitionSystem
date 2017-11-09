using PlateRecognitionSystem.NeutralNetwork.NeuronComponents;
using System;
using System.Collections.Generic;
using PlateRecognitionSystem.Model;


namespace PlateRecognitionSystem.NeutralNetwork.Layers
{
    [Serializable]
    public class DoubleLayer<T> : AbstractHelperClass<T>, IBackPropagation<T> where T : IComparable<T>
    {
        private int _preInputNum;
        private int _inputNum;
        private int _outputNum;
        private FirstLayerInput[] _preInputLayer;
        private ClassicInput[] _inputLayer;
        private Output<T>[] _outputLayer;

        private double _learningRate = 0.2;

        public DoubleLayer(int preInputNum, int inputNum, int outputNum)
        {
            _preInputNum = preInputNum;
            _inputNum = inputNum;
            _outputNum = outputNum;
            _preInputLayer = new FirstLayerInput[_preInputNum];
            _inputLayer = new ClassicInput[_inputNum];
            _outputLayer = new Output<T>[_outputNum];
        }

        public void BackPropagate()
        {
            int i, j;
            double total;

            //Fix Input Layer's Error
            for (i = 0; i < _inputNum; i++)
            {
                total = 0.0;
                for (j = 0; j < _outputNum; j++)
                {
                    total += _inputLayer[i].Weights[j] * _outputLayer[j].Error;
                }
                _inputLayer[i].Error = total;
            }

            //Update The First Layer's Weights
            for (i = 0; i < _inputNum; i++)
            {
                for (j = 0; j < _preInputNum; j++)
                {
                    _preInputLayer[j].Weights[i] +=
                        _learningRate * _inputLayer[i].Error * _preInputLayer[j].Value;
                }
            }

            //Update The Second Layer's Weights
            for (i = 0; i < _outputNum; i++)
            {
                for (j = 0; j < _inputNum; j++)
                {
                    _inputLayer[j].Weights[i] +=
                        _learningRate * _outputLayer[i].Error * _inputLayer[j].Output;
                }
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

            //Calculate The First(Input) Layer's Inputs and Outputs
            for (i = 0; i < _inputNum; i++)
            {
                total = 0.0;
                for (j = 0; j < _preInputNum; j++)
                {
                    total += _preInputLayer[j].Value * _preInputLayer[j].Weights[i];
                }

                _inputLayer[i].InputSum = total;
                _inputLayer[i].Output = F(total);
            }

            //Calculate The Second(Output) Layer's Inputs, Outputs, Targets and Errors
            for (i = 0; i < _outputNum; i++)
            {
                total = 0.0;
                for (j = 0; j < _inputNum; j++)
                {
                    total += _inputLayer[j].Output * _inputLayer[j].Weights[i];
                }

                _outputLayer[i].InputSum = total;
                _outputLayer[i].output = F(total);
                _outputLayer[i].Target = _outputLayer[i].Value.CompareTo(output) == 0 ? 1.0 : 0.0;
                _outputLayer[i].Error = (_outputLayer[i].Target - _outputLayer[i].output) * (_outputLayer[i].output) * (1 - _outputLayer[i].output);
            }
        }

        public double GetError()
        {
            double total = 0.0;
            for (int j = 0; j < _outputNum; j++)
            {
                total += Math.Pow((_outputLayer[j].Target - _outputLayer[j].output), 2) / 2;
            }
            return total;
        }

        public void InitializeNetwork(Dictionary<T, double[]> TrainingSet)
        {
            int i, j;
            Random rand = new Random();
            for (i = 0; i < _preInputNum; i++)
            {
                _preInputLayer[i] = new FirstLayerInput();
                _preInputLayer[i].Weights = new double[_inputNum];
                for (j = 0; j < _inputNum; j++)
                {
                    _preInputLayer[i].Weights[j] = 0.01 + ((double)rand.Next(0, 5) / 100);
                }
            }

            for (i = 0; i < _inputNum; i++)
            {
                _inputLayer[i] = new ClassicInput();
                _inputLayer[i].Weights = new double[_outputNum];
                for (j = 0; j < _outputNum; j++)
                {
                    _inputLayer[i].Weights[j] = 0.01 + ((double)rand.Next(0, 5) / 100);
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

            //Apply input to the network
            for (i = 0; i < _preInputNum; i++)
            {
                _preInputLayer[i].Value = Input[i];
            }

            //Calculate Input Layer's Inputs and Outputs
            for (i = 0; i < _inputNum; i++)
            {
                total = 0.0;
                for (j = 0; j < _preInputNum; j++)
                {
                    total += _preInputLayer[j].Value * _preInputLayer[j].Weights[i];
                }
                _inputLayer[i].InputSum = total;
                _inputLayer[i].Output = F(total);
            }

            //Find the [Two] Highest Outputs   
            for (i = 0; i < _outputNum; i++)
            {
                total = 0.0;
                for (j = 0; j < _inputNum; j++)
                {
                    total += _inputLayer[j].Output * _inputLayer[j].Weights[i];
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
