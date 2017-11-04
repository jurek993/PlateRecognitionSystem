using PlateRecognitionSystem.NeutralNetwork.NeuronComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateRecognitionSystem.Model
{
    public class NeuronModel<T> where T : IComparable<T> //odpuszczam ten pomysł. dowiedz się co tu jest kurwa tym neuronem
    {
        public FirstLayerInput FirstLayerInput { get; set; }
        public ClassicInput ClassicInput { get; set; }
        public Hidden Hidden { get; set; }
        public Output<T> Output { get; set; }
        public NeuronModel()
        {
            this.FirstLayerInput = new FirstLayerInput();
            this.ClassicInput = new ClassicInput();
            this.Hidden = new Hidden();
            this.Output = new Output<T>();
        }
   


    }
}
