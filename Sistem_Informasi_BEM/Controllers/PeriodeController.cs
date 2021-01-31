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
    public class PeriodeController : Controller
    {
        private DBSIBEMEntities db = new DBSIBEMEntities();

        public ActionResult Index(int? id)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            if (id == 0)
            {
                ViewBag.status = 0;
                var periode = db.msperiodes.Where(m => m.status == 0);
                return View(periode.ToList());
            }
            else
            {
                ViewBag.status = 1;
                var periode = db.msperiodes.Where(m => m.status == 1);
                return View(periode.ToList());
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
            msperiode msperiode = db.msperiodes.Find(id);
            if (msperiode == null)
            {
                return HttpNotFound();
            }
            return View(msperiode);
        }

        public ActionResult Create()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(msperiode msperiode)
        {
            var cek = db.msperiodes.FirstOrDefault(x => x.tahunperiode == msperiode.tahunperiode);

            if (cek == null)
            {
                if (ModelState.IsValid)
                {
                    msperiode.status = 1;
                    msperiode.creadate = DateTime.Now;
                    db.msperiodes.Add(msperiode);
                    db.SaveChanges();
                    ViewBag.Jabatan = this.Session["Jabatan"];
                    ViewBag.Departemen = this.Session["Departemen"];
                    return RedirectToAction("Index");
                }

                return View(msperiode);
            }
            else
            {
                ViewBag.Jabatan = this.Session["Jabatan"];
                ViewBag.Departemen = this.Session["Departemen"];
                ViewBag.Message = "PERIODE YANG ANDA MASUKAN SUDAH ADA";
                return View(msperiode);
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
            msperiode msperiode = db.msperiodes.Find(id);
            if (msperiode == null)
            {
                return HttpNotFound();
            }
            return View(msperiode);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idperiode,tahunperiode,status,creaby,creadate,modiby,modidate")] msperiode msperiode)
        {
            var cek = db.msperiodes.FirstOrDefault(x => x.tahunperiode == msperiode.tahunperiode);
            if (cek == null)
            {
                if (ModelState.IsValid)
                {
                    msperiode periode = db.msperiodes.Find(msperiode.idperiode);
                    periode.tahunperiode = msperiode.tahunperiode;
                    periode.modidate = DateTime.Now;
                    periode.modiby = periode.modiby;
                    db.SaveChanges();
                    ViewBag.Jabatan = this.Session["Jabatan"];
                    ViewBag.Departemen = this.Session["Departemen"];
                    return RedirectToAction("Index");
                }
                return View(msperiode);
            }
            else
            {
                ViewBag.Jabatan = this.Session["Jabatan"];
                ViewBag.Departemen = this.Session["Departemen"];
                ViewBag.Message = "PERIODE YANG ANDA MASUKAN SUDAH ADA";
                return View(msperiode);
            }  
        }

        public ActionResult Delete(int id)
        {
            msperiode msperiode = db.msperiodes.Find(id);
            msperiode.status = 0;
            db.SaveChanges();
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
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
