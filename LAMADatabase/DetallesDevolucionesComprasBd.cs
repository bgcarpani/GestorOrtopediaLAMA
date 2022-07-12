using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LAMAModels;

namespace LAMADatabase
{
    public class DetallesDevolucionesComprasBd
    {
        public static int GetCantidadDevuelta(Compra compra, Producto Producto)
        {
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                string cadenaComando = "SELECT SUM(Cantidad) AS Total FROM DetalleDevolucionDeCompras " +
                     "inner join DevolucionesDeCompras on DevolucionesDeCompras.DevolucionDeCompraId=DetalleDevolucionDeCompras.DevolucionDeCompraId " +
                     " WHERE ProductoId=@prod AND CompraId=@compra " +
                     " GROUP BY ProductoId, CompraId ";
                SqlCommand comando = new SqlCommand(cadenaComando, cn);
                comando.Parameters.AddWithValue("@compra", compra.CompraId);
                comando.Parameters.AddWithValue("@prod", Producto.ProductoId);
                if (comando.ExecuteScalar() != null)
                {
                    return (int)comando.ExecuteScalar();
                }
                else
                {
                    return 0;
                }
            }
        }

        public static void Agregar(DetalleDevolucionCompra detalle, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                string cadenaComando =
                    "Insert Into DetalleDevolucionDeCompras (DevolucionDeCompraId, ProductoId, PrecioUnitario, Total, Cantidad, KardexId)" +
                    "Values (@dc, @ps, @pu, @total, @cant, @kardex)";
                SqlCommand comando = new SqlCommand(cadenaComando, cn, tran);
                comando.Parameters.AddWithValue("@dc", detalle.DevolucionCompra.DevolucionCompraId);
                comando.Parameters.AddWithValue("@ps", detalle.Producto.ProductoId);
                comando.Parameters.AddWithValue("@pu", detalle.PrecioUnitario);
                comando.Parameters.AddWithValue("@total", detalle.Total);
                comando.Parameters.AddWithValue("@cant", detalle.Cantidad);
                comando.Parameters.AddWithValue("@kardex", detalle.Kardex.KardexId);
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
