﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PlateRecognitionSystem.NeutralNetwork
{
    public class TrainingNetwork
    {
        private delegate bool TrainingCallBack();
        private AsyncCallback asyCallBack = null;
        private IAsyncResult res = null;
        private ManualResetEvent ManualReset = null;
        private NeuralNetwork _neuralNetwork;
        private MainViewModel _viewModel;

        public TrainingNetwork(NeuralNetwork neuralNetwork, MainViewModel viewModel)
        {
            asyCallBack = new AsyncCallback(TraningCompleted);
            ManualReset = new ManualResetEvent(false); //TODO: do wywalenia
            _neuralNetwork = neuralNetwork;
            _viewModel = viewModel;
        }

        public void StartTraining()
        {
            _neuralNetwork.ViewModel = _viewModel;
            TrainingCallBack TR = new TrainingCallBack(_neuralNetwork.Train);
            res = TR.BeginInvoke(asyCallBack, TR);
        }

        private void TraningCompleted(IAsyncResult result)
        {
            if (result.AsyncState is TrainingCallBack)
            {
                TrainingCallBack TR = (TrainingCallBack)result.AsyncState;

                bool isSuccess = TR.EndInvoke(res);
                if (isSuccess)
                {
                    _viewModel.LogTextBox += "- Nauka sieci zakończona pomyśnie\r\n";
                    _viewModel.TrainingSuccess = true;
                }

                else
                    _viewModel.LogTextBox += "- Błąd - przekroczona maksymalna iteracja\r\n";
            }
        }


    }
}
