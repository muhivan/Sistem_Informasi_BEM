using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sistem_Informasi_BEM.Models;

namespace Sistem_Informasi_BEM.Controllers
{
    public class KasController : Controller
    {
        private DBSIBEMEntities db = new DBSIBEMEntities();

        public ActionResult Index()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            var idagt_ = this.Session["idanggota"];
            int agt = Convert.ToInt32(idagt_);
            var trkas = db.trkas.Include(t => t.msanggotabem).Where(t=>t.idanggota==agt);
            return View(trkas.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.idanggota2 = this.Session["idanggota"];
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            ViewBag.idanggota = new SelectList(db.msanggotabems, "idanggota", "nama");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(trka trka)
        {
            if (ModelState.IsValid)
            {
                ViewBag.idanggota2 = this.Session["idanggota"];
                trka.status = 1;
                trka.creadate = DateTime.Now;
                db.trkas.Add(trka);
                db.SaveChanges();
                ViewBag.Jabatan = this.Session["Jabatan"];
                ViewBag.Departemen = this.Session["Departemen"];
                return RedirectToAction("Index");
            }
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            ViewBag.idanggota = new SelectList(db.msanggotabems, "idanggota", "nama", trka.idanggota);
            return View(trka);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            trka trka = db.trkas.Find(id);
            if (trka == null)
            {
                return HttpNotFound();
            }
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            ViewBag.idanggota = new SelectList(db.msanggotabems, "idanggota", "nama", trka.idanggota);
            return View(trka);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(trka trka)
        {
            if (ModelState.IsValid)
            {
                ViewBag.idanggota2 = this.Session["idanggota"];
                trka khas = db.trkas.Find(trka.idkas);
                khas.modidate = DateTime.Now;
                khas.nominal = trka.nominal;
                khas.modiby = trka.modiby;
                khas.jeniskas = trka.jeniskas;
                db.SaveChanges();
                ViewBag.Jabatan = this.Session["Jabatan"];
                ViewBag.Departemen = this.Session["Departemen"];
                return RedirectToAction("Index");
            }
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            ViewBag.idanggota = new SelectList(db.msanggotabems, "idanggota", "nama", trka.idanggota);
            return View(trka);
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
