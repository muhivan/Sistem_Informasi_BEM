using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Sistem_Informasi_BEM.Models;

namespace Sistem_Informasi_BEM.Controllers
{
    public class AnggotaController : Controller
    {

        //object database
        private DBSIBEMEntities db = new DBSIBEMEntities();
        //onject random
        private static Random random = new Random();

        //fungsi random password
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        //menampilkan data anggota
        public ActionResult Index(int? id)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            if (id == 0)
            {
                var msanggotas = db.msanggotabems.Include(m => m.msdeparteman).Include(m => m.msjabatan).Include(m => m.msperiode).Where(m => m.status == 0);
                return View(msanggotas.ToList());
            }
            else
            {
                var msanggotas = db.msanggotabems.Include(m => m.msdeparteman).Include(m => m.msjabatan).Include(m => m.msperiode).Where(m => m.status == 1);
                return View(msanggotas.ToList());
            }
        }

        //menampilkan data details
        public ActionResult Details(int? id)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            msanggotabem msanggotabem = db.msanggotabems.Find(id);
            if (msanggotabem == null)
            {
                return HttpNotFound();
            }
            return View(msanggotabem);
        }

        public ActionResult Create()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            var ddldept = db.msdepartemen.Where(m => m.status == 1);
            var ddljab = db.msjabatans.Where(m => m.status == 1);
            var ddlper = db.msperiodes.Where(m => m.status == 1);
            ViewBag.iddepartemen = new SelectList(ddldept, "iddepartemen", "namadepartemen");
            ViewBag.idjabatan = new SelectList(ddljab, "idjabatan", "namajabatan");
            ViewBag.idperiode = new SelectList(ddlper, "idperiode", "tahunperiode");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(msanggotabem msanggotabem, HttpPostedFileBase imgfile)
        {
            string path = uploadimage(imgfile);
            var cek = db.msanggotabems.FirstOrDefault(x => x.nim == msanggotabem.nim || x.email == msanggotabem.email);
            if (cek == null)
            {
                if (ModelState.IsValid)
                {
                    msanggotabem.password = RandomString(8);
                    msanggotabem.modiby = msanggotabem.creaby;
                    msanggotabem.creadate = DateTime.Now;
                    msanggotabem.foto = path;
                    msanggotabem.status = 1;
                    db.msanggotabems.Add(msanggotabem);
                    db.SaveChanges();
                    //kirim username dan password ke email
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress("bempolmanasta@gmail.com");
                        mail.To.Add(msanggotabem.email);
                        mail.Subject = "Bem Polman Astra";
                        mail.Body = "<h2>Hello, " + msanggotabem.nama +
                            "</h2>Berkaitan dengan website Sistem Informasi Bem, Berikut Terlampir detail informasi akun anda<br>"
                            + "Username : <b>" + msanggotabem.nim + "</b><br>Password   : <b>" + msanggotabem.password +
                            "</b><br>Sekian info yang dapat kami sampaikan atas perhatiannya kami ucapkan terimakasih." +
                            "<br><br>Admin";
                        mail.IsBodyHtml = true;

                        using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                        {
                            smtp.Credentials = new NetworkCredential("bempolmanasta@gmail.com", "bempolman");
                            smtp.EnableSsl = true;
                            smtp.Send(mail);
                        }
                    }
                    //kembali ke index
                    return RedirectToAction("Index");
                }
                ViewBag.Jabatan = this.Session["Jabatan"];
                ViewBag.Departemen = this.Session["Departemen"];
                var ddldept = db.msdepartemen.Where(m => m.status == 1);
                var ddljab = db.msjabatans.Where(m => m.status == 1);
                var ddlper = db.msperiodes.Where(m => m.status == 1);
                ViewBag.iddepartemen = new SelectList(ddldept, "iddepartemen", "namadepartemen");
                ViewBag.idjabatan = new SelectList(ddljab, "idjabatan", "namajabatan");
                ViewBag.idperiode = new SelectList(ddlper, "idperiode", "tahunperiode");
                return View(msanggotabem);
            }
            else
            {
                ViewBag.Jabatan = this.Session["Jabatan"];
                ViewBag.Departemen = this.Session["Departemen"];
                ViewBag.Message = "NIM ATAU EMAIL YANG ANDA MASUKAN SUDAH ADA";
                var ddldept = db.msdepartemen.Where(m => m.status == 1);
                var ddljab = db.msjabatans.Where(m => m.status == 1);
                var ddlper = db.msperiodes.Where(m => m.status == 1);
                ViewBag.iddepartemen = new SelectList(ddldept, "iddepartemen", "namadepartemen");
                ViewBag.idjabatan = new SelectList(ddljab, "idjabatan", "namajabatan");
                ViewBag.idperiode = new SelectList(ddlper, "idperiode", "tahunperiode");
                return View(msanggotabem);
            }
        }

        //menampung data
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            msanggotabem msanggotabem = db.msanggotabems.Find(id);
            if (msanggotabem == null)
            {
                return HttpNotFound();
            }
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            var ddldept = db.msdepartemen.Where(m => m.status == 1);
            var ddljab = db.msjabatans.Where(m => m.status == 1);
            var ddlper = db.msperiodes.Where(m => m.status == 1);
            ViewBag.iddepartemen = new SelectList(ddldept, "iddepartemen", "namadepartemen", msanggotabem.iddepartemen);
            ViewBag.idjabatan = new SelectList(ddljab, "idjabatan", "namajabatan", msanggotabem.idjabatan);
            ViewBag.idperiode = new SelectList(ddlper, "idperiode", "tahunperiode", msanggotabem.idperiode);
            return View(msanggotabem);
        }

        //mengedit data
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(msanggotabem msanggotabem, string cek_email, string cek_nim, HttpPostedFileBase imgfile)
        {
            string path = uploadimage(imgfile);
            var cek = db.msanggotabems.FirstOrDefault(x => x.email == msanggotabem.email);
            if (msanggotabem.email == cek_email)
            {
                var cek2 = db.msanggotabems.FirstOrDefault(x => x.nim == msanggotabem.nim);
                if (msanggotabem.nim == cek_nim)
                {
                    if (ModelState.IsValid)
                    {
                        msanggotabem msanggotabems = db.msanggotabems.Find(msanggotabem.idanggota);
                        msanggotabems.nama = msanggotabem.nama;
                        msanggotabems.nim = msanggotabem.nim;
                        msanggotabems.no_telp = msanggotabem.no_telp;
                        msanggotabems.alamat = msanggotabem.alamat;
                        msanggotabems.email = msanggotabem.email;
                        msanggotabems.iddepartemen = msanggotabem.iddepartemen;
                        msanggotabems.idjabatan = msanggotabem.idjabatan;
                        msanggotabems.idperiode = msanggotabem.idperiode;
                        msanggotabems.modiby = msanggotabem.modiby;
                        msanggotabems.modidate = DateTime.Now;
                        if (path != "")
                        {
                            msanggotabems.foto = path;
                        }
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    if (cek2 != null)
                    {
                        ViewBag.Message = "NIM YANG ANDA MASUKAN SUDAH ADA";
                        ViewBag.Jabatan = this.Session["Jabatan"];
                        ViewBag.Departemen = this.Session["Departemen"];
                        return View(msanggotabem);
                    }
                    else
                    {
                        if (ModelState.IsValid)
                        {
                            msanggotabem msanggotabems = db.msanggotabems.Find(msanggotabem.idanggota);
                            msanggotabems.nama = msanggotabem.nama;
                            msanggotabems.nim = msanggotabem.nim;
                            msanggotabems.no_telp = msanggotabem.no_telp;
                            msanggotabems.alamat = msanggotabem.alamat;
                            msanggotabems.email = msanggotabem.email;
                            msanggotabems.iddepartemen = msanggotabem.iddepartemen;
                            msanggotabems.idjabatan = msanggotabem.idjabatan;
                            msanggotabems.idperiode = msanggotabem.idperiode;
                            msanggotabems.modiby = msanggotabem.modiby;
                            msanggotabems.modidate = DateTime.Now;
                            if (path != "")
                            {
                                msanggotabems.foto = path;
                            }
                            db.SaveChanges();
                            return RedirectToAction("Index");
                        }
                    }   
                }
                ViewBag.Jabatan = this.Session["Jabatan"];
                ViewBag.Departemen = this.Session["Departemen"];
                var ddldept = db.msdepartemen.Where(m => m.status == 1);
                var ddljab = db.msjabatans.Where(m => m.status == 1);
                var ddlper = db.msperiodes.Where(m => m.status == 1);
                ViewBag.iddepartemen = new SelectList(ddldept, "iddepartemen", "namadepartemen", msanggotabem.iddepartemen);
                ViewBag.idjabatan = new SelectList(ddljab, "idjabatan", "namajabatan", msanggotabem.idjabatan);
                ViewBag.idperiode = new SelectList(ddlper, "idperiode", "tahunperiode", msanggotabem.idperiode);
                return View(msanggotabem);
            }
            else
            {
                if (cek != null)
                {
                    ViewBag.Message = "EMAIL YANG ANDA MASUKAN SUDAH ADA";
                    ViewBag.Jabatan = this.Session["Jabatan"];
                    ViewBag.Departemen = this.Session["Departemen"];
                    var ddldept = db.msdepartemen.Where(m => m.status == 1);
                    var ddljab = db.msjabatans.Where(m => m.status == 1);
                    var ddlper = db.msperiodes.Where(m => m.status == 1);
                    ViewBag.iddepartemen = new SelectList(ddldept, "iddepartemen", "namadepartemen", msanggotabem.iddepartemen);
                    ViewBag.idjabatan = new SelectList(ddljab, "idjabatan", "namajabatan", msanggotabem.idjabatan);
                    ViewBag.idperiode = new SelectList(ddlper, "idperiode", "tahunperiode", msanggotabem.idperiode);
                    return View(msanggotabem);
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        msanggotabem msanggotabems = db.msanggotabems.Find(msanggotabem.idanggota);
                        msanggotabems.nama = msanggotabem.nama;
                        msanggotabems.nim = msanggotabem.nim;
                        msanggotabems.no_telp = msanggotabem.no_telp;
                        msanggotabems.alamat = msanggotabem.alamat;
                        msanggotabems.email = msanggotabem.email;
                        msanggotabems.iddepartemen = msanggotabem.iddepartemen;
                        msanggotabems.idjabatan = msanggotabem.idjabatan;
                        msanggotabems.idperiode = msanggotabem.idperiode;
                        msanggotabems.modiby = msanggotabem.modiby;
                        msanggotabems.modidate = DateTime.Now;
                        if (path != "")
                        {
                            msanggotabems.foto = path;
                        }
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    ViewBag.Jabatan = this.Session["Jabatan"];
                    ViewBag.Departemen = this.Session["Departemen"];
                    var ddldept = db.msdepartemen.Where(m => m.status == 1);
                    var ddljab = db.msjabatans.Where(m => m.status == 1);
                    var ddlper = db.msjabatans.Where(m => m.status == 1);
                    ViewBag.iddepartemen = new SelectList(ddldept, "iddepartemen", "namadepartemen", msanggotabem.iddepartemen);
                    ViewBag.idjabatan = new SelectList(ddljab, "idjabatan", "namajabatan", msanggotabem.idjabatan);
                    ViewBag.idperiode = new SelectList(ddlper, "idperiode", "tahunperiode", msanggotabem.idperiode);
                    return View(msanggotabem);
                }
            }    
        }

        //menghapus data dengan status=0
        public ActionResult Delete(int id)
        {
            msanggotabem msanggotabem = db.msanggotabems.Find(id);
            msanggotabem.status = 0;
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
    }
}
