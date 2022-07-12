using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LAMAModels;

namespace LAMADatabase
{
    public class ClientesBd
    {
        public static List<Cliente> GetLista()
        {
            List<Cliente> lista = new List<Cliente>();
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string strComando = "SELECT * FROM Clientes order by Apellido, Nombre";
                    SqlCommand comando = new SqlCommand(strComando, cn);
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        Cliente p = new Cliente();
                        p.ClienteId = reader.GetInt32(0);
                        p.DNI = reader.GetString(1);
                        p.Domicilio = new Domicilio() { };
                        p.Domicilio.Provincia = ProvinciasBd.GetObjeto(reader.GetInt32(2));
                        p.Nombre = reader.GetString(3);
                        p.Apellido = reader.GetString(4);
                        p.Domicilio.Direccion = reader[5] == DBNull.Value ? string.Empty : reader.GetString(5);
                        p.Domicilio.CodigoPostal = reader[6] == DBNull.Value ? string.Empty : reader.GetString(6);
                        p.TelefonoFijo = reader[7] == DBNull.Value ? string.Empty : reader.GetString(7);
                        p.TelefonoTrabajo = reader[8] == DBNull.Value ? string.Empty : reader.GetString(8);
                        p.TelefonoMovil = reader[9] == DBNull.Value ? string.Empty : reader.GetString(9);
                        p.CorreoElectronico = reader[10] == DBNull.Value ? string.Empty : reader.GetString(10);
                        p.Observaciones = reader[11] == DBNull.Value ? string.Empty : reader.GetString(11);
                        p.Domicilio.Localidad = LocalidadesBd.GetObjeto(reader.GetInt32(12));
                        p.Eliminado = reader.GetBoolean(13);
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

        public static Cliente GetObjetoPorDNI(string text)
        {
            Cliente p = null;
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComnando = "SELECT * FROM Clientes WHERE DNI=@dni";
                    SqlCommand comando = new SqlCommand(cadenaComnando, cn);
                    comando.Parameters.AddWithValue("@dni", text);
                    SqlDataReader reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        p = new Cliente();
                        p.ClienteId = reader.GetInt32(0);
                        p.DNI = reader.GetString(1);
                        p.Domicilio = new Domicilio() { };
                        p.Domicilio.Provincia = ProvinciasBd.GetObjeto(reader.GetInt32(2));
                        p.Nombre = reader.GetString(3);
                        p.Apellido = reader.GetString(4);
                        p.Domicilio.Direccion = reader[5] == DBNull.Value ? string.Empty : reader.GetString(5);
                        p.Domicilio.CodigoPostal = reader[6] == DBNull.Value ? string.Empty : reader.GetString(6);
                        p.TelefonoFijo = reader[7] == DBNull.Value ? string.Empty : reader.GetString(7);
                        p.TelefonoTrabajo = reader[8] == DBNull.Value ? string.Empty : reader.GetString(8);
                        p.TelefonoMovil = reader[9] == DBNull.Value ? string.Empty : reader.GetString(9);
                        p.CorreoElectronico = reader[10] == DBNull.Value ? string.Empty : reader.GetString(10);
                        p.Observaciones = reader.GetString(11);
                        p.Domicilio.Localidad = LocalidadesBd.GetObjeto(reader.GetInt32(12));
                        p.Eliminado = reader.GetBoolean(13);
                    }

                    return p;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Cliente> GetLista(int id)
        {
            List<Cliente> lista = new List<Cliente>();
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string strComando = "SELECT * FROM Clientes WHERE ID_CLIENTE = @id ORDER BY Apellido, Nombre";
                    SqlCommand comando = new SqlCommand(strComando, cn);
                    comando.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        Cliente p = new Cliente();
                        p.ClienteId = reader.GetInt32(0);
                        p.DNI = reader.GetString(1);
                        p.Domicilio = new Domicilio() { };
                        p.Domicilio.Provincia = ProvinciasBd.GetObjeto(reader.GetInt32(2));
                        p.Nombre = reader.GetString(3);
                        p.Apellido = reader.GetString(4);
                        p.Domicilio.Direccion = reader[5] == DBNull.Value ? string.Empty : reader.GetString(5);
                        p.Domicilio.CodigoPostal = reader[6] == DBNull.Value ? string.Empty : reader.GetString(6);
                        p.TelefonoFijo = reader[7] == DBNull.Value ? string.Empty : reader.GetString(7);
                        p.TelefonoTrabajo = reader[8] == DBNull.Value ? string.Empty : reader.GetString(8);
                        p.TelefonoMovil = reader[9] == DBNull.Value ? string.Empty : reader.GetString(9);
                        p.CorreoElectronico = reader[10] == DBNull.Value ? string.Empty : reader.GetString(10);
                        p.Observaciones = reader.GetString(11);
                        p.Domicilio.Localidad = LocalidadesBd.GetObjeto(reader.GetInt32(12));
                        p.Eliminado = reader.GetBoolean(13);
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

        public static Cliente GetObjeto(int id)
        {
            Cliente p = null;
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComnando = "SELECT * FROM Clientes WHERE ID_Cliente=@id";
                    SqlCommand comando = new SqlCommand(cadenaComnando, cn);
                    comando.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        p = new Cliente();
                        p.ClienteId = reader.GetInt32(0);
                        p.DNI = reader.GetString(1);
                        p.Domicilio = new Domicilio() { };
                        p.Domicilio.Provincia = ProvinciasBd.GetObjeto(reader.GetInt32(2));
                        p.Nombre = reader.GetString(3);
                        p.Apellido = reader.GetString(4);
                        p.Domicilio.Direccion = reader[5] == DBNull.Value ? string.Empty : reader.GetString(5);
                        p.Domicilio.CodigoPostal = reader[6] == DBNull.Value ? string.Empty : reader.GetString(6);
                        p.TelefonoFijo = reader[7] == DBNull.Value ? string.Empty : reader.GetString(7);
                        p.TelefonoTrabajo = reader[8] == DBNull.Value ? string.Empty : reader.GetString(8);
                        p.TelefonoMovil = reader[9] == DBNull.Value ? string.Empty : reader.GetString(9);
                        p.CorreoElectronico = reader[10] == DBNull.Value ? string.Empty : reader.GetString(10);
                        p.Observaciones = reader.GetString(11);
                        p.Domicilio.Localidad = LocalidadesBd.GetObjeto(reader.GetInt32(12));
                        p.Eliminado = reader.GetBoolean(13);
                    }

                    return p;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void Borrar(Cliente p)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string strComando = "DELETE FROM Clientes WHERE ID_Cliente=@id";
                    SqlCommand comando = new SqlCommand(strComando, cn);
                    comando.Parameters.AddWithValue("@id", p.ClienteId);
                    comando.ExecuteNonQuery();

                    CtaCteBd.BorrarCompleta(p.ClienteId);
                    PagosBd.BorrarPorCliente(p.ClienteId);

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

        public static void Editar(Cliente p)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "UPDATE Clientes Set DNI=@dni, Apellido=@ape, Nombre=@nom, " +
                        "Domicilio=@dom, LocalidadId=@Loc," +
                        "ID_PROVINCIA=@prov, CP=@cp, TEL_Particular=@tel, TEL_TRABAJO=@trab, CELULAR=@mov, " +
                    "E_MAIL=@Email, NOTA=@obs, Eliminado=@elim WHERE ID_CLIENTE=@id";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@dni", p.DNI);
                    comando.Parameters.AddWithValue("@ape", p.Apellido);
                    comando.Parameters.AddWithValue("@nom", p.Nombre);
                    comando.Parameters.AddWithValue("@dom", p.Domicilio.Direccion);
                    comando.Parameters.AddWithValue("@loc", p.Domicilio.Localidad.LocalidadId);
                    comando.Parameters.AddWithValue("@prov", p.Domicilio.Provincia.ProvinciaId);
                    if (p.Domicilio.CodigoPostal == string.Empty)
                    {
                        comando.Parameters.AddWithValue("@cp", DBNull.Value);
                    }
                    else
                    {
                        comando.Parameters.AddWithValue("@cp", p.Domicilio.CodigoPostal);
                    }
                    if (p.TelefonoFijo == string.Empty)
                    {
                        comando.Parameters.AddWithValue("@tel", DBNull.Value);
                    }
                    else
                    {
                        comando.Parameters.AddWithValue("@tel", p.TelefonoFijo);
                    }
                    if (p.TelefonoTrabajo == string.Empty)
                    {
                        comando.Parameters.AddWithValue("@trab", DBNull.Value);
                    }
                    else
                    {
                        comando.Parameters.AddWithValue("@trab", p.TelefonoTrabajo);
                    }
                    if (p.TelefonoMovil == string.Empty)
                    {
                        comando.Parameters.AddWithValue("@mov", DBNull.Value);
                    }
                    else
                    {
                        comando.Parameters.AddWithValue("@mov", p.TelefonoMovil);
                    }

                    if (p.CorreoElectronico == string.Empty)
                    {
                        comando.Parameters.AddWithValue("@Email", DBNull.Value);
                    }
                    else
                    {
                        comando.Parameters.AddWithValue("@Email", p.CorreoElectronico);
                    }
                    if (p.Observaciones == string.Empty)
                    {
                        comando.Parameters.AddWithValue("@obs", DBNull.Value);
                    }
                    else
                    {
                        comando.Parameters.AddWithValue("@obs", p.Observaciones);
                    }
                    comando.Parameters.AddWithValue("@elim", p.Eliminado);
                    comando.Parameters.AddWithValue("@id", p.ClienteId);
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

        public static int AgregarTemporal(Cliente p)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string strComando = "INSERT INTO Clientes (DNI, Apellido, Nombre, Domicilio, " +
                        " LocalidadId, ID_Provincia, CP, CELULAR, Nota, Eliminado)" +
                        " VALUES(@dni, @ape, @nom, @dom, @loc, @prov, @cp, @mov, @notas, @elim)";
                    SqlCommand comando = new SqlCommand(strComando, cn);
                    comando.Parameters.AddWithValue("@dni", p.DNI);
                    if (p.Apellido == string.Empty)
                    {
                        comando.Parameters.AddWithValue("@ape", "");
                    }
                    else
                    {
                        comando.Parameters.AddWithValue("@ape", p.Apellido);
                    }
                    comando.Parameters.AddWithValue("@nom", p.Nombre);
                    if (p.Domicilio.Direccion == string.Empty)
                    {
                        comando.Parameters.AddWithValue("@dom", DBNull.Value);
                    }
                    else
                    {
                        comando.Parameters.AddWithValue("@dom", p.Domicilio.Direccion);
                    }

                    comando.Parameters.AddWithValue("@loc", p.Domicilio.Localidad.LocalidadId);
                    comando.Parameters.AddWithValue("@prov", p.Domicilio.Provincia.ProvinciaId);
                    if (p.Domicilio.CodigoPostal == string.Empty)
                    {
                        comando.Parameters.AddWithValue("@cp", DBNull.Value);
                    }
                    else
                    {
                        comando.Parameters.AddWithValue("@cp", p.Domicilio.CodigoPostal);
                    }
                    if (p.TelefonoMovil == string.Empty)
                    {
                        comando.Parameters.AddWithValue("@mov", DBNull.Value);
                    }
                    else
                    {
                        comando.Parameters.AddWithValue("@mov", p.TelefonoMovil);
                    }
                    comando.Parameters.AddWithValue("@notas", "");
                    comando.Parameters.AddWithValue("@elim", p.Eliminado);
                    comando.ExecuteNonQuery();
                    strComando = "SELECT @@IDENTITY";
                    comando = new SqlCommand(strComando, cn);
                    int id = (int)(decimal)comando.ExecuteScalar();
                    return id;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static int AgregarConReturn(Cliente p)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string strComando = "INSERT INTO Clientes (DNI, Apellido, Nombre, Domicilio, " +
                        " LocalidadId, ID_Provincia, CP, TEL_PARTICULAR, TEL_TRABAJO, CELULAR," +
                        " E_MAIL, NOTA, Eliminado) " +
                        " VALUES(@dni, @ape, @nom, @dom, @loc, @prov, @cp, @tel, @trab, @mov," +
                        "@Email, @obs, @elim)";
                    SqlCommand comando = new SqlCommand(strComando, cn);
                    comando.Parameters.AddWithValue("@dni", p.DNI);
                    comando.Parameters.AddWithValue("@ape", p.Apellido);
                    comando.Parameters.AddWithValue("@nom", p.Nombre);
                    if (p.Domicilio.Direccion == string.Empty)
                    {
                        comando.Parameters.AddWithValue("@dom", DBNull.Value);
                    }
                    else
                    {
                        comando.Parameters.AddWithValue("@dom", p.Domicilio.Direccion);

                    }

                    comando.Parameters.AddWithValue("@loc", p.Domicilio.Localidad.LocalidadId);
                    comando.Parameters.AddWithValue("@prov", p.Domicilio.Provincia.ProvinciaId);
                    if (p.Domicilio.CodigoPostal == string.Empty)
                    {
                        comando.Parameters.AddWithValue("@cp", DBNull.Value);
                    }
                    else
                    {
                        comando.Parameters.AddWithValue("@cp", p.Domicilio.CodigoPostal);
                    }
                    comando.Parameters.AddWithValue("@tel", " ");
                    comando.Parameters.AddWithValue("@trab", " ");
                    if (p.TelefonoMovil == string.Empty)
                    {
                        comando.Parameters.AddWithValue("@mov", DBNull.Value);
                    }
                    else
                    {
                        comando.Parameters.AddWithValue("@mov", p.TelefonoMovil);
                    }

                    if (p.CorreoElectronico == string.Empty)
                    {
                        comando.Parameters.AddWithValue("@Email", DBNull.Value);
                    }
                    else
                    {
                        comando.Parameters.AddWithValue("@Email", p.CorreoElectronico);
                    }
                    if (p.Observaciones == string.Empty)
                    {
                        comando.Parameters.AddWithValue("@obs", DBNull.Value);
                    }
                    else
                    {
                        comando.Parameters.AddWithValue("@obs", p.Observaciones);
                    }
                    comando.Parameters.AddWithValue("@elim", false);
                    comando.ExecuteNonQuery();
                    strComando = "SELECT @@IDENTITY";
                    comando = new SqlCommand(strComando, cn);
                    int id = (int)(decimal)comando.ExecuteScalar();
                    return id;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static void Agregar(Cliente p)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string strComando = "INSERT INTO Clientes (DNI, Apellido, Nombre, Domicilio, " +
                        " LocalidadId, ID_Provincia, CP, TEL_PARTICULAR, TEL_TRABAJO, CELULAR," +
                        " E_MAIL, NOTA, Eliminado) " +
                        " VALUES(@dni, @ape, @nom, @dom, @loc, @prov, @cp, @tel, @trab, @mov," +
                        "@Email, @obs, @elim)";
                    SqlCommand comando = new SqlCommand(strComando, cn);
                    comando.Parameters.AddWithValue("@dni", p.DNI);
                    comando.Parameters.AddWithValue("@ape", p.Apellido);
                    comando.Parameters.AddWithValue("@nom", p.Nombre);
                    if (p.Domicilio.Direccion == string.Empty)
                    {
                        comando.Parameters.AddWithValue("@dom", DBNull.Value);
                    }
                    else
                    {
                        comando.Parameters.AddWithValue("@dom", p.Domicilio.Direccion);

                    }

                    comando.Parameters.AddWithValue("@loc", p.Domicilio.Localidad.LocalidadId);
                    comando.Parameters.AddWithValue("@prov", p.Domicilio.Provincia.ProvinciaId);
                    if (p.Domicilio.CodigoPostal == string.Empty)
                    {
                        comando.Parameters.AddWithValue("@cp", DBNull.Value);
                    }
                    else
                    {
                        comando.Parameters.AddWithValue("@cp", p.Domicilio.CodigoPostal);
                    }
                    comando.Parameters.AddWithValue("@tel", " ");
                    comando.Parameters.AddWithValue("@trab", " ");
                    if (p.TelefonoMovil == string.Empty)
                    {
                        comando.Parameters.AddWithValue("@mov", DBNull.Value);
                    }
                    else
                    {
                        comando.Parameters.AddWithValue("@mov", p.TelefonoMovil);
                    }

                    if (p.CorreoElectronico == string.Empty)
                    {
                        comando.Parameters.AddWithValue("@Email", DBNull.Value);
                    }
                    else
                    {
                        comando.Parameters.AddWithValue("@Email", p.CorreoElectronico);
                    }
                    if (p.Observaciones == string.Empty)
                    {
                        comando.Parameters.AddWithValue("@obs", DBNull.Value);
                    }
                    else
                    {
                        comando.Parameters.AddWithValue("@obs", p.Observaciones);
                    }
                    comando.Parameters.AddWithValue("@elim", false);
                    comando.ExecuteNonQuery();
                    strComando = "SELECT @@IDENTITY";
                    comando = new SqlCommand(strComando, cn);
                    int id = (int)(decimal)comando.ExecuteScalar();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static void CargarCombo(ref ComboBox cboCliente)
        {
            List<Cliente> cliente = GetLista();
            cliente = cliente.Where(c => c.ClienteId != int.Parse(ConfigurationManager.ConnectionStrings["ConsumidorFinal"].ToString())).ToList();
            cliente = cliente.Where(c => c.Eliminado == false).ToList();
            Cliente defaultCliente = new Cliente()
            {
                Apellido = "<Seleccione un",
                Nombre = "cliente>"
            };
            cliente.Insert(0, defaultCliente);
            cboCliente.DataSource = cliente;
            cboCliente.ValueMember = "ClienteId";
            cboCliente.DisplayMember = "NombreCompleto";
            cboCliente.SelectedIndex = 0;
        }

        public static void CargarCombo(ref ComboBox cboCliente, int clienteId)
        {
            List<Cliente> cliente = GetLista(clienteId);
            Cliente defaultCliente = new Cliente()
            {
                Apellido = "<Seleccione un",
                Nombre = "cliente>"
            };
            cliente.Insert(0, defaultCliente);
            cboCliente.DataSource = cliente;
            cboCliente.ValueMember = "ClienteId";
            cboCliente.DisplayMember = "NombreCompleto";
            cboCliente.SelectedIndex = 0;
        }
    }
}
