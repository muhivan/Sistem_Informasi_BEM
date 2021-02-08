using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
            if (trka.jeniskas == null || trka.jeniskas == "")
            {
                ViewBag.Message = "Data Tidak Boleh Kosong";
                ViewBag.idanggota = new SelectList(db.msanggotabems, "idanggota", "nama", trka.idanggota);
                ViewBag.idanggota2 = this.Session["idanggota"];
                return View(trka);
            }

            string number = RemoveNonNumeric(nominal);
            int nominalkhas = Int32.Parse(number);

            string path = uploadimage(imgfile);
           
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
            trka.nominal = nominalkhas;
            db.trkas.Add(trka);
            db.SaveChanges();
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            ViewBag.idanggota2 = this.Session["idanggota"];
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
        public ActionResult Edit(trka trka, HttpPostedFileBase imgfile, string nominal)
        {
            string number = RemoveNonNumeric(nominal);
            int nominalkhas = Int32.Parse(number);
            string path = uploadimage(imgfile);

            ViewBag.idanggota2 = this.Session["idanggota"];
            trka khas = db.trkas.Find(trka.idkas);
            khas.modidate = DateTime.Now;
            khas.nominal = nominalkhas;
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
