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
        public ActionResult Create(trka trka, HttpPostedFileBase imgfile)
        {
            string path = uploadimage(imgfile);
            if (ModelState.IsValid)
            {
                ViewBag.idanggota2 = this.Session["idanggota"];
                if (path == "")
                {
                    trka.status = 1;
                }
                else
                {
                    trka.status = 2;
                }
                trka.uplod_bukti = path;
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
        public ActionResult Edit(trka trka, HttpPostedFileBase imgfile)
        {
            string path = uploadimage(imgfile);
            if (ModelState.IsValid)
            {
                ViewBag.idanggota2 = this.Session["idanggota"];
                trka khas = db.trkas.Find(trka.idkas);
                khas.modidate = DateTime.Now;
                khas.nominal = trka.nominal;
                khas.modiby = trka.modiby;
                khas.jeniskas = trka.jeniskas;
                if (path == "")
                {
                    khas.status = 1;
                }
                else
                {
                    khas.status = 2;
                    khas.uplod_bukti = path;
                }
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

        public string uploadimage(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/Bukti_khas"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/Bukti_khas/" + random + Path.GetFileName(file.FileName);
                    }
                    catch
                    {
                        path = "";
                    }
                }
                else
                {
                    Response.Write("<script>alert('Format Harus Sesuai');</script");
                }
            }
            else
            {
                path = "";
            }
            return path;
        }
    }
}
