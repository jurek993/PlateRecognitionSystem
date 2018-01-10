using AForge.Video;
using AForge.Video.DirectShow;
using Emgu.CV;
using Emgu.CV.Structure;
using PlateRecognitionSystem.Initialize;
using PlateRecognitionSystem.NeutralNetwork;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Media.Imaging;


namespace PlateRecognitionSystem
{
    /// <summary>
    /// Interaction logic for CameraWindow.xaml
    /// </summary>
    public partial class CameraWindow : Window
    {
        VideoCaptureDevice LocalWebCam;
        NameValueCollection appSettings = ConfigurationManager.AppSettings;
        public FilterInfoCollection LoaclWebCamsCollection;
        private PlateWindow _plateWindow = new PlateWindow();
        private static int i = 0;
        private bool _isRecognize = false;
        private int _frequency;
        private MainViewModel _model;
        private InitializeNeutralNetwork _initializeNeutralNetwork;
        private GlobalSettings _settings;
        public CameraWindow(MainViewModel viewModel, InitializeNeutralNetwork initializeNetwork, GlobalSettings settings)
        {
            InitializeComponent();
            _model = viewModel;
            _settings = settings;
            _initializeNeutralNetwork = initializeNetwork;
            _frequency = int.Parse(appSettings["FrequencyOFTakingPhoto"]);
            Loaded += CameraWindow_Loaded;
        }

        void CameraWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoaclWebCamsCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            LocalWebCam = new VideoCaptureDevice(LoaclWebCamsCollection[0].MonikerString);
            LocalWebCam.NewFrame += new NewFrameEventHandler(Cam_NewFrame);

            LocalWebCam.Start();
        }

        void Cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                System.Drawing.Image img = (Bitmap)eventArgs.Frame.Clone();
                if (_plateWindow.IsRecognise)
                {
                    System.Timers.Timer aTimer = new System.Timers.Timer();
                    aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                    aTimer.Interval = 8000;
                    aTimer.Enabled = true;

                }
                if (i % _frequency == 0 && i!= 0 && !_plateWindow.IsRecognise)
                {

                    i = 0;
                    Image<Bgr, Byte> imageCV = new Image<Bgr, byte>((Bitmap)img);
                    Mat mat = imageCV.Mat;
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        _plateWindow = new PlateWindow(_model, mat, _initializeNeutralNetwork, _settings);
                    });
                }
                i++;
                MemoryStream ms = new MemoryStream();
                img.Save(ms, ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = ms;
                bi.EndInit();

                bi.Freeze();
                Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    frameHolder.Source = bi;
                }));
            }
            catch (Exception ex)
            {
            }
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            _plateWindow.IsRecognise = false;
        }
    }
}
