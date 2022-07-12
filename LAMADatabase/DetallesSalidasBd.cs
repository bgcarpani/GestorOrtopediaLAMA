using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LAMAModels;

namespace LAMADatabase
{
    public class DetallesSalidasBd
    {
        public static void Agregar(DetalleSalida detalleSalida, SqlConnection cn, SqlTransaction trans)
        {
            try
            {
                string cadenaComando =
                    "INSERT INTO DetallesSalidas (SalidaId, ProductoSucursalId, Cantidad, KardexId, Motivo) VALUES (@sa, @ps, @cant, @ka, @mo)";
                SqlCommand comando = new SqlCommand(cadenaComando, cn, trans);
                comando.Parameters.AddWithValue("@sa", detalleSalida.Salida.SalidaId);
                comando.Parameters.AddWithValue("@ps", detalleSalida.Producto.ProductoId);
                comando.Parameters.AddWithValue("@cant", detalleSalida.Cantidad);
                comando.Parameters.AddWithValue("@ka", detalleSalida.Kardex.KardexId);
                if (detalleSalida.Motivo == string.Empty)
                {
                    comando.Parameters.AddWithValue("@mo", DBNull.Value);
                }
                else
                {
                    comando.Parameters.AddWithValue("@mo", detalleSalida.Motivo);
                }
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
