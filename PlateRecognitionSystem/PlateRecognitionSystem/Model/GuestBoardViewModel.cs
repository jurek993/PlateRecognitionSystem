using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateRecognitionSystem.Model
{
    public class GuestBoardViewModel
    {
        public List<Board> Boards { get; set; }
        public string EntryOrExitDate { get; set; }
        public double Cost { get; set; }
        public string LicencePlate { get; set; }
        public string AdditionalInformation { get; set; }
    }
}
