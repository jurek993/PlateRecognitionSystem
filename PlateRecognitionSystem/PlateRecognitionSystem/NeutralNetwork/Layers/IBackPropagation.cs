using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateRecognitionSystem.NeutralNetwork.Layers
{
    interface IBackPropagation<T>
    {
        void BackPropagate();
        double GetError();
        void ForwardPropagate(double[] pattern, T output);
        void InitializeNetwork(Dictionary<T, double[]> TrainingSet);
        void Recognize(double[] Input, ref T MatchedHigh, ref double OutputValueHight,
                                        ref T MatchedLow, ref double OutputValueLow);
    }
}
