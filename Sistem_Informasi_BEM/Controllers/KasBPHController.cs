using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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

            var trkas = db.trkas.Include(t => t.msanggotabem).Where(t => t.jeniskas == "BEM" || t.jeniskas == "BPH");
            return View(trkas.ToList());
        }

        public static string RemoveNonNumeric(string s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
                if (Char.IsNumber(s[i]))
                    sb.Append(s[i]);
            return sb.ToString();
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
        public ActionResult Create(trka trka, HttpPostedFileBase imgfile, string nominal)
        {
            string number = RemoveNonNumeric(nominal);
            int nominalkhas = Int32.Parse(number);

            string path = uploadimage(imgfile);
            ViewBag.idanggota2 = this.Session["idanggota"];
            ViewBag.iddept = (string)Session["idDept"];

            if (path == "")
            {
                trka.status = 1;
            }
            else
            {
                trka.status = 3;
            }
            trka.nominal = nominalkhas;
            trka.uplod_bukti = path;
            trka.creadate = DateTime.Now;
            db.trkas.Add(trka);
            db.SaveChanges();
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            return RedirectToAction("Index");
        }

        public ActionResult CreateBPH()
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
        public ActionResult CreateBPH(trka trka, HttpPostedFileBase imgfile, string nominal)
        {
            string number = RemoveNonNumeric(nominal);
            int nominalkhas = Int32.Parse(number);

            string path = uploadimage(imgfile);
            ViewBag.idanggota2 = this.Session["idanggota"];
            ViewBag.iddept = (string)Session["idDept"];

            if (path == "")
            {
                trka.status = 1;
            }
            else
            {
                trka.status = 3;
            }
            trka.nominal = nominalkhas;
            trka.uplod_bukti = path;
            trka.creadate = DateTime.Now;
            db.trkas.Add(trka);
            db.SaveChanges();
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            return RedirectToAction("IndexBPH");
        }

        public ActionResult EditDept(int? id)
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
        public ActionResult EditDept(trka trka, HttpPostedFileBase imgfile, string nominal)
        {
            string number = RemoveNonNumeric(nominal);
            int nominalkhas = Int32.Parse(number);
            string path = uploadimage(imgfile);

            ViewBag.idanggota2 = this.Session["idanggota"];
            trka khas = db.trkas.Find(trka.idkas);
            khas.modidate = DateTime.Now;
            khas.nominal = nominalkhas;
            khas.modiby = (string)Session["modiby"];
            khas.jeniskas = trka.jeniskas;
            if (path == "")
            {
                khas.status = 1;
            }
            else
            {
                if(trka.jeniskas == "BEM")
                {
                    khas.status = 3;
                    khas.uplod_bukti = path;
                }
                else
                {
                    khas.status = 2;
                    khas.uplod_bukti = path;
                } 
            }
            db.SaveChanges();
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            return RedirectToAction("Index");
        }

        public ActionResult EditBPH(int? id)
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
        public ActionResult EditBPH(trka trka, HttpPostedFileBase imgfile, string nominal)
        {
            string number = RemoveNonNumeric(nominal);
            int nominalkhas = Int32.Parse(number);
            string path = uploadimage(imgfile);

            ViewBag.idanggota2 = this.Session["idanggota"];
            trka khas = db.trkas.Find(trka.idkas);
            khas.modidate = DateTime.Now;
            khas.nominal = nominalkhas;
            khas.modiby = (string)Session["modiby"];
            khas.jeniskas = trka.jeniskas;
            if (path == "")
            {
                khas.status = 1;
            }
            else
            {
                khas.status = 3;
                khas.uplod_bukti = path;
            }
            db.SaveChanges();
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            return RedirectToAction("IndexBPH");
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
        public ActionResult Edit(trka trka) { 
            ViewBag.idanggota2 = this.Session["idanggota"];
            trka khas = db.trkas.Find(trka.idkas);
            khas.modidate = DateTime.Now;
            khas.modiby = (string)Session["modiby"];
            khas.status = 4;
            db.SaveChanges();
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            return RedirectToAction("Index");
        }

        public ActionResult EditData(int? id)
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
        public ActionResult EditData(trka trka)
        {
            ViewBag.idanggota2 = this.Session["idanggota"];
            trka khas = db.trkas.Find(trka.idkas);
            khas.modidate = DateTime.Now;
            khas.modiby = (string)Session["modiby"];
            khas.status = 4;
            db.SaveChanges();
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            return RedirectToAction("IndexBPH");
        }

        public ActionResult TidakValidDept(int id)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            trka khas = db.trkas.Find(id);
            khas.status = 5;
            khas.modidate = DateTime.Now;
            khas.modiby = (string)Session["modiby"];
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult TidakValidBPH(int id)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            trka khas = db.trkas.Find(id);
            khas.status = 5;
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
