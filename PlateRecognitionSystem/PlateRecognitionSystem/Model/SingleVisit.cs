using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateRecognitionSystem.Model
{
    public class SingleVisit
    {
        [Key]
        public int ID { get; set; }
        public double?  Price { get; set; }
        public  DateTime EntryDate { get; set; }
        public DateTime? ExitDate { get; set; }
        public virtual Vehicle Vehicle { get; set; }

    }
}
