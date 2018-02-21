using PlateRecognitionSystem.Enums;
using PlateRecognitionSystem.Helpers;
using PlateRecognitionSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateRecognitionSystem.Database
{
    public class GarageDatabase : GarageDBContext
    {
        private MainViewModel _viewModel;

        public BoardDatabase BoardDatabase = new BoardDatabase();
        public GuestBoardViewModel DataForDriverViewModel { get; set; }
        public GarageDatabase(MainViewModel model)
        {
            _viewModel = model;
        }
        public bool CheckPermission(ref string plateNumber, string loosedPlateNumber)
        {
            Vehicle vehicle = null;
            string[] expectedPlateNumber = null;
            if (plateNumber != string.Empty && plateNumber.Count() >= 5)
            {
                string tempPlateNumber = plateNumber;
                vehicle = Vehicles.SingleOrDefault(x => x.NumberPlate == tempPlateNumber);
                if (vehicle == null)
                {
                    expectedPlateNumber = Vehicles.Select(x => x.NumberPlate).ToArray().Where(l => CalculationHelpers.LevenshteinDistance(tempPlateNumber, l) == 1).ToArray();
                    if (expectedPlateNumber.Count() == 1)
                    {
                        if (plateNumber.Count() == expectedPlateNumber[0].Count())
                        {
                            string expectedString = expectedPlateNumber[0];
                            for (int i = 0; i < plateNumber.Count(); i++)
                            {
                                if (expectedString[i] == loosedPlateNumber[i] && expectedString[i] != plateNumber[i])
                                {
                                    vehicle = Vehicles.SingleOrDefault(x => x.NumberPlate == expectedString);
                                    plateNumber = expectedString;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                return false;
            }
             return true;
        }


        public SingleVisit SaveSingleVisit(string licencePlate)
        { 
            SingleVisit visit = SingleVisits.SingleOrDefault(x => x.ExitDate == null && x.Vehicle.NumberPlate == licencePlate);
            Vehicle vehicle = Vehicles.SingleOrDefault(x => x.NumberPlate == licencePlate);
            if (vehicle == null) //nie ma abo i pierwszy raz wjeżdża
            {
                vehicle = new Vehicle { NumberPlate = licencePlate };
                Vehicles.Add(vehicle);
            }
            if (visit == null) //entry
            {
                visit = new SingleVisit
                {
                    EntryDate = DateTime.Now,
                    Vehicle = vehicle
                };
                SingleVisits.Add(visit);
            }
            else //exit
            {
                visit.ExitDate = DateTime.Now;
                if (visit.Vehicle.Owner == null) //nie ma abonamentu
                {
                    TimeSpan visitTime = (DateTime)visit.ExitDate - visit.EntryDate;
                    visit.Price = CalculationHelpers.CauntThePrice(visitTime, Prices.ToList());
                    visit.Vehicle.TotalPay += (double)visit.Price;                }
            }
            SaveChanges();
            return visit;
        }

        public void ChangeTheGarageOccupancy(TypeOfBoards type)
        {
            switch (type)
            {
                case TypeOfBoards.EnterBoard:
                    GarageInformation.FirstOrDefault().Occupancy++;
                    SaveChanges();
                    break;
                case TypeOfBoards.ExitBoard:
                    GarageInformation.FirstOrDefault().Occupancy--;
                    SaveChanges();
                    break;
            }
        }
    }
}
