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
    public class TiposBd
    {
        public static void CargarCombo(ref ComboBox cbo)
        {
            var tipos = TiposBd.GetLista();
            var defaultMarca = new Tipo() { Descripcion = "<Seleccione Tipo>" };
            tipos.Insert(0, defaultMarca);
            cbo.DataSource = tipos;
            cbo.DisplayMember = "Descripcion";
            cbo.ValueMember = "TipoId";
            cbo.SelectedIndex = 0;
        }

        public static List<Tipo> GetLista()
        {
            var lista = new List<Tipo>();
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                string cadenaComando = "SELECT ID_Tipo, Tipo FROM Tipos";
                SqlCommand comando = new SqlCommand(cadenaComando, cn);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    var tipo = new Tipo
                    {
                        TipoId = reader.GetInt32(0),
                        Descripcion = reader.GetString(1)

                    };
                    lista.Add(tipo);
                }
            }
            return lista;
        }

        public static void Agregar(Tipo tipo)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "INSERT INTO Tipos (Tipo) VALUES (@desc)";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@desc", tipo.Descripcion);
                    comando.ExecuteNonQuery();
                    cadenaComando = "SELECT @@IDENTITY";
                    comando = new SqlCommand(cadenaComando, cn);
                    int id = (int)(decimal)comando.ExecuteScalar();
                    tipo.TipoId = id;
                }

            }
            catch (Exception ex)
            {

                if (ex.Message.Contains("IX_Tipos_Tipo"))
                {
                    throw new Exception("Tipo repetida");
                }
            }
        }

        public static Tipo GetObjeto(int tipoId)
        {
            Tipo tipo = null;
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                string cadenaComando = "SELECT ID_Tipo, Tipo FROM Tipos WHERE ID_Tipo=@id";
                SqlCommand comando = new SqlCommand(cadenaComando, cn);
                comando.Parameters.AddWithValue("@id", tipoId);
                SqlDataReader reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    tipo = new Tipo
                    {
                        TipoId = reader.GetInt32(0),
                        Descripcion = reader.GetString(1),

                    };

                }
            }
            return tipo;
        }

    }
}
