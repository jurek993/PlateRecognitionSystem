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
        //TODO: a co jeśli wjeżdzając pojazd zostanie źle rozpoznany i przy wyjeździe rozpozna dobrze? Wtedy nie będzie zgodności. Jednak na razie nie ma czasu się tym zająć. 
        //Trzeba założyć, że skuteczność rozpoznawania jest bardzo wysoka :)
        [Key]
        public int ID { get; set; }
        public Vehicle Vehicle { get; set; }
        public  DateTime EntryDate { get; set; }
        public DateTime? ExitDate { get; set; }
        public virtual Price Price { get; set; }
    }
}
