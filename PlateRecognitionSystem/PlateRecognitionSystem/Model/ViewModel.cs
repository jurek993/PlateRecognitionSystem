using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


public class ViewModel :  INotifyPropertyChanged
{
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

