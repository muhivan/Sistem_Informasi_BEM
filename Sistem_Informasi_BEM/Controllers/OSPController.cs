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
    public class OSPController : Controller
    {
        private DBSIBEMEntities db = new DBSIBEMEntities();

        // GET: OSP
        public ActionResult Index()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            var idDept_ = (string)Session["idDept"];
            int idDept = Convert.ToInt32(idDept_);

            var trlaporanksk = db.trlaporanksks.SqlQuery("SELECT t.* FROM trlaporanksk t INNER JOIN msukm_hima u on t.idukm_hima=u.idukm_hima where u.iddepartemen = " + idDept+ "AND t.keterangan ='OSP'").ToList();

            return View(trlaporanksk);
        }

        // GET: OSP/Details/5
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
            trlaporanksk.files = null;
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
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            var cek = db.trlaporanksks.FirstOrDefault(x => x.status == 1 || x.status == 3);
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


                            msformat.namaberkas = fileName.ToString();

                        }

                        if (trlaporanksk.komentar == "" || trlaporanksk.komentar == "-" || trlaporanksk.komentar == null)
                        {
                            trlaporanksk.komentar = "-";
                            msformat.status = 2;
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
            }else
            {
                ViewBag.Jabatan = this.Session["Jabatan"];
                ViewBag.Departemen = this.Session["Departemen"];
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
