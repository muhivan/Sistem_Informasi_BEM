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
    public class dtlrapatController : Controller
    {
        private DBSIBEMEntities db = new DBSIBEMEntities();

        // GET: dtlrapat
        public ActionResult Index()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            var id =Convert.ToInt32(Session["idanggota"]);
            var dtlrapats = db.dtlrapats.Include(d => d.msanggotabem).Include(d => d.trrapat).Where(d => d.idanggota == id);
            return View(dtlrapats.ToList());
        }

        // GET: dtlrapat/Details/5
        public ActionResult Details(int? id, int? id1)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dtlrapat dtlrapat = db.dtlrapats.Find(id,id1);
            if (dtlrapat == null)
            {
                return HttpNotFound();
            }
            return View(dtlrapat);
        }

        // GET: dtlrapat/Create
        //public ActionResult Create()
        //{
        //    ViewBag.idanggota = new SelectList(db.msanggotabems, "idanggota", "nama");
        //    ViewBag.idrapat = new SelectList(db.trrapats, "idrapat", "judulrapat");
        //    return View();
        //}

        //// POST: dtlrapat/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "idrapat,idanggota,keterangan")] dtlrapat dtlrapat)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.dtlrapats.Add(dtlrapat);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.idanggota = new SelectList(db.msanggotabems, "idanggota", "nama", dtlrapat.idanggota);
        //    ViewBag.idrapat = new SelectList(db.trrapats, "idrapat", "judulrapat", dtlrapat.idrapat);
        //    return View(dtlrapat);
        //}

        // GET: dtlrapat/Edit/5
        public ActionResult Edit(int? id, int? id1)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            dtlrapat dtlrapat = db.dtlrapats.Find(id, id1);
            if (dtlrapat == null)
            {
                return HttpNotFound();
            }
            ViewBag.idanggota = new SelectList(db.msanggotabems, "idanggota", "nama", dtlrapat.idanggota);
            ViewBag.idrapat = new SelectList(db.trrapats, "idrapat", "judulrapat", dtlrapat.idrapat);
            return View(dtlrapat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idrapat,idanggota,keterangan")] dtlrapat dtlrapat)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            dtlrapat rapat = db.dtlrapats.Find(dtlrapat.idrapat, dtlrapat.idanggota);
            trrapat r = db.trrapats.Find(rapat.idrapat);
            if (r.status == 1)
            {
                if (ModelState.IsValid)
                {


                    if (dtlrapat.keterangan != null)
                    {
                        rapat.keterangan = dtlrapat.keterangan.ToUpper();
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            } else if (r.status == 0)
            {
                ViewBag.Ubah = "TIDAK BISA DIUBAH! RAPAT TELAH DIBATALKAN!";
            } else
            {
                ViewBag.Ubah = "TIDAK BISA DIUBAH! RAPAT TELAH TERLAKSANA!";
            }
            ViewBag.idanggota = new SelectList(db.msanggotabems, "idanggota", "nama", dtlrapat.idanggota);
            ViewBag.idrapat = new SelectList(db.trrapats, "idrapat", "judulrapat", dtlrapat.idrapat);
            return View(dtlrapat);
        }

        // GET: dtlrapat/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    dtlrapat dtlrapat = db.dtlrapats.Find(id);
        //    if (dtlrapat == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(dtlrapat);
        //}

        //// POST: dtlrapat/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    dtlrapat dtlrapat = db.dtlrapats.Find(id);
        //    db.dtlrapats.Remove(dtlrapat);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
