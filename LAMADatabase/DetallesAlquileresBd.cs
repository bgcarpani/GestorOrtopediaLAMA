using LAMAModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMADatabase
{
    public class DetallesAlquileresBd
    {
        public static List<DetalleAlquiler> GetDetalles(Alquiler alquiler)
        {
            List<DetalleAlquiler> lista = new List<DetalleAlquiler>();
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                string cadenaComando = "SELECT ID_DetalleAlquiler, ID_Alquiler, ID_Producto, ID_Stock, Cantidad FROM Detalle_Alquileres WHERE ID_Alquiler=@id";
                SqlCommand comando = new SqlCommand(cadenaComando, cn);
                comando.Parameters.AddWithValue("@id", alquiler.AlquilerId);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    DetalleAlquiler dv = new DetalleAlquiler();
                    dv.DetalleAlquilerId = reader.GetInt32(0);
                    dv.Alquiler = AlquileresBd.GetObjeto(reader.GetInt32(1));
                    dv.Producto = ProductosBd.GetObjeto(reader.GetInt32(2));
                    dv.Stock = StocksBd.GetObjeto(reader.GetInt32(3));
                    dv.Cantidad = reader.GetInt32(4);
                    lista.Add(dv);
                }
            }
            return lista;

        }

        internal static void Agregar(DetalleAlquiler detalle, int alquilerId, SqlConnection cn, SqlTransaction transaction)
        {
            try
            {
                string cadenaComando = "INSERT INTO DETALLE_ALQUILERES (ID_ALQUILER, ID_PRODUCTO, ID_STOCK, Cantidad)" +
                                       " VALUES(@alqId, @prodId, @stockId, @cant)";
                SqlCommand comando = new SqlCommand(cadenaComando, cn, transaction);
                comando.Parameters.AddWithValue("@alqId", alquilerId);
                comando.Parameters.AddWithValue("@prodId", detalle.Producto.ProductoId);
                comando.Parameters.AddWithValue("@stockId", detalle.Stock.StockId);
                comando.Parameters.AddWithValue("@cant", detalle.Cantidad);
                comando.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Eliminar(int id, bool porAlquilerId = false)
        {
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                using (var transaction = cn.BeginTransaction())
                {
                    try
                    {
                        string strComando;
                        if (!porAlquilerId)
                        {
                            strComando = "DELETE FROM Detalle_Alquileres WHERE ID_DETALLEALQUILER=@id";
                        }
                        else
                        {
                            strComando = "DELETE FROM Detalle_Alquileres WHERE ID_ALQUILER=@id";
                        }
                        SqlCommand comando = new SqlCommand(strComando, cn, transaction);
                        comando.Parameters.AddWithValue("@id", id);
                        comando.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        if (ex.Message.Contains("REFERENCE"))
                        {
                            throw new Exception("Registro relacionado con otra tabla\nBaja denegada");
                        }
                        throw new Exception(ex.Message);
                    }

                }

            }
        }
    }
}
