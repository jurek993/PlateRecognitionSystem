using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateRecognitionSystem.Model
{
    public class SubscriptionBoardViewModel
    {
        public List<Board> Boards { get; set; } 
        public string Name { get; set;  }
        public string EntryOrExitDate { get; set; }
        public string LicencePlate { get; set; }
        public string ExpirationDate { get; set; }
        public string AdditionalInformation { get; set; }
    }
}
