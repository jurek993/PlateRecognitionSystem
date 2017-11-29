using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateRecognitionSystem.Model
{
    public class RecognizeModel
    {
        public string  MatchedHigh { get; set; }
        public string MatchedLow { get; set; }
        public double OutputHightValue { get; set; } = 0;
        public double OutputLowValue { get; set; } = 0;
    }
}
