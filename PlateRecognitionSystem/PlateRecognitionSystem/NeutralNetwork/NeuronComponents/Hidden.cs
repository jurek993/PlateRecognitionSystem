using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateRecognitionSystem.NeutralNetwork.NeuronComponents
{
    [Serializable]
    public class Hidden
    {
        public double InputSum { get; set; }
        public double Output { get; set; }
        public double Error { get; set; }
        public double[] Weights { get; set; }
    }
}
