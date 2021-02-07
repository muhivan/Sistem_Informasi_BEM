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
    public class KSKController : Controller
    {
        private DBSIBEMEntities db = new DBSIBEMEntities();

        // GET: KSK
        public ActionResult Index()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            var idukmhima = (string)Session["idUKM_Hima"];
            int idukm_hima = Convert.ToInt32(idukmhima);
            var trlaporanksks = db.trlaporanksks.SqlQuery("SELECT t.* FROM trlaporanksk t INNER JOIN msukm_hima u on t.idukm_hima=u.idukm_hima where t.idukm_hima = " + idukm_hima + "AND (t.keterangan ='LPJ' OR t.keterangan = 'BAK')").ToList();
            return View(trlaporanksks.ToList());
        }

        public ActionResult IndexBPH()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            var jabatan = (string)Session["Jabatan"];
            string j = jabatan.ToString();
            var idDept_ = (string)Session["idDept"];
            int idDept = Convert.ToInt32(idDept_);
            if (j.Contains("Departemen"))
            {
                var tr = db.trlaporanksks.SqlQuery("SELECT t.* FROM trlaporanksk t INNER JOIN msukm_hima u on t.idukm_hima=u.idukm_hima where u.iddepartemen= " + idDept + "AND (t.keterangan ='LPJ' OR t.keterangan = 'BAK')").ToList();
                return View(tr.ToList());
            } else
            {
                var trlaporanksks = db.trlaporanksks.SqlQuery("SELECT t.* FROM trlaporanksk t  where (t.status > 1 AND t.status <=7) AND (t.keterangan ='LPJ' OR t.keterangan = 'BAK')").ToList();
                return View(trlaporanksks.ToList());
            }
            
        }

        // GET: KSK/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    trlaporanksk trlaporanksk = db.trlaporanksks.Find(id);
        //    if (trlaporanksk == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(trlaporanksk);
        //}

        // GET: KSK/Create
        public ActionResult Create()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            ViewBag.idukm_hima = new SelectList(db.msukm_hima, "idukm_hima", "nama");
            return View();
        }

        // POST: KSK/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(trlaporanksk trlaporanksk)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            var idukmhima = (string)Session["idUKM_Hima"];
            if (ModelState.IsValid)
            {
                foreach (var file in trlaporanksk.files)
                {

                    if (file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(Server.MapPath("~/Files"), fileName);
                        file.SaveAs(filePath);

                        trlaporanksk.idukm_hima = Convert.ToInt32(idukmhima);
                        trlaporanksk.namaberkas = fileName.ToString();
                        trlaporanksk.keterangan = trlaporanksk.keterangan;
                        trlaporanksk.status = 1;
                        trlaporanksk.komentar = "-";
                        trlaporanksk.creadate = DateTime.Now;
                        trlaporanksk.creaby = trlaporanksk.creaby.ToString();

                        db.trlaporanksks.Add(trlaporanksk);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }

            ViewBag.idukm_hima = new SelectList(db.msukm_hima, "idukm_hima", "nama", trlaporanksk.idukm_hima);
            return View(trlaporanksk);
        }

        // GET: KSK/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            trlaporanksk trlaporanksk = db.trlaporanksks.Find(id);
            if (trlaporanksk == null)
            {
                return HttpNotFound();
            }
            ViewBag.idukm_hima = new SelectList(db.msukm_hima, "idukm_hima", "nama", trlaporanksk.idukm_hima);
            return View(trlaporanksk);
        }

        // POST: KSK/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(trlaporanksk trlaporanksk)
        {
            if (ModelState.IsValid)
            {
                trlaporanksk msformat = db.trlaporanksks.Find(trlaporanksk.idlpksk);
                foreach (var file in trlaporanksk.files)
                {

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(Server.MapPath("~/Files"), fileName);
                        file.SaveAs(filePath);

                        msformat.namaberkas = fileName.ToString();

                    }
                    msformat.keterangan = trlaporanksk.keterangan;
                    msformat.modidate = DateTime.Now;
                    msformat.modiby = trlaporanksk.modiby.ToString();

                    db.SaveChanges();
                    ViewBag.Jabatan = this.Session["Jabatan"];
                    ViewBag.Departemen = this.Session["Departemen"];
                    return RedirectToAction("Index");

                }
            }
            ViewBag.idukm_hima = new SelectList(db.msukm_hima, "idukm_hima", "nama", trlaporanksk.idukm_hima);
            return View(trlaporanksk);
        }

        public ActionResult EditBPH(int? id)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            trlaporanksk trlaporanksk = db.trlaporanksks.Find(id);
            if (trlaporanksk == null)
            {
                return HttpNotFound();
            }
            ViewBag.idukm_hima = new SelectList(db.msukm_hima, "idukm_hima", "nama", trlaporanksk.idukm_hima);
            return View(trlaporanksk);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBPH(trlaporanksk trlaporanksk)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            if (ModelState.IsValid)
            {
                trlaporanksk msformat = db.trlaporanksks.Find(trlaporanksk.idlpksk);
                foreach (var file in trlaporanksk.files)
                {

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var filePath = Path.Combine(Server.MapPath("~/Files"), fileName);
                        file.SaveAs(filePath);

                        msformat.namaberkas = fileName.ToString();

                    }

                    if (trlaporanksk.komentar != null)
                    {
                        msformat.komentar = trlaporanksk.komentar;
                    }
                    setStatus(trlaporanksk,msformat);
                    msformat.modidate = DateTime.Now;
                    msformat.modiby = trlaporanksk.modiby.ToString();

                    db.SaveChanges();
                    ViewBag.Jabatan = this.Session["Jabatan"];
                    ViewBag.Departemen = this.Session["Departemen"];
                    return RedirectToAction("IndexBPH");

                }
            }
            ViewBag.idukm_hima = new SelectList(db.msukm_hima, "idukm_hima", "nama", trlaporanksk.idukm_hima);
            return View(trlaporanksk);
        }

        public void setStatus(trlaporanksk trlaporanksk, trlaporanksk msformat)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            var jabatan = (string)Session["Jabatan"];
            string j = jabatan.ToString();
            var idDept_ = (string)Session["idDept"];
            int idDept = Convert.ToInt32(idDept_);
            if (j.Contains("Departemen"))
            {
                statusDept(trlaporanksk,msformat);
            }
            else
            {
                statusBPH(trlaporanksk,msformat);
            }
        }
        public void statusDept(trlaporanksk trlaporanksk, trlaporanksk msformat)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            if (trlaporanksk.komentar == "" || trlaporanksk.komentar == "-" || trlaporanksk.komentar == null)
            {
                trlaporanksk.komentar = "-";
                msformat.status = 2;
            }
            else
            {
                msformat.status = 3;
            }
        }

        public void statusBPH(trlaporanksk trlaporanksk, trlaporanksk msformat)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            if ((trlaporanksk.komentar == "" || trlaporanksk.komentar == "-" || trlaporanksk.komentar == null) && trlaporanksk.status == null)
            {
                trlaporanksk.komentar = "-";
                msformat.status = 5;
            }

            else if (trlaporanksk.status == 5)
            {
                msformat.status = 5;
            }
            else
            {
                msformat.status = 3;
            }
        }

        // GET: KSK/Delete/5
        public ActionResult Hapus(int id)
        {
            trlaporanksk trlaporanksk = db.trlaporanksks.Find(id);
            trlaporanksk.status = 0;
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
