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
    public class DepartemenController : Controller
    {
        //object database
        private DBSIBEMEntities db = new DBSIBEMEntities();
        //menampilkan data
        public ActionResult Index(int? id)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            if (id == 0)
            {
                ViewBag.status = 0;
                var depts = db.msdepartemen.Where(m => m.status == 0);
                return View(depts.ToList());
            }
            else
            {
                ViewBag.status = 1;
                var depts = db.msdepartemen.Where(m => m.status == 1);
                return View(depts.ToList());
            }
        }
        //menampilkan detils
        public ActionResult Details(int? id)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            msdeparteman msdeparteman = db.msdepartemen.Find(id);
            if (msdeparteman == null)
            {
                return HttpNotFound();
            }
            return View(msdeparteman);
        }

        public ActionResult Create()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(msdeparteman msdeparteman)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            var cek = db.msdepartemen.FirstOrDefault(x => x.namadepartemen == msdeparteman.namadepartemen);
            if (cek == null)
            {
                if (ModelState.IsValid)
                {
                    msdeparteman.status = 1;
                    msdeparteman.creadate = DateTime.Now;
                    db.msdepartemen.Add(msdeparteman);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(msdeparteman);
            }
            else
            {
                ViewBag.Message = "DEPARTEMEN YANG ANDA MASUKAN SUDAH ADA";
                return View(msdeparteman);
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
            msdeparteman msdeparteman = db.msdepartemen.Find(id);
            if (msdeparteman == null)
            {
                return HttpNotFound();
            }
            return View(msdeparteman);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(msdeparteman msdeparteman)
        {
            var cek = db.msdepartemen.FirstOrDefault(x => x.namadepartemen == msdeparteman.namadepartemen);
            if (cek == null)
            {
                if (ModelState.IsValid)
                {
                    msdeparteman msdepart = db.msdepartemen.Find(msdeparteman.iddepartemen);
                    msdepart.namadepartemen = msdeparteman.namadepartemen;
                    msdepart.modiby = msdeparteman.modiby;
                    msdepart.modidate = DateTime.Now;
                    db.SaveChanges();
                    ViewBag.Jabatan = this.Session["Jabatan"];
                    ViewBag.Departemen = this.Session["Departemen"];
                    return RedirectToAction("Index");
                }
                return View(msdeparteman);
            }
            else
            {
                ViewBag.Jabatan = this.Session["Jabatan"];
                ViewBag.Departemen = this.Session["Departemen"];
                ViewBag.Message = "DEPARTEMEN YANG ANDA MASUKAN SUDAH ADA";
                return View(msdeparteman);
            }     
        }

        public ActionResult Delete(int id)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            msdeparteman msdeparteman = db.msdepartemen.Find(id);
            msdeparteman.status = 0;
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
