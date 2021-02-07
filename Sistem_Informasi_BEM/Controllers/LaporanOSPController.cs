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
    public class LaporanOSPController : Controller
    {
        private DBSIBEMEntities db = new DBSIBEMEntities();
        private DBSIBEMDataSet db1 = new DBSIBEMDataSet();

        // GET: LaporanOSP
        public ActionResult Index(string option, DateTime? date = null)
        {
            string y = Convert.ToDateTime(date).Year.ToString();
            string m = Convert.ToDateTime(date).Month.ToString();
            var kas = db.trlaporanksks.SqlQuery("SELECT * FROM trlaporanksk WHERE keterangan='OSP' AND status='" + option + "' AND YEAR(creadate)='" + y + "' AND MONTH(creadate)='" + m + "'").ToList();
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
            string yy;
            string mm;
            string op;
            yy = TempData["Tahun"].ToString();
            mm = TempData["Bulan"].ToString();
            op = TempData["Option"].ToString();
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/LaporanOSP.rdlc");
            ReportDataSource rd = new ReportDataSource();
            rd.Name = "LaporanOSPds";
            rd.Value = db.trlaporanksks.SqlQuery("SELECT * FROM trlaporanksk WHERE keterangan='OSP' AND status='" + op + "' AND YEAR(creadate)='" + yy + "' AND MONTH(creadate)='" + mm + "'").ToList();
            localReport.DataSources.Add(rd);
            string rt = ReportType;
            string mimeType, encoding, extension;
            if (rt == "Excel")
            {
                extension = "xlsx";

            }
            else if (rt == "Word")
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
            TempData["Tahun"] = yy;
            TempData["Bulan"] = mm;
            TempData["Option"] = op;
            string[] streams;
            Warning[] warnings;
            byte[] renderByte;
            renderByte = localReport.Render(rt, "", out mimeType, out encoding, out extension, out streams, out warnings);
            Response.AddHeader("content-disposition", "attachment;filename=Laporan_OSP_."+ DateTime.Now+"."+ extension);
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
