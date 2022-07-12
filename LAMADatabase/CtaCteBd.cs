using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LAMAModels;

namespace LAMADatabase
{
    public class CtaCteBd
    {
        public static List<ConsultaCtaCte> GetSaldos()
        {
            List<ConsultaCtaCte> lista = new List<ConsultaCtaCte>();
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "SELECT ClienteId, SUM(HABER-DEBE) as Saldo FROM CuentasCorrientes GROUP BY  ClienteId";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        ConsultaCtaCte consulta = new ConsultaCtaCte()
                        {
                            Cliente = ClientesBd.GetObjeto(reader.GetInt32(0)),
                            Saldo = reader.GetDecimal(1)
                        };
                        lista.Add(consulta);
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static decimal GetSaldo(Cliente cliente)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "SELECT Saldo FROM CuentasCorrientes WHERE ClienteId=@id AND CtaCteId=(" +
                                           "SELECT Max(CtaCteId) FROM CuentasCorrientes WHERE ClienteId=@id)";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@id", cliente.ClienteId);
                    decimal saldo = comando.ExecuteScalar() == null ? 0 : (decimal)comando.ExecuteScalar();
                    return saldo;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CtaCte GetObjeto(int id, string porMovimiento = "")
        {
            CtaCte cta = null;
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando;
                    string movimiento = "";
                    if (string.IsNullOrEmpty(porMovimiento))
                    {
                      cadenaComando = "SELECT CtaCteId, FechaMovimiento, Movimiento, Debe, Haber, Saldo, ClienteId FROM CuentasCorrientes WHERE CtaCteId=@Id";
                    }
                    else
                    {
                      movimiento = $"{porMovimiento} {id}";
                      cadenaComando = "SELECT CtaCteId, FechaMovimiento, Movimiento, Debe, Haber, Saldo, ClienteId FROM CuentasCorrientes WHERE Movimiento=@Id";
                    }
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    if (string.IsNullOrEmpty(porMovimiento))
                        comando.Parameters.AddWithValue("@id", id);
                    else
                        comando.Parameters.AddWithValue("@id", movimiento);
                    SqlDataReader reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        cta = new CtaCte()
                        {
                            CtaCteId = reader.GetInt32(0),
                            FechaMovimiento = reader.GetDateTime(1),
                            Movimiento = reader.GetString(2),
                            Debe = reader.GetDecimal(3),
                            Haber = reader.GetDecimal(4),
                            Saldo = reader.GetDecimal(5),
                            Cliente = ClientesBd.GetObjeto(reader.GetInt32(6))

                        };

                    }
                    reader.Close();
                    return cta;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static void ActualizarPago(Pago pago, SqlConnection cn, SqlTransaction tran)
        {
            try
            {
                string cadenaComando = "UPDATE CuentasCorrientes Set Movimiento=@mov WHERE CtaCteId=@id";
                SqlCommand comando = new SqlCommand(cadenaComando, cn, tran);
                comando.Parameters.AddWithValue("@mov", $"PA {pago.PagoId}");
                comando.Parameters.AddWithValue("@id", pago.CtaCte.CtaCteId);
                comando.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static void BorrarCompleta(int clienteId)
        {
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                using (SqlTransaction transaction = cn.BeginTransaction())
                {
                    try
                    {

                        string strComando = "DELETE FROM CuentasCorrientes WHERE ClienteId=@id";
                        SqlCommand comando = new SqlCommand(strComando, cn, transaction);
                        comando.Parameters.AddWithValue("@id", clienteId);
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

        public static void BorrarUna(int ctaId)
        {
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                using (SqlTransaction transaction = cn.BeginTransaction())
                {
                    try
                    {

                        string strComando = "DELETE FROM CuentasCorrientes WHERE CtaCteId=@id";
                        SqlCommand comando = new SqlCommand(strComando, cn, transaction);
                        comando.Parameters.AddWithValue("@id", ctaId);
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





        public static List<CtaCte> GetMovimientos(Cliente cliente)
        {
            List<CtaCte> lista = new List<CtaCte>();
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "SELECT FechaMovimiento, Movimiento, Debe, Haber FROM CuentasCorrientes " +
                        "WHERE ClienteId=@id ORDER BY FechaMovimiento desc";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@id", cliente.ClienteId);
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        CtaCte cuenta = new CtaCte()
                        {
                            FechaMovimiento = reader.GetDateTime(0),
                            Movimiento = reader.GetString(1),
                            Debe = reader.GetDecimal(2),
                            Haber = reader.GetDecimal(3),

                        };
                        lista.Add(cuenta);
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static decimal GetSaldo(Cliente cliente, SqlConnection cn, SqlTransaction transaction)
        {
            try
            {
                string cadenaComando = "SELECT Saldo FROM CuentasCorrientes WHERE ClienteId=@id AND CtaCteId=(" +
                                       "SELECT Max(CtaCteId) FROM CuentasCorrientes WHERE ClienteId=@id)";
                SqlCommand comando = new SqlCommand(cadenaComando, cn, transaction);
                comando.Parameters.AddWithValue("@id", cliente.ClienteId);
                decimal saldo = comando.ExecuteScalar() == null ? 0 : (decimal)comando.ExecuteScalar();
                return saldo;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static void Agregar(CtaCte cuenta, SqlConnection cn, SqlTransaction transaction, string referido = "")
        {
            try
            {
                string cadenaComando = "INSERT INTO CuentasCorrientes (FechaMovimiento, Movimiento, Debe, Haber, Saldo," +
                                   "ClienteId, Referido) VALUES(@fecha, @mov, @debe, @haber, @saldo, @cli, @ref)";
                SqlCommand comando = new SqlCommand(cadenaComando, cn, transaction);
                comando.Parameters.AddWithValue("@fecha", cuenta.FechaMovimiento);
                comando.Parameters.AddWithValue("@mov", cuenta.Movimiento);
                comando.Parameters.AddWithValue("@debe", cuenta.Debe);
                comando.Parameters.AddWithValue("@haber", cuenta.Haber);
                comando.Parameters.AddWithValue("@saldo", cuenta.Saldo);
                comando.Parameters.AddWithValue("@cli", cuenta.Cliente.ClienteId);
                comando.Parameters.AddWithValue("@ref", referido);
                comando.ExecuteNonQuery();
                cadenaComando = "Select @@Identity";
                comando = new SqlCommand(cadenaComando, cn, transaction);
                cuenta.CtaCteId = (int)(decimal)comando.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<CtaCte> GetDetalle(Cliente cliente)
        {
            List<CtaCte> lista = new List<CtaCte>();
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "SELECT FechaMovimiento, Movimiento, Debe, Haber, Saldo FROM CuentasCorrientes " +
                        "WHERE ClienteId=@id ORDER BY FechaMovimiento";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@id", cliente.ClienteId);
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        CtaCte cuenta = new CtaCte()
                        {
                            FechaMovimiento = reader.GetDateTime(0),
                            Movimiento = reader.GetString(1),
                            Debe = reader.GetDecimal(2),
                            Haber = reader.GetDecimal(3),
                            Saldo = reader.GetDecimal(4)
                        };
                        lista.Add(cuenta);
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static void Editar(CtaCte cc)
        {
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                using (var transaction = cn.BeginTransaction())
                {
                    try
                    {
                        string cadenaComando = "UPDATE CuentasCorrientes Set FechaMovimiento=@fec, Movimiento=@mov, Debe=@debe, Haber=@hab, Saldo=@sal, ClienteId=@clie WHERE CtaCteId=@id";
                        SqlCommand comando = new SqlCommand(cadenaComando, cn, transaction);
                        comando.Parameters.AddWithValue("@fec", cc.FechaMovimiento);
                        comando.Parameters.AddWithValue("@mov", cc.Movimiento);
                        comando.Parameters.AddWithValue("@debe", cc.Debe);
                        comando.Parameters.AddWithValue("@hab", cc.Haber);
                        comando.Parameters.AddWithValue("@sal", cc.Saldo);
                        comando.Parameters.AddWithValue("@clie", cc.Cliente.ClienteId);
                        comando.Parameters.AddWithValue("@id", cc.CtaCteId);
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
   

        //public static void ActualizarSaldoCuenta(decimal saldo, Cliente clienteId, Pago pago, SqlConnection cn = null, SqlTransaction transaction = null)
        //{
        //    using (cn = ConexionBd.GetConexion())
        //    {
        //        cn.Open();
        //        using (transaction = cn.BeginTransaction())
        //        {
        //            try
        //            {
        //                string cadenaComando = "update CuentasCorrientes set Saldo = @Saldo + @pago where ClienteId = @id and ctaCteId = (Select Max(CtaCteId) from CuentasCorrientes where ClienteId = @id)";
        //                SqlCommand comando = new SqlCommand(cadenaComando, cn, transaction);
        //                comando.Parameters.AddWithValue("@saldo", pago.CtaCte.Saldo);
        //                comando.Parameters.AddWithValue("@pago", pago.Importe);
        //                comando.Parameters.AddWithValue("@id", pago.Cliente.ClienteId);
        //                comando.ExecuteNonQuery();
        //                transaction.Commit();
        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();
        //                throw ex;
        //            }
        //        }
        //    }
           
        //}
        public static void Borrar(int ctaCteId, SqlConnection cn, SqlTransaction transaction)
        {
            try
            {
                string cadenaComando = "DELETE FROM CuentasCorrientes WHERE CtaCteId=@id";
                SqlCommand comando = new SqlCommand(cadenaComando, cn, transaction);
                comando.Parameters.AddWithValue("@id", ctaCteId);
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void BorrarPorReferido(string mov, int id)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string desc = $"{mov}-{id}";
                    string strComando = "DELETE FROM CuentasCorrientes WHERE Referido=@desc";
                    SqlCommand comando = new SqlCommand(strComando, cn);
                    comando.Parameters.AddWithValue("@desc", desc);
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
    }
}