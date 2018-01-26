using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateRecognitionSystem.Model
{
    public class Vehicle
    {
        [Key]
        public int ID { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string NumberPlate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public double TotalPay { get; set; }
        public virtual User Owner { get; set; }
    }
}
