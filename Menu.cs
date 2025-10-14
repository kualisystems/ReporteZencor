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
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void seleccionarArchivoToolStripMenuItem_Click(object sender, EventArgs e)
        {
          var winform = new ReadExcel();
            winform.Show();
        }

        private void reporteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var winform = new Reporte();
            winform.Show();
        }

        private void borrarRegistrosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var winform = new Borrar();
            winform.Show();
        }
    }
}
