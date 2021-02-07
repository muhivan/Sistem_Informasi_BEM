using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Sistem_Informasi_BEM.Models;

namespace Sistem_Informasi_BEM.Controllers
{
    public class trrapatController : Controller
    {
        private DBSIBEMEntities db = new DBSIBEMEntities();

        // GET: trrapat
        public ActionResult Index()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            var idDept_ = (string)Session["idDept"];
            int idDept = Convert.ToInt32(idDept_);

            var trrapat = db.trrapats.SqlQuery("SELECT t.* FROM trrapat t INNER JOIN  msdepartemen d on t.iddepartemen = d.iddepartemen where t.iddepartemen = " + idDept).ToList();
            return View(trrapat.ToList());
        }

        // GET: trrapat/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    trrapat trrapat = db.trrapats.Find(id);
        //    if (trrapat == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(trrapat);
        //}

        // GET: trrapat/Create
        public ActionResult Create()
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            ViewBag.iddepartemen = new SelectList(db.msdepartemen, "iddepartemen", "namadepartemen");
            return View();
        }

        // POST: trrapat/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(trrapat trrapat)
        {
            var idDept_ = (string)Session["idDept"];
            var namaDept = (string)Session["Departemen"];
            var namaDept1 = namaDept.ToString();
            int idDept = Convert.ToInt32(idDept_);
            var cek = db.trrapats.FirstOrDefault(x => x.tglrapat == trrapat.tglrapat);
            if (cek == null)
            {
                if (ModelState.IsValid)
                {
                    trrapat.iddepartemen = idDept;
                    trrapat.modiby = trrapat.modiby;
                    trrapat.judulrapat = trrapat.judulrapat;
                    trrapat.tglrapat = trrapat.tglrapat;
                    trrapat.creadate = DateTime.Now;
                    trrapat.creaby = trrapat.creaby;
                    trrapat.modidate = DateTime.Now;
                    trrapat.status = 1;
                    db.trrapats.Add(trrapat);
                    db.SaveChanges();
                    getEmail(namaDept1, trrapat.judulrapat, trrapat.tglrapat.ToString());

                }
                ViewBag.iddepartemen = new SelectList(db.msdepartemen, "iddepartemen", "namadepartemen", trrapat.iddepartemen);

            }

            ViewBag.iddepartemen = new SelectList(db.msdepartemen, "iddepartemen", "namadepartemen", trrapat.iddepartemen);
            return RedirectToAction("Index");
        }

        // GET: trrapat/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            trrapat trrapat = db.trrapats.Find(id);
            if (trrapat == null)
            {
                return HttpNotFound();
            }
            ViewBag.iddepartemen = new SelectList(db.msdepartemen, "iddepartemen", "namadepartemen", trrapat.iddepartemen);
            return View(trrapat);
        }

        // POST: trrapat/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(trrapat trrapat)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            var namaDept = (string)Session["Departemen"];
            var namaDept1 = namaDept.ToString();
            var cek = db.trrapats.FirstOrDefault(x => x.status == 1);
            if (cek != null)
            {
                if (ModelState.IsValid)
                {
                    trrapat rapat = db.trrapats.Find(trrapat.idrapat);

                    if (trrapat.tglrapat != null)
                    {
                        rapat.judulrapat = trrapat.judulrapat;

                    }
                    if (trrapat.judulrapat != null)
                    {
                        rapat.tglrapat = trrapat.tglrapat;
                    }
                    rapat.modidate = DateTime.Now;
                    rapat.modiby = trrapat.modiby.ToString();
                    db.SaveChanges();
                    getEmail(namaDept1, trrapat.judulrapat, trrapat.tglrapat.ToString());

                }
            }
            ViewBag.iddepartemen = new SelectList(db.msdepartemen, "iddepartemen", "namadepartemen", trrapat.iddepartemen);
            return RedirectToAction("Index");
        }

        public void getEmail(string namaDept, string judul, string tgl)
        {
            string ConnectionString = "Integrated Security = true; " +
            "Initial Catalog= DBSIBEM; " + " Data source =.; ";
            string SQL = "SELECT * FROM msanggotabem WHERE status=1";

            // create a connection object  
            SqlConnection conn = new SqlConnection(ConnectionString);

            // Create a command object  
            SqlCommand cmd = new SqlCommand(SQL, conn);
            conn.Open();

            string e;
            string nama;
            // Call ExecuteReader to return a DataReader  
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    e = reader["email"].ToString();
                    nama = reader["nama"].ToString();
                    sendEmail(e, namaDept, judul, nama, tgl);
                }
            }

            //Release resources  
            reader.Close();
            conn.Close();
        }

        public void sendEmail(string e, string namaDept, string judul, string nama, string tgl)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("bempolmanasta@gmail.com");
                {
                    mail.To.Add(e);
                    mail.Subject = "Rapat Bem Polman Astra";
                    mail.Body = "<h2>Hello, " + nama +
                        "</h2>Berkaitan dengan rapat BEM Polman Astra yang diselenggarakan oleh " + namaDept + ", Berikut Terlampir detail rapat.<br>"
                        + "Penyelenggara : <b>" + namaDept + "</b><br>Agenda Rapat   : <b>" + judul + "</b><br>Waktu Rapat   : <b>" + tgl +
                        "</b><br>Sekian info yang dapat kami sampaikan atas perhatiannya kami ucapkan terimakasih." +
                        "<br><br>" + namaDept;
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("bempolmanasta@gmail.com", "bempolman");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
            }
        }

        public void getEmailBatal(string namaDept, string judul, string tgl)
        {
            string ConnectionString = "Integrated Security = true; " +
            "Initial Catalog= DBSIBEM; " + " Data source =.; ";
            string SQL = "SELECT * FROM msanggotabem WHERE status=1";

            // create a connection object  
            SqlConnection conn = new SqlConnection(ConnectionString);

            // Create a command object  
            SqlCommand cmd = new SqlCommand(SQL, conn);
            conn.Open();

            string e;
            string nama;
            // Call ExecuteReader to return a DataReader  
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    e = reader["email"].ToString();
                    nama = reader["nama"].ToString();
                    sendEmailBatal(e, namaDept, judul, nama, tgl);
                }
            }

            //Release resources  
            reader.Close();
            conn.Close();
        }

        public void sendEmailBatal(string e, string namaDept, string judul, string nama, string tgl)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("bempolmanasta@gmail.com");
                {
                    mail.To.Add(e);
                    mail.Subject = "Rapat Bem Polman Astra";
                    mail.Body = "<h2>Hello, " + nama +
                        "</h2>Berkaitan dengan rapat BEM Polman Astra yang diselenggarakan oleh " + namaDept + ", dengan detail rapat.<br>"
                        + "Penyelenggara : <b>" + namaDept + "</b><br>Agenda Rapat   : <b>" + judul + "</b><br>Waktu Rapat     : <b>" + tgl +
                        "</b><br>Telah dibatalkan oleh penyelenggara rapat." +
                        "<br>Sekian info yang dapat kami sampaikan atas perhatiannya kami ucapkan terimakasih." +
                        "<br><br>" + namaDept;
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("bempolmanasta@gmail.com", "bempolman");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
            }
        }
        // GET: trrapat/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    trrapat trrapat = db.trrapats.Find(id);
        //    if (trrapat == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(trrapat);
        //}

        // POST: trrapat/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult BatalRapat(int id)
        {
            var namaDept = (string)Session["Departemen"];
            var namaDept1 = namaDept.ToString();
            trrapat trrapat = db.trrapats.Find(id);
            if (trrapat.status == 1)
            {
                trrapat.status = 0;
                trrapat.modiby = (string)Session["modiby"];
                trrapat.modidate = DateTime.Now;
                db.SaveChanges();
                getEmailBatal(namaDept1, trrapat.judulrapat, trrapat.tglrapat.ToString());
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Jabatan = this.Session["Jabatan"];
                ViewBag.Departemen = this.Session["Departemen"];
                ViewBag.Hapus = "TIDAK BISA DIBATALKAN! RAPAT TELAH TERLAKSANA!";
                return RedirectToAction("Index");
            }
        }
        public ActionResult rapatTerlaksana(int id)
        {
            var namaDept = (string)Session["Departemen"];
            var namaDept1 = namaDept.ToString();
            trrapat trrapat = db.trrapats.Find(id);
            
                trrapat.status = 2;
                trrapat.modiby = (string)Session["modiby"];
                trrapat.modidate = DateTime.Now;
                db.SaveChanges();
            getEmailTerlaksana(namaDept1, trrapat.judulrapat, trrapat.tglrapat.ToString());
                return RedirectToAction("Index");
            
        }

        public void getEmailTerlaksana(string namaDept, string judul, string tgl)
        {
            string ConnectionString = "Integrated Security = true; " +
            "Initial Catalog= DBSIBEM; " + " Data source =.; ";
            string SQL = "SELECT * FROM dtlrapat d INNER JOIN msanggotabem a ON d.idanggota=a.idanggota WHERE d.keterangan='HADIR'";

            // create a connection object  
            SqlConnection conn = new SqlConnection(ConnectionString);

            // Create a command object  
            SqlCommand cmd = new SqlCommand(SQL, conn);
            conn.Open();

            string e;
            string nama;
            // Call ExecuteReader to return a DataReader  
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader.HasRows)
                {
                    e = reader["email"].ToString();
                    nama = reader["nama"].ToString();
                    sendEmailTerlaksana(e, namaDept, judul, nama, tgl);
                }
            }

            //Release resources  
            reader.Close();
            conn.Close();
        }

        public void sendEmailTerlaksana(string e, string namaDept, string judul, string nama, string tgl)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("bempolmanasta@gmail.com");
                {
                    mail.To.Add(e);
                    mail.Subject = "Rapat Bem Polman Astra";
                    mail.Body = "<h2>Hello, " + nama +
                        "</h2>Terimakasih telah mengikuti rapat BEM Polman Astra yang diselenggarakan oleh " + namaDept + ", Berikut Terlampir detail rapat.<br>"
                        + "Penyelenggara : <b>" + namaDept + "</b><br>Agenda Rapat   : <b>" + judul + "</b><br>Waktu Rapat   : <b>" + tgl +
                        "</b><br>Sekian info yang dapat kami sampaikan atas perhatiannya kami ucapkan terimakasih." +
                        "<br><br>" + namaDept;
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("bempolmanasta@gmail.com", "bempolman");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
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
    }
}
