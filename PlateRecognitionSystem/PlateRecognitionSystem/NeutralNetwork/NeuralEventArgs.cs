using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateRecognitionSystem.NeutralNetwork
{
    public class NeuralEventArgs : EventArgs
    {
        public bool Stop { get; set; } = false;
        public double CurrentError { get; set; } = 0;
        public int CurrentIteration { get; set; } = 0;
    }
}
