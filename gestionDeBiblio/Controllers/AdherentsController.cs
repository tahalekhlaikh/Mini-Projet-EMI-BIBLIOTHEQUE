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
    public class AdherentsController : Controller
    {
        private DbContextBibliotheque db = new DbContextBibliotheque();

        
        public ActionResult Index(String nom, String prenom)
        {
            var adherents = from a in db.adherents select a;
            if (!string.IsNullOrEmpty(nom))
            {
                adherents = adherents.Where(s => s.nom.Contains(nom));
            }
            if (!string.IsNullOrEmpty(prenom))
            {
                adherents = adherents.Where(s => s.prenom.Contains(prenom));
            }
            return View(adherents);

        }

       
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adherent adherent = db.adherents.Find(id);
            if (adherent == null)
            {
                return HttpNotFound();
            }

           var livresempruntes = from l in db.livres
                                  join p in db.prets on l.ID equals p.LivreID
                                  join a in db.adherents on p.AdherentID equals a.ID
                                  where p.AdherentID == id

                                  select l;    
                     
            ViewBag.livresemprunte = livresempruntes;
            
            return View(adherent);
        }

        
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,nom,prenom")] Adherent adherent)
        {
            if (ModelState.IsValid)
            {
                db.adherents.Add(adherent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(adherent);
        }

        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adherent adherent = db.adherents.Find(id);
            if (adherent == null)
            {
                return HttpNotFound();
            }
            return View(adherent);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,nom,prenom")] Adherent adherent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(adherent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(adherent);
        }

        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Adherent adherent = db.adherents.Find(id);
            if (adherent == null)
            {
                return HttpNotFound();
            }
            return View(adherent);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            
            Adherent adherent = db.adherents.Find(id);
            db.adherents.Remove(adherent);
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
