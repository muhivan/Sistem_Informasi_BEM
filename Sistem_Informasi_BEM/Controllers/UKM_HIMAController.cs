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
    public class UKM_HIMAController : Controller
    {
        private DBSIBEMEntities db = new DBSIBEMEntities();

        public ActionResult Index(int? id)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            if (id == 0)
            {
                ViewBag.status = 0;
                var ukmhima = db.msukm_hima.Where(m => m.status == 0);
                return View(ukmhima.ToList());
            }
            else
            {
                ViewBag.status = 1;
                var ukmhima = db.msukm_hima.Where(m => m.status == 1);
                return View(ukmhima.ToList());
            }
        }

        public ActionResult Details(int? id)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            msukm_hima msukm_hima = db.msukm_hima.Find(id);
            if (msukm_hima == null)
            {
                return HttpNotFound();
            }
            return View(msukm_hima);
        }

        public ActionResult Create()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            var ddldept = db.msdepartemen.Where(m => m.status == 1);
            ViewBag.iddepartemen = new SelectList(ddldept, "iddepartemen", "namadepartemen");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(msukm_hima msukm_hima)
        {
            var cek = db.msukm_hima.FirstOrDefault(x => x.nama == msukm_hima.nama);
            if (cek == null)
            {
                if (ModelState.IsValid)
                {
                    msukm_hima.status = 1;
                    msukm_hima.creadate = DateTime.Now;
                    db.msukm_hima.Add(msukm_hima);
                    db.SaveChanges();
                    ViewBag.Jabatan = this.Session["Jabatan"];
                    ViewBag.Departemen = this.Session["Departemen"];
                    return RedirectToAction("Index");
                }
                return View(msukm_hima);
            }
            else
            {
                ViewBag.Jabatan = this.Session["Jabatan"];
                ViewBag.Departemen = this.Session["Departemen"];
                ViewBag.Message = "UKM ATAU HIMA YANG ANDA MASUKAN SUDAH ADA";
                return View(msukm_hima);
            }
        }

        public ActionResult Edit(int? id)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            msukm_hima msukm_hima = db.msukm_hima.Find(id);
            if (msukm_hima == null)
            {
                return HttpNotFound();
            }
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            var ddldept = db.msdepartemen.Where(m => m.status == 1);
            ViewBag.iddepartemen = new SelectList(ddldept, "iddepartemen", "namadepartemen", msukm_hima.iddepartemen);
            return View(msukm_hima);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(msukm_hima msukm_hima)
        {
            var cek = db.msukm_hima.FirstOrDefault(x => x.nama == msukm_hima.nama);

            if (cek == null)
            {
                if (ModelState.IsValid)
                {
                    msukm_hima msukmhima = db.msukm_hima.Find(msukm_hima.idukm_hima);
                    msukmhima.nama = msukm_hima.nama;
                    msukmhima.modiby = msukm_hima.modiby;
                    msukmhima.modidate = DateTime.Now;
                    db.SaveChanges();
                    ViewBag.Jabatan = this.Session["Jabatan"];
                    ViewBag.Departemen = this.Session["Departemen"];
                    return RedirectToAction("Index");
                }
                return View(msukm_hima);
            }
            else
            {
                ViewBag.Jabatan = this.Session["Jabatan"];
                ViewBag.Departemen = this.Session["Departemen"];
                ViewBag.Message = "UKM ATAU HIMA YANG ANDA MASUKAN SUDAH ADA";
                var ddldept = db.msdepartemen.Where(m => m.status == 1);
                ViewBag.iddepartemen = new SelectList(ddldept, "iddepartemen", "namadepartemen", msukm_hima.iddepartemen);
                return View(msukm_hima);
            }
            
        }

        public ActionResult Delete(int id)
        {
            msukm_hima msukm_hima = db.msukm_hima.Find(id);
            msukm_hima.status = 0;
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
