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
    public class LivresController : Controller
    {
        private DbContextBibliotheque db = new DbContextBibliotheque();

        
        public ActionResult Index(String titre, String auteur)
        {
            var livres = from a in db.livres select a;
            if (!string.IsNullOrEmpty(titre))
            {
                livres = livres.Where(s => s.titre.Contains(titre));
            }
            if (!string.IsNullOrEmpty(auteur))
            {
                livres = livres.Where(s => s.auteur.Contains(auteur));
            }
            return View(livres);
        }

        
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Livre livre = db.livres.Find(id);
            if (livre == null)
            {
                return HttpNotFound();
            }

            var emprunteurs = from a in db.adherents
                                  join p in db.prets on a.ID equals p.AdherentID
                                  join l in db.livres on p.LivreID equals l.ID
                                  where p.LivreID == id

                                  select a;

            ViewBag.emprunteurs = emprunteurs;
            return View(livre);
        }

        
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,titre,auteur")] Livre livre)
        {
            var livretrouve = from l in db.livres where l.ID == livre.ID select l;

            if (livretrouve.Count() == 0)
            {
                if (ModelState.IsValid)
                {
                    db.livres.Add(livre);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("L'ISBN saisit existe déjà");
            }

            return View(livre);
        }

        
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Livre livre = db.livres.Find(id);
            if (livre == null)
            {
                return HttpNotFound();
            }
            return View(livre);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,titre,auteur")] Livre livre)
        {
            
                if (ModelState.IsValid)
                {
                    db.Entry(livre).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
          
            
            return View(livre);
        }

        
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Livre livre = db.livres.Find(id);
            if (livre == null)
            {
                return HttpNotFound();
            }
            return View(livre);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Livre livre = db.livres.Find(id);
            var prets = from p in db.prets where p.LivreID == id select p;
            if (prets.Count() == 0)
            {
                db.livres.Remove(livre);
                db.SaveChanges();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Impossible de supprimer ce livre, il est pêté");
            }
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
