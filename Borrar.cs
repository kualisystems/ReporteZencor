using ReporteZencor.Context;
using ReporteZencor.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReporteZencor
{
    public partial class Borrar : Form
    {
        DataTable dt;
        string DateIni;
        string DateEnd;
        int IdOperador;

        public Borrar()
        {
            InitializeComponent();
            GetOperador();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Buscar();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DateIni = dateTimeIni.Value.ToString("yyyy-MM-dd");
                DateEnd = dateTimeFin.Value.ToString("yyyy-MM-dd");

                IdOperador = Convert.ToInt32(cmbOperador.SelectedValue.ToString());
                DbContext.DeleteDb(DateIni, DateEnd, IdOperador);
                MessageBox.Show("Registros Borrados Exitosamente!!");
                Buscar();
            }
            catch (Exception)
            {

                throw;
            }
          
        }

        private void Buscar()
        {
            DateIni = dateTimeIni.Value.ToString("yyyy-MM-dd");
            DateEnd = dateTimeFin.Value.ToString("yyyy-MM-dd");

            IdOperador = Convert.ToInt32(cmbOperador.SelectedValue.ToString());

            dt = DbContext.SearchDbDelete(DateIni, DateEnd, IdOperador);

            if (dt.Rows.Count > 0)
            {
                grdReporte.DataSource = dt;
            }
            else
            {
                grdReporte.DataSource = DBNull.Value;
                MessageBox.Show("No se encontró ningún registro");
            }
        }

        private void GetOperador()
        {
            List<Operador> lOperador;

            lOperador = DbContext.GetOperador();

            if (lOperador.Count > 0)
            {
                cmbOperador.DataSource = lOperador;
                cmbOperador.DisplayMember = "Nombre";
                cmbOperador.ValueMember = "Id";
            }

        }
    }
}
