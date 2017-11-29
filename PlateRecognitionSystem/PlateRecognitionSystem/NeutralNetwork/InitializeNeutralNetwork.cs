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
        public NeuralNetwork NeuralNetwork { get; set; }
        private MainViewModel model;
        public InitializeNeutralNetwork(GlobalSettingsModel settingsModel, MainViewModel mainViewModel)
        {
            model = mainViewModel;
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
            model.NetworkInput = settingsModel.AverageImageHeight * settingsModel.AverageImageWidth;
            int secondLayerNeurons = (int)((double)(model.NetworkInput + settingsModel.NumberOfPatterns) * .33);
            switch (settingsModel.NumberOfLayers)
            {
                case 0:
                    NeuralNetwork = new NeuralNetwork
                        (new SingleLayer(model.NetworkInput, settingsModel.NumberOfPatterns), settingsModel);
                    break;
                case 1:
                    NeuralNetwork = new NeuralNetwork
                       (new DoubleLayer(model.NetworkInput, secondLayerNeurons, settingsModel.NumberOfPatterns), settingsModel);
                    break;
                case 2:
                    int hiddenLayerNeurons = (int)((double)(model.NetworkInput + settingsModel.NumberOfPatterns) * .11);
                    NeuralNetwork = new NeuralNetwork
                        (new TripleLayer(model.NetworkInput, secondLayerNeurons, hiddenLayerNeurons, settingsModel.NumberOfPatterns), settingsModel);
                    break;
            }
        }
    }
}