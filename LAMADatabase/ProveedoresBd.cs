using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LAMAModels;

namespace LAMADatabase
{
    public class ProveedoresBd
    {
        public static List<Proveedor> GetLista()
        {
            List<Proveedor> lista = new List<Proveedor>();
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();

                    string strComando = "SELECT ID_PROVEEDOR, ID_LOCALIDAD, RAZON_SOCIAL, DIRECCION," +
                        " TELEFONO, EMAIL, WEB FROM Proveedores ORDER BY RAZON_SOCIAL";
                    SqlCommand comando = new SqlCommand(strComando, cn);
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        Proveedor p = new Proveedor();
                        p.ProveedorId = reader.GetInt32(0);
                        p.Localidad = LocalidadesBd.GetObjeto(reader.GetInt32(1));
                        p.RazonSocial = reader.GetString(2);
                        p.Direccion = reader.GetString(3);
                        p.Telefono = reader.GetString(4);
                        p.Email = reader.GetString(5);
                        p.Web = reader.GetString(6);
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

        public static Proveedor GetObjeto(int id)
        {
            Proveedor p = null;
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string strComando = "SELECT ID_PROVEEDOR, ID_LOCALIDAD, RAZON_SOCIAL, DIRECCION," +
                        " TELEFONO, EMAIL, WEB FROM Proveedores WHERE ID_PROVEEDOR=@id";
                    SqlCommand comando = new SqlCommand(strComando, cn);
                    comando.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = comando.ExecuteReader(CommandBehavior.SingleResult);
                    if (reader.HasRows)
                    {
                        reader.Read();
                        p = new Proveedor();
                        p.ProveedorId = reader.GetInt32(0);
                        p.Localidad = LocalidadesBd.GetObjeto(reader.GetInt32(1));
                        p.RazonSocial = reader.GetString(2);
                        p.Direccion = reader.GetString(3);
                        p.Telefono = reader.GetString(4);
                        p.Email = reader.GetString(5);
                        p.Web = reader.GetString(6);
                    }
                }
                return p;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static void Borrar(Proveedor p)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string strComando = "DELETE FROM Proveedores WHERE ID_PROVEEDOR=@id";
                    SqlCommand comando = new SqlCommand(strComando, cn);
                    comando.Parameters.AddWithValue("@id", p.ProveedorId);
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
        public static void Editar(Proveedor p)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "UPDATE Proveedores Set ID_LOCALIDAD = @loc, RAZON_SOCIAL = @raz, DIRECCION = @dir," +
                        " TELEFONO = @tel, EMAIL = @ema, WEB = @web WHERE ProveedorId=@id";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);

                    comando.Parameters.AddWithValue("@loc", p.Localidad.LocalidadId);

                    if (p.RazonSocial == string.Empty)
                    {
                        comando.Parameters.AddWithValue("@raz", DBNull.Value);
                    }
                    else
                    {
                        comando.Parameters.AddWithValue("@raz", p.Email);
                    }

                    if (p.Direccion == string.Empty)
                    {
                        comando.Parameters.AddWithValue("@dir", DBNull.Value);
                    }
                    else
                    {
                        comando.Parameters.AddWithValue("@dir", p.Email);
                    }


                    if (p.Telefono == string.Empty)
                    {
                        comando.Parameters.AddWithValue("@tel", DBNull.Value);
                    }
                    else
                    {
                        comando.Parameters.AddWithValue("@tel", p.Email);
                    }

                    if (p.Email == string.Empty)
                    {
                        comando.Parameters.AddWithValue("@ema", DBNull.Value);
                    }
                    else
                    {
                        comando.Parameters.AddWithValue("@ema", p.Email);
                    }


                    if (p.Web == string.Empty)
                    {
                        comando.Parameters.AddWithValue("@web", DBNull.Value);
                    }
                    else
                    {
                        comando.Parameters.AddWithValue("@web", p.Email);
                    }

                    comando.Parameters.AddWithValue("@id", p.ProveedorId);
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
        public static void Agregar(Proveedor p)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string strComando = "INSERT INTO Proveedores (ID_LOCALIDAD, RAZON_SOCIAL, Direccion, Telefono, Email, Web) " +
                        " VALUES (@loc, @razon, @calle, @tel, @Email, @web)";

                    SqlCommand comando = new SqlCommand(strComando, cn);
                    comando.Parameters.AddWithValue("@loc", p.Localidad.LocalidadId);
                    comando.Parameters.AddWithValue("@razon", p.RazonSocial);
                    comando.Parameters.AddWithValue("@tel", p.Telefono);
                    comando.Parameters.AddWithValue("@calle", p.Direccion);
                    comando.Parameters.AddWithValue("@Email", p.Email);
                    comando.Parameters.AddWithValue("@web", p.Web);
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

        public static void CargarCombo(ref ComboBox cboProveedores)
        {
            List<Proveedor> lista = ProveedoresBd.GetLista();
            Proveedor defaultProveedor = new Proveedor
            {
                RazonSocial = "<Seleccione Proveedor>"
            };
            lista.Insert(0, defaultProveedor);
            cboProveedores.DataSource = lista;
            cboProveedores.DisplayMember = "RazonSocial";
            cboProveedores.ValueMember = "ProveedorId";
            cboProveedores.SelectedIndex = 0;
        }
    }
}
