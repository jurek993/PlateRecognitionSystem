using PlateRecognitionSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateRecognitionSystem.NeutralNetwork.Layers
{
    public interface IBackPropagation<T> where T : IComparable<T>
    {
        void BackPropagate();
        double GetError();
        void ForwardPropagate(double[] pattern, T output);
        void InitializeNetwork(Dictionary<T, double[]> TrainingSet);
        void Recognize(double[] Input, RecognizeModel<T> recognizeModel);
    }
}
