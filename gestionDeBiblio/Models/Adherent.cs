using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using gestionDeBiblio.Models;

namespace gestionDeBiblio.Models
{
    public class Adherent
    {
        [Display(Name = "N° Adherent")]
        public int ID { get; set; }
        [Display(Name = "Nom")]
        public String nom { get; set; }
        [Display(Name = "Prénom")]
        public String prenom { get; set; }
        public ICollection<Pret> pretsEnCours { get; set; }


    }
}