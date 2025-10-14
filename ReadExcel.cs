using ReporteZencor.Context;
using ReporteZencor.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ReporteZencor
{
    public partial class ReadExcel : Form
    {
        DataTable dtExcel = new DataTable();

        public ReadExcel()
        {
            InitializeComponent();
            GetOperador();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;
            string fileExt = string.Empty;
            OpenFileDialog file = new OpenFileDialog();//open dialog to choose file
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)//if there is a file choosen by the user
            {
                filePath = file.FileName;//get the path of the file
                fileExt = Path.GetExtension(filePath);//get the file extension
                if (fileExt.CompareTo(".xls") == 0 || fileExt.CompareTo(".xlsx") == 0)
                {
                    try
                    {
                       
                        dtExcel = ReadExcelFile(filePath, fileExt);//read excel file

                        for (int i = dtExcel.Rows.Count - 1; i >= 0; i--) 
                        {
                            if (dtExcel.Rows[i][1] == DBNull.Value)
                            {
                                dtExcel.Rows[i].Delete(); // delete empty rows
                            }
                        }

                        dtExcel.AcceptChanges();

                        grdData.Visible = true;
                        grdData.DataSource = dtExcel;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("Please choose .xls or .xlsx file only.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);//custom messageBox to show error
                }
            }
        }

        public DataTable ReadExcelFile(string fileName, string fileExt)
        {
            string conn = string.Empty;
            DataTable dtexcel = new DataTable();
            if (fileExt.CompareTo(".xls") == 0)//compare the extension of the file
                conn = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';";//for below excel 2007
            else
                conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1';";//for above excel 2007
            using (OleDbConnection con = new OleDbConnection(conn))
            {
                try
                {
                    OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [Hoja1$]", con);//here we read data from sheet1
                    oleAdpt.Fill(dtexcel);//fill excel data into dataTable
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            return dtexcel;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int Id_Operador;
                Id_Operador = Convert.ToInt32(cmbOperador.SelectedValue.ToString());
                DbContext.SaveDb(dtExcel, Id_Operador);
                MessageBox.Show("La información se guardo exitosamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
          
        }

        private void GetOperador()
        {
            List<Operador> lOperador;

            lOperador=DbContext.GetOperador();

            if (lOperador.Count > 0)
            {
                cmbOperador.DataSource = lOperador;
                cmbOperador.DisplayMember = "Nombre";
                cmbOperador.ValueMember = "Id";
            }
            
        }
    }
}
