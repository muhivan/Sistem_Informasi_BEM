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
    public class PengajuanOSPController : Controller
    {
        private DBSIBEMEntities db = new DBSIBEMEntities();

        public ActionResult Index()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            var idukmhima = (string)Session["idUKM_Hima"];
            int idukm_hima = Convert.ToInt32(idukmhima);
            var cek = db.trlaporanksks.FirstOrDefault(x => x.idukm_hima == idukm_hima);
            var format = db.trlaporanksks.Where(m => m.idukm_hima == idukm_hima);
            if (cek != null)
            {
                return View(format.ToList());
            }
            else
            {
                return View(format.ToList());
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
            trlaporanksk trlaporanksk = db.trlaporanksks.Find(id);
            if (trlaporanksk == null)
            {
                return HttpNotFound();
            }
            return View(trlaporanksk);
        }

        // GET: PengajuanOSP/Create
        public ActionResult Create()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            ViewBag.idukm_hima = new SelectList(db.msukm_hima, "idukm_hima", "nama");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(trlaporanksk trlaporanksk)
        {
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
                        trlaporanksk.keterangan = "OSP";
                        trlaporanksk.status = 1;
                        trlaporanksk.komentar = "-";
                        trlaporanksk.tglpresent = trlaporanksk.tglpresent;
                        trlaporanksk.creadate = DateTime.Now;
                        trlaporanksk.creaby = trlaporanksk.creaby.ToString();

                        db.trlaporanksks.Add(trlaporanksk);
                        db.SaveChanges();
                        ViewBag.Jabatan = this.Session["Jabatan"];
                        ViewBag.Departemen = this.Session["Departemen"];
                        return RedirectToAction("Index");
                    }
                }
            }
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            ViewBag.idukm_hima = new SelectList(db.msukm_hima, "idukm_hima", "nama", trlaporanksk.idukm_hima);
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
            var cek = db.trlaporanksks.FirstOrDefault(x => x.status == 1 || x.status == 3);
            if (cek != null)
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
                ViewBag.Jabatan = this.Session["Jabatan"];
                ViewBag.Departemen = this.Session["Departemen"];
                ViewBag.Message = "TIDAK BISA DIUBAH! BERKAS DALAM PROSES PENGAJUAN!";
                return View(trlaporanksk);
            }
        }

        public ActionResult Delete(int id)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            var cek = db.trlaporanksks.FirstOrDefault(x => x.status != 1);
            if (cek == null)
            {
                trlaporanksk trlaporanksk = db.trlaporanksks.Find(id);
                trlaporanksk.status = 0;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Jabatan = this.Session["Jabatan"];
                ViewBag.Departemen = this.Session["Departemen"];
                ViewBag.Message = "TIDAK BISA DIHAPUS! BERKAS DALAM PROSES PENGAJUAN!";
                return View();
            }
        }

        public ActionResult EditBatal(int id)
        {
            trlaporanksk osp = db.trlaporanksks.Find(id);
            if(osp.status == 7)
            {
                osp.status = 0;
                osp.modiby = (string)Session["modiby"];
                osp.modidate = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Jabatan = this.Session["Jabatan"];
                ViewBag.Departemen = this.Session["Departemen"];
                ViewBag.Hapus = "TIDAK BISA DIHAPUS! BERKAS DALAM PROSES PENGAJUAN!";
                return RedirectToAction("Index");
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
