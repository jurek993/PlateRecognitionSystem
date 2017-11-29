using PlateRecognitionSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateRecognitionSystem.NeutralNetwork.Layers
{
    public interface IBackPropagation
    {
        void BackPropagate();
        double GetError();
        void ForwardPropagate(double[] pattern, string output);
        void InitializeNetwork(Dictionary<string, double[]> TrainingSet);
        void Recognize(double[] Input, RecognizeModel recognizeModel);
    }
}
