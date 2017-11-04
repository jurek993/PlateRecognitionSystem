using PlateRecognitionSystem.Model;
using PlateRecognitionSystem.NeutralNetwork.Layers;
using System;

namespace PlateRecognitionSystem.NeutralNetwork
{
    public class InitializeNeutralNetwork
    {
        private NeuralNetwork<string> _neuralNetwork = null; 

        public InitializeNeutralNetwork(GlobalSettingsModel settingsModel)
        {
            CreateNeuralNetwork(settingsModel);
        }
        public void CreateNeuralNetwork(GlobalSettingsModel settingsModel)
        {
            if (settingsModel.TrainingSet == null)
                throw new Exception("Unable to Create Neural Network As There is No Data to Train..");

            if (settingsModel.NumberOfLayers == 0)
            {
                _neuralNetwork = new NeuralNetwork<string>
                    (new SingleLayer<string>(settingsModel.AverageImageHeight * settingsModel.AverageImageWidth, settingsModel.NumberOfPatterns), settingsModel.TrainingSet);
            }
            else if (settingsModel.NumberOfLayers == 1)
            {
                //int InputNum = Int16.Parse(textBoxInputUnit.Text);

               // neuralNetwork = new NeuralNetwork<string>
                //    (new BP2Layer<string>(av_ImageHeight * av_ImageWidth, InputNum, NumOfPatterns), TrainingSet);

            }
            else if (settingsModel.NumberOfLayers == 2)
            {
                //int InputNum = Int16.Parse(textBoxInputUnit.Text);
                //int HiddenNum = Int16.Parse(textBoxHiddenUnit.Text);

                //neuralNetwork = new NeuralNetwork<string>
                  //  (new BP3Layer<string>(av_ImageHeight * av_ImageWidth, InputNum, HiddenNum, NumOfPatterns), TrainingSet);

            }

        }
    }
}
