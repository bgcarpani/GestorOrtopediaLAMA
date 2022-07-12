using LAMAModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMADatabase
{
    public class AlquileresBd
    {
        public static int Agregar(Alquiler alquiler)
        {
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                using (var transaction = cn.BeginTransaction())
                {
                    try
                    {
                        string cadenaComando =
                            "INSERT INTO Alquileres (ID_CLIENTE, FECHADESDE, FECHAHASTA, OBSERVACION, ENUSO, IMPORTE, FechaDevolucion) VALUES (@cliente, @fechaD, @fechaH, @obse, @enuso, @importe, @fecha)";
                        SqlCommand comando = new SqlCommand(cadenaComando, cn, transaction);
                        comando.Parameters.AddWithValue("@fechaD", alquiler.FechaDesde);
                        comando.Parameters.AddWithValue("@fechaH", alquiler.FechaHasta);
                        comando.Parameters.AddWithValue("@importe", alquiler.Importe);
                        comando.Parameters.AddWithValue("@cliente", alquiler.Cliente.ClienteId);
                        comando.Parameters.AddWithValue("@obse", alquiler.Observacion?? null);
                        comando.Parameters.AddWithValue("@enUso", alquiler.EstaEnUso);
                        comando.Parameters.AddWithValue("@fecha", new DateTime(1900, 1, 1));
                        comando.ExecuteNonQuery();
                        cadenaComando = "select @@identity";
                        comando = new SqlCommand(cadenaComando, cn, transaction);
                        int id = (int)(decimal)comando.ExecuteScalar();
                        alquiler.AlquilerId = id;

                        var saldoCta = CtaCteBd.GetSaldo(alquiler.Cliente);
                        var nuevoSaldo = saldoCta - alquiler.Importe;
                        CtaCte cta = new CtaCte();
                        cta.Cliente = alquiler.Cliente;
                        cta.Debe = alquiler.Importe;
                        cta.Haber = 0;
                        cta.Movimiento = $"AL {alquiler.AlquilerId}";
                        cta.Saldo = nuevoSaldo;
                        cta.FechaMovimiento = DateTime.Now;

                        CtaCteBd.Agregar(cta, cn, transaction);

                        foreach (var detalle in alquiler.Detalle)
                        {
                            DetallesAlquileresBd.Agregar(detalle, alquiler.AlquilerId, cn, transaction);
                            ProductoStocksBd.BajarStock(detalle.Stock.StockId, detalle.Cantidad, detalle.Producto.ProductoId, cn, transaction);
                        }
                        transaction.Commit();
                        return id;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        internal static Alquiler GetObjeto(int id)
        {
            Alquiler a = null;
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string strComando = "SELECT * FROM Alquileres " +
                        " WHERE ID_ALQUILER=@id";
                    SqlCommand comando = new SqlCommand(strComando, cn);
                    comando.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        a = new Alquiler();
                        a.AlquilerId = reader.GetInt32(0);
                        a.Cliente = ClientesBd.GetObjeto(reader.GetInt32(1));
                        a.FechaDesde = reader.GetDateTime(2);
                        a.FechaHasta = reader.GetDateTime(3);
                        a.Observacion = reader.GetString(4);
                        a.EstaEnUso = reader.GetBoolean(5);
                        a.Importe = reader.GetDecimal(6);
                        a.FechaDevolucion = reader.GetDateTime(7);
                    }
                }
                return a;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public static Alquiler GetUltimoId()
        {
            Alquiler v = null;
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "select TOP 1 * from ALQUILERES order by ID_ALQUILER desc";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        v = new Alquiler();
                        v.AlquilerId = reader.GetInt32(0);
                        v.Cliente = ClientesBd.GetObjeto(reader.GetInt32(1));
                        v.FechaDesde = reader.GetDateTime(2);
                        v.FechaHasta = reader.GetDateTime(3);
                        v.Observacion = reader.GetString(4);
                        v.EstaEnUso = reader.GetBoolean(5);
                        v.Importe = reader.GetDecimal(6);
                        v.FechaDevolucion = reader.GetDateTime(7);
                    }
                }
                return v;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<Alquiler> GetLista()
        {
            List<Alquiler> lista = new List<Alquiler>();
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "SELECT * FROM Alquileres ORDER BY ID_Alquiler desc";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        Alquiler v = new Alquiler();
                        v.AlquilerId = reader.GetInt32(0);
                        v.Cliente = ClientesBd.GetObjeto(reader.GetInt32(1));
                        v.FechaDesde = reader.GetDateTime(2);
                        v.FechaHasta = reader.GetDateTime(3);
                        v.Observacion = reader.GetString(4);
                        v.EstaEnUso = reader.GetBoolean(5);
                        v.Importe = reader.GetDecimal(6);
                        v.FechaDevolucion = reader.GetDateTime(7);
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

        public static void Devolucion(Alquiler alquiler)
        {
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                using (var transaction = cn.BeginTransaction())
                {
                    try
                    {
                        string cadenaComando =
                            "UPDATE Alquileres SET EnUso = @enUso,Observacion=@obs, FechaDevolucion = @fecha WHERE ID_Alquiler = @id";
                        SqlCommand comando = new SqlCommand(cadenaComando, cn, transaction);
                        comando.Parameters.AddWithValue("@id", alquiler.AlquilerId);
                        comando.Parameters.AddWithValue("@enUso", false);
                        comando.Parameters.AddWithValue("@obs", alquiler.Observacion);
                        comando.Parameters.AddWithValue("@fecha", DateTime.Now);
                        comando.ExecuteNonQuery();

                        foreach (var detallesVenta in alquiler.Detalle)
                        {
                            ProductoStocksBd.SubirStock(detallesVenta.Stock.StockId, detallesVenta.Cantidad, detallesVenta.Producto.ProductoId, cn, transaction);
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

        public static void Borrar(Alquiler p, bool edicionCtaCte = false)
        {
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                using (SqlTransaction transaction = cn.BeginTransaction())
                {
                    try
                    {
                        string strComando = "DELETE FROM Alquileres WHERE ID_ALQUILER=@id";
                        SqlCommand comando = new SqlCommand(strComando, cn, transaction);
                        comando.Parameters.AddWithValue("@id", p.AlquilerId);
                        comando.ExecuteNonQuery();

                        if (edicionCtaCte)
                        {
                            CtaCte cc = CtaCteBd.GetObjeto(p.AlquilerId, "AL");
                           
                            CtaCteBd.BorrarUna(cc.CtaCteId);

                            PagosBd.Borrar("ALQUILER", p.AlquilerId);

                            CtaCteBd.BorrarPorReferido("ALQUILER", p.AlquilerId);

                            TransaccionesBd.Borrar("ALQUILER", p.AlquilerId);

                            PagosBd.Borrar("RENOVACION", p.AlquilerId);

                            CtaCteBd.BorrarPorReferido("RENOVACION", p.AlquilerId);

                            TransaccionesBd.Borrar("RENOVACION", p.AlquilerId);

                            DetallesAlquileresBd.Eliminar(p.AlquilerId, true);

                            foreach (var item in p.Detalle)
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

        public static decimal Renovacion(Alquiler p)
        {
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                using (var transaction = cn.BeginTransaction())
                {
                    try
                    {

                        Alquiler alquilerAnterior = GetObjeto(p.AlquilerId);

                        string cadenaComando = "UPDATE ALQUILERES SET FechaHasta=@fech, Importe=@importe WHERE ID_ALQUILER=@id";
                        SqlCommand comando = new SqlCommand(cadenaComando, cn, transaction);
                        comando.Parameters.AddWithValue("@id", p.AlquilerId);
                        comando.Parameters.AddWithValue("@fech", p.FechaHasta);
                        comando.Parameters.AddWithValue("@importe", p.Importe);
                        comando.ExecuteNonQuery();

                        var saldoCta = CtaCteBd.GetSaldo(p.Cliente);
                        var nuevoSaldo = saldoCta - p.Importe;
                        CtaCte cta = new CtaCte();
                        cta.Cliente = p.Cliente;
                        cta.Debe = p.Importe;
                        cta.Haber = 0;
                        cta.Movimiento = $"RE {p.AlquilerId}";
                        cta.Saldo = nuevoSaldo;
                        cta.FechaMovimiento = DateTime.Now;
                        string refe = $"RENOVACION-{p.AlquilerId}";
                        CtaCteBd.Agregar(cta, cn, transaction, refe);

                        transaction.Commit();
                        return p.Importe;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }
    }
}
