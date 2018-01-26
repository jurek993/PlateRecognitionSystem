using PlateRecognitionSystem.Database;
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

        public GuestBoardViewModel DataForGuestBoard()
        {
            string stringDate = String.Empty;
            //if wjazd i wyjazd
            if (_visit.ExitDate == null)
            {
                var boards = _database.GetListOfBoards("EntryTable");
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
                var date = (DateTime) _visit.ExitDate;
                stringDate = date.ToShortTimeString();
                stringDate += " " + date.ToShortDateString();
                GuestBoardViewModel guest = new GuestBoardViewModel
                {
                    Boards = _database.GetListOfBoards("ExitTable"),
                    EntryOrExitDate = stringDate,
                    LicencePlate = _visit.Vehicle.NumberPlate,
                    Cost = (double)_visit.Price
                };
                return guest;
            }
        }
    }
}
