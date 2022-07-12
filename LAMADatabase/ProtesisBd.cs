using LAMAModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAMADatabase
{
    public class ProtesisBd
    {
        public static Protesis GetObjeto(int id)
        {
            Protesis p = null;
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComnando = "SELECT * FROM Protesis WHERE ID_Protesis=@id";
                    SqlCommand comando = new SqlCommand(cadenaComnando, cn);
                    comando.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        p = new Protesis();
                        p.ProtesisId = reader.GetInt32(0);
                        p.Tipo = reader.GetString(1);
                        p.Descripcion = reader.GetString(2);
                        p.Importe = reader.GetDecimal(3);
                    }

                    return p;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int Agregar(Protesis protesis)
        {
            try
            {
                using (var conexion = ConexionBd.GetConexion())
                {
                    conexion.Open();
                    string cadenaComando = "INSERT INTO Protesis (TIPO, DESCRIPCION, IMPORTE) VALUES (@tip, @desc, @imp)";
                    SqlCommand comando = new SqlCommand(cadenaComando, conexion);
                    comando.Parameters.AddWithValue("@tip", protesis.Tipo);
                    comando.Parameters.AddWithValue("@desc", protesis.Descripcion);
                    comando.Parameters.AddWithValue("@imp", protesis.Importe);
                    comando.ExecuteNonQuery();
                    cadenaComando = "SELECT @@IDENTITY";
                    comando = new SqlCommand(cadenaComando, conexion);
                    int id = (int)(decimal)comando.ExecuteScalar();
                    protesis.ProtesisId = id;
                    return id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Eliminar(Protesis protesis)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string strComando = "DELETE FROM Protesis WHERE ID_Protesis=@id";
                    SqlCommand comando = new SqlCommand(strComando, cn);
                    comando.Parameters.AddWithValue("@id", protesis.ProtesisId);
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

        public static void CargarCombo(ref ComboBox cbo)
        {
            List<Protesis> prot = GetLista();
            Protesis defaultCliente = new Protesis()
            {
                Descripcion = "<Seleccione una protesis>",
            };
            prot.Insert(0, defaultCliente);
            cbo.DataSource = prot;
            cbo.ValueMember = "ProtesisId";
            cbo.DisplayMember = "NombreProtesis";
            cbo.SelectedIndex = 0;
        }

        public static List<Protesis> GetLista(int id = 0)
        {
            List<Protesis> lista = new List<Protesis>();
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string strComando = "";
                    if (id != 0)
                    {
                        strComando = "SELECT * FROM Protesis WHERE ID_Protesis = @id order by Tipo";
                    }
                    else
                    {
                        strComando = "SELECT * FROM Protesis order by Tipo";
                    }
                    SqlCommand comando = new SqlCommand(strComando, cn);
                    comando.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        Protesis p = new Protesis();
                        p.ProtesisId = reader.GetInt32(0);
                        p.Tipo = reader.GetString(1);
                        p.Descripcion = reader.GetString(2);
                        p.Importe = reader.GetDecimal(3);
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

        public static void Borrar(Protesis prot)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "DELETE FROM Protesis WHERE ID_Protesis=@id";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@id", prot.ProtesisId);
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

        public static void Editar(Protesis p)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string strComando = "UPDATE Protesis SET Tipo = @tipo, Descripcion = @desc, Importe = @cost WHERE ID_Protesis=@id";
                    SqlCommand comando = new SqlCommand(strComando, cn);
                    comando.Parameters.AddWithValue("@tipo", p.Tipo);
                    comando.Parameters.AddWithValue("@desc", p.Descripcion);
                    comando.Parameters.AddWithValue("@cost", p.Importe);
                    comando.Parameters.AddWithValue("@id", p.ProtesisId);
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

        public static void CargarCombo(ref ComboBox cbo, int protesisId)
        {
            List<Protesis> prot = GetLista(protesisId);
            Protesis defaultCliente = new Protesis()
            {
                Descripcion = "<Seleccione una protesis>",
            };
            prot.Insert(0, defaultCliente);
            cbo.DataSource = prot;
            cbo.ValueMember = "ProtesisId";
            cbo.DisplayMember = "NombreProtesis";
            cbo.SelectedIndex = 0;
        }
    }
}
