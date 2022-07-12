using LAMAModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMADatabase
{
    public class TransaccionesBd
    {
        public static List<Transaccion> GetLista()
        {
            var lista = new List<Transaccion>();
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                string cadenaComando = "SELECT * FROM Transaccion order by ID_Transaccion desc";
                SqlCommand comando = new SqlCommand(cadenaComando, cn);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    var tipo = new Transaccion
                    {
                        TransaccionId = reader.GetInt32(0),
                        TipoTransaccion = TipoTransaccionesBd.GetObjeto(reader.GetInt32(1)),
                        Cliente = ClientesBd.GetObjeto(reader.GetInt32(2)),
                        FechaTransaccion = reader.GetDateTime(3),
                        Importe = reader.GetDecimal(4)
                    };
                    lista.Add(tipo);
                }
            }
            return lista;
        }

        internal static void Agregar(int tipoTransaccionId, int cliId, decimal importe, SqlConnection cn, SqlTransaction transaction, string desc)
        {
            //1=ventas
            //2=compras
            //3=alquileres
            //4=ordenes de trabajo
            if (importe != 0)
            {
                try
                {
                    string cadenaComando = "INSERT INTO Transaccion (ID_TipoTransaccion, ID_Cliente, Fecha_Transaccion, Importe, Descripcion)" +
                                           " VALUES(@tip, @cli, @fec, @imp, @desc)";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn, transaction);
                    comando.Parameters.AddWithValue("@tip", tipoTransaccionId);
                    comando.Parameters.AddWithValue("@cli", cliId);
                    comando.Parameters.AddWithValue("@fec", DateTime.Now);
                    comando.Parameters.AddWithValue("@imp", importe);
                    comando.Parameters.AddWithValue("@desc", desc);
                    comando.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
           
        }

        public static void Borrar(Transaccion item)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string strComando = "DELETE FROM Transaccion WHERE ID_Transaccion=@id";
                    SqlCommand comando = new SqlCommand(strComando, cn);
                    comando.Parameters.AddWithValue("@id", item.TransaccionId);
                    comando.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {

                if (ex.Message.Contains("REFERENCE"))
                {
                    throw new Exception("Registro relacionado con otra tabla\nBaja denegada");
                }
                throw new Exception(ex.Message);
            }
        }

        public static void Borrar(string mov, int id)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string desc = $"{mov}-{id}";
                    string strComando = "DELETE FROM Transaccion WHERE Descripcion=@desc";
                    SqlCommand comando = new SqlCommand(strComando, cn);
                    comando.Parameters.AddWithValue("@desc", desc);
                    comando.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {

                if (ex.Message.Contains("REFERENCE"))
                {
                    throw new Exception("Registro relacionado con otra tabla\nBaja denegada");
                }
                throw new Exception(ex.Message);
            }
        }
    }

}
