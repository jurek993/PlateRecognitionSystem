using Emgu.CV;
using PlateRecognitionSystem.Plate;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PlateRecognitionSystem.Model
{
    class PlateViewModel : ViewModelBase
    {
        public MainViewModel MainViewModel { get; set; }
        public Mat Mat { get; set; }
       

        private ImageSource _cannyImage;
        private ImageSource _monoImage;
        private ObservableCollection<ImageSource> _detectedPlates;
        public PlateViewModel()
        {
          DetectedPlates = new ObservableCollection<ImageSource>();
        }
        public ObservableCollection<ImageSource> DetectedPlates
        {
            get
            {
                return _detectedPlates;
            }

            set
            {
                _detectedPlates = value;
                OnPropertyChanged();
            }
        }

        public ImageSource CannyImage
        {
            get
            {
                return _cannyImage;
            }
            set
            {
                _cannyImage = value;
                OnPropertyChanged();
            }
        }


        public ImageSource MonoImage
        {
            get
            {
                return _monoImage;
            }
            set
            {
                _monoImage = value;
                OnPropertyChanged();
            }
        }
    }
}
