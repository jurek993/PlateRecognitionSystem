using PlateRecognitionSystem.NeutralNetwork.Layers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

public class MainViewModel : AbstractHelperClass, INotifyPropertyChanged
{
    private string _logTextBox;
    private ImageSource _image;
    private double _currentError;
    private double _maximumError;
    private double _learningRate;
    private string _matchedHightValue;
    private string _matchedLowValue;
    private string _matchedHightPercent;
    private string _matchedLowPercent;
    private bool _imageLoaded;
    int _distractionPatterns;
    double _currentIteration;
    private bool _trainingSuccess;
    private int _networkInput;
    private int _networkOutput;
    public string LogTextBox
    {
        get
        {
            return _logTextBox;
        }

        set
        {
            _logTextBox = value;
            OnPropertyChanged();

        }
    }
    public double CurrentError
    {
        get
        {
            return _currentError;
        }

        set
        {
            _currentError = value;
            OnPropertyChanged();
        }
    }
    public double MaximumError
    {
        get
        {
            return _maximumError;
        }

        set
        {
            if (!Double.Equals(_maximumError, value))
            {
                _maximumError = value;
                OnPropertyChanged();
            }

        }
    }
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
                OnPropertyChanged();
            }
        }
    }
    public string MatchedHightValue
    {
        get
        {
            return _matchedHightValue;
        }

        set
        {
            _matchedHightValue = value;
            OnPropertyChanged();
        }
    }
    public string MatchedLowValue
    {
        get
        {
            return _matchedLowValue;
        }

        set
        {
            _matchedLowValue = value;
            OnPropertyChanged();
        }
    }
    public string MatchedHightPercent
    {
        get
        {
            return _matchedHightPercent;
        }

        set
        {
            _matchedHightPercent = value;
            OnPropertyChanged();
        }
    }
    public string MatchedLowPercent
    {
        get
        {
            return _matchedLowPercent;
        }

        set
        {
            _matchedLowPercent = value;
            OnPropertyChanged();
        }
    }
    public double CurrentIteration
    {
        get
        {
            return _currentIteration;
        }

        set
        {
            _currentIteration = value;
            OnPropertyChanged();
        }
    }
    public bool TrainingSuccess
    {
        get
        {
            return _trainingSuccess;
        }

        set
        {
            _trainingSuccess = value;
            OnPropertyChanged();
        }
    }
    public bool ImageLoaded
    {
        get
        {
            return _imageLoaded;
        }

        set
        {
            _imageLoaded = value;
            OnPropertyChanged();
        }
    }
    public ImageSource Image {
        get
        {
            return _image;
        }
        set
        {
            _image = value;
            OnPropertyChanged();
        }
    }
    public int NumberOfDistractionPatterns { get { return _distractionPatterns; } set { _distractionPatterns = value; OnPropertyChanged(); } }
    public int NetworkInput { get { return _networkInput; } set { _networkInput = value; OnPropertyChanged(); } }
    public int NetworkOutput { get { return _networkOutput; } set { _networkOutput = value; OnPropertyChanged(); } }
    //public int NumberOf { get { return _distractionPatterns; } set { _distractionPatterns = value; OnPropertyChanged(); } }
    //public int DistractionPatterns { get { return _distractionPatterns; } set { _distractionPatterns = value; OnPropertyChanged(); } }
}

