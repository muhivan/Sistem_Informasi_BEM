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
using System.Web.Security;
using Sistem_Informasi_BEM.Models;

namespace Sistem_Informasi_BEM.Controllers
{
    public class LoginController : Controller
    {
        //object database
        private DBSIBEMEntities db = new DBSIBEMEntities();

        //Menampilkan Data
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(msanggotabem model)
        {
            using (DBSIBEMEntities objContext = new DBSIBEMEntities())
            {
                var objUser = objContext.msanggotabems.FirstOrDefault(x => x.nim == model.nim && x.password == model.password);
                if (objUser == null)
                {
                    ViewBag.Login = "Username dan Password Salah";
                    ModelState.Clear();
                    return View("Index");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(objUser.nama, true);
                    string role = objContext.msanggotabems.Where(m => m.nim == model.nim).FirstOrDefault().msjabatan.namajabatan;
                    string idukmhima = objContext.msanggotabems.Where(m => m.nim == model.nim).FirstOrDefault().msjabatan.idukm_hima.ToString();
                    string idDept = objContext.msanggotabems.Where(m => m.nim == model.nim).FirstOrDefault().msdeparteman.iddepartemen.ToString();
                    string departemen = objContext.msanggotabems.Where(m => m.nim == model.nim).FirstOrDefault().msdeparteman.namadepartemen.ToString();
                    Session["idUKM_Hima"] = idukmhima;
                    Session["idDept"] = idDept;
                    Session["Departemen"] = departemen;
                    Session["Jabatan"] = role;
                    Session["modiby"] = objUser.nama;
                    ViewBag.role = role;
                    this.Session["idanggota"] = objUser.idanggota;
                    if (role.Equals("Admin"))
                    {
                        return RedirectToAction("MenuAdmin");
                    }
                    else if (role.Contains("PIC"))
                    {
                        return RedirectToAction("MenuPIC");
                    }
                    else if (role.Contains("Departemen"))
                    {
                        return RedirectToAction("MenuBPH");
                    }
                    else
                    {
                        return RedirectToAction("MenuBPHUmum");
                    }
                }
            }
        }

        public ActionResult ProfilAnggota()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            var idanggota_ = Session["idanggota"];
            int idAgt = Convert.ToInt32(idanggota_);
            var msanggotas = db.msanggotabems.Include(m => m.msdeparteman).Include(m => m.msjabatan).Include(m => m.msperiode).Where(m => m.idanggota == idAgt);
            return View(msanggotas.ToList());
        }

