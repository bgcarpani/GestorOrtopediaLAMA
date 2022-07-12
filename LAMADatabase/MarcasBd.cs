using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LAMAModels;

namespace LAMADatabase
{
    public class MarcasBd
    {
        public static List<Marca> GetLista()
        {
            var lista = new List<Marca>();
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                string cadenaComando = "SELECT MarcaId, Descripcion FROM Marcas";
                SqlCommand comando = new SqlCommand(cadenaComando, cn);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    var marca = new Marca
                    {
                        MarcaId = reader.GetInt32(0),
                        Descripcion = reader.GetString(1)
                        
                    };
                    lista.Add(marca);
                }
            }
            return lista;
        }

        public static void Borrar(int id)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "DELETE FROM Marcas WHERE MarcaId=@id";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@id", id);
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

        public static void Agregar(Marca marca)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "INSERT INTO Marcas (Descripcion) VALUES (@desc)";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@desc", marca.Descripcion);
                    comando.ExecuteNonQuery();
                    //Creo la cadena de comando para obtener el id del
                    //registro agregado recientemente
                    cadenaComando = "SELECT @@IDENTITY";
                    comando = new SqlCommand(cadenaComando, cn);
                    //Ejecuto un comando Scalar que lo que hace es obtener 
                    //el valor de la primera columna de la tabla.
                    //Luego hago un doble casting para pasarlo a entero
                    int id = (int)(decimal)comando.ExecuteScalar();
                    //Asigno el valor obtenido al atributo de la clase
                    marca.MarcaId = id;
                }

            }
            catch (Exception ex)
            {

                if (ex.Message.Contains("IX_Marcas_Descripcion"))
                {
                    throw new Exception("Marca repetida");
                }
            }
        }

        public static void Editar(Marca marca)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "UPDATE Marcas SET Descripcion=@desc WHERE MarcaId=@id AND RowVersion=@row";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@desc", marca.Descripcion);
                    comando.Parameters.AddWithValue("@id", marca.MarcaId);
                    int cantidad=comando.ExecuteNonQuery();
                    if (cantidad == 0)
                    {
                        throw new Exception("Registro no encontrado o modificado por otro usuario");
                    }
                    else
                    {
                        cadenaComando = "SELECT RowVersion FROM Marcas WHERE MarcaId=@id";
                        comando = new SqlCommand(cadenaComando, cn);
                        comando.Parameters.AddWithValue("@id", marca.MarcaId);
                    }

                }

            }
            catch (Exception ex)
            {

                if (ex.Message.Contains("IX_Marcas_Descripcion"))
                {
                    throw new Exception("Marca repetida");
                }
                
            }
        }

        public static Marca GetObjeto(int marcaId)
        {
            Marca marca=null;
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                string cadenaComando = "SELECT MarcaId, Descripcion FROM Marcas WHERE MarcaId=@id";
                SqlCommand comando = new SqlCommand(cadenaComando, cn);
                comando.Parameters.AddWithValue("@id", marcaId);
                SqlDataReader reader = comando.ExecuteReader();
                if(reader.HasRows)
                {
                    reader.Read();
                    marca = new Marca
                    {
                        MarcaId = reader.GetInt32(0),
                        Descripcion = reader.GetString(1),

                    };
                    
                }
            }
            return marca;
        }

        public static void CargarCombo(ref ComboBox cbo)
        {
            var marcas = MarcasBd.GetLista();
            var defaultMarca = new Marca() { Descripcion = "<Seleccione Marca>" };
            marcas.Insert(0, defaultMarca);
            cbo.DataSource = marcas;
            cbo.DisplayMember = "Descripcion";
            cbo.ValueMember = "MarcaId";
            cbo.SelectedIndex = 0;

        }
    }
}
