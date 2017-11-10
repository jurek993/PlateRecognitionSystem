using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateRecognitionSystem.Model
{
    public class GlobalSettingsModel   
    {
        public int AverageImageHeight { get; set; }
        public int NumberOfLayers { get; set; }
        public int AverageImageWidth { get; set; }
        public int NumberOfPatterns { get; set; }
        public Dictionary<string, double[]> TrainingSet { get; set; }
        public string PatternPath { get; set; }
        //public int NumberOfInputLa
    }
}