        public ActionResult ProfilAnggotaPIC()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            var idanggota_ = Session["idanggota"];
            int idAgt = Convert.ToInt32(idanggota_);
            var msanggotas = db.msanggotabems.Include(m => m.msdeparteman).Include(m => m.msjabatan).Include(m => m.msperiode).Where(m => m.idanggota == idAgt);
            return View(msanggotas.ToList());
        }

        public ActionResult ProfilAnggotaDept()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            var idanggota_ = Session["idanggota"];
            int idAgt = Convert.ToInt32(idanggota_);
            var msanggotas = db.msanggotabems.Include(m => m.msdeparteman).Include(m => m.msjabatan).Include(m => m.msperiode).Where(m => m.idanggota == idAgt);
            return View(msanggotas.ToList());
        }

        public ActionResult ProfilAnggotaBPH()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            var idanggota_ = Session["idanggota"];
            int idAgt = Convert.ToInt32(idanggota_);
            var msanggotas = db.msanggotabems.Include(m => m.msdeparteman).Include(m => m.msjabatan).Include(m => m.msperiode).Where(m => m.idanggota == idAgt);
            return View(msanggotas.ToList());
        }

        public ActionResult Home()
        {
            var news = db.msnews.Where(m => m.status == 1);
            return View(news.ToList());
        }

        public ActionResult MenuAdmin()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            return View();
        }
        public ActionResult MenuPIC()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            ViewBag.idanggota = this.Session["idanggota"];
            return View();
        }

        public ActionResult MenuBPH()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            return View();
        }

        public ActionResult MenuBPHUmum()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            return View();
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

        public ActionResult EditPass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EditPass(msanggotabem msanggota)
        {
            using (DBSIBEMEntities objContext = new DBSIBEMEntities())
            {
                var objUser = objContext.msanggotabems.FirstOrDefault(x => x.email == msanggota.email || x.password == msanggota.password);
                if (objUser != null)
                {
                    if (msanggota.creaby.Equals(msanggota.alamat))
                    {
                        msanggotabem anggota = objContext.msanggotabems.Find(objUser.idanggota);
                        anggota.password = msanggota.creaby;
                        objContext.SaveChanges();
                        using (MailMessage mail = new MailMessage())
                        {
                            mail.From = new MailAddress("bempolmanasta@gmail.com");
                            mail.To.Add(msanggota.email);
                            mail.Subject = "Bem Polman Astra";
                            mail.Body = "<h2>Hello, " + objUser.nama +
                                "</h2>Berkaitan dengan website Sistem Informasi Bem, Berikut Terlampir detail informasi akun anda<br>"
                                + "Username : <b>" + objUser.nim + "</b><br>Password   : <b>" + msanggota.creaby +
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
                        ViewBag.Login = "Kata Sandi Berhasil diubah";
                        ModelState.Clear();
                        return View("EditPass");
                    }
                    else
                    {
                        ViewBag.Login = "Password Tidak Sama";
                        ModelState.Clear();
                        return View("EditPass");
                    }
                }
                else
                {
                    ViewBag.Login = "Data Tidak sesuai";
                    ModelState.Clear();
                    return View("EditPass");
                }
            }
        }

        public ActionResult EditAdmin(int? id)
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
            ViewBag.iddepartemen = new SelectList(db.msdepartemen, "iddepartemen", "namadepartemen", msanggotabem.iddepartemen);
            ViewBag.idjabatan = new SelectList(db.msjabatans, "idjabatan", "namajabatan", msanggotabem.idjabatan);
            ViewBag.idperiode = new SelectList(db.msperiodes, "idperiode", "tahunperiode", msanggotabem.idperiode);
            return View(msanggotabem);
        }

        //mengedit data
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAdmin(msanggotabem msanggotabem, string cek_email, string cek_nim, HttpPostedFileBase imgfile)
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
                        return RedirectToAction("ProfilAnggota");
                    }
                }
                else
                {
                    if (cek2 != null)
                    {
                        ViewBag.Message = "NIM YANG ANDA MASUKAN SUDAH ADA";
                        ViewBag.Jabatan = this.Session["Jabatan"];
                        ViewBag.Departemen = this.Session["Departemen"];
                        ViewBag.iddepartemen = new SelectList(db.msdepartemen, "iddepartemen", "namadepartemen", msanggotabem.iddepartemen);
                        ViewBag.idjabatan = new SelectList(db.msjabatans, "idjabatan", "namajabatan", msanggotabem.idjabatan);
                        ViewBag.idperiode = new SelectList(db.msperiodes, "idperiode", "tahunperiode", msanggotabem.idperiode);
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
                            return RedirectToAction("ProfilAnggota");
                        }
                    }
                }
                ViewBag.Jabatan = this.Session["Jabatan"];
                ViewBag.Departemen = this.Session["Departemen"];
                ViewBag.iddepartemen = new SelectList(db.msdepartemen, "iddepartemen", "namadepartemen", msanggotabem.iddepartemen);
                ViewBag.idjabatan = new SelectList(db.msjabatans, "idjabatan", "namajabatan", msanggotabem.idjabatan);
                ViewBag.idperiode = new SelectList(db.msperiodes, "idperiode", "tahunperiode", msanggotabem.idperiode);
                return View(msanggotabem);
            }
            else
            {
                if (cek != null)
                {
                    ViewBag.Message = "EMAIL YANG ANDA MASUKAN SUDAH ADA";
                    ViewBag.Jabatan = this.Session["Jabatan"];
                    ViewBag.Departemen = this.Session["Departemen"];
                    ViewBag.iddepartemen = new SelectList(db.msdepartemen, "iddepartemen", "namadepartemen", msanggotabem.iddepartemen);
                    ViewBag.idjabatan = new SelectList(db.msjabatans, "idjabatan", "namajabatan", msanggotabem.idjabatan);
                    ViewBag.idperiode = new SelectList(db.msperiodes, "idperiode", "tahunperiode", msanggotabem.idperiode);
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
                        return RedirectToAction("ProfilAnggota");
                    }
                    ViewBag.Jabatan = this.Session["Jabatan"];
                    ViewBag.Departemen = this.Session["Departemen"];
                    ViewBag.iddepartemen = new SelectList(db.msdepartemen, "iddepartemen", "namadepartemen", msanggotabem.iddepartemen);
                    ViewBag.idjabatan = new SelectList(db.msjabatans, "idjabatan", "namajabatan", msanggotabem.idjabatan);
                    ViewBag.idperiode = new SelectList(db.msperiodes, "idperiode", "tahunperiode", msanggotabem.idperiode);
                    return View(msanggotabem);
                }
            }
        }

        public ActionResult EditPIC(int? id)
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
            ViewBag.iddepartemen = new SelectList(db.msdepartemen, "iddepartemen", "namadepartemen", msanggotabem.iddepartemen);
            ViewBag.idjabatan = new SelectList(db.msjabatans, "idjabatan", "namajabatan", msanggotabem.idjabatan);
            ViewBag.idperiode = new SelectList(db.msperiodes, "idperiode", "tahunperiode", msanggotabem.idperiode);
            return View(msanggotabem);
        }

        //mengedit data
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPIC(msanggotabem msanggotabem, string cek_email, string cek_nim, HttpPostedFileBase imgfile)
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
                        return RedirectToAction("ProfilAnggotaPIC");
                    }
                }
                else
                {
                    if (cek2 != null)
                    {
                        ViewBag.Message = "NIM YANG ANDA MASUKAN SUDAH ADA";
                        ViewBag.Jabatan = this.Session["Jabatan"];
                        ViewBag.Departemen = this.Session["Departemen"];
                        ViewBag.iddepartemen = new SelectList(db.msdepartemen, "iddepartemen", "namadepartemen", msanggotabem.iddepartemen);
                        ViewBag.idjabatan = new SelectList(db.msjabatans, "idjabatan", "namajabatan", msanggotabem.idjabatan);
                        ViewBag.idperiode = new SelectList(db.msperiodes, "idperiode", "tahunperiode", msanggotabem.idperiode);
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
                            return RedirectToAction("ProfilAnggotaPIC");
                        }
                    }
                }
                ViewBag.Jabatan = this.Session["Jabatan"];
                ViewBag.Departemen = this.Session["Departemen"];
                ViewBag.iddepartemen = new SelectList(db.msdepartemen, "iddepartemen", "namadepartemen", msanggotabem.iddepartemen);
                ViewBag.idjabatan = new SelectList(db.msjabatans, "idjabatan", "namajabatan", msanggotabem.idjabatan);
                ViewBag.idperiode = new SelectList(db.msperiodes, "idperiode", "tahunperiode", msanggotabem.idperiode);
                return View(msanggotabem);
            }
            else
            {
                if (cek != null)
                {
                    ViewBag.Message = "EMAIL YANG ANDA MASUKAN SUDAH ADA";
                    ViewBag.Jabatan = this.Session["Jabatan"];
                    ViewBag.Departemen = this.Session["Departemen"];
                    ViewBag.iddepartemen = new SelectList(db.msdepartemen, "iddepartemen", "namadepartemen", msanggotabem.iddepartemen);
                    ViewBag.idjabatan = new SelectList(db.msjabatans, "idjabatan", "namajabatan", msanggotabem.idjabatan);
                    ViewBag.idperiode = new SelectList(db.msperiodes, "idperiode", "tahunperiode", msanggotabem.idperiode);
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
                        return RedirectToAction("ProfilAnggotaPIC");
                    }
                    ViewBag.Jabatan = this.Session["Jabatan"];
                    ViewBag.Departemen = this.Session["Departemen"];
                    ViewBag.iddepartemen = new SelectList(db.msdepartemen, "iddepartemen", "namadepartemen", msanggotabem.iddepartemen);
                    ViewBag.idjabatan = new SelectList(db.msjabatans, "idjabatan", "namajabatan", msanggotabem.idjabatan);
                    ViewBag.idperiode = new SelectList(db.msperiodes, "idperiode", "tahunperiode", msanggotabem.idperiode);
                    return View(msanggotabem);
                }
            }
        }

        public ActionResult EditBPH(int? id)
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
            ViewBag.iddepartemen = new SelectList(db.msdepartemen, "iddepartemen", "namadepartemen", msanggotabem.iddepartemen);
            ViewBag.idjabatan = new SelectList(db.msjabatans, "idjabatan", "namajabatan", msanggotabem.idjabatan);
            ViewBag.idperiode = new SelectList(db.msperiodes, "idperiode", "tahunperiode", msanggotabem.idperiode);
            return View(msanggotabem);
        }

        //mengedit data
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBPH(msanggotabem msanggotabem, string cek_email, string cek_nim, HttpPostedFileBase imgfile)
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
                        return RedirectToAction("ProfilAnggotaDept");
                    }
                }
                else
                {
                    if (cek2 != null)
                    {
                        ViewBag.Message = "NIM YANG ANDA MASUKAN SUDAH ADA";
                        ViewBag.Jabatan = this.Session["Jabatan"];
                        ViewBag.Departemen = this.Session["Departemen"];
                        ViewBag.iddepartemen = new SelectList(db.msdepartemen, "iddepartemen", "namadepartemen", msanggotabem.iddepartemen);
                        ViewBag.idjabatan = new SelectList(db.msjabatans, "idjabatan", "namajabatan", msanggotabem.idjabatan);
                        ViewBag.idperiode = new SelectList(db.msperiodes, "idperiode", "tahunperiode", msanggotabem.idperiode);
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
                            return RedirectToAction("ProfilAnggotaDept");
                        }
                    }
                }
                ViewBag.Jabatan = this.Session["Jabatan"];
                ViewBag.Departemen = this.Session["Departemen"];
                ViewBag.iddepartemen = new SelectList(db.msdepartemen, "iddepartemen", "namadepartemen", msanggotabem.iddepartemen);
                ViewBag.idjabatan = new SelectList(db.msjabatans, "idjabatan", "namajabatan", msanggotabem.idjabatan);
                ViewBag.idperiode = new SelectList(db.msperiodes, "idperiode", "tahunperiode", msanggotabem.idperiode);
                return View(msanggotabem);
            }
            else
            {
                if (cek != null)
                {
                    ViewBag.Message = "EMAIL YANG ANDA MASUKAN SUDAH ADA";
                    ViewBag.Jabatan = this.Session["Jabatan"];
                    ViewBag.Departemen = this.Session["Departemen"];
                    ViewBag.iddepartemen = new SelectList(db.msdepartemen, "iddepartemen", "namadepartemen", msanggotabem.iddepartemen);
                    ViewBag.idjabatan = new SelectList(db.msjabatans, "idjabatan", "namajabatan", msanggotabem.idjabatan);
                    ViewBag.idperiode = new SelectList(db.msperiodes, "idperiode", "tahunperiode", msanggotabem.idperiode);
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
                        return RedirectToAction("ProfilAnggotaDept");
                    }
                    ViewBag.Jabatan = this.Session["Jabatan"];
                    ViewBag.Departemen = this.Session["Departemen"];
                    ViewBag.iddepartemen = new SelectList(db.msdepartemen, "iddepartemen", "namadepartemen", msanggotabem.iddepartemen);
                    ViewBag.idjabatan = new SelectList(db.msjabatans, "idjabatan", "namajabatan", msanggotabem.idjabatan);
                    ViewBag.idperiode = new SelectList(db.msperiodes, "idperiode", "tahunperiode", msanggotabem.idperiode);
                    return View(msanggotabem);
                }
            }
        }

        public ActionResult EditBPHUmum(int? id)
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
            ViewBag.iddepartemen = new SelectList(db.msdepartemen, "iddepartemen", "namadepartemen", msanggotabem.iddepartemen);
            ViewBag.idjabatan = new SelectList(db.msjabatans, "idjabatan", "namajabatan", msanggotabem.idjabatan);
            ViewBag.idperiode = new SelectList(db.msperiodes, "idperiode", "tahunperiode", msanggotabem.idperiode);
            return View(msanggotabem);
        }

        //mengedit data
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBPHUmum(msanggotabem msanggotabem, string cek_email, string cek_nim, HttpPostedFileBase imgfile)
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
                        return RedirectToAction("ProfilAnggotaBPH");
                    }
                }
                else
                {
                    if (cek2 != null)
                    {
                        ViewBag.Message = "NIM YANG ANDA MASUKAN SUDAH ADA";
                        ViewBag.Jabatan = this.Session["Jabatan"];
                        ViewBag.Departemen = this.Session["Departemen"];
                        ViewBag.iddepartemen = new SelectList(db.msdepartemen, "iddepartemen", "namadepartemen", msanggotabem.iddepartemen);
                        ViewBag.idjabatan = new SelectList(db.msjabatans, "idjabatan", "namajabatan", msanggotabem.idjabatan);
                        ViewBag.idperiode = new SelectList(db.msperiodes, "idperiode", "tahunperiode", msanggotabem.idperiode);
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
                            return RedirectToAction("ProfilAnggotaBPH");
                        }
                    }
                }
                ViewBag.Jabatan = this.Session["Jabatan"];
                ViewBag.Departemen = this.Session["Departemen"];
                ViewBag.iddepartemen = new SelectList(db.msdepartemen, "iddepartemen", "namadepartemen", msanggotabem.iddepartemen);
                ViewBag.idjabatan = new SelectList(db.msjabatans, "idjabatan", "namajabatan", msanggotabem.idjabatan);
                ViewBag.idperiode = new SelectList(db.msperiodes, "idperiode", "tahunperiode", msanggotabem.idperiode);
                return View(msanggotabem);
            }
            else
            {
                if (cek != null)
                {
                    ViewBag.Message = "EMAIL YANG ANDA MASUKAN SUDAH ADA";
                    ViewBag.Jabatan = this.Session["Jabatan"];
                    ViewBag.Departemen = this.Session["Departemen"];
                    ViewBag.iddepartemen = new SelectList(db.msdepartemen, "iddepartemen", "namadepartemen", msanggotabem.iddepartemen);
                    ViewBag.idjabatan = new SelectList(db.msjabatans, "idjabatan", "namajabatan", msanggotabem.idjabatan);
                    ViewBag.idperiode = new SelectList(db.msperiodes, "idperiode", "tahunperiode", msanggotabem.idperiode);
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
                        return RedirectToAction("ProfilAnggotaBPH");
                    }
                    ViewBag.Jabatan = this.Session["Jabatan"];
                    ViewBag.Departemen = this.Session["Departemen"];
                    ViewBag.iddepartemen = new SelectList(db.msdepartemen, "iddepartemen", "namadepartemen", msanggotabem.iddepartemen);
                    ViewBag.idjabatan = new SelectList(db.msjabatans, "idjabatan", "namajabatan", msanggotabem.idjabatan);
                    ViewBag.idperiode = new SelectList(db.msperiodes, "idperiode", "tahunperiode", msanggotabem.idperiode);
                    return View(msanggotabem);
                }
            }
        }
    }
}