using PlateRecognitionSystem.NeutralNetwork.NeuronComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PlateRecognitionSystem.NeutralNetwork.Layers
{
    [Serializable]
    public abstract class AbstractHelperClass
    {
        public static  double LearningRate { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
