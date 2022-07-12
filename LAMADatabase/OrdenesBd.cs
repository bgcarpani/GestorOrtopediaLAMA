using LAMAModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMADatabase
{
    public class OrdenesBd
    {
        public static List<Orden> GetLista()
        {
            List<Orden> lista = new List<Orden>();
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "SELECT * FROM Ordenes ORDER BY ID_Orden desc";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        Orden v = new Orden
                        {
                            OrdenId = reader.GetInt32(0),
                            Cliente = ClientesBd.GetObjeto(reader.GetInt32(1)),
                            Protesis = ProtesisBd.GetObjeto(reader.GetInt32(2)),
                            Costo = reader.GetDecimal(3),
                            FechaInicio = reader.GetDateTime(4),
                            FechaEntrega = reader.GetDateTime(5),
                            Notas = reader.GetString(6),
                            Entregado = reader.GetBoolean(7),
                            Eliminado = reader.GetBoolean(8),
                            FechaEliminacion = reader.GetDateTime(9),
                            Senia = reader.GetDecimal(10),
                            DiasEstimados = reader.GetInt32(11),
                            ImporteOS = reader.GetDecimal(12)
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

        public static int Agregar(Orden orden)
        {
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                using (var transaction = cn.BeginTransaction())
                {
                    try
                    {
                        string cadenaComando =
                            "INSERT INTO Ordenes (ID_CLIENTE, ID_PROTESIS, COSTO, FECHA_INICIO, FECHA_ENTREGA, NOTAS, ENTREGADO, ELIMINADO, FECHA_ELIMINACION, " +
                            "SENIA, Dias_Estimados, ImporteOS) VALUES (@cliente, @prot, @cost, @fechaInicio, @fechaEntrega, @notas, @entregado, @eliminado, @fechaElim, @senia, @dias, @os)";
                        SqlCommand comando = new SqlCommand(cadenaComando, cn, transaction);
                        comando.Parameters.AddWithValue("@cliente", orden.Cliente.ClienteId);
                        comando.Parameters.AddWithValue("@prot", orden.Protesis.ProtesisId);
                        comando.Parameters.AddWithValue("@cost", orden.Costo);
                        comando.Parameters.AddWithValue("@fechaInicio", orden.FechaInicio);
                        comando.Parameters.AddWithValue("@fechaEntrega", orden.FechaEntrega);
                        comando.Parameters.AddWithValue("@notas", orden.Notas);
                        comando.Parameters.AddWithValue("@entregado", orden.Entregado);
                        comando.Parameters.AddWithValue("@eliminado", orden.Eliminado);
                        comando.Parameters.AddWithValue("@fechaElim", orden.FechaEliminacion);
                        comando.Parameters.AddWithValue("@senia", orden.Senia);
                        comando.Parameters.AddWithValue("@dias", orden.DiasEstimados);
                        comando.Parameters.AddWithValue("@os", orden.ImporteOS);
                        comando.ExecuteNonQuery();
                        cadenaComando = "select @@identity";
                        comando = new SqlCommand(cadenaComando, cn, transaction);
                        int id = (int)(decimal)comando.ExecuteScalar();
                        orden.OrdenId = id;

                        decimal saldo = CtaCteBd.GetSaldo(orden.Cliente, cn, transaction);
                        CtaCte cuenta = new CtaCte()
                        {
                            Cliente = orden.Cliente,
                            FechaMovimiento = DateTime.Now,
                            Movimiento = $"OT {orden.OrdenId}",
                            Debe = orden.Costo,
                            Haber = orden.Senia+orden.ImporteOS,
                            Saldo = (saldo - (orden.Costo)) + orden.Senia
                        };
                        CtaCteBd.Agregar(cuenta, cn, transaction, $"ORDEN-{orden.OrdenId}");
                        TransaccionesBd.Agregar(4, orden.Cliente.ClienteId, orden.Senia, cn, transaction, $"ORDEN-{orden.OrdenId}");

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

        public static void Entrega(Orden ord)
        {
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                using (var transaction = cn.BeginTransaction())
                {
                    try
                    {
                        string cadenaComando =
                            "UPDATE Ordenes SET Entregado = @entregado, Fecha_Entrega=@fecha WHERE ID_ORDEN = @id";
                        SqlCommand comando = new SqlCommand(cadenaComando, cn, transaction);
                        comando.Parameters.AddWithValue("@id", ord.OrdenId);
                        comando.Parameters.AddWithValue("@entregado", true);
                        comando.Parameters.AddWithValue("@fecha", DateTime.Now);
                        comando.ExecuteNonQuery();
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

        public static void EditarCliente(Orden item, int clienteId)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "UPDATE Ordenes Set ID_Cliente=@clienteId WHERE ID_ORDEN=@id";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@clienteId", clienteId);
                    comando.Parameters.AddWithValue("@id", item.OrdenId);
                    int cantidad = comando.ExecuteNonQuery();
                    if (cantidad == 0)
                    {
                        throw new Exception("Registro no encontrado.");
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static void Borrar(Orden ord)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string strComando = "DELETE FROM Ordenes WHERE ID_ORDEN=@id";
                    SqlCommand comando = new SqlCommand(strComando, cn);
                    comando.Parameters.AddWithValue("@id", ord.OrdenId);
                    comando.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {

                if (ex.Message.Contains("REFERENCE"))
                {
                    throw new Exception("Registro relacionado con otra tabla\nBaja denegada");
                }
                throw new Exception(ex.Message);
            }
        }

        public static Orden GetObjeto(int id)
        {
            Orden p = null;
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComnando = "SELECT * FROM Ordenes WHERE ID_Orden=@id";
                    SqlCommand comando = new SqlCommand(cadenaComnando, cn);
                    comando.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        p = new Orden();
                        p.OrdenId = reader.GetInt32(0);
                        p.Cliente = ClientesBd.GetObjeto(reader.GetInt32(1));
                        p.Protesis = ProtesisBd.GetObjeto(reader.GetInt32(2));
                        p.Costo = reader.GetDecimal(3);
                        p.FechaInicio = reader.GetDateTime(4);
                        p.FechaEntrega = reader.GetDateTime(5);
                        p.Notas = reader.GetString(6);
                        p.Entregado = reader.GetBoolean(7);
                        p.Eliminado = reader.GetBoolean(8);
                        p.FechaEliminacion = reader.GetDateTime(9);
                        p.Senia = reader.GetDecimal(10);
                        p.DiasEstimados = reader.GetInt32(11);
                        p.ImporteOS = reader.GetDecimal(12);
                    }

                    return p;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
