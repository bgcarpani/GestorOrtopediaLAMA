using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LAMAModels;

namespace LAMADatabase
{
    public class DetallesComprasBd
    {
        public static void Agregar(DetalleCompra detalle, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                string cadenaComando =
                    "INSERT INTO DETALLE_COMPRAS (ID_COMPRA, ID_PRODUCTO, PRECIO_UNIDAD, CANTIDAD, ID_KARDEX) VALUES (@comp, @prod, @precio, @cant, @kar)";
                SqlCommand comando = new SqlCommand(cadenaComando, cn, tran);
                comando.Parameters.AddWithValue("@comp", detalle.Compra.CompraId);
                comando.Parameters.AddWithValue("@prod",detalle.Producto.ProductoId);
                comando.Parameters.AddWithValue("@cant", detalle.Cantidad);
                comando.Parameters.AddWithValue("@precio", detalle.PrecioUnidad);
                comando.Parameters.AddWithValue("@kar", detalle.Kardex.KardexId);

                comando.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static List<DetalleCompra> GetDetalles(Compra compra)
        {
            List<DetalleCompra> lista=new List<DetalleCompra>();
            using (var cn=ConexionBd.GetConexion())
            {
                cn.Open();
                string cadenaComando =
                    "SELECT ID_PRODUCTO, Cantidad, PRECIO_UNIDAD FROM DETALLE_COMPRAS " +
                    "WHERE ID_COMPRA=@id";
                SqlCommand comando=new SqlCommand(cadenaComando,cn);
                comando.Parameters.AddWithValue("@id", compra.CompraId);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    DetalleCompra detalleCompra = new DetalleCompra
                    {
                       Producto = ProductosBd.GetObjeto(reader.GetInt32(0)),
                       Cantidad = reader.GetInt32(1),
                       PrecioUnidad = reader.GetDecimal(2),
                    };
                    detalleCompra.Total = detalleCompra.Cantidad * detalleCompra.PrecioUnidad;
                    lista.Add(detalleCompra);
                }
                reader.Close();
                return lista;
            }
        }

        public static void Eliminar(int compraId, bool porVentaId = false)
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
                            strComando = "DELETE FROM DETALLE_COMPRAS WHERE ID_DETALLECOMPRA=@id";
                        }
                        else
                        {
                            strComando = "DELETE FROM DETALLE_COMPRAS WHERE ID_COMPRA=@id";
                        }
                        SqlCommand comando = new SqlCommand(strComando, cn, transaction);
                        comando.Parameters.AddWithValue("@id", compraId);
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
