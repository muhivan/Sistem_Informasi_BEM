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
    public class KasBPHController : Controller
    {
        private DBSIBEMEntities db = new DBSIBEMEntities();

        public ActionResult Index()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            var idDept_ = (string)Session["idDept"];
            int idDept = Convert.ToInt32(idDept_);

            var trkas = db.trkas.SqlQuery("SELECT t.* FROM trkas t INNER JOIN msanggotabem j on t.idanggota = j.idanggota where iddepartemen = " + idDept).ToList();
            return View(trkas.ToList());
        }

        public ActionResult IndexBPH()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            var idDept_ = (string)Session["idDept"];
            int idDept = Convert.ToInt32(idDept_);

            var trkas = db.trkas.Include(t => t.msanggotabem).Where(t => t.jeniskas == "BEM");
            return View(trkas.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.idanggota2 = this.Session["idanggota"];
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            ViewBag.idanggota = new SelectList(db.msanggotabems, "idanggota", "nama");
            ViewBag.iddept = (string)Session["idDept"];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(trka trka)
        {
            ViewBag.idanggota2 = this.Session["idanggota"];
            ViewBag.iddept = (string)Session["idDept"];
            if (ModelState.IsValid)
            {
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

        public ActionResult Edit(int id)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            trka khas = db.trkas.Find(id);
            if (khas.jeniskas.Equals("BEM"))
            {
                khas.status = 2;
            }
            else
            {
                khas.status = 3;
            }
            khas.modidate = DateTime.Now;
            khas.modiby = (string)Session["modiby"];
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult EditBPH(int id)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            trka khas = db.trkas.Find(id);
            khas.status = 3;
            khas.modidate = DateTime.Now;
            khas.modiby = (string)Session["modiby"];
            db.SaveChanges();
            return RedirectToAction("IndexBPH");
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
