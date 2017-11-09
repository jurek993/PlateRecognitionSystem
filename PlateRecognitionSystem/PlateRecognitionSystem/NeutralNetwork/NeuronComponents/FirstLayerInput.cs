using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateRecognitionSystem.NeutralNetwork.NeuronComponents
{
    [Serializable]
    public class FirstLayerInput
    {
        public double Value { get; set; } = 0;
        public double[] Weights { get; set; }
    }
}