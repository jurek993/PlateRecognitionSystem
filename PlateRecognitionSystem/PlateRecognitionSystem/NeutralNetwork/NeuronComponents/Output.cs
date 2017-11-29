using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateRecognitionSystem.NeutralNetwork.NeuronComponents
{
    [Serializable]
    public  class Output
    {
        public double InputSum { get; set; }
        public double output { get; set; }
        public double Delta { get; set; } //its off line? 
        public double Target { get; set; }
        public string Value { get; set; }
    }
}
