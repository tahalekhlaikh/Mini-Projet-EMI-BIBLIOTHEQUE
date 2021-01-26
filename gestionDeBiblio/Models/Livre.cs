using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace gestionDeBiblio.Models
{
    public class Livre
    {
        [Display(Name = "ISBN")]
        public String ID { get; set; }
        [Display(Name = "Titre")]
        public String titre { get; set; }
        [Display(Name = "Auteur")]
        public String auteur { get; set; }

    }
}