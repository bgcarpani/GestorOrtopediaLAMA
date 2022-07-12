using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LAMAModels;

namespace LAMADatabase
{
    public class KardexBd
    {
        public static Kardex UltimoKardex(Producto producto, SqlConnection cn, SqlTransaction tran)
        {
            Kardex kardex = null;

            try
            {
                string strComando = "SELECT iD_KARDEX, ID_Producto, FechaMovimiento, Movimiento, Entrada, Salida," +
            "Saldo, UltimoCosto, CostoPromedio FROM Kardex WHERE ID_Producto=@prod  AND " +
            "FechaMovimiento=(SELECT Max(FechaMovimiento) FROM Kardex WHERE ID_Producto=@prod)";
                SqlCommand comando = new SqlCommand(strComando, cn, tran);
                comando.Parameters.AddWithValue("@prod", producto.ProductoId);
                SqlDataReader reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    kardex = new Kardex();
                    kardex.KardexId = reader.GetInt32(0);
                    kardex.Producto = ProductosBd.GetObjeto(reader.GetInt32(1));
                    kardex.FechaMovimiento = reader.GetDateTime(2);
                    kardex.Movimiento = reader.GetString(3);
                    kardex.Entrada = reader.GetInt32(4);
                    kardex.Salida = reader.GetInt32(5);
                    kardex.Saldo = reader.GetInt32(6);
                    kardex.UltimoCosto = reader.GetDecimal(7);
                    kardex.CostoPromedio = reader.GetDecimal(8);


                }
                reader.Close();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return kardex;

        }

        public static void Agregar(Kardex kardex, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                string strComando = "INSERT INTO Kardex(ID_Producto, FechaMovimiento, Movimiento, Entrada, Salida," +
            "Saldo, UltimoCosto, CostoPromedio) VALUES(@prod, @fecha,@mov,@entrada,@salida,@saldo,@costo,@prom)";
                SqlCommand comando = new SqlCommand(strComando, cn, tran);
                comando.Parameters.AddWithValue("@prod", kardex.Producto.ProductoId);
                comando.Parameters.AddWithValue("@fecha", kardex.FechaMovimiento);
                comando.Parameters.AddWithValue("@mov", kardex.Movimiento);
                comando.Parameters.AddWithValue("@entrada", kardex.Entrada);
                comando.Parameters.AddWithValue("@salida", kardex.Salida);
                comando.Parameters.AddWithValue("@saldo", kardex.Saldo);
                comando.Parameters.AddWithValue("@costo", kardex.UltimoCosto);
                comando.Parameters.AddWithValue("@prom", kardex.CostoPromedio);
                comando.ExecuteNonQuery();
                strComando = "SELECT @@IDENTITY";
                comando = new SqlCommand(strComando, cn, tran);
                int id = (int)(decimal)comando.ExecuteScalar();
                kardex.KardexId = id;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static List<Kardex> ConsultarKardex(ProductoStock prod)
        {
            List<Kardex> lista = new List<Kardex>();
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string strComando = "SELECT FechaMovimiento, Movimiento, Entrada, Salida," +
            "Saldo, UltimoCosto, CostoPromedio FROM Kardex WHERE ID_Producto=@prod ORDER BY FechaMovimiento desc";
                    SqlCommand comando = new SqlCommand(strComando, cn);
                    comando.Parameters.AddWithValue("@prod", prod.Producto.ProductoId);
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        Kardex kardex = new Kardex();
                        kardex.FechaMovimiento = reader.GetDateTime(0);
                        kardex.Movimiento = reader.GetString(1);
                        kardex.Entrada = reader.GetInt32(2);
                        kardex.Salida = reader.GetInt32(3);
                        kardex.Saldo = reader.GetInt32(4);
                        kardex.UltimoCosto = reader.GetDecimal(5);
                        kardex.CostoPromedio = reader.GetDecimal(6);

                        lista.Add(kardex);
                    }

                }
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        internal static void Borrar(int id, SqlConnection cn, SqlTransaction transaction)
        {
            try
            {
                    string strComando = "DELETE FROM Kardex WHERE ID_KARDEX=@id";
                    SqlCommand comando = new SqlCommand(strComando, cn, transaction);
                    comando.Parameters.AddWithValue("@id", id);
                    comando.ExecuteNonQuery();
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
