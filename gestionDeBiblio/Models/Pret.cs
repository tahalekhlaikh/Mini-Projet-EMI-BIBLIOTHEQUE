using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using gestionDeBiblio.Models;
using System.ComponentModel.DataAnnotations;

namespace gestionDeBiblio.Models
{
    public class Pret
    {
        public int ID { get; set; }
        [Display(Name = "Date du prêt")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString="{0:yyyy-MM-dd}", ApplyFormatInEditMode=true)]
        public DateTime date { get; set; }
        public String LivreID { get; set; }
        public int AdherentID { get; set; }
        public Adherent emprunteur { get; set; }
        public Livre leLivrePrete { get; set; }

        public DateTime getDateRetour()
        {
            return date.AddDays(15);
        }
    }
}