using ClosedXML.Excel;
using ReporteZencor.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReporteZencor
{
    public partial class Reporte : Form
    {
        DataTable dt;
        public Reporte()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            string DateIni = dateTimeIni.Value.ToString("yyyy-MM-dd");
            string DateEnd = dateTimeFin.Value.ToString("yyyy-MM-dd");

            dt = DbContext.SearchDb(DateIni,DateEnd);

            if(dt.Rows.Count>0)
            {
                grdReporte.DataSource = dt;
            }
        }

        private void btnOp_Click(object sender, EventArgs e)
        {
            string DateIni = dateTimeIni.Value.ToString("yyyy-MM-dd");
            string DateEnd = dateTimeFin.Value.ToString("yyyy-MM-dd");

            dt = DbContext.GetReporteOperadores(DateIni, DateEnd);

            if (dt.Rows.Count > 0)
            {
                grdReporte.DataSource = dt;
            }
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            string DateIni = dateTimeIni.Value.ToString("yyyy-MM-dd");
            string DateEnd = dateTimeFin.Value.ToString("yyyy-MM-dd");

            dt = DbContext.GetReporteClientes(DateIni, DateEnd);

            if (dt.Rows.Count > 0)
            {
                grdReporte.DataSource = dt;
            }
        }

        private void btnCostos_Click(object sender, EventArgs e)
        {
            string DateIni = dateTimeIni.Value.ToString("yyyy-MM-dd");
            string DateEnd = dateTimeFin.Value.ToString("yyyy-MM-dd");

            dt = DbContext.GetReporteCostos(DateIni, DateEnd);

            if (dt.Rows.Count > 0)
            {
                grdReporte.DataSource = dt;
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            string strPath;
            XLWorkbook wb = new XLWorkbook();
            wb.Worksheets.Add(dt, "Hoja1");
            strPath = FilePathExcel("Reporte");
            wb.SaveAs(strPath);
            openExcelFile(strPath);

        }

        private string FilePathExcel(string NombreArchivo)
        {
            string filePath = System.IO.Path.ChangeExtension(System.IO.Path.GetTempFileName(), "xlsx");

            if (Directory.Exists(@"c:\reporte_zencor\"))
            {
                if (File.Exists(@"c:\reporte_zencor\" + "" + NombreArchivo + "" + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx"))
                    File.Delete(@"c:\reporte_zencor\" + "" + NombreArchivo + "" + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx");
               
                filePath = @"c:\reporte_zencor\" + "" + NombreArchivo + "" + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx";
            }
            else
            {
                Directory.CreateDirectory(@"C:\reporte_zencor");

                if (File.Exists(@"c:\reporte_zencor\" + "" + NombreArchivo + "" + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx"))
                    File.Delete(@"c:\reporte_zencor\" + "" + NombreArchivo + "" + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx");

                filePath = @"c:\reporte_zencor\" + "" + NombreArchivo + "" + DateTime.Now.ToShortDateString().Replace("/", "-") + ".xlsx";
            }
            return filePath;
        }

        private void openExcelFile(string strPath)
        {
            Microsoft.Office.Interop.Excel.Application xlApp;

            xlApp = new Microsoft.Office.Interop.Excel.Application();
            xlApp.Visible = true;
            xlApp.Workbooks.Open(strPath);
        }

    }
}
