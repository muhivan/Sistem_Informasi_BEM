using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using Sistem_Informasi_BEM.Models;

namespace Sistem_Informasi_BEM.Controllers
{
    public class LaporanKasController : Controller
    {
        private DBSIBEMEntities db = new DBSIBEMEntities();
        private DBSIBEMDataSet db1 = new DBSIBEMDataSet();

        // GET: LaporanKas
        public ActionResult Index(string option, DateTime? date = null)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            string y = Convert.ToDateTime(date).Year.ToString();
            string m = Convert.ToDateTime(date).Month.ToString();
            var kas = db.LaporanKas.SqlQuery("SELECT * FROM LaporanKas WHERE jeniskas='" + option + "' AND YEAR(date)='" + y + "' AND MONTH(date)='" + m + "'").ToList();
            if (option != "" || option != null)
            {
                TempData["Tahun"] = y.ToString();
                TempData["Bulan"] = m.ToString();
                TempData["Option"] = option;
            }
            return View(kas.ToList());
        }

        public ActionResult Reports(string ReportType)
        {
            ViewBag.Jabatan = this.Session["Jabatan"];
            ViewBag.Departemen = this.Session["Departemen"];
            string yy;
            string mm;
            string op;
            yy = TempData["Tahun"].ToString();
            mm = TempData["Bulan"].ToString();
            op = TempData["Option"].ToString();
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/LaporanKas.rdlc");
            ReportDataSource rd = new ReportDataSource();
            rd.Name = "LaporanKasDataSet";
            rd.Value = db.LaporanKas.SqlQuery("SELECT * FROM LaporanKas WHERE jeniskas='" + op + "' AND YEAR(date)='" + yy + "' AND MONTH(date)='" + mm + "'").ToList();
            localReport.DataSources.Add(rd);
            string rt = ReportType;
            string mimeType, encoding, extension;
            if (rt == "Excel")
            {
                extension = "xlsx";

            } else if (rt == "Word")
            {
                extension = "docx";

            }
            else if (rt == "PDF")
            {
                extension = "pdf";

            }
            else
            {
                extension = "jpg";

            }
            TempData["Tahun"] =yy;
            TempData["Bulan"] =mm;
            TempData["Option"]=op;
            string[] streams;
            Warning[] warnings;
            byte[] renderByte;
            renderByte = localReport.Render(rt,"",out mimeType,out encoding, out extension,out streams,out warnings);
            Response.AddHeader("content-disposition", "attachment;filename=Laporan_Kas." + extension);
            return File(renderByte, extension);

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
