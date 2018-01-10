using PlateRecognitionSystem.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateRecognitionSystem.Model
{
    public class Price
    {
        [Key]
        public int ID { get; set; }
        public PriceEnum PriceEnum { get; set; }
        public double Cost { get; set; }
    }
}
