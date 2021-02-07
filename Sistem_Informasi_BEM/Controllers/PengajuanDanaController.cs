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
    public class PengajuanDanaController : Controller
    {
        private DBSIBEMEntities db = new DBSIBEMEntities();

        public ActionResult Index()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            var idanggota_ = Session["idanggota"];
            int idagt = Convert.ToInt32(idanggota_);
            var trdana = db.trpengajuandanas.SqlQuery("SELECT * FROM trpengajuandana where idanggota = " + idagt).ToList();
            return View(trdana.ToList());
        }

        public ActionResult IndexBPH()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            var idanggota_ = Session["idanggota"];
            int idagt = Convert.ToInt32(idanggota_);
            var trdana = db.trpengajuandanas.SqlQuery("SELECT * FROM trpengajuandana where kepada = 'BEM'").ToList();
            return View(trdana.ToList());
        }

        public ActionResult IndexDept()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            var idanggota_ = Session["idanggota"];
            int idagt = Convert.ToInt32(idanggota_);
            var idDept_ = (string)Session["idDept"];
            int idDept = Convert.ToInt32(idDept_);
            var trdana = db.trpengajuandanas.SqlQuery("select t.* from trpengajuandana t INNER JOIN msanggotabem j on t.idanggota = j.idanggota where t.kepada = 'Departemen' and iddepartemen = " + idDept + " or t.idanggota = "+idagt).ToList();
            return View(trdana.ToList());
        }

        public ActionResult Details(int? id)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            trpengajuandana trpengajuandana = db.trpengajuandanas.Find(id);
            if (trpengajuandana == null)
            {
                return HttpNotFound();
            }
            return View(trpengajuandana);
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(trpengajuandana trpengajuandana, string nominal)
        {
            string number = RemoveNonNumeric(nominal);
            int nominalkhas = Int32.Parse(number);

            trpengajuandana.jumlah = nominalkhas;
            trpengajuandana.status = 1;
            trpengajuandana.creadate = DateTime.Now;
            trpengajuandana.Bukti_kirim = "";
            db.trpengajuandanas.Add(trpengajuandana);
            db.SaveChanges();
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            return RedirectToAction("Index");
        }

        public ActionResult CreateDept()
        {
            ViewBag.idanggota2 = this.Session["idanggota"];
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            ViewBag.idanggota = new SelectList(db.msanggotabems, "idanggota", "nama");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDept(trpengajuandana trpengajuandana, string nominal)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            string number = RemoveNonNumeric(nominal);
            int nominalkhas = Int32.Parse(number);

            trpengajuandana.jumlah = nominalkhas;
            trpengajuandana.status = 1;
            trpengajuandana.creadate = DateTime.Now;
            trpengajuandana.Bukti_kirim = "";
            db.trpengajuandanas.Add(trpengajuandana);
            db.SaveChanges();
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            return RedirectToAction("IndexDept");
        }

        public ActionResult Edit(int? id)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            trpengajuandana trpengajuandana = db.trpengajuandanas.Find(id);
            if (trpengajuandana == null)
            {
                return HttpNotFound();
            }
            ViewBag.idanggota = new SelectList(db.msanggotabems, "idanggota", "nama", trpengajuandana.idanggota);
            return View(trpengajuandana);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(trpengajuandana trpengajuandana, string nominal)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            string number = RemoveNonNumeric(nominal);
            int nominalkhas = Int32.Parse(number);

            trpengajuandana dana = db.trpengajuandanas.Find(trpengajuandana.id);
            dana.@event = trpengajuandana.@event;
            dana.tujuan = trpengajuandana.tujuan;
            dana.jumlah = nominalkhas;
            dana.kepada = trpengajuandana.kepada;
            dana.modidate = DateTime.Now;
            dana.modiby = trpengajuandana.modiby;
            db.SaveChanges();
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            return RedirectToAction("Index");
        }

        public ActionResult EditDept(int? id)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            trpengajuandana trpengajuandana = db.trpengajuandanas.Find(id);
            if (trpengajuandana == null)
            {
                return HttpNotFound();
            }
            ViewBag.idanggota = new SelectList(db.msanggotabems, "idanggota", "nama", trpengajuandana.idanggota);
            return View(trpengajuandana);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDept(trpengajuandana trpengajuandana, HttpPostedFileBase imgfile)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            string path = uploadimage(imgfile);
            if (ModelState.IsValid)
            {
                trpengajuandana dana = db.trpengajuandanas.Find(trpengajuandana.id);
                
                dana.modidate = DateTime.Now;
                dana.modiby = (string)Session["modiby"];
                if (path != "")
                {
                    dana.status = 5;
                    dana.Bukti_kirim = path;
                }
                db.SaveChanges();
                ViewBag.Jabatan = this.Session["Jabatan"];
                ViewBag.Departemen = this.Session["Departemen"];
                if (trpengajuandana.kepada == "BEM")
                {
                    return RedirectToAction("IndexBPH");
                }
                else
                {
                    return RedirectToAction("IndexDept");
                }
            }
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            ViewBag.idanggota = new SelectList(db.msanggotabems, "idanggota", "nama", trpengajuandana.idanggota);
            return View(trpengajuandana);
        }

        public ActionResult EditData(int? id)
        {

            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            trpengajuandana trpengajuandana = db.trpengajuandanas.Find(id);
            if (trpengajuandana == null)
            {
                return HttpNotFound();
            }
            ViewBag.idanggota = new SelectList(db.msanggotabems, "idanggota", "nama", trpengajuandana.idanggota);
            return View(trpengajuandana);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditData(trpengajuandana trpengajuandana, string nominal)
        {
            string number = RemoveNonNumeric(nominal);
            int nominalkhas = Int32.Parse(number);

            trpengajuandana dana = db.trpengajuandanas.Find(trpengajuandana.id);
            dana.@event = trpengajuandana.@event;
            dana.tujuan = trpengajuandana.tujuan;
            dana.jumlah = nominalkhas;
            dana.modidate = DateTime.Now;
            dana.modiby = trpengajuandana.modiby;
            db.SaveChanges();
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            return RedirectToAction("IndexDept");
        }

        public ActionResult EditTolakDept(int id)
        {
            trpengajuandana dana = db.trpengajuandanas.Find(id);
            dana.status = 4;
            dana.modiby = (string)Session["modiby"];
            db.SaveChanges();
            if (dana.kepada == "BEM")
            {
                return RedirectToAction("IndexBPH");
            }
            else
            {
                return RedirectToAction("IndexDept");
            }
        }

        public ActionResult EditBatal(int id)
        {
            trpengajuandana dana = db.trpengajuandanas.Find(id);
            dana.status = 6;
            dana.modiby = (string)Session["modiby"];
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult EditBatalDept(int id)
        {
            trpengajuandana dana = db.trpengajuandanas.Find(id);
            dana.status = 6;
            dana.modiby = (string)Session["modiby"];
            db.SaveChanges();
            return RedirectToAction("IndexDept");
        }

        public ActionResult Kirim(int id)
        {
            trpengajuandana dana = db.trpengajuandanas.Find(id);
            if(dana.kepada == "BEM")
            {
                dana.status = 3;
            }
            else
            {
                dana.status = 2;
            } 
            dana.modiby = (string)Session["modiby"];
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult KirimDept(int id)
        {
            trpengajuandana dana = db.trpengajuandanas.Find(id);
            if (dana.kepada == "BEM")
            {
                dana.status = 3;
            }
            else
            {
                dana.status = 2;
            }
            dana.modiby = (string)Session["modiby"];
            db.SaveChanges();
            return RedirectToAction("IndexDept");
        }

        public ActionResult EditTerima(int id)
        {
            trpengajuandana dana = db.trpengajuandanas.Find(id);
            dana.status = 5;
            dana.modiby = (string)Session["modiby"];
            db.SaveChanges();
            if (dana.kepada == "BEM")
            {
                return RedirectToAction("IndexBPH");
            }
            else
            {
                return RedirectToAction("IndexDept");
            } 
        }

        public ActionResult EditTolak(int id)
        {
            trpengajuandana dana = db.trpengajuandanas.Find(id);
            dana.status = 4;
            dana.modiby = (string)Session["modiby"];
            db.SaveChanges();
            if (dana.kepada == "BEM")
            {
                return RedirectToAction("IndexBPH");
            }
            else
            {
                return RedirectToAction("IndexDept");
            }
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
