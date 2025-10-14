using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReporteZencor.Entities
{
    public class Cotizacion
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; }
        public string Tipo_servicio { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public double Costo { get; set; }
        public int Servicio_realizado { get; set; }
        public string Comentarios { get; set; }
        public string Numero_servicio { get; set; }
        public string NombreCouriers { get; set; }
        public int NoCouriers { get; set; }
    }

    public class Operadores
    {
        public string Nombre { get; set; }
        public int Cotizados { get; set; }
        public int Confirmados { get; set; }
        public double Porcentaje_Parcial { get; set; }
        public double Porcentaje_Total { get; set; }

    }

    public class Clientes
    {
        public string Cliente { get; set; }
        public int Cotizados { get; set; }
        public int Confirmados { get; set; }
        public double Porcentaje { get; set; }
        public int NoCouriers { get; set; }
    }

    public class Costos
    {
        public string Cliente { get; set; }
        public double Costo { get; set; }
        public int Cotizados { get; set; }
        public int Confirmados { get; set; }
    }

}
