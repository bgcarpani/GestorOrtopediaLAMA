using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using LAMAModels;

namespace LAMADatabase
{
    public class ProvinciasBd
    {
        public static List<Provincia> GetLista()
        {
            //Creo una lista de provincias
            var lista = new List<Provincia>();
            //Bloque using hace el dispose de la conexion
            //cuando no se utiliza 
            using (var conexion = ConexionBd.GetConexion())
            {
                //Abro la conexion
                conexion.Open();
                //Creo una variable de tipo string con el comando
                //de sql que quiero ejecutar
                string cadenaComando = "SELECT ID_Provincia, Provincia FROM Provincias";
                //Creo el comando pasando el string con la instrccion SQL y la conexion
                SqlCommand comando = new SqlCommand(cadenaComando, conexion);
                //Creo un obj sqldatareader para almacenar
                //los registros de la tabla de Provincias
                SqlDataReader reader = comando.ExecuteReader();
                //Mientras tenga registros para leer
                while (reader.Read())
                {
                    //Creo un objeto Provincia
                    Provincia provincia = new Provincia
                    {
                        //Leo el primer campo del reader que es de tipo Int
                        //y lo asigno a la prop ProvinciaId
                        ProvinciaId = reader.GetInt32(0),
                        NombreProvincia = reader.GetString(1),
                    };
                    //Agrego la provincia a la lista
                    lista.Add(provincia);
                }
                //retorno la lista
                return lista;



            }
        }


        public static void Agregar(Provincia provincia)
        {
            try
            {
                using (var conexion = ConexionBd.GetConexion())
                {
                    conexion.Open();
                    //Creo el string del comando con los parametros de los campos
                    //a guardar
                    string cadenaComando = "INSERT INTO Provincias (Provincia) VALUES (@Nombre)";
                    SqlCommand comando = new SqlCommand(cadenaComando, conexion);
                    //Agrego el parametro con un valor asignado que corresponde al 
                    //atributo NombreProvincia del objeto Provincia
                    comando.Parameters.AddWithValue("@Nombre", provincia.NombreProvincia);
                    //Ejecuto el comando
                    comando.ExecuteNonQuery();
                    cadenaComando = "SELECT @@IDENTITY";
                    comando = new SqlCommand(cadenaComando, conexion);
                    int id = (int)(decimal)comando.ExecuteScalar();
                    provincia.ProvinciaId = id;

                }

            }
            catch (Exception ex)
            {

                if (ex.Message.Contains("IX_Provincias_NombreProvincia"))
                {
                    throw new Exception("Provincia existente");
                }
            }
        }

        public static void Editar(Provincia provincia)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "UPDATE Provincias SET Provincia=@prov WHERE ID_Provincia=@id";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@prov", provincia.NombreProvincia);
                    comando.Parameters.AddWithValue("@id", provincia.ProvinciaId);
                    int cantidad=comando.ExecuteNonQuery();
                    if (cantidad == 0)
                    {
                        throw new Exception("Registro no encontrado o modificado por otro usuario");
                    }
                }

            }
            catch (Exception ex)
            {

                if (ex.Message.Contains("IX_Provincias_NombreProvincia"))
                {
                    throw new Exception("Provincia existente");
                }
                throw ex;
            }
        }

        public static void Borrar(Provincia provincia)
        {
            try
            {
                using (SqlConnection cn=ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "DELETE FROM Provincias WHERE ID_Provincia=@id";
                    SqlCommand comando=new SqlCommand(cadenaComando,cn);
                    comando.Parameters.AddWithValue("@id", provincia.ProvinciaId);
                    comando.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public static Provincia GetObjeto(int id)
        {
            Provincia provincia = null;
            try
            {
                using (SqlConnection cn=ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "SELECT ID_Provincia, Provincia FROM Provincias WHERE ID_Provincia=@id";
                    SqlCommand comando=new SqlCommand(cadenaComando,cn);
                    comando.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = comando.ExecuteReader(CommandBehavior.SingleRow);
                    if (reader.HasRows)
                    {
                        reader.Read();
                        provincia=new Provincia();
                        provincia.ProvinciaId = reader.GetInt32(0);
                        provincia.NombreProvincia = reader.GetString(1);

                    }
                    return provincia;

                }
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public static void CargarCombo(ref ComboBox cbo)
        {
            var provincias = GetLista();
            var defaultProvincia = new Provincia() {NombreProvincia = "<Seleccione Provincia>"};
            provincias.Insert(0,defaultProvincia);
            cbo.DataSource = provincias;
            cbo.DisplayMember = "NombreProvincia";
            cbo.ValueMember = "ProvinciaId";
            cbo.SelectedIndex = 0;

        }
    }
}
