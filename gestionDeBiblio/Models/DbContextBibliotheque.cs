using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace gestionDeBiblio.Models
{
    public class DbContextBibliotheque :DbContext
    {
        public DbSet<Bibliotheque> bibliotheques { get; set; }
        public DbSet<Catalogue> catalogues { get; set; }
        public DbSet<Adherent> adherents { get; set; }
        public DbSet<Livre> livres { get; set; }
        public DbSet<Pret> prets { get; set; }
        
    }
}