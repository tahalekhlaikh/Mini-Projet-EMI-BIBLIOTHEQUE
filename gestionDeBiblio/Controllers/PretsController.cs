using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using gestionDeBiblio.Models;

namespace gestionDeBiblio.Controllers
{
    public class PretsController : Controller
    {
        private DbContextBibliotheque db = new DbContextBibliotheque();

        
        public ActionResult Index(String nom,String titre)
        {
           
            var prets = db.prets.Include(p => p.emprunteur).Include(p => p.leLivrePrete);
            if (!string.IsNullOrEmpty(titre))
            {
                prets = prets.Where(s => s.leLivrePrete.titre.Contains(titre));
            }
            if (!string.IsNullOrEmpty(nom))
            {
                prets = prets.Where(s => s.emprunteur.nom.Contains(nom));
            }
            return View(prets);
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pret pret = db.prets.Find(id);
            if (pret == null)
            {
                return HttpNotFound();
            }

            Adherent adherent = db.adherents.Find(pret.AdherentID);
            pret.emprunteur = adherent;
            Livre livre = db.livres.Find(pret.LivreID);
            pret.leLivrePrete = livre;
            return View(pret);
        }

        
        public ActionResult Create()
        {
            var livresPretes = from l in db.prets select l.leLivrePrete;
            var livreNonPretes = from l in db.livres where !livresPretes.Contains(l) select l;

            var adherentAutorisable = from a in db.adherents where a.pretsEnCours.Count == 3 select a;
            List<Adherent> adherentAutorise = adherentAutorisable.ToList<Adherent>();

            List<Adherent> adhWithDepas = new List<Adherent>();
            var listpret = from p in db.prets select p;
            List<Pret> listprets = listpret.ToList<Pret>();

            var listAdherents = from a in db.adherents select a;
            List<Adherent> listAdherent = listAdherents.ToList<Adherent>();


            foreach (Pret pr in listprets)
            {
                Adherent d = db.adherents.Find(pr.AdherentID);
                if (DateTime.Compare(pr.getDateRetour().Date, DateTime.Now.Date) < 0)
                {
                    adhWithDepas.Add(d);
                    listAdherent.Remove(d);
                }
            }
            foreach (Adherent d in adherentAutorise)
            {
                listAdherent.Remove(d);
            }



            var list = adhWithDepas.GroupBy(x => x.ID).Select(y => y.First());
            List<Adherent> lista = list.ToList<Adherent>();
            lista.Union(adherentAutorise);



            if (livreNonPretes.Count() == 0 || listAdherent.Count() == 0)
            {
                System.Windows.Forms.MessageBox.Show("Pas de livre à pêter ou d'emprunteur autorisé");
                return RedirectToAction("Index");
            }

            ViewBag.AdherentID = new SelectList(listAdherent, "ID", "nom");
            ViewBag.LivreID = new SelectList(livreNonPretes, "ID", "titre");

            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,date,LivreID,AdherentID")] Pret pret)
        {
            if (ModelState.IsValid)
            {
                db.prets.Add(pret);   
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AdherentID = new SelectList(db.adherents, "ID", "nom", pret.AdherentID);
            ViewBag.LivreID = new SelectList(db.livres, "ID", "titre", pret.LivreID);
            return View(pret);
        }

        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pret pret = db.prets.Find(id);
            if (pret == null)
            {
                return HttpNotFound();
            }
           
            pret.emprunteur = db.adherents.Find(pret.AdherentID);
            pret.leLivrePrete = db.livres.Find(pret.LivreID);

            var livresPretes = from l in db.prets select l.leLivrePrete;
            var livreNonPretes = from l in db.livres where !livresPretes.Contains(l) select l;

            var adherentAutorisable = from a in db.adherents where a.pretsEnCours.Count == 3 select a;
            List<Adherent> adherentAutorise = adherentAutorisable.ToList<Adherent>();

            List<Adherent> adhWithDepas = new List<Adherent>();
            var listpret = from p in db.prets select p;
            List<Pret> listprets = listpret.ToList<Pret>();

            var listAdherents = from a in db.adherents select a;
            List<Adherent> listAdherent = listAdherents.ToList<Adherent>();


            foreach (Pret pr in listprets)
            {
                Adherent d = db.adherents.Find(pr.AdherentID);
                if (DateTime.Compare(pr.getDateRetour().Date, DateTime.Now.Date) < 0)
                {
                    adhWithDepas.Add(d);
                    listAdherent.Remove(d);
                }
            }
            foreach (Adherent d in adherentAutorise)
            {
                listAdherent.Remove(d);
            }



            var list = adhWithDepas.GroupBy(x => x.ID).Select(y => y.First());
            List<Adherent> lista = list.ToList<Adherent>();
            lista.Union(adherentAutorise);

            if (livreNonPretes.Count() == 0 || listAdherent.Count() == 0)
            {
                var adherentAutorisee = from a in db.adherents where a.ID==pret.AdherentID select a; ;
                livreNonPretes = from l in db.livres where l.ID==pret.LivreID select l;
                ViewBag.AdherentID = new SelectList(adherentAutorisee, "ID", "nom", pret.AdherentID);
            }
            else
            {
                ViewBag.AdherentID = new SelectList(listAdherent, "ID", "nom", pret.AdherentID);
            }

            
            ViewBag.LivreID = new SelectList(livreNonPretes, "ID", "titre", pret.LivreID);
            return View(pret);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,date,LivreID,AdherentID")] Pret pret)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pret).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AdherentID = new SelectList(db.adherents, "ID", "nom", pret.AdherentID);
            ViewBag.LivreID = new SelectList(db.livres, "ID", "titre", pret.LivreID);
            return View(pret);
        }

        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pret pret = db.prets.Find(id);
            if (pret == null)
            {
                return HttpNotFound();
            }
            Adherent adherent = db.adherents.Find(pret.AdherentID);
            pret.emprunteur = adherent;
            Livre livre = db.livres.Find(pret.LivreID);
            pret.leLivrePrete = livre;
            return View(pret);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pret pret = db.prets.Find(id);
            db.prets.Remove(pret);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
