using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateRecognitionSystem.Model
{
    public class RecognizeModel<T>  where T : IComparable<T>
    {
        public T  MatchedHigh { get; set; }
        public T MatchedLow { get; set; }
        public double OutputHightValue { get; set; } = 0;
        public double OutputLowValue { get; set; } = 0;
    }
}
