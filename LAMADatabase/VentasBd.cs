using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LAMAModels;

namespace LAMADatabase
{
    public class VentasBd
    {
        public static List<Venta> GetLista()
        {
            List<Venta> lista = new List<Venta>();
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "SELECT * FROM Ventas ORDER BY FECHA_VENTA desc";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        Venta v = new Venta
                        {
                            VentaId = reader.GetInt32(0),
                            Cliente = ClientesBd.GetObjeto(reader.GetInt32(1)),
                            FechaVenta = reader.GetDateTime(2),
                            Total = reader.GetDecimal(3),
                            EsConsumidorFinal = reader.GetBoolean(4),
                            Devuelto = reader.GetBoolean(5)
                        };
                        lista.Add(v);

                    }

                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void Agregar(Venta venta, bool esConsumidorFinal)
        {
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                using (var transaction = cn.BeginTransaction())
                {
                    try
                    {
                        #region encabezado
                        string cadenaComando =
                            "INSERT INTO Ventas (ID_CLIENTE, FECHA_VENTA, IMPORTE, EsConsumidorFinal, Devuelto) VALUES (@cliente, @fecha, @total, @escon, @dev)";
                        SqlCommand comando = new SqlCommand(cadenaComando, cn, transaction);
                        comando.Parameters.AddWithValue("@fecha", venta.FechaVenta);
                        if (!esConsumidorFinal)
                        {
                            comando.Parameters.AddWithValue("@cliente", venta.Cliente.ClienteId);
                        }
                        else
                        {
                            string consumidorFinalId = ConfigurationManager.ConnectionStrings["ConsumidorFinal"].ToString();
                            comando.Parameters.AddWithValue("@cliente", int.Parse(consumidorFinalId));
                        }
                        comando.Parameters.AddWithValue("@total", venta.Total);
                        comando.Parameters.AddWithValue("@escon", esConsumidorFinal);
                        comando.Parameters.AddWithValue("@dev", false);
                        comando.ExecuteNonQuery();
                        cadenaComando = "select @@identity";
                        comando = new SqlCommand(cadenaComando, cn, transaction);
                        int id = (int)(decimal)comando.ExecuteScalar();
                        venta.VentaId = id;

                        #endregion termina encabezado
                        #region detalle
                        foreach (var detallesVenta in venta.Detalle)
                        {
                            detallesVenta.Venta = venta;
                            Kardex kardex = KardexBd.UltimoKardex(detallesVenta.Producto, cn, transaction);
                            if (kardex == null)
                            {
                                kardex = new Kardex();
                                kardex.Producto = detallesVenta.Producto;
                                kardex.FechaMovimiento = venta.FechaVenta;
                                kardex.Movimiento = $"VE {venta.VentaId}";
                                kardex.Entrada = 0;
                                kardex.Salida = detallesVenta.Cantidad;
                                kardex.Saldo = detallesVenta.Cantidad;
                                kardex.UltimoCosto = detallesVenta.PrecioUnidad;
                                kardex.CostoPromedio = detallesVenta.PrecioUnidad;
                            }
                            else
                            {
                                int NuevoSaldo = kardex.Saldo - detallesVenta.Cantidad;
                                if (NuevoSaldo == 0)
                                {
                                    NuevoSaldo = 1;
                                }
                                decimal NuevoPromedio = ((kardex.CostoPromedio * (decimal)kardex.Saldo) + (detallesVenta.PrecioUnidad * (decimal)detallesVenta.Cantidad)) / (decimal)NuevoSaldo;
                                kardex.Producto = detallesVenta.Producto;
                                kardex.FechaMovimiento = venta.FechaVenta;
                                kardex.Movimiento = $"VE {venta.VentaId}";
                                kardex.Entrada = 0;
                                kardex.Salida = detallesVenta.Cantidad;
                                kardex.Saldo = NuevoSaldo;
                                kardex.UltimoCosto = detallesVenta.PrecioUnidad;
                                kardex.CostoPromedio = NuevoPromedio;

                            }
                            KardexBd.Agregar(kardex, cn, transaction);
                            detallesVenta.Kardex = kardex;
                            int clienteId = 0;
                            if (esConsumidorFinal)
                            {
                                clienteId = int.Parse(ConfigurationManager.ConnectionStrings["ConsumidorFinal"].ToString());
                                TransaccionesBd.Agregar(1, clienteId, venta.Total-venta.ImporteOS, cn, transaction, $"VENTA-{venta.VentaId}");

                            }
                            DetallesVentasBd.Agregar(detallesVenta, cn, transaction);
                            ProductoStocksBd.BajarStock(1, detallesVenta.Cantidad, detallesVenta.Producto.ProductoId, cn, transaction);
                        }
                        if (!esConsumidorFinal)
                        {
                            decimal saldo = CtaCteBd.GetSaldo(venta.Cliente, cn, transaction);
                            CtaCte cuenta = new CtaCte()
                            {
                                Cliente = venta.Cliente,
                                FechaMovimiento = DateTime.Now,
                                Movimiento = $"VE {venta.VentaId}",
                                Debe = venta.Total,
                                Haber = 0,
                                Saldo = saldo - venta.Total
                            };
                            CtaCteBd.Agregar(cuenta, cn, transaction);
                        }
                        #endregion termina detalle
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        internal static void Devuelto(int ventaId)
        {
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                using (var transaction = cn.BeginTransaction())
                {
                    try
                    {
                        string strComando = "update VENTAS set Devuelto = 1 WHERE ID_VENTA = @id";

                        SqlCommand comando = new SqlCommand(strComando, cn, transaction);
                        comando.Parameters.AddWithValue("@id", ventaId);
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

        public static List<Caja> GetCaja(string caja)
        {
            List<Caja> lista = new List<Caja>();
            try
            {
                string cadenaComando = "";
                switch (caja)
                {
                    case "diaria":
                        cadenaComando = "select TOP 10 FECHA_VENTA, Sum(IMPORTE) as Recaudado from VENTAS group by FECHA_VENTA " +
                            "order by FECHA_VENTA desc";
                        break;
                    case "mensual":
                        cadenaComando = "select MONTH(FECHA_VENTA) as Mes, Sum(IMPORTE) as Recaudado from VENTAS " +
                            "where YEAR(FECHA_VENTA) = YEAR(GETDATE()) " +
                            "group by Month(FECHA_VENTA) " +
                            "order by Mes desc";
                        break;
                    case "anual":
                        cadenaComando = "select TOP 10 YEAR(FECHA_VENTA) as Anio, Sum(IMPORTE) as Recaudado " +
                            "from VENTAS group by YEAR(FECHA_VENTA) order by Anio desc";
                        break;
                    case "eterna":
                        cadenaComando = "select GETDATE() as Hoy, SUM(IMPORTE) from VENTAS";
                        break;
                    default:
                        break;
                }
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        Caja v = new Caja();
                        switch (caja)
                        {
                            case "diaria":
                                v.Fecha = reader.GetDateTime(0);
                                break;
                            case "mensual":
                                int mes = reader.GetInt32(0);
                                v.Tabla = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(mes);
                                break;
                            case "anual":
                                v.Tabla = reader.GetInt32(0).ToString();
                                break;
                            case "eterna":
                                v.Fecha = reader.GetDateTime(0);
                                break;
                            default:
                                break;
                        }
                        v.Recaudado = reader.GetDecimal(1);
                        lista.Add(v);

                    }

                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Venta GetObjeto(int id)
        {
            Venta p = null;
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComnando = "SELECT * FROM Ventas WHERE ID_VENTA=@id";
                    SqlCommand comando = new SqlCommand(cadenaComnando, cn);
                    comando.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        p = new Venta();
                        p.VentaId = reader.GetInt32(0);
                        p.Cliente = ClientesBd.GetObjeto(reader.GetInt32(1));
                        p.FechaVenta = reader.GetDateTime(2);
                        p.Total = reader.GetDecimal(3);
                        p.EsConsumidorFinal = reader.GetBoolean(4);
                        p.Devuelto = reader.GetBoolean(5);
                    }

                    return p;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Editar(Venta p, bool edicionCtaCte = false)
        {
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                using (var transaction = cn.BeginTransaction())
                {
                    try
                    {

                        string cadenaComando = "UPDATE VENTAS Set ID_CLIENTE=@cli, FECHA_VENTA=@fec, IMPORTE=@tot, EsConsumidorFinal=@escon, Devuelto = @dev WHERE ID_VENTA=@id";
                        SqlCommand comando = new SqlCommand(cadenaComando, cn, transaction);
                        comando.Parameters.AddWithValue("@id", p.VentaId);
                        comando.Parameters.AddWithValue("@cli", p.Cliente.ClienteId);
                        comando.Parameters.AddWithValue("@fec", p.FechaVenta);
                        comando.Parameters.AddWithValue("@tot", p.Total);
                        comando.Parameters.AddWithValue("@escon", p.EsConsumidorFinal);
                        comando.Parameters.AddWithValue("@dev", false);
                        comando.ExecuteNonQuery();

                        if (edicionCtaCte)
                        {

                            CtaCte cc = CtaCteBd.GetObjeto(p.VentaId, "VE");
                            cc.Cliente.ClienteId = p.Cliente.ClienteId;
                            decimal saldoAux = cc.Debe + cc.Saldo;
                            cc.Saldo = saldoAux - p.Total;
                            cc.Debe = p.Total;
                            CtaCteBd.Editar(cc);


                            foreach (var detallesVenta in p.Detalle)
                            {
                                detallesVenta.Venta = p;
                                Kardex kardex = KardexBd.UltimoKardex(detallesVenta.Producto, cn, transaction);
                                int kardexAEliminar = kardex.KardexId;
                                if (kardex == null)
                                {
                                    kardex = new Kardex();
                                    kardex.Producto = detallesVenta.Producto;
                                    kardex.FechaMovimiento = p.FechaVenta;
                                    kardex.Movimiento = $"VE {p.VentaId}";
                                    kardex.Entrada = 0;
                                    kardex.Salida = detallesVenta.Cantidad;
                                    kardex.Saldo = detallesVenta.Cantidad;
                                    kardex.UltimoCosto = detallesVenta.PrecioUnidad;
                                    kardex.CostoPromedio = detallesVenta.PrecioUnidad;
                                }
                                else
                                {
                                    int NuevoSaldo = kardex.Saldo - detallesVenta.Cantidad;
                                    decimal NuevoPromedio = ((kardex.CostoPromedio * (decimal)kardex.Saldo) + (detallesVenta.PrecioUnidad * (decimal)detallesVenta.Cantidad)) / (decimal)NuevoSaldo;
                                    kardex.Producto = detallesVenta.Producto;
                                    kardex.FechaMovimiento = p.FechaVenta;
                                    kardex.Movimiento = $"VE {p.VentaId}";
                                    kardex.Entrada = 0;
                                    kardex.Salida = detallesVenta.Cantidad;
                                    kardex.Saldo = NuevoSaldo;
                                    kardex.UltimoCosto = detallesVenta.PrecioUnidad;
                                    kardex.CostoPromedio = NuevoPromedio;

                                }
                                KardexBd.Borrar(kardexAEliminar, cn, transaction);
                                KardexBd.Agregar(kardex, cn, transaction);
                                detallesVenta.Kardex = kardex;

                                DetallesVentasBd.Eliminar(p.VentaId, true);

                                detallesVenta.Venta = p;
                                DetallesVentasBd.Agregar(detallesVenta, cn, transaction);

                            }

                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }



        public static void Borrar(Venta venta, bool edicionVenta = false)
        {
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                using (SqlTransaction transaction = cn.BeginTransaction())
                {
                    try
                    {
                        string strComando = "DELETE FROM Ventas WHERE ID_VENTA=@id";
                        SqlCommand comando = new SqlCommand(strComando, cn, transaction);
                        comando.Parameters.AddWithValue("@id", venta.VentaId);
                        comando.ExecuteNonQuery();
                        if (edicionVenta)
                        {
                            CtaCte cc = CtaCteBd.GetObjeto(venta.VentaId, "VE");

                            CtaCteBd.BorrarUna(cc.CtaCteId);
                           
                            CtaCteBd.BorrarPorReferido("VENTA", venta.VentaId);

                            PagosBd.Borrar("VENTA", venta.VentaId);

                            TransaccionesBd.Borrar("VENTA", venta.VentaId);

                            DetallesVentasBd.Eliminar(venta.VentaId, true);

                            foreach (var item in venta.Detalle)
                            {
                                ProductoStocksBd.SubirStock(2, item.Cantidad, item.Producto.ProductoId, cn, transaction);
                            }
                        }
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
