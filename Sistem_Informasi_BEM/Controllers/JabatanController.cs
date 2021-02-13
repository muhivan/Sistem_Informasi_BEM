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
    public class JabatanController : Controller
    {
        private DBSIBEMEntities db = new DBSIBEMEntities();

        public ActionResult Index(int? id)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            if (id == 0)
            {
                ViewBag.status = 0;
                var jabatan = db.msjabatans.Where(m => m.status == 0);
                return View(jabatan.ToList());
            }
            else
            {
                ViewBag.status = 1;
                var jabatan = db.msjabatans.Where(m => m.status == 1);
                return View(jabatan.ToList());
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
            msjabatan msjabatan = db.msjabatans.Find(id);
            if (msjabatan == null)
            {
                return HttpNotFound();
            }
            return View(msjabatan);
        }

        public ActionResult Create()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            var ddlukmhima = db.msukm_hima.Where(m => m.status == 1);
            ViewBag.idukm_hima = new SelectList(ddlukmhima, "idukm_hima", "nama");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(msjabatan msjabatan)
        {
            var cek = db.msjabatans.FirstOrDefault(x => x.namajabatan == msjabatan.namajabatan && x.status == 1);

            if(msjabatan.idukm_hima == null || msjabatan.idukm_hima == 0)
            {
                ViewBag.Jabatan = this.Session["Jabatan"];
                ViewBag.Departemen = this.Session["Departemen"];
                ViewBag.Message = "JABATAN ATAU UKM/HIMA YANG ANDA MASUKAN SUDAH ADA";
                return View(msjabatan);
            }

            if (cek == null)
            {
                if (ModelState.IsValid)
                {
                    msjabatan.status = 1;
                    msjabatan.creaby = msjabatan.creaby;
                    msjabatan.creadate = DateTime.Now;
                    db.msjabatans.Add(msjabatan);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.Jabatan = this.Session["Jabatan"];
                ViewBag.Departemen = this.Session["Departemen"];
                ViewBag.idukm_hima = new SelectList(db.msukm_hima, "idukm_hima", "nama", msjabatan.idukm_hima);
                return View(msjabatan);
            }
            else
            {
                ViewBag.Jabatan = this.Session["Jabatan"];
                ViewBag.Departemen = this.Session["Departemen"];
                ViewBag.Message = "JABATAN ATAU UKM/HIMA YANG ANDA MASUKAN SUDAH ADA";
                return View(msjabatan);
            }

        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            msjabatan msjabatan = db.msjabatans.Find(id);
            if (msjabatan == null)
            {
                return HttpNotFound();
            }
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            ViewBag.idukm_hima = new SelectList(db.msukm_hima, "idukm_hima", "nama", msjabatan.idukm_hima);
            return View(msjabatan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(msjabatan msjabatan, string cek_namajabatan)
        {
            var cek = db.msjabatans.FirstOrDefault(x => x.namajabatan == msjabatan.namajabatan && x.status==1);

            if(msjabatan.namajabatan.Equals(cek_namajabatan))
            {
                if (ModelState.IsValid)
                {
                    msjabatan jabatan = db.msjabatans.Find(msjabatan.idjabatan);
                    jabatan.namajabatan = msjabatan.namajabatan;
                    jabatan.idukm_hima = msjabatan.idukm_hima;
                    jabatan.modidate = DateTime.Now;
                    jabatan.modiby = msjabatan.modiby;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.Jabatan = this.Session["Jabatan"];
                ViewBag.Departemen = this.Session["Departemen"];
                ViewBag.idukm_hima = new SelectList(db.msukm_hima, "idukm_hima", "nama", msjabatan.idukm_hima);
                return View(msjabatan);
            }
            else
            {
                if (cek == null)
                {
                    if (ModelState.IsValid)
                    {
                        msjabatan jabatan = db.msjabatans.Find(msjabatan.idjabatan);
                        jabatan.namajabatan = msjabatan.namajabatan;
                        jabatan.idukm_hima = msjabatan.idukm_hima;
                        jabatan.modidate = DateTime.Now;
                        jabatan.modiby = msjabatan.modiby;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    ViewBag.Jabatan = this.Session["Jabatan"];
                    ViewBag.Departemen = this.Session["Departemen"];
                    ViewBag.idukm_hima = new SelectList(db.msukm_hima, "idukm_hima", "nama", msjabatan.idukm_hima);
                    return View(msjabatan);
                }
                else
                {
                    ViewBag.Jabatan = this.Session["Jabatan"];
                    ViewBag.Departemen = this.Session["Departemen"];
                    ViewBag.idukm_hima = new SelectList(db.msukm_hima, "idukm_hima", "nama", msjabatan.idukm_hima);
                    ViewBag.Message = "NAMA JABATAN YANG ANDA MASUKAN SUDAH ADA";
                    return View(msjabatan);
                }
            } 
        }

        public ActionResult Delete(int id)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            msjabatan msjabatan = db.msjabatans.Find(id);
            msjabatan.status = 0;
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
