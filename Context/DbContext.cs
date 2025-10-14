using ReporteZencor.Entities;
using ReporteZencor.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ReporteZencor.Context
{
    public class DbContext
    {
           
        public static void SaveDb(DataTable dt, int Id_Operador)
        {
            try
            {
                using (var ctx = GetInstance())
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var query = "INSERT INTO Cotizacion(Fecha,Cliente,Tipo_servicio,Origen,Destino,Costo,Servicio_realizado,Comentarios,Id_Operador, NoCouriers, NombreCouriers, Numero_servicio) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
                        DataRow dr = dt.Rows[i];
                        // do something with dr
                        using (var command = new SQLiteCommand(query, ctx))
                        {
                            command.Parameters.Add(new SQLiteParameter("Fecha", dr.Field<DateTime>("Fecha")));
                            command.Parameters.Add(new SQLiteParameter("Cliente", dr[1].ToString()));
                            command.Parameters.Add(new SQLiteParameter("Tipo_servicio", dr[2].ToString()));
                            command.Parameters.Add(new SQLiteParameter("Origen", dr[3].ToString()));
                            command.Parameters.Add(new SQLiteParameter("Destino", dr[4].ToString()));
                            if(dr[5]==DBNull.Value)
                            {
                                dr[5] = 0;
                            }
                            command.Parameters.Add(new SQLiteParameter("Costo", Convert.ToDouble(dr[5])));
                            if(dr[6]==DBNull.Value)
                            {
                                dr[6] = 0;
                            }
                            command.Parameters.Add(new SQLiteParameter("Servicio_realizado", Convert.ToInt32(dr[6])));
                            command.Parameters.Add(new SQLiteParameter("Comentarios", dr[10].ToString()));
                            command.Parameters.Add(new SQLiteParameter("Id_Operador", Id_Operador));
                            if (dr[7] == DBNull.Value)
                            {
                                dr[7] = 0;
                            }
                            command.Parameters.Add(new SQLiteParameter("NoCouriers", dr[7].ToString()));
                            //se agregan nuevos campos 22/12/2023
                            command.Parameters.Add(new SQLiteParameter("NombreCouriers", dr[8].ToString()));
                            command.Parameters.Add(new SQLiteParameter("Numero_servicio", dr[9].ToString()));
                            command.ExecuteNonQuery();
                        }
                    } 
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }

        public static void DeleteDb(string StartDate, string EndDate, int IdOperador)
        {
            try
            {
                using (var ctx = DbContext.GetInstance())
                {
                    var query = "DELETE FROM Cotizacion where DATE(Fecha) BETWEEN '" + StartDate + "' AND '" + EndDate + "' and Id_Operador=" + IdOperador;
                    using (var command = new SQLiteCommand(query, ctx))
                    {
                        command.ExecuteNonQuery();
                    }
                        
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }

        public static DataTable SearchDb(string StartDate, string EndDate)
        {
            var result = new List<Cotizacion>();
            var dt = new DataTable();

            using (var ctx = DbContext.GetInstance())
            {
                var query = "SELECT * FROM Cotizacion  WHERE DATE(Fecha) BETWEEN '" + StartDate + "' AND '" + EndDate + "' "; //26/12/2023 se agrega filtro de busqueda

                using (var command = new SQLiteCommand(query, ctx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Cotizacion
                            {
                                Id = Convert.ToInt32(reader["id"].ToString()),
                                Fecha = Convert.ToDateTime(reader["Fecha"] == DBNull.Value ? "" : reader["Fecha"]),
                                Cliente = reader["Cliente"].ToString(),
                                Tipo_servicio = reader["Tipo_servicio"].ToString(),
                                Origen = reader["Origen"].ToString(),
                                Destino = reader["Destino"].ToString(),
                                Costo =Convert.ToDouble(reader["Costo"] == DBNull.Value ? 0 : reader["Costo"]),
                                Servicio_realizado = Convert.ToInt32(reader["Servicio_realizado"] == DBNull.Value ? 0: reader["Servicio_realizado"]),
                                Numero_servicio = reader["Numero_servicio"].ToString(),
                                NoCouriers = Convert.ToInt32(reader["NoCouriers"] == DBNull.Value ? 0 : reader["NoCouriers"]),
                                NombreCouriers = reader["NombreCouriers"].ToString(),
                                Comentarios = reader["Comentarios"].ToString()
                            });
                        }
                    }
                }
            }

            dt= ConvertDataTable.ToDataTable(result);
            return dt;
        }

        public static DataTable SearchDbDelete(string StartDate, string EndDate, int IdOperador)
        {
            var result = new List<Cotizacion>();
            var dt = new DataTable();

            using (var ctx = DbContext.GetInstance())
            {
                var query = "SELECT Fecha, Cliente, Tipo_servicio, Origen, Destino, Costo, Comentarios FROM Cotizacion" +
                            " where DATE(Fecha) BETWEEN  '" + StartDate + "' AND '" + EndDate + "' and Id_Operador=" + IdOperador;

                using (var command = new SQLiteCommand(query, ctx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Cotizacion
                            {
                                
                                Fecha = Convert.ToDateTime(reader["Fecha"] == DBNull.Value ? "" : reader["Fecha"]),
                                Cliente = reader["Cliente"].ToString(),
                                Tipo_servicio = reader["Tipo_servicio"].ToString(),
                                Origen = reader["Origen"].ToString(),
                                Destino = reader["Destino"].ToString(),
                                Costo = Convert.ToDouble(reader["Costo"] == DBNull.Value ? 0 : reader["Costo"]),
                                Comentarios = reader["Comentarios"].ToString()
                            });
                        }
                    }
                }
            }

            dt = ConvertDataTable.ToDataTable(result);
            return dt;
        }

        public static DataTable GetReporteOperadores(string StartDate, string EndDate)
        {
            var result = new List<Operadores>();
            var dt = new DataTable();

            using (var ctx = DbContext.GetInstance())
            {
                var query = "SELECT  b.Nombre, count(*) as cotizados, sum(Servicio_realizado) as confirmados, " +
                            " round(CAST(sum(Servicio_realizado) as REAL)/ count(*)*100,2) as porcentaje_parcial, " +
                            " round(CAST(sum(Servicio_realizado) as REAL)/(select count(*) from Cotizacion WHERE DATE(Fecha) BETWEEN '" + StartDate + "' AND '" + EndDate + "' " + ")*100,2) as porcentaje_total" +
                            " FROM Cotizacion  a INNER JOIN Operador b on a.Id_Operador = b.Id" +
                            " WHERE DATE(Fecha) BETWEEN '" + StartDate + "' AND '" + EndDate +"' " +
                            " GROUP by Id_Operador " + 
                            " UNION ALL " +
                            " SELECT 'Total' Total, " +
                            " count(*) as cotizados, sum(Servicio_realizado) as confirmados, " +
                            " round(CAST(sum(Servicio_realizado) as REAL)/ count(*)*100,2) as porcentaje_parcial, " +
                            " round(CAST(sum(Servicio_realizado) as REAL)/(select count(*) from Cotizacion WHERE DATE(Fecha) BETWEEN '" + StartDate + "' AND '" + EndDate + "' " + ")*100,2) as porcentaje_total " +
                            " FROM Cotizacion  a INNER JOIN Operador b on a.Id_Operador = b.Id " +
                            " WHERE DATE(Fecha) BETWEEN '" + StartDate + "' AND '" + EndDate + "' ";

                using (var command = new SQLiteCommand(query, ctx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Operadores
                            {
                                Cotizados = Convert.ToInt32(reader["cotizados"].ToString()),
                                Confirmados = Convert.ToInt32(reader["confirmados"].ToString()),
                                Porcentaje_Parcial = Convert.ToDouble(reader["porcentaje_parcial"].ToString()),
                                Porcentaje_Total = Convert.ToDouble(reader["porcentaje_total"].ToString()),
                                Nombre = reader["nombre"].ToString()
                            });
                        }
                    }
                }
            }

            dt = ConvertDataTable.ToDataTable(result);
            return dt;
        }

        public static DataTable GetReporteClientes(string StartDate, string EndDate)
        {
            var result = new List<Clientes>();
            var dt = new DataTable();

            using (var ctx = DbContext.GetInstance())
            {
                var query = " SELECT Cliente, count(*) as cotizados, sum(Servicio_realizado) as confirmados, " +
                            " round((CAST(sum(Servicio_realizado) as REAL)/count(*))* 100,2) as porcentaje, sum(NoCouriers) as noCouriers " +
                            " FROM Cotizacion " +
                            " WHERE DATE(Fecha) BETWEEN '" + StartDate + "' AND '" + EndDate + "' " +
                            " GROUP by Cliente " +
                            " UNION ALL " +
                            " SELECT 'Total' Total, count(*) as cotizados, sum(Servicio_realizado) as confirmados, " +
                            " round((CAST(sum(Servicio_realizado) as REAL)/count(*))* 100,2) as porcentaje, sum(NoCouriers) as noCouriers " +
                            " FROM Cotizacion " +
                            " WHERE DATE(Fecha) BETWEEN '" + StartDate + "' AND '" + EndDate + "' "
                            ;

                using (var command = new SQLiteCommand(query, ctx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Clientes
                            {
                                Cliente = reader["cliente"].ToString(),
                                Cotizados = Convert.ToInt32(reader["cotizados"].ToString()),
                                Confirmados = Convert.ToInt32(reader["confirmados"].ToString()),
                                Porcentaje = Convert.ToDouble(reader["porcentaje"]),
                                NoCouriers = Convert.ToInt32(reader["noCouriers"])
                            });
                        }
                    }
                }
            }

            dt = ConvertDataTable.ToDataTable(result);
            return dt;
        }

        public static DataTable GetReporteCostos(string StartDate, string EndDate)
        {
            var result = new List<Costos>();
            var dt = new DataTable();

            using (var ctx = DbContext.GetInstance())
            {
                var query = " SELECT Cliente, costo, count(*) as cotizados, sum(Servicio_realizado) as confirmados " +
                            " FROM Cotizacion " +
                            " WHERE DATE(Fecha) BETWEEN '" + StartDate + "' AND '" + EndDate + "' AND Servicio_realizado=1" +
                            " GROUP by Cliente, Costo " +
                            " UNION ALL " +
                            " SELECT 'Total' Total, sum(costo) as Costo, count(*) as cotizados, sum(Servicio_realizado) as confirmados " +
                            " FROM Cotizacion " +
                            " WHERE DATE(Fecha) BETWEEN '" + StartDate + "' AND '" + EndDate + "' AND Servicio_realizado=1"
                            ;

                using (var command = new SQLiteCommand(query, ctx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Costos
                            {
                                Cliente = reader["cliente"].ToString(),
                                Costo = Convert.ToDouble(reader["costo"].ToString()),
                                Cotizados = Convert.ToInt32(reader["cotizados"].ToString()),
                                Confirmados = Convert.ToInt32(reader["confirmados"].ToString())

                            });
                        }
                    }
                }
            }

            dt = ConvertDataTable.ToDataTable(result);
            return dt;
        }

        public static List<Operador>GetOperador()
        {
            var result = new List<Operador>();

            using (var ctx = DbContext.GetInstance())
            {
                var query = "SELECT Id, Nombre FROM Operador";
                using (var command = new SQLiteCommand(query, ctx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Operador
                            {
                                Id = Convert.ToInt32(reader["id"].ToString()),
                                Nombre = reader["Nombre"].ToString()
                            });
                        }
                    }
                }
            }
            
            return result;
        }

        public static SQLiteConnection GetInstance()
        {
            var db = new SQLiteConnection(
                string.Format("Data Source={0};Version=3;", ConfigurationManager.AppSettings["activeConnection"])
            );

            db.Open();

            return db;
        }

       
    }
}
