using PlateRecognitionSystem.Model;
using PlateRecognitionSystem.NeutralNetwork.Layers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PlateRecognitionSystem.NeutralNetwork
{
    public class InitializeNeutralNetwork
    {
        public NeuralNetwork<string> NeuralNetwork { get; set; }

        public InitializeNeutralNetwork(GlobalSettingsModel settingsModel)
        {
            CreateNeuralNetwork(settingsModel);
        }
        public InitializeNeutralNetwork()
        {
            //I used it
        }
        public void CreateNeuralNetwork(GlobalSettingsModel settingsModel)
        {
            if (settingsModel.TrainingSet == null)
                throw new Exception("Unable to Create Neural Network As There is No Data to Train..");
            int networkInput = settingsModel.AverageImageHeight * settingsModel.AverageImageWidth;
            int FirstLayerNeurons = (int)((double)(networkInput + settingsModel.NumberOfPatterns) * .33);
            switch (settingsModel.NumberOfLayers)
            {
                case 0:
                    NeuralNetwork = new NeuralNetwork<string>
    (new SingleLayer<string>(networkInput, settingsModel.NumberOfPatterns), settingsModel.TrainingSet);
                    break;
                case 1:
                    NeuralNetwork = new NeuralNetwork<string>
                       (new DoubleLayer<string>(networkInput, FirstLayerNeurons, settingsModel.NumberOfPatterns), settingsModel.TrainingSet);
                    break;
                case 2:
                    int HiddenLayerNeurons = (int)((double)(networkInput + settingsModel.NumberOfPatterns) * .11);
                    NeuralNetwork = new NeuralNetwork<string>
                        (new TripleLayer<string>(networkInput, FirstLayerNeurons, HiddenLayerNeurons, settingsModel.NumberOfPatterns), settingsModel.TrainingSet);
                    break;
            }
        }
    }
}