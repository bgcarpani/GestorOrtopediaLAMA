using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LAMAModels;

namespace LAMADatabase
{
    public class DevolucionVentaBd
    {
        public static List<DevolucionVenta> GetLista()
        {
            List<DevolucionVenta> lista = new List<DevolucionVenta>();
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando =
                        "SELECT ID_Devolucion, FechaDevolucion, ID_VENTA, Total FROM DevolucionesVentas ORDER BY FechaDevolucion";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        DevolucionVenta dv = new DevolucionVenta()
                        {
                            DevolucionVentaId = reader.GetInt32(0),
                            FechaDevolucion = reader.GetDateTime(1),
                            Venta = VentasBd.GetObjeto(reader.GetInt32(2)),
                            Total = reader.GetDecimal(3),
                        };
                        lista.Add(dv);
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Agregar(DevolucionVenta devolucion, bool moverAAlquileres = false)
        {
            SqlTransaction tran = null;
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    tran = cn.BeginTransaction();
                    string cadenaComando = "INSERT INTO DevolucionesVentas (FechaDevolucion, ID_VENTA, Total)" +
                                           " VALUES(@fec, @vta, @total)";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn, tran);
                    comando.Parameters.AddWithValue("@fec", devolucion.FechaDevolucion);
                    comando.Parameters.AddWithValue("@vta", devolucion.Venta.VentaId);
                    comando.Parameters.AddWithValue("@total", devolucion.Total);

                    comando.ExecuteNonQuery();
                    cadenaComando = "Select @@Identity";
                    comando = new SqlCommand(cadenaComando, cn, tran);
                    int id = (int) (decimal) comando.ExecuteScalar();
                    devolucion.DevolucionVentaId = id;
                    foreach (var item in devolucion.DetalleDevolucionVentas)
                    {
                        DetallesVentasBd.Devuelto(item);
                        VentasBd.Devuelto(item.DevolucionVenta.Venta.VentaId);
                        if (!moverAAlquileres)
                        {
                            ProductoStocksBd.SubirStock(1, item.Cantidad, item.Producto.ProductoId, cn, tran);
                        }
                        else
                        {
                            List<ProductoStock> lista = ProductoStocksBd.GetLista(2);
                            lista = lista.Where(c => c.Producto.ProductoId == item.Producto.ProductoId).ToList();
                            if (lista.Any())
                            {
                                ProductoStocksBd.SubirStock(2, item.Cantidad, item.Producto.ProductoId, cn, tran);
                            }
                            else
                            {
                                ProductoStocksBd.Agregar(2, item.Cantidad, item.Producto.ProductoId);
                            }
                        }

                        Kardex kardex = KardexBd.UltimoKardex(item.Producto, cn, tran);
                        if (kardex == null)
                        {
                            kardex = new Kardex();
                            kardex.Producto = item.Producto;
                            kardex.FechaMovimiento = devolucion.FechaDevolucion;
                            kardex.Movimiento = $"DV {devolucion.DevolucionVentaId}";
                            kardex.Entrada = item.Cantidad;
                            kardex.Salida = 0;
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
                            kardex.Movimiento = $"DV {devolucion.DevolucionVentaId}";
                            kardex.Entrada = item.Cantidad;
                            kardex.Salida = 0;
                            kardex.Saldo = NuevoSaldo;
                            kardex.UltimoCosto = item.PrecioUnitario;
                            kardex.CostoPromedio = NuevoPromedio;
                        }
                        TransaccionesBd.Agregar(1, devolucion.Venta.Cliente.ClienteId, -devolucion.Total, cn, tran, $"DEVOLUCION-{devolucion.DevolucionVentaId}");
                        KardexBd.Agregar(kardex, cn, tran);
                        item.Kardex = kardex;

                        item.DevolucionVenta = devolucion;
             
                        DetallesDevolucionVentasBd.Agregar(item, cn, tran);
                    }

                    var saldo = CtaCteBd.GetSaldo(devolucion.Venta.Cliente, cn, tran);
                    CtaCte cta = new CtaCte
                    {
                        FechaMovimiento = DateTime.Now,
                        Saldo = saldo + devolucion.Total,
                        Debe = 0,
                        Haber = devolucion.Total,
                        Cliente = devolucion.Venta.Cliente,
                        Movimiento = $"DV {devolucion.DevolucionVentaId}"
                    };

                    CtaCteBd.Agregar(cta, cn, tran);
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
