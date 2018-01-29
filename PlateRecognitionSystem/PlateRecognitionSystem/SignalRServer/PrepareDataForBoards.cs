using PlateRecognitionSystem.Database;
using PlateRecognitionSystem.Enums;
using PlateRecognitionSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateRecognitionSystem.SignalRServer
{
    public class PrepareDataForBoards
    {
        private SingleVisit _visit { get; set; }
        private BoardDatabase _database { get; set; }
        public PrepareDataForBoards(SingleVisit visit)
        {
            _visit = visit;
            _database = new BoardDatabase();
        }

        public PrepareDataForBoards()
        {
            _database = new BoardDatabase();
        }

        public GuestBoardViewModel DataForGuestBoard()
        {
            string stringDate = String.Empty;
            //if wjazd lub wyjazd
            if (_visit.ExitDate == null)
            {
                var boards = _database.GetListOfBoards(TypeOfBoards.EnterBoard);
                stringDate = _visit.EntryDate.ToShortTimeString();
                stringDate += " " + _visit.EntryDate.ToShortDateString();
                GuestBoardViewModel guest = new GuestBoardViewModel
                {
                    Boards = boards,
                    EntryOrExitDate = stringDate,
                    LicencePlate = _visit.Vehicle.NumberPlate
                };
                if (_visit.Vehicle.ExpirationDate != null && _visit.Vehicle.ExpirationDate < DateTime.Now)
                {
                    guest.AdditionalInformation = "Upłynął termin abonamentu - wjazd jednorazowy";
                }
                return guest;
            }
            else
            {
                var date = (DateTime)_visit.ExitDate;
                stringDate = date.ToShortTimeString();
                stringDate += " " + date.ToShortDateString();
                GuestBoardViewModel guest = new GuestBoardViewModel
                {
                    Boards = _database.GetListOfBoards(TypeOfBoards.ExitBoard),
                    EntryOrExitDate = stringDate,
                    LicencePlate = _visit.Vehicle.NumberPlate,
                    Cost = (double)_visit.Price
                };
                return guest;
            }
        }

        public SubscriptionBoardViewModel DataForSubscriptionViewModel()
        {
            string stringDate = String.Empty;
            //enter
            if (_visit.ExitDate == null)
            {
                stringDate = _visit.EntryDate.ToShortTimeString();
                stringDate += " " + _visit.EntryDate.ToShortDateString();
                var expirationDate = (DateTime)_visit.Vehicle.ExpirationDate;
                SubscriptionBoardViewModel subscriber = new SubscriptionBoardViewModel
                {
                    Boards = _database.GetListOfBoards(TypeOfBoards.EnterBoard),
                    EntryOrExitDate = stringDate,
                    LicencePlate = _visit.Vehicle.NumberPlate,
                    ExpirationDate = expirationDate.ToShortDateString(),
                    Name = _visit.Vehicle.Owner.Name
                };
                return subscriber;
            }
            else //exit
            {
                var date = (DateTime)_visit.ExitDate;
                stringDate = date.ToShortTimeString();
                stringDate += " " + date.ToShortDateString();
                var expirationDate = (DateTime)_visit.Vehicle.ExpirationDate;
                SubscriptionBoardViewModel subscriber = new SubscriptionBoardViewModel
                {
                    Boards = _database.GetListOfBoards(TypeOfBoards.ExitBoard),
                    EntryOrExitDate = stringDate,
                    LicencePlate = _visit.Vehicle.NumberPlate,
                    ExpirationDate = expirationDate.ToShortDateString(),
                    Name = _visit.Vehicle.Owner.Name,
                };
                return subscriber;
            }
        }

        public NormalBoardViewModel DataForNormalMessage(TypeOfBoards boardName)
        {
            return new NormalBoardViewModel
            {
                FreePlaces = _database.GetFreeSpaceInGarage(),
                Boards = _database.GetListOfBoards(boardName)
            };
        }
    }
}
