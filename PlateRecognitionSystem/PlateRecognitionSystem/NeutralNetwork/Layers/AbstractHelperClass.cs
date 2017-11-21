using PlateRecognitionSystem.NeutralNetwork.NeuronComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateRecognitionSystem.NeutralNetwork.Layers
{
    [Serializable]
    public abstract class AbstractHelperClass<T> where T : IComparable<T>
    {
        public static  double LearningRate { get; set; }


        public double F(double x)
        {
            return (1 / (1 + Math.Exp(-x)));
        }

        public void SetLearningRate(double learningRate)
        {
            LearningRate = learningRate;
        }
    }
}
