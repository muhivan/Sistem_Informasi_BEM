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
    public class NewsController : Controller
    {
        private DBSIBEMEntities db = new DBSIBEMEntities();

        public ActionResult Index(int? id)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            if (id == 0)
            {
                ViewBag.status = 0;
                var news = db.msnews.Where(m => m.status == 0);
                return View(news.ToList());
            }
            else
            {
                ViewBag.status = 1;
                var news = db.msnews.Where(m => m.status == 1);
                return View(news.ToList());
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
            msnew msnew = db.msnews.Find(id);
            if (msnew == null)
            {
                return HttpNotFound();
            }
            return View(msnew);
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
                        path = Path.Combine(Server.MapPath("~/SaveImage"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/SaveImage/" + random + Path.GetFileName(file.FileName);
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

        public ActionResult Create()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(msnew msnew, HttpPostedFileBase imgfile)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            string path = uploadimage(imgfile);

            if (ModelState.IsValid)
            {
                msnew.status = 1;
                msnew.gambar = path;
                msnew.creadate = DateTime.Now;
                db.msnews.Add(msnew);
                db.SaveChanges();
                ViewBag.Jabatan = this.Session["Jabatan"];
                ViewBag.Departemen = this.Session["Departemen"];
                return RedirectToAction("Index");
            }
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            return View(msnew);
        }

        public ActionResult Edit(int? id)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            msnew msnew = db.msnews.Find(id);
            if (msnew == null)
            {
                return HttpNotFound();
            }
            return View(msnew);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(msnew msnew, HttpPostedFileBase imgfile)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            string path = uploadimage(imgfile);

            if (ModelState.IsValid)
            {
                msnew news = db.msnews.Find(msnew.idnews);
                news.judul = msnew.judul;
                news.preview = msnew.preview;
                news.deskripsi = msnew.deskripsi;
                news.modiby = msnew.modiby;
                news.modidate = DateTime.Now;
                if (path != "")
                {
                    news.gambar = path;
                }
                db.SaveChanges();
                ViewBag.Jabatan = this.Session["Jabatan"];
                ViewBag.Departemen = this.Session["Departemen"];
                return RedirectToAction("Index");
            }
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            return View(msnew);
        }

        public ActionResult Delete(int id)
        {
            msnew msnew = db.msnews.Find(id);
            msnew.status = 0;
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
