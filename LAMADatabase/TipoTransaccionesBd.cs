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
    public class TipoTransaccionesBd
    {
        public static void CargarCombo(ref ComboBox cbo)
        {
            var tipos = GetLista();
            var defaultMarca = new TipoTransaccion() { Descripcion = "<Seleccione una opción>" };
            tipos.Insert(0, defaultMarca);
            cbo.DataSource = tipos;
            cbo.DisplayMember = "Descripcion";
            cbo.ValueMember = "TipoTransaccionId";
            cbo.SelectedIndex = 1;
        }

        private static List<TipoTransaccion> GetLista()
        {
            var lista = new List<TipoTransaccion>();
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                string cadenaComando = "SELECT * FROM TiposTransacciones order by ID_TipoTransaccion desc";
                SqlCommand comando = new SqlCommand(cadenaComando, cn);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    var tipo = new TipoTransaccion
                    {
                        TipoTransaccionId = reader.GetInt32(0),
                        Descripcion = reader.GetString(1)

                    };
                    lista.Add(tipo);
                }
            }
            return lista;
        }

        internal static TipoTransaccion GetObjeto(int v)
        {
            TipoTransaccion tipo = null;
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                string cadenaComando = "SELECT * FROM TiposTransacciones WHERE ID_TipoTransaccion=@id";
                SqlCommand comando = new SqlCommand(cadenaComando, cn);
                comando.Parameters.AddWithValue("@id", v);
                SqlDataReader reader = comando.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    tipo = new TipoTransaccion
                    {
                        TipoTransaccionId = reader.GetInt32(0),
                        Descripcion = reader.GetString(1),
                    };

                }
            }
            return tipo;
        }
    }
}
