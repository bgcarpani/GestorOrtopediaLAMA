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
    public class AlicuotasIvaBd
    {
        public static List<AlicuotaIva> GetLista()
        {
            var lista = new List<AlicuotaIva>();
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                string cadenaComando = "SELECT AlicuotaIvaId, Descripcion, Porcentaje, RowVersion FROM AlicuotasIva";
                SqlCommand comando = new SqlCommand(cadenaComando, cn);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    var alicuotaIva = new AlicuotaIva
                    {
                        AlicuotaIvaId = reader.GetInt32(0),
                        Descripcion = reader.GetString(1),
                        Porcentaje =(float) reader.GetDouble(2),
                        RowVersion = (byte[])reader[3]

                    };
                    lista.Add(alicuotaIva);
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
                    string cadenaComando = "DELETE FROM AlicuotasIva WHERE AlicuotaIvaId=@id";
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

        public static void Agregar(AlicuotaIva alicuotaIva)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "INSERT INTO AlicuotasIva (Descripcion, Porcentaje) VALUES (@desc, @porc)";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@desc", alicuotaIva.Descripcion);
                    comando.Parameters.AddWithValue("@porc", alicuotaIva.Porcentaje);

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
                    alicuotaIva.AlicuotaIvaId = id;
                }

            }
            catch (Exception ex)
            {

                if (ex.Message.Contains("IX_AlicuotasIva_Descripcion"))
                {
                    throw new Exception("AlicuotaIva repetida");
                }
            }
        }

        public static void Editar(AlicuotaIva alicuotaIva)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "UPDATE AlicuotasIva SET Descripcion=@desc, Porcentaje=@porc WHERE AlicuotaIvaId=@id AND RowVersion=@row";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@desc", alicuotaIva.Descripcion);
                    comando.Parameters.AddWithValue("@porc", alicuotaIva.Porcentaje);
                    comando.Parameters.AddWithValue("@id", alicuotaIva.AlicuotaIvaId);
                    comando.Parameters.AddWithValue("@row", alicuotaIva.RowVersion);
                    int cantidad = comando.ExecuteNonQuery();
                    if (cantidad == 0)
                    {
                        throw new Exception("Registro no encontrado o modificado por otro usuario");
                    }
                    else
                    {
                        cadenaComando = "SELECT RowVersion FROM AlicuotasIva WHERE AlicuotaIvaId=@id";
                        comando = new SqlCommand(cadenaComando, cn);
                        comando.Parameters.AddWithValue("@id", alicuotaIva.AlicuotaIvaId);
                        alicuotaIva.RowVersion = (byte[])comando.ExecuteScalar();
                    }

                }

            }
            catch (Exception ex)
            {

                if (ex.Message.Contains("IX_AlicuotasIva_Descripcion"))
                {
                    throw new Exception("AlicuotaIva repetida");
                }

            }
        }

        public static AlicuotaIva GetObjeto(int alicuotaIvaId)
        {
            AlicuotaIva alicuotaIva = null;
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                string cadenaComando = "SELECT AlicuotaIvaId, Descripcion,Porcentaje, RowVersion FROM AlicuotasIva WHERE AlicuotaIvaId=@id";
                SqlCommand comando = new SqlCommand(cadenaComando, cn);
                comando.Parameters.AddWithValue("@id", alicuotaIvaId);
                SqlDataReader reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    alicuotaIva = new AlicuotaIva
                    {
                        AlicuotaIvaId = reader.GetInt32(0),
                        Descripcion = reader.GetString(1),
                        Porcentaje =(float) reader.GetDouble(2),
                        RowVersion = (byte[])reader[3]

                    };

                }
            }
            return alicuotaIva;
        }

        public static void CargarCombo(ref ComboBox cbo)
        {
            var alicuotasIva = AlicuotasIvaBd.GetLista();
            var defaultAlicuotaIva = new AlicuotaIva() { Descripcion = "<Seleccione AlicuotaIva>" };
            alicuotasIva.Insert(0, defaultAlicuotaIva);
            cbo.DataSource = alicuotasIva;
            cbo.DisplayMember = "Descripcion";
            cbo.ValueMember = "AlicuotaIvaId";
            cbo.SelectedIndex = 0;

        }
    }
}
