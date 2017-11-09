using PlateRecognitionSystem.Model;
using PlateRecognitionSystem.NeutralNetwork.Layers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        }
        public void CreateNeuralNetwork(GlobalSettingsModel settingsModel)
        {
            if (settingsModel.TrainingSet == null)
                throw new Exception("Unable to Create Neural Network As There is No Data to Train..");
            int networkInput = settingsModel.AverageImageHeight * settingsModel.AverageImageWidth;
            int InputNum = (int)((double)(networkInput + settingsModel.NumberOfPatterns) * .33);
            switch (settingsModel.NumberOfLayers)
            {
                case 0:
                    NeuralNetwork = new NeuralNetwork<string>
    (new SingleLayer<string>(networkInput, settingsModel.NumberOfPatterns), settingsModel.TrainingSet);
                    break;
                case 1:
                    ////TODO: dowiedzieć się skąd 0.33
                    NeuralNetwork = new NeuralNetwork<string>
                       (new DoubleLayer<string>(networkInput, InputNum, settingsModel.NumberOfPatterns), settingsModel.TrainingSet);
                    break;
                case 2:
                    int HiddenNum = (int)((double)(networkInput + settingsModel.NumberOfPatterns) * .11); ////TODO: to też by się przydało dowiedziec o chuj chodzi. Learning rate 0.2?
                    NeuralNetwork = new NeuralNetwork<string>
                        (new TripleLayer<string>(networkInput, InputNum, HiddenNum, settingsModel.NumberOfPatterns), settingsModel.TrainingSet);
                    break;
                    // NeutralNetwork.MaximumError = Double.Parse(textBoxMaxError.Text, CultureInfo.InvariantCulture);
            }
        }
    }
}