using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LAMAModels;

namespace LAMADatabase
{
    public class DetallesVentasBd
    {
        public static void Agregar(DetalleVenta detallesVenta, SqlConnection cn, SqlTransaction transaction)
        {
                try
                {
                string cadenaComando = "INSERT INTO DETALLE_VENTAS (ID_VENTA, ID_PRODUCTO, Precio_unidad, Cantidad, Devuelto)" +
                                       " VALUES(@venta, @prod, @pre, @cant, @dev)";
                SqlCommand comando = new SqlCommand(cadenaComando, cn, transaction);
                comando.Parameters.AddWithValue("@venta", detallesVenta.Venta.VentaId);
                comando.Parameters.AddWithValue("@prod", detallesVenta.Producto.ProductoId);
                comando.Parameters.AddWithValue("@pre", detallesVenta.PrecioUnidad);
                comando.Parameters.AddWithValue("@cant", detallesVenta.Cantidad);
                comando.Parameters.AddWithValue("@dev", detallesVenta.Devuelto);
                comando.Parameters.AddWithValue("@kar", detallesVenta.Kardex.KardexId);
                comando.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<DetalleVenta> GetDetalles(Venta venta)
        {
            List<DetalleVenta> lista = new List<DetalleVenta>();
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                string cadenaComando = "SELECT ID_DVENTAS, ID_VENTA, ID_PRODUCTO, PRECIO_UNIDAD, CANTIDAD, DEVUELTO FROM DETALLE_VENTAS WHERE ID_VENTA=@id";
                SqlCommand comando = new SqlCommand(cadenaComando, cn);
                comando.Parameters.AddWithValue("@id", venta.VentaId);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    DetalleVenta dv = new DetalleVenta();
                    dv.DetalleVentaId = reader.GetInt32(0);
                    dv.Venta = VentasBd.GetObjeto(reader.GetInt32(1));
                    dv.Producto = ProductosBd.GetObjeto(reader.GetInt32(2));
                    dv.PrecioUnidad = reader.GetDecimal(3);
                    dv.Cantidad = reader.GetInt32(4);
                    dv.Total = dv.PrecioUnidad * dv.Cantidad;
                    dv.Devuelto = reader.GetBoolean(5);
                    lista.Add(dv);
                }
            }
            return lista;

        }

        internal static void Editar(List<DetalleVenta> detalle)
        {

        }

        public static void Eliminar(int id, bool porVentaId = false)
        {

            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                using (var transaction = cn.BeginTransaction())
                {
                    try
                    {
                        string strComando;
                        if (!porVentaId)
                        {
                            strComando = "DELETE FROM DETALLE_VENTAS WHERE ID_DVENTAS=@id";
                        }
                        else
                        {
                            strComando = "DELETE FROM DETALLE_VENTAS WHERE ID_VENTA=@id";
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

        internal static void Devuelto(DetalleDevolucionVenta item)
        {
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                using (var transaction = cn.BeginTransaction())
                {
                    try
                    {
                        string strComando = "update DETALLE_VENTAS set Devuelto = 1 WHERE ID_VENTA = @id AND ID_PRODUCTO = @prodId";
                        
                        SqlCommand comando = new SqlCommand(strComando, cn, transaction);
                        comando.Parameters.AddWithValue("@id", item.DevolucionVenta.Venta.VentaId);
                        comando.Parameters.AddWithValue("@prodId", item.Producto.ProductoId);
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

