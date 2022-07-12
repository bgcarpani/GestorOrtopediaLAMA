using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LAMAModels;

namespace LAMADatabase
{
    public class LocalidadesBd
    {
        public static List<Localidad> GetLista()
        {
            var lista=new List<Localidad>();
            try
            {
                using (SqlConnection cn=ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "SELECT LocalidadId, Descripcion, ProvinciaId FROM Localidades";
                    SqlCommand comando=new SqlCommand(cadenaComando,cn);
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        Localidad localidad=new Localidad();
                        localidad.LocalidadId = reader.GetInt32(0);
                        localidad.Descripcion = reader.GetString(1);
                        localidad.Provincia = ProvinciasBd.GetObjeto(reader.GetInt32(2));

                        lista.Add(localidad);
                    }
                }
                return lista;

            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public static void Agregar(Localidad localidad)
        {
            try
            {
                using (SqlConnection cn=ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "INSERT INTO Localidades (Descripcion,ProvinciaId) VALUES (@nomb, @prov)";
                    SqlCommand comando=new SqlCommand(cadenaComando,cn);
                    comando.Parameters.AddWithValue("@nomb", localidad.Descripcion);
                    comando.Parameters.AddWithValue("@prov", localidad.Provincia.ProvinciaId);
                    comando.ExecuteNonQuery();
                    cadenaComando = "SELECT @@IDENTITY";
                    comando=new SqlCommand(cadenaComando,cn);
                    int id = (int) (decimal) comando.ExecuteScalar();
                    localidad.LocalidadId = id;
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_Localidades_Descripcion_ProvinciaId"))
                {
                    throw new Exception("Localidad repetida");
                }
                
            }
        }

        public static List<Localidad> GetLista(Provincia provincia)
        {
            var lista = new List<Localidad>();
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "SELECT LocalidadId, Descripcion, ProvinciaId FROM Localidades WHERE ProvinciaId=@id";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@id", provincia.ProvinciaId);
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        Localidad localidad = new Localidad
                        {
                            LocalidadId = reader.GetInt32(0),
                            Descripcion = reader.GetString(1),
                            Provincia = ProvinciasBd.GetObjeto(reader.GetInt32(2)),
                        };

                        lista.Add(localidad);
                    }
                }
                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public static List<object> GetResumen()
        {
            var lista = new List<object>();
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    //string cadenaComando = "SELECT  NombreProvincia ,COUNT(Descripcion) AS Cantidad  FROM Localidades, Provincias WHERE Localidades.ProvinciaId=Provincias.ProvinciaId GROUP BY NombreProvincia";
                    string cadenaComando =
                        "SELECT NombreProvincia, COUNT(Descripcion) as Cantidad FROM Localidades RIGHT JOIN Provincias ON Localidades.ProvinciaId=Provincias.ProvinciaId GROUP BY NombreProvincia ORDER BY Cantidad Desc";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        lista.Add(
                            new
                            {
                                NombreProvincia=reader.GetString(0),
                                Cantidad=reader.GetInt32(1)
                            });
                    }
                }
                return lista;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public static Localidad GetObjeto(int localidadId)
        {
            Localidad localidad = null;
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "SELECT LocalidadId, Descripcion, ProvinciaId FROM Localidades WHERE LocalidadId=@id";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@id", localidadId);
                    SqlDataReader reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        localidad = new Localidad
                        {
                            LocalidadId = reader.GetInt32(0),
                            Descripcion = reader.GetString(1),
                            Provincia = ProvinciasBd.GetObjeto(reader.GetInt32(2)),
                        };

                    }
                   
                }
                return localidad;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        public static void Editar(Localidad localidad)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "UPDATE Localidades SET Descripcion=@loc, ProvinciaId=@prov WHERE LocalidadId=@id";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@loc", localidad.Descripcion);

                    comando.Parameters.AddWithValue("@prov", localidad.Provincia.ProvinciaId);
                    comando.Parameters.AddWithValue("@id", localidad.LocalidadId);
                    int cantidad = comando.ExecuteNonQuery();
                    if (cantidad == 0)
                    {
                        throw new Exception("Registro no encontrado");
                    }
                }

            }
            catch (Exception ex)
            {

                if (ex.Message.Contains("IX_Localidades_Descripcion"))
                {
                    throw new Exception("Localidad existente");
                }
                throw ex;
            }
        }

        public static void Borrar(Localidad localidad)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "DELETE FROM Localidades WHERE LocalidadId=@id";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@id", localidad.LocalidadId);
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

        public static void CargarCombo(ref ComboBox cbo, Provincia provincia)
        {
            var lista = LocalidadesBd.GetLista(provincia);
            var defaultLocalidad = new Localidad() { Descripcion = "<Seleccione Localidad>" };
            lista.Insert(0, defaultLocalidad  );
            cbo.DataSource = lista;
            cbo.DisplayMember = "Descripcion";
            cbo.ValueMember = "LocalidadId";
            cbo.SelectedIndex = 0;

        }
    }
}
