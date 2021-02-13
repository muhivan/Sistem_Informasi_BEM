using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sistem_Informasi_BEM.Models;

namespace Sistem_Informasi_BEM.Controllers
{
    public class FormatkskController : Controller
    {
        private DBSIBEMEntities db = new DBSIBEMEntities();

        public ActionResult Index(int? id)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            if (id == 0)
            {
                ViewBag.status = 0;
                var format = db.msformatksks.Where(m => m.status == 0);
                return View(format.ToList());
            }
            else
            {
                ViewBag.status = 1;
                var format = db.msformatksks.Where(m => m.status == 1);
                return View(format.ToList());
            }
        }

        public ActionResult Berkas()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            var format = db.msformatksks.Where(m => m.status == 1);
            return View(format.ToList());
        }

        public ActionResult BerkasDept()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            var format = db.msformatksks.Where(m => m.status == 1);
            return View(format.ToList());
        }

        public ActionResult BerkasBPH()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            var format = db.msformatksks.Where(m => m.status == 1);
            return View(format.ToList());
        }

        public ActionResult Details(int? id)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            msformatksk msformatksk = db.msformatksks.Find(id);
            if (msformatksk == null)
            {
                return HttpNotFound();
            }
            return View(msformatksk);
        }

        public ActionResult Create()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(msformatksk msformatksk, string files)
        {
            if (files == null)
            {
                ViewBag.Jabatan = this.Session["Jabatan"];
                ViewBag.Departemen = this.Session["Departemen"];
                ViewBag.Message = "Berkas Belum Dipilih";
                return View(msformatksk);
            }
            try
            {
                var cek = db.msformatksks.FirstOrDefault(x => x.dataBerkas == msformatksk.dataBerkas);
                if (cek == null)
                {
                    if (ModelState.IsValid)
                    {
                        foreach (var file in msformatksk.files)
                        {
                            if (file.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(file.FileName);
                                var filePath = Path.Combine(Server.MapPath("~/Files"), fileName);
                                file.SaveAs(filePath);

                                msformatksk.nama = msformatksk.nama.ToString();
                                msformatksk.dataBerkas = fileName.ToString();
                                msformatksk.creadate = DateTime.Now;
                                msformatksk.status = 1;
                                msformatksk.creaby = msformatksk.creaby.ToString();

                                ViewBag.Jabatan = this.Session["Jabatan"];
                                ViewBag.Departemen = this.Session["Departemen"];
                                db.msformatksks.Add(msformatksk);
                                db.SaveChanges();
                                return RedirectToAction("Index");
                            }
                        }
                    }

                    return View(msformatksk);
                }
                else
                {
                    ViewBag.Jabatan = this.Session["Jabatan"];
                    ViewBag.Departemen = this.Session["Departemen"];
                    ViewBag.Message = "FORMAT KESEKRETARIATAN YANG ANDA MASUKAN SUDAH ADA";
                    return View(msformatksk);
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                        validationErrors.Entry.Entity.ToString(),
                        validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
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
            msformatksk msformatksk = db.msformatksks.Find(id);
            if (msformatksk == null)
            {
                return HttpNotFound();
            }
            return View(msformatksk);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(msformatksk msformatksk)
        {
            var cek = db.msformatksks.FirstOrDefault(x => x.dataBerkas == msformatksk.dataBerkas);
            if (cek == null)
            {
                if (ModelState.IsValid)
                {
                    msformatksk msformat = db.msformatksks.Find(msformatksk.idftksk);
                    foreach (var file in msformatksk.files)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var filePath = Path.Combine(Server.MapPath("~/Files"), fileName);
                            file.SaveAs(filePath);

                            msformat.nama = msformatksk.nama;
                            msformat.dataBerkas = fileName.ToString();
                            msformat.modidate = DateTime.Now;
                            msformat.modiby = msformatksk.modiby;
                            db.SaveChanges();
                        }
                        ViewBag.Jabatan = this.Session["Jabatan"];
                        ViewBag.Departemen = this.Session["Departemen"];
                        msformat.nama = msformatksk.nama;
                        return RedirectToAction("Index");
                    }
                }
                return View(msformatksk);
            }
            else
            {
                ViewBag.Jabatan = this.Session["Jabatan"];
                ViewBag.Departemen = this.Session["Departemen"];
                ViewBag.Message = "BERKAS YANG ANDA MASUKAN SUDAH ADA";
                return View(msformatksk);
            }
        }

        public ActionResult Delete(int id)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            msformatksk msformatksk = db.msformatksks.Find(id);
            msformatksk.status = 0;
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

        public FileResult Download(string fileName)
        {
            string fullPath = Path.Combine(Server.MapPath("~/Files"), fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}
