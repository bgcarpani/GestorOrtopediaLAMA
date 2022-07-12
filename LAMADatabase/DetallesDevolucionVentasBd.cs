using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LAMAModels;

namespace LAMADatabase
{
    public class DetallesDevolucionVentasBd
    {
        public static int GetCantidadDevuelta(Venta venta, Producto productoSucursal)
        {
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                string cadenaComando = "SELECT SUM(Cantidad) AS Total FROM DetalleDevolucionVENTA" +
                     "inner join DEVOLUCIONESVENTAS on DEVOLUCIONESVENTAS.ID_DEVOLUCION=DetalleDevolucionVENTA.ID_DETALLEDEVOLUCION " +
                     " WHERE ID_PRODUCTO=@prod AND ID_VENTA=@vta " +
                     " GROUP BY ID_PRODUCTO, ID_VENTA ";
                SqlCommand comando = new SqlCommand(cadenaComando, cn);
                comando.Parameters.AddWithValue("@vta", venta.VentaId);
                comando.Parameters.AddWithValue("@prod", productoSucursal.ProductoId);
                if (comando.ExecuteScalar() != null)
                {
                    return (int) comando.ExecuteScalar();
                }
                else
                {
                    return 0;
                }
            }
        }

        public static void Agregar(DetalleDevolucionVenta detalle, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                string cadenaComando = "INSERT INTO DetalleDevolucionVenta (ID_PRODUCTO, PrecioUnitario, Cantidad, Total, ID_KARDEX) " +
            "VALUES (@prod, @pre,@cant,  @tot, @kar)";
                SqlCommand comando = new SqlCommand(cadenaComando, cn, tran);
                comando.Parameters.AddWithValue("@prod", detalle.Producto.ProductoId);
                comando.Parameters.AddWithValue("@pre", detalle.PrecioUnitario);
                comando.Parameters.AddWithValue("@cant", detalle.Cantidad);
                comando.Parameters.AddWithValue("@tot", detalle.Total);
                comando.Parameters.AddWithValue("@kar", detalle.Kardex.KardexId);
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
