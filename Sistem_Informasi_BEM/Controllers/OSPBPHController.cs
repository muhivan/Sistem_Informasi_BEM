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
    public class OSPBPHController : Controller
    {
        private DBSIBEMEntities db = new DBSIBEMEntities();

        public ActionResult Index()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            var trlaporanksks = db.trlaporanksks.SqlQuery("SELECT t.* FROM trlaporanksk t  where (t.status > 1 AND t.status <=7) AND t.keterangan ='OSP'").ToList();
            return View(trlaporanksks.ToList());
        }

        public ActionResult Details(int? id)
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
            return View(trlaporanksk);
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(trlaporanksk trlaporanksk)
        {
            var cek = db.trlaporanksks.FirstOrDefault(x => x.status == 2 || x.status == 3 || x.status == 4);
            if (cek != null)
            {
                if (ModelState.IsValid)
                {
                    trlaporanksk msformat = db.trlaporanksks.Find(trlaporanksk.idlpksk);
                    //trlaporanksk.files = msformat.files;

                    foreach (var file in trlaporanksk.files)
                    {
                        if (file != null && file.ContentLength > 0)
                        {

                            var fileName = Path.GetFileName(file.FileName);
                            var filePath = Path.Combine(Server.MapPath("~/Files"), fileName);
                            file.SaveAs(filePath);

                            if (msformat.status == 5 || msformat.status == 6)
                            {
                                ViewBag.Message = "TIDAK BISA DIUBAH! BERKAS TELAH DITOLAK/DITERIMA!";
                                return View(trlaporanksk);
                            }
                            else
                            {
                                msformat.namaberkas = fileName.ToString();
                            }
                        }
                        if ((trlaporanksk.komentar == "" || trlaporanksk.komentar == "-" || trlaporanksk.komentar == null) && trlaporanksk.status == null)
                        {
                            trlaporanksk.komentar = "-";
                            msformat.status = 4;
                        }

                        else if (trlaporanksk.status == 5)
                        {
                            msformat.status = 5;
                        }
                        else if (trlaporanksk.status == 6)
                        {
                            msformat.status = 6;
                        }
                        else
                        {
                            msformat.status = 3;
                        }
                        msformat.komentar = trlaporanksk.komentar;
                        msformat.tglpresent = trlaporanksk.tglpresent;
                        msformat.modidate = DateTime.Now;
                        msformat.modiby = trlaporanksk.modiby.ToString();
                        db.SaveChanges();
                        ViewBag.Jabatan = this.Session["Jabatan"];
                        ViewBag.Departemen = this.Session["Departemen"];
                        return RedirectToAction("Index");
                    }
                }
                ViewBag.Jabatan = this.Session["Jabatan"];
                ViewBag.Departemen = this.Session["Departemen"];
                ViewBag.idukm_hima = new SelectList(db.msukm_hima, "idukm_hima", "nama", trlaporanksk.idukm_hima);
                return View(trlaporanksk);
            }
            else
            {
                ViewBag.Message = "TIDAK BISA DIUBAH! BERKAS DALAM PROSES PENGAJUAN!";
                return View(trlaporanksk);
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

        public FileResult Download(string fileName)
        {
            string fullPath = Path.Combine(Server.MapPath("~/Files"), fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}
