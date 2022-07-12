using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LAMAModels;

namespace LAMADatabase
{
    public class DevolucionCompraBd
    {
        public static List<DevolucionCompra> GetLista()
        {
            List<DevolucionCompra> lista = new List<DevolucionCompra>();
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando =
                        "Select DevolucionDeCompraId, FechaDevolucion, CompraId, Total, RowVersion From DevolucionesDeCompras Order by FechaDevolucion";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        DevolucionCompra dc = new DevolucionCompra();

                        dc.DevolucionCompraId = reader.GetInt32(0);
                        dc.FechaDevolucion = reader.GetDateTime(1);
                       // dc.Compra = ComprasBd.GetObjeto(reader.GetInt32(2));
                        dc.Total = reader.GetDecimal(3);
                        dc.RowVersion = (byte[]) reader[4];
                        
                        lista.Add(dc);
                    }
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Agregar(DevolucionCompra devolucion)
        {
            SqlTransaction tran = null;
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    tran = cn.BeginTransaction();
                    string cadenaComando =
                        "Insert Into DevolucionesDeCompras (FechaDevolucion, CompraId, Total) Values (@fecha, @compra, @total)";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn, tran);
                    comando.Parameters.AddWithValue("@fecha", devolucion.FechaDevolucion);
                    comando.Parameters.AddWithValue("@compra", devolucion.Compra.CompraId);
                    comando.Parameters.AddWithValue("@total", devolucion.Total);
                    comando.ExecuteNonQuery();

                    cadenaComando = "Select @@Identity";
                    comando = new SqlCommand(cadenaComando, cn, tran);
                    int id = (int) (decimal) comando.ExecuteScalar();
                    devolucion.DevolucionCompraId = id;

                    foreach (var item in devolucion.DetalleDevolucionCompras)
                    {
                        ProductosBd.BajarStock(item.Producto, item.Cantidad, cn, tran);

                        Kardex kardex = KardexBd.UltimoKardex(item.Producto, cn, tran);
                        if (kardex == null)
                        {
                            kardex = new Kardex();
                            kardex.Producto = item.Producto;
                            kardex.FechaMovimiento = devolucion.FechaDevolucion;
                            kardex.Movimiento = $"DC {devolucion.DevolucionCompraId}";
                            kardex.Entrada = 0;
                            kardex.Salida = item.Cantidad;
                            kardex.Saldo = item.Cantidad;
                            kardex.UltimoCosto = item.PrecioUnitario;
                            kardex.CostoPromedio = item.PrecioUnitario;
                        }
                        else
                        {
                            int NuevoSaldo = kardex.Saldo + item.Cantidad;
                            decimal NuevoPromedio = ((kardex.CostoPromedio * (decimal)kardex.Saldo) + (item.PrecioUnitario * (decimal)item.Cantidad)) / (decimal)NuevoSaldo;
                            kardex.Producto = item.Producto;
                            kardex.FechaMovimiento = devolucion.FechaDevolucion;
                            kardex.Movimiento = $"DC {devolucion.DevolucionCompraId}";
                            kardex.Entrada = 0;
                            kardex.Salida = item.Cantidad;
                            kardex.Saldo = NuevoSaldo;
                            kardex.UltimoCosto = item.PrecioUnitario;
                            kardex.CostoPromedio = NuevoPromedio;
                        }
                        KardexBd.Agregar(kardex, cn, tran);
                        item.Kardex = kardex;

                        item.DevolucionCompra = devolucion;
                        DetallesDevolucionesComprasBd.Agregar(item, cn, tran);
                    }

                    tran.Commit();
                }

                
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
        }
    }
}
