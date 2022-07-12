using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LAMAModels;

namespace LAMADatabase
{
    public class SalidasBd
    {
        public static List<Salida> GetLista()
        {
            List<Salida> lista = new List<Salida>();
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando =
                        "SELECT SalidaId, FechaSalida, SucursalId, Total, RowVersion FROM Salidas ORDER BY FechaSalida";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        Salida salida = new Salida();

                        salida.SalidaId = reader.GetInt32(0);
                        salida.FechaSalida = reader.GetDateTime(1);
                        //salida.Sucursal = SucursalesBd.GetObjeto(reader.GetInt32(2));
                        salida.Total = reader.GetDecimal(3);
                        salida.RowVersion = (byte[]) reader[4];
                        
                        lista.Add(salida);
                    }
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Agregar(Salida salida)
        {
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                using (var trans = cn.BeginTransaction())
                {
                    try
                    {
                        foreach (var detalleSalida in salida.DetalleSalidas)
                    {
                        detalleSalida.Salida = salida;
                        Kardex kardex = KardexBd.UltimoKardex(detalleSalida.Producto, cn, trans);
                        if (kardex == null)
                        {
                            kardex = new Kardex();
                            kardex.Producto = detalleSalida.Producto;
                            kardex.FechaMovimiento = salida.FechaSalida;
                            kardex.Movimiento = $"SA {salida.SalidaId}";
                            kardex.Entrada = 0;
                            kardex.Salida = detalleSalida.Cantidad;
                            kardex.Saldo = detalleSalida.Cantidad;
                            kardex.UltimoCosto = detalleSalida.Producto.Precio;
                            kardex.CostoPromedio = detalleSalida.Producto.Precio;
                        }
                        else
                        {
                            int NuevoSaldo = kardex.Saldo + detalleSalida.Cantidad;
                            decimal NuevoPromedio = ((kardex.CostoPromedio * (decimal)kardex.Saldo) + (detalleSalida.Producto.Precio * (decimal)detalleSalida.Cantidad)) / (decimal)NuevoSaldo;
                            kardex.Producto = detalleSalida.Producto;
                            kardex.FechaMovimiento = salida.FechaSalida;
                            kardex.Movimiento = $"SA {salida.SalidaId}";
                            kardex.Entrada = 0;
                            kardex.Salida = detalleSalida.Cantidad;
                            kardex.Saldo = NuevoSaldo;
                            kardex.UltimoCosto = detalleSalida.Producto.Precio;
                            kardex.CostoPromedio = NuevoPromedio;

                        }
                        KardexBd.Agregar(kardex, cn, trans);
                        detalleSalida.Kardex = kardex;

                        DetallesSalidasBd.Agregar(detalleSalida, cn, trans);
                        ProductosBd.BajarStock(detalleSalida.Producto, detalleSalida.Cantidad, cn, trans);
                    }
                            trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw ex;
                    }
                }
            }
        }
    }
}
