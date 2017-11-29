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
        private MainViewModel _viewModel;
        public GlobalSettingsModel(MainViewModel viewModel)
        {
            _viewModel = viewModel;
        }
        private int _numberOfPatterns;
        public int AverageImageHeight { get; set; }
        public int NumberOfLayers { get; set; }
        public int AverageImageWidth { get; set; }
        public Dictionary<string, double[]> TrainingSet { get; set; }
        public Dictionary<string, double[]> DistractionTrainingSet { get; set; }
        public string PatternPath { get; set; }
        public string DistractionsPatternPath { get; set; }
        public Dictionary<string, double[]> SampleTrainingSet { get; set; } 
        public string TestSamplesPath { get; set; }
        public int NumberOfPatterns
        {
            get
            {
                return _numberOfPatterns;
            }
            set
            {
                _numberOfPatterns = value;
                _viewModel.NetworkOutput = value;

            }
        }
    }
}
