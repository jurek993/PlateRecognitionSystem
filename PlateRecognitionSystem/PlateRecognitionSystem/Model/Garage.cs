using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateRecognitionSystem.Model
{
   public class Garage
    {
        [Key]
        public int ID { get; set; }
        public int Capacity { get; set; }
        public int Occupancy { get; set; }
    }
}
