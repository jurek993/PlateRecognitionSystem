using PlateRecognitionSystem.Initialize;
using PlateRecognitionSystem.Model;
using PlateRecognitionSystem.NeutralNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PlateRecognitionSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GlobalSettings settings = null;
        public MainWindow()
        {

            InitializeComponent();
            settings = new GlobalSettings();
            settings.InitializeSettings();
            settings.GenerateTrainingSet();
        }

        private void comboBoxLayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            settings.SettingsModel.NumberOfLayers = int.Parse((comboBoxLayers.SelectedItem as ComboBoxItem).Content.ToString());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            InitializeNeutralNetwork neutralNetwork = new InitializeNeutralNetwork(settings.SettingsModel);
        }
    }
}
