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
    public class NeuralNetwork<T> : AbstractHelperClass<T> 
         where T : IComparable<T>
    {
        public int MaximumIteration { get; set; } = 10000;
        public ViewModel ViewModel { get;set; }


        internal Dictionary<T, double[]> TrainingSet;

        private IBackPropagation<T> _neuralNet;

        public NeuralNetwork(IBackPropagation<T> IBackPro, Dictionary<T, double[]> trainingSet)
        {
            _neuralNet = IBackPro;
            TrainingSet = trainingSet;
            _neuralNet.InitializeNetwork(TrainingSet);
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
                foreach (KeyValuePair<T, double[]> p in TrainingSet)
                {

                    _neuralNet.ForwardPropagate(p.Value, p.Key);
                    _neuralNet.BackPropagate();
                    currentError += _neuralNet.GetError();
                }

                currentIteration++;

                if (currentIteration % 5 == 0)
                {
                    ViewModel.CurrentError = currentError;
                    ViewModel.CurrentIteration = currentIteration;
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

        public void Recognize(double[] Input, RecognizeModel<T> recognizeModel)
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
            _neuralNet = (IBackPropagation<T>)binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
        }
    }
}
