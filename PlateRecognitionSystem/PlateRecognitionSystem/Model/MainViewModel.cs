﻿using PlateRecognitionSystem.NeutralNetwork.Layers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

public class MainViewModel : AbstractHelperClass<string> ,  INotifyPropertyChanged 
{
    //TODO: przerobić ten viewModel tak jak plateViewModel. Klasa dziezicząca jest ciekawszym pomysłem. do tego podoba mi się implementacja propertyChange


    private string _logTextBox;
    public string LogTextBox
    {
        get
        {
            return _logTextBox;
        }

        set
        {
            _logTextBox = value;
            OnPropertyChanged("LogTextBox");

        }
    }
    double _currentError;
    public double CurrentError
    {
        get
        {
            return _currentError;
        }

        set
        {
            _currentError = value;
            OnPropertyChanged("CurrentError");
        }
    }
    private double _maximumError;
    public double MaximumError
    {
        get
        {
            return _maximumError;
        }

        set
        {
            if(!Double.Equals(_maximumError,value))
            {
                _maximumError = value;
                OnPropertyChanged("MaximumError");
            }

        }
    }
    private double _learningRate;
    public double LearningRate
    {
        get
        {
            return _learningRate;
        }

        set
        {
            if (!Double.Equals(_learningRate, value))
            {
                _learningRate = value;
                SetLearningRate(value);
                OnPropertyChanged("LearningRate");
            }

        }
    }

    private string _matchedHightValue;
    public string MatchedHightValue
    {
        get
        {
            return _matchedHightValue;
        }

        set
        {
            _matchedHightValue = value;
            OnPropertyChanged("MatchedHightValue");
        }
    }

    private string _matchedLowValue;
    public string MatchedLowValue
    {
        get
        {
            return _matchedLowValue;
        }

        set
        {
            _matchedLowValue = value;
            OnPropertyChanged("MatchedLowValue");
        }
    }

    private string _matchedHightPercent;
    public string MatchedHightPercent
    {
        get
        {
            return _matchedHightPercent;
        }

        set
        {
            _matchedHightPercent = value;
            OnPropertyChanged("MatchedHightPercent");
        }
    }

    private string _matchedLowPercent;
    public string MatchedLowPercent
    {
        get
        {
            return _matchedLowPercent;
        }

        set
        {
            _matchedLowPercent = value;
            OnPropertyChanged("MatchedLowPercent");
        }
    }

    double _currentIteration;
    public double CurrentIteration
    {
        get
        {
            return _currentIteration;
        }

        set
        {
            _currentIteration = value;
            OnPropertyChanged("CurrentIteration");
        }
    }


    private bool _trainingSuccess;
    public bool TrainingSuccess
    {
        get
        {
            return _trainingSuccess;
        }

        set
        {
            _trainingSuccess = value;
            OnPropertyChanged("TrainingSuccess");
        }
    }


    private bool _imageLoaded;
    public bool ImageLoaded
    {
        get
        {
            return _imageLoaded;
        }

        set
        {
            _imageLoaded = value;
            OnPropertyChanged("ImageLoaded");
        }
    }

    private ImageSource _image;
    public ImageSource Image
    {
        get
        {
            return _image;
        }
        set
        {
            _image = value;
            OnPropertyChanged("Image");
        }
    }


    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(String propertyName)
    {
        PropertyChangedEventHandler handler = PropertyChanged;
        if (null != handler)
        {
            handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

