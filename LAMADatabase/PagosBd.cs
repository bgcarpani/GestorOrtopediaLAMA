using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LAMAModels;

namespace LAMADatabase
{
    public class PagosBd
    {
        public static List<Pago> GetLista()
        {
            List<Pago> lista = new List<Pago>();
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "SELECT ID_PAGO, ID_CLIENTE, CtaCteId, Importe, Notas, FechaPago FROM Pagos order by FechaPago desc";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        Pago p = new Pago()
                        {
                            PagoId = reader.GetInt32(0),
                            Cliente = ClientesBd.GetObjeto(reader.GetInt32(1)),
                            CtaCte = CtaCteBd.GetObjeto(reader.GetInt32(2)),
                            Importe = reader.GetDecimal(3),
                            Descripcion = reader.GetString(4),
                            FechaPago = reader.GetDateTime(5)
                            //FormaDePago = FormasDePagoBd.GetObjeto(reader.GetInt32(4)),
                        };
                        lista.Add(p);
                    }
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void Agregar(Pago pago, int tipoTransaccionId = 0, string referido = "")
        {

            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                using (var transaction = cn.BeginTransaction())
                {
                    try
                    {
                        var saldoCta = CtaCteBd.GetSaldo(pago.Cliente);
                        var nuevoSaldo = saldoCta + pago.Importe;
                        var cta = new CtaCte
                        {
                            Cliente = pago.Cliente,
                            Debe = 0,
                            FechaMovimiento = pago.FechaPago,
                            Haber = pago.Importe,
                            Movimiento = "PA",
                            Saldo = nuevoSaldo
                        };
                        CtaCteBd.Agregar(cta, cn, transaction, referido);
                        pago.CtaCte = cta;

                        string cadenaComando =
                        "INSERT INTO Pagos (ID_CLIENTE, CtaCteId, Importe, NOTAS, FechaPago)" +
                        "VALUES (@cliente, @cta, @imp, @desc, @fech)";
                        SqlCommand comando = new SqlCommand(cadenaComando, cn, transaction);
                        comando.Parameters.AddWithValue("@cliente", pago.Cliente.ClienteId);
                        comando.Parameters.AddWithValue("@cta", pago.CtaCte.CtaCteId);
                        comando.Parameters.AddWithValue("@desc", pago.Descripcion);
                        comando.Parameters.AddWithValue("@imp", pago.Importe);
                        comando.Parameters.AddWithValue("@fech", pago.FechaPago);
                        comando.ExecuteNonQuery();
                        cadenaComando = "Select @@Identity";
                        comando = new SqlCommand(cadenaComando, cn, transaction);
                        pago.PagoId = (int)(decimal)comando.ExecuteScalar();

                        CtaCteBd.ActualizarPago(pago, cn, transaction);
                        if (tipoTransaccionId == 0)
                        {
                            tipoTransaccionId = 5;
                        }
                        decimal importeTransaccion = pago.Importe - pago.ImporteOS;
                        TransaccionesBd.Agregar(tipoTransaccionId, pago.Cliente.ClienteId, importeTransaccion, cn, transaction, pago.Descripcion);

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

        public static Pago GetObjeto(int id)
        {
            Pago pago = null;
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "SELECT PagoId, ID_CLIENTE, CtaCteId, Descripcion, FormaDePagoId, Importe, FechaPago FROM Pagos WHERE PagoId=@id";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        pago = new Pago()
                        {
                            PagoId = reader.GetInt32(0),
                            Cliente = ClientesBd.GetObjeto(reader.GetInt32(1)),
                            CtaCte = CtaCteBd.GetObjeto(reader.GetInt32(2)),
                            Descripcion = reader.GetString(3),
                            FormaDePago = FormasDePagoBd.GetObjeto(reader.GetInt32(4)),
                            Importe = reader.GetDecimal(5),
                            FechaPago = reader.GetDateTime(6)
                        };
                    }
                    reader.Close();
                    return pago;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Borrar(Pago pago)
        {
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                try
                {
                    using (var transaction = cn.BeginTransaction())
                    {
                        try
                        {

                            string cadenaComando = "DELETE FROM Pagos WHERE ID_PAGO=@id";
                            SqlCommand comando = new SqlCommand(cadenaComando, cn, transaction);
                            comando.Parameters.AddWithValue("@id", pago.PagoId);
                            comando.ExecuteNonQuery();

                            CtaCteBd.Borrar(pago.CtaCte.CtaCteId, cn, transaction);
                            ConsultaCtaCte cons = new ConsultaCtaCte()
                            {
                                Cliente = pago.Cliente,
                                Saldo = pago.CtaCte.Saldo
                            };
                            if (pago.Descripcion.Contains("ALQUILER") || pago.Descripcion.Contains("VENTA") || pago.Descripcion.Contains("ORDEN") || pago.Descripcion.Contains("RENOVA") || pago.Descripcion.Contains("PAGO"))
                            {
                                string[] mov = pago.Descripcion.Split('-');
                                TransaccionesBd.Borrar(mov[0], int.Parse(mov[1]));
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
                catch (Exception ex)
                {

                    throw ex;
                }


            }
        }

        public static void Editar(Pago pago, int idAEditar, int idCta)
        {
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                using (var transaction = cn.BeginTransaction())
                {
                    try
                    {
                        string cadenaComando = "UPDATE PAGOS Set ID_CLIENTE=@CLI, CTACTEID=@CTAID, IMPORTE=@IMP, NOTAS=@NOT, FECHAPAGO=@FEC WHERE ID_PAGO=@id";
                        SqlCommand comando = new SqlCommand(cadenaComando, cn, transaction);
                        comando.Parameters.AddWithValue("@ID", idAEditar);
                        comando.Parameters.AddWithValue("@CLI", pago.Cliente.ClienteId);
                        comando.Parameters.AddWithValue("@CTAID", idCta);
                        comando.Parameters.AddWithValue("@IMP", pago.Importe);
                        comando.Parameters.AddWithValue("@NOT", pago.Descripcion);
                        comando.Parameters.AddWithValue("@FEC", pago.FechaPago);
                        comando.ExecuteNonQuery();
                        
                        CtaCteBd.Editar(pago.CtaCte);

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

        public static List<Pago> GetPagos(Cliente cliente)
        {
            List<Pago> lista = new List<Pago>();
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando =
                        "SELECT * FROM Pagos WHERE ID_CLIENTE=@id order by FechaPago desc";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@id", cliente.ClienteId);
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        Pago p = new Pago();
                        p.PagoId = reader.GetInt32(0);
                        p.Cliente = ClientesBd.GetObjeto(reader.GetInt32(1));
                        p.CtaCte = CtaCteBd.GetObjeto(reader.GetInt32(2));
                        p.Importe = reader.GetDecimal(3);
                        p.Descripcion = reader.GetString(4);
                        p.FechaPago = reader.GetDateTime(5);
                        lista.Add(p);
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {   
                throw ex;
            }
        }

        public static void Borrar(string mov, int id)
        {

            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                using (SqlTransaction transaction = cn.BeginTransaction())
                {
                    try
                    {
                        string nota = $"{mov}-{id}";
                        string strComando = $"DELETE FROM PAGOS WHERE NOTAS=@nota";
                        SqlCommand comando = new SqlCommand(strComando, cn, transaction);
                        comando.Parameters.AddWithValue("@nota", nota);
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

        internal static void BorrarPorCtaCteId(int ctaCteId)
        {
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                try
                {
                    using (var transaction = cn.BeginTransaction())
                    {
                        try
                        {

                            string cadenaComando = "DELETE FROM Pagos WHERE CtaCteID=@id";
                            SqlCommand comando = new SqlCommand(cadenaComando, cn, transaction);
                            comando.Parameters.AddWithValue("@id", ctaCteId);
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
                catch (Exception ex)
                {

                    throw ex;
                }

            }
        }

        internal static void BorrarPorCliente(int clienteId)
        {
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                try
                {
                    using (var transaction = cn.BeginTransaction())
                    {
                        try
                        {

                            string cadenaComando = "DELETE FROM Pagos WHERE ID_CLIENTE=@id";
                            SqlCommand comando = new SqlCommand(cadenaComando, cn, transaction);
                            comando.Parameters.AddWithValue("@id", clienteId);
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
                catch (Exception ex)
                {

                    throw ex;
                }


            }
        }

        public static int GetUltimo(int clienteId = 0)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "";
                    if (clienteId == 0)
                    {
                        cadenaComando = "Select max(ID_PAGO) From Pagos";
                    }
                    else
                    {
                        cadenaComando = "Select max(ID_PAGO) From Pagos WHERE ID_Cliente=@id";
                    }
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@id", clienteId);
                    object value = comando.ExecuteScalar();
                    if (int.TryParse(value.ToString(), out int id))
                    {
                        id = (int)comando.ExecuteScalar(); 
                    }
                    else
                    {
                        id = 0;
                    }
                    return id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
           

      

        
    }
}
