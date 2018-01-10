using Emgu.CV;
using PlateRecognitionSystem.NeutralNetwork.Layers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PlateRecognitionSystem.Model
{
    class PlateViewModel : AbstractHelperClass
    {
        public MainViewModel MainViewModel { get; set; }
        public Mat Mat { get; set; }
        public List<List<UMat>>  filteredCharatersInSinglePlate { get; set; }

        private ImageSource _cannyImage;
        private ImageSource _monoImage;
        private ObservableCollection<ImageSource> _detectedPlates;
        private ObservableCollection<ImageSource> _filteredDetectedCharacters;
        public PlateViewModel()
        {
            DetectedPlates = new ObservableCollection<ImageSource>();
            FilteredDetectedCharacters = new ObservableCollection<ImageSource>();
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

        public ObservableCollection<ImageSource> FilteredDetectedCharacters
        {
            get
            {
                return _filteredDetectedCharacters;
            }

            set
            {
                _filteredDetectedCharacters = value;
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
