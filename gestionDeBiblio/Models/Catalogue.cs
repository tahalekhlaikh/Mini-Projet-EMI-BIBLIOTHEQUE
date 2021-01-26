using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gestionDeBiblio.Models

{
    public class Catalogue
    {
        private static Catalogue instance;

        private Catalogue() {}

        public static Catalogue Instance
        {
            get 
            {
            if (instance == null)
            {
            instance = new Catalogue();
            }
            return instance;
         }
     }

        public int ID { get; set; }
        public ICollection<Livre> livres { get; set; }

        public Livre chercherLivre(String ISBN) {
            Livre monLivre = new Livre();
            foreach(var livre in livres) {
                if (livre.ID == ISBN)
                    monLivre = livre;
            }
            return monLivre;
        }
    }
}