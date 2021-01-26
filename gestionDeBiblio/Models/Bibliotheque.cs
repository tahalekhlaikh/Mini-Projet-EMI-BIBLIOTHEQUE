using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using gestionDeBiblio.Models;

namespace gestionDeBiblio.Models
{
    public class Bibliotheque
    {
        private static Bibliotheque instance;

        private Bibliotheque() {}

        public static Bibliotheque  Instance
        {
            get 
            {
            if (instance == null)
            {
            instance = new Bibliotheque();
            }
            return instance;
         }
     }
        public int ID { get; set; }
        public int CatalogueID { get; set; }
        public ICollection<Pret> lesPrets { get; set; }
        public Catalogue leCatalogue { get; set; }
        public ICollection<Adherent> mesAdherents { get; set; }
        public Adherent adherentCourant { get; set; }

        public Adherent indiquerEmprunteur(int ID)
        {
            
            foreach (Pret pret in lesPrets)
            {
                if (pret.AdherentID == ID)
                {
                    foreach (Adherent adherent in mesAdherents){
                        if (adherent.ID == ID)
                        {
                            return adherent;
                        }
                    }
                }
            }
            return null;
        }

        public void emprunterLivre(String ISBN){

            Pret pret =new Pret ();
            pret.AdherentID= adherentCourant.ID;
            pret.LivreID= ISBN;
            pret.date= DateTime.Now;
            
            pret.emprunteur = adherentCourant;
            pret.leLivrePrete= leCatalogue.chercherLivre (ISBN);
 
        }

        public ICollection<Pret> editerBulletinDePret()
        {
            return lesPrets;
        }
        
    }
}