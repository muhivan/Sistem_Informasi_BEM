using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sistem_Informasi_BEM.Models;

namespace Sistem_Informasi_BEM.Controllers
{
    public class trnotulensiController : Controller
    {
        private DBSIBEMEntities db = new DBSIBEMEntities();

        // GET: trnotulensi
        public ActionResult Index()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            var idDept_ = (string)Session["idDept"];
            int idDept = Convert.ToInt32(idDept_);

            var notulen = db.trnotulensis.SqlQuery("SELECT n.* FROM trnotulensi n INNER JOIN  trrapat t on n.idrapat = t.idrapat where t.iddepartemen = " + idDept + "AND n.status = '1'").ToList();
            return View(notulen.ToList());
        }

        // GET: trnotulensi/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    trnotulensi trnotulensi = db.trnotulensis.Find(id);
        //    if (trnotulensi == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    var notulen = db.trrapats.SqlQuery("SELECT t.* FROM trnotulensi n INNER JOIN  trrapat t on n.idrapat = t.idrapat where n.idnotulensi = " + id).ToList();
        //    return View(notulen);
        //}

        // GET: trnotulensi/Create
        public ActionResult Create()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            ViewBag.idrapat = new SelectList(db.trrapats, "idrapat", "judulrapat");
            return View();
        }

        // POST: trnotulensi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(trnotulensi trnotulensi)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            if(trnotulensi.idrapat == 0 || trnotulensi.idrapat == null)
            {
                ViewBag.Message = "Data Tidak Boleh Kosong";
                ViewBag.idrapat = new SelectList(db.trrapats, "idrapat", "judulrapat", trnotulensi.idrapat);
                return View(trnotulensi);
            }
            if (ModelState.IsValid)
            {
                foreach (var file in trnotulensi.files)
                {

                    if (file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(Server.MapPath("~/Files"), fileName);
                        file.SaveAs(filePath);

                        trnotulensi.idrapat = trnotulensi.idrapat;
                        trnotulensi.notulensi = fileName.ToString();
                        trnotulensi.status = 1;
                        trnotulensi.creadate = DateTime.Now;
                        trnotulensi.modidate = DateTime.Now; 
                        trnotulensi.creaby = trnotulensi.creaby.ToString();
                        trnotulensi.modiby = trnotulensi.modiby.ToString();

                        db.trnotulensis.Add(trnotulensi);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            ViewBag.Message = "Data Tidak Boleh Kosong";
            ViewBag.idrapat = new SelectList(db.trrapats, "idrapat", "judulrapat", trnotulensi.idrapat);
            return View(trnotulensi);
        }

        // GET: trnotulensi/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            trnotulensi trnotulensi = db.trnotulensis.Find(id);
            if (trnotulensi == null)
            {
                return HttpNotFound();
            }
            ViewBag.idrapat = new SelectList(db.trrapats, "idrapat", "judulrapat", trnotulensi.idrapat);
            return View(trnotulensi);
        }

        // POST: trnotulensi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(trnotulensi trnotulensi)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            if (ModelState.IsValid)
            {
                trnotulensi msformat = db.trnotulensis.Find(trnotulensi.idnotulensi);
                foreach (var file in trnotulensi.files)
                {

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(Server.MapPath("~/Files"), fileName);
                        file.SaveAs(filePath);

                        msformat.notulensi = fileName.ToString();

                    }

                    msformat.modidate = DateTime.Now;
                    msformat.modiby = trnotulensi.modiby.ToString();

                    db.SaveChanges();
                    ViewBag.Jabatan = this.Session["Jabatan"];
                    ViewBag.Departemen = this.Session["Departemen"];
                    return RedirectToAction("Index");
                }

            }
            ViewBag.idrapat = new SelectList(db.trrapats, "idrapat", "judulrapat", trnotulensi.idrapat);
            return View(trnotulensi);

        }

        // GET: trnotulensi/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    trnotulensi trnotulensi = db.trnotulensis.Find(id);
        //    if (trnotulensi == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(trnotulensi);
        //}

        //// POST: trnotulensi/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    trnotulensi trnotulensi = db.trnotulensis.Find(id);
        //    db.trnotulensis.Remove(trnotulensi);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        public ActionResult Hapus(int id)
        {
            trnotulensi osp = db.trnotulensis.Find(id);
                osp.status = 0;
                osp.modiby = (string)Session["modiby"];
                osp.modidate = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");

        }
        public FileResult Download(string fileName)
        {
            string fullPath = Path.Combine(Server.MapPath("~/Files"), fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
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
