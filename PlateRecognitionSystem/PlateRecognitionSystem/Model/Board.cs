using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateRecognitionSystem.Model
{
   public class Board
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string FunctionName { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
