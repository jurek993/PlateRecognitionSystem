using PlateRecognitionSystem.Helpers;
using PlateRecognitionSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateRecognitionSystem.Database
{
    public class GarageDatabase
    {
        private GarageDBContext dBContext = new GarageDBContext();
        private MainViewModel _viewModel;
        public DataForDriverViewModel DataForDriverViewModel { get; set; }
        public GarageDatabase(MainViewModel model)
        {
            _viewModel = model;
        }
        public bool CheckPermission(string plateNumber, string loosedPlateNumber)
        { //TODO: ta metoda prawdopodobnie będzie użyta wielokrotnie. Przyda się równiesz jak auto nie ma abonamentu. wjazd każdego auta i tak musi być zapisywany w db
            Vehicle vehicle = null;
            string[] expectedPlateNumber = null;
            if (plateNumber!= string.Empty)
            {
                vehicle =  dBContext.Vehicles.SingleOrDefault(x => x.NumberPlate == plateNumber);
                if(vehicle == null)
                {
                    expectedPlateNumber = dBContext.Vehicles.Select(x => x.NumberPlate).ToArray().Where(l => SimmilarString.LevenshteinDistance(plateNumber, l) == 1).ToArray();
                    if(expectedPlateNumber.Count() == 1)
                    {
                        if(plateNumber.Count() == expectedPlateNumber[0].Count())
                        {
                            string expectedString = expectedPlateNumber[0];
                            for (int i = 0; i < plateNumber.Count(); i++)
                            {
                                if (expectedString[i] == loosedPlateNumber[i] && expectedString[i] != plateNumber[i])
                                {
                                    vehicle = dBContext.Vehicles.SingleOrDefault(x => x.NumberPlate == expectedString);
                                    plateNumber = expectedString;
                                }

                            }
                        }
                        //TODO: nie chciało mi się już. trzeba tego difa porównac z tymi przegranymi w rozpoznawaniu. najlepiej jakby porównało na tym samym miejscu w stringu. może zwykłym forem? 
                    }
                }
                
            }
            
            if (vehicle != null && vehicle.ExpirationDate >= DateTime.Now)
            {
                DataForDriverViewModel = new DataForDriverViewModel
                {
                    TestedValue = "Chyba Cię rozpoznałem"
                };
                _viewModel.LogTextBox += String.Format("Otwarcie szlabanu dla pojazdu o numerach {0}\n",plateNumber);
                return true;
            } //TODO: pamiętać o tym, zeby nie wpuścić przeterminowanego pojazdu, ale wyświetlić info, ze abonament mu się skończył
            _viewModel.LogTextBox += String.Format("Brak zezwolenia wjazdu dla pojazdu o numerach {0}\n", plateNumber);
            return false;
        }

    }
}
