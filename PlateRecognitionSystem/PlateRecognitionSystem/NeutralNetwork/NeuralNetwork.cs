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
    class NeuralNetwork<T> : AbstractHelperClass<T>
         where T : IComparable<T>
    {
        private IBackPropagation<T> _neuralNet;
        private double _maximumError = 1.0;
        private int maximumIteration = 10000;
        //public int MaximumIteration { get; set; } = 10000;
        Dictionary<T, double[]> TrainingSet;

        public delegate void IterationChangedCallBack(object o, NeuralEventArgs args);
        public event IterationChangedCallBack IterationChanged = null;

        public NeuralNetwork(IBackPropagation<T> IBackPro, Dictionary<T, double[]> trainingSet)
        {
            _neuralNet = IBackPro;
            TrainingSet = trainingSet;
            _neuralNet.InitializeNetwork(TrainingSet);
        }

        public bool Train()
        {
            double currentError = 0;
            int currentIteration = 0;
            NeuralEventArgs Args = new NeuralEventArgs();

            do
            {
                currentError = 0;
                foreach (KeyValuePair<T, double[]> p in TrainingSet)
                {

                    _neuralNet.ForwardPropagate(p.Value, p.Key);
                    _neuralNet.BackPropagate();
                    currentError += _neuralNet.GetError();
                }

                currentIteration++;

                if (IterationChanged != null && currentIteration % 5 == 0)
                {
                    Args.CurrentError = currentError;
                    Args.CurrentIteration = currentIteration;
                    IterationChanged(this, Args);
                }

            } while (currentError > _maximumError && currentIteration < MaximumIteration && !Args.Stop);

            if (IterationChanged != null)
            {
                Args.CurrentError = currentError;
                Args.CurrentIteration = currentIteration;
                IterationChanged(this, Args);
            }

            if (currentIteration >= maximumIteration || Args.Stop)
                return false;//Training Not Successful

            return true;
        }

        public void Recognize(double[] Input, ref T MatchedHigh, ref double OutputValueHight,
            ref T MatchedLow, ref double OutputValueLow)
        {
            _neuralNet.Recognize(Input, ref MatchedHigh, ref OutputValueHight, ref MatchedLow, ref OutputValueLow);
        }

        public void SaveNetwork(string path)
        {
            FileStream FS = new FileStream(path, FileMode.Create);
            BinaryFormatter BF = new BinaryFormatter();
            BF.Serialize(FS, _neuralNet);
            FS.Close();
        }

        public void LoadNetwork(string path)
        {
            FileStream FS = new FileStream(path, FileMode.Open);
            BinaryFormatter BF = new BinaryFormatter();
            _neuralNet = (IBackPropagation<T>)BF.Deserialize(FS);
            FS.Close();
        }

        public double MaximumError
        {
            get { return _maximumError; }
            set { _maximumError = value; }
        }
        public int MaximumIteration
        {
            get { return maximumIteration; }
            set { maximumIteration = value; }
        }
    }
}
