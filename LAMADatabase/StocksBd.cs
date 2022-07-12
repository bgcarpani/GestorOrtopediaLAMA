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
    public class StocksBd
    {
        public static void CargarCombo(ref ComboBox cboProducto)
        {
            {
                List<Stock> lista = GetLista();
                Stock defaultProductoSucural = new Stock
                {
                    Descripcion = "<Seleccione tipo de Stock>"
                };
                lista.Insert(0, defaultProductoSucural);
                cboProducto.DataSource = lista;
                cboProducto.ValueMember = "StockId";
                cboProducto.DisplayMember = "Descripcion";
                cboProducto.SelectedIndex = 0;
            }
        }

        public static void CargarComboVentaAlquileres(ref ComboBox cboProducto)
        {
            {
                List<Stock> lista = GetLista();
                lista = lista.Where(c => c.StockId == 1 || c.StockId == 2).ToList();
                Stock defaultProductoSucural = new Stock
                {
                    Descripcion = "<Seleccione tipo de Stock>"
                };
                lista.Insert(0, defaultProductoSucural);
                cboProducto.DataSource = lista;
                cboProducto.ValueMember = "StockId";
                cboProducto.DisplayMember = "Descripcion";
                cboProducto.SelectedIndex = 0;
            }
        }


        private static List<Stock> GetLista()
        {
            List<Stock> lista = new List<Stock>();
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "SELECT * FROM Stocks";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        Stock v = new Stock();
                        v.StockId = reader.GetInt32(0);
                        v.Descripcion = reader.GetString(1);
                        lista.Add(v);
                    }
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static Stock GetObjeto(int id)
        {
            Stock s = null;
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string strComando = "SELECT * FROM Stocks " +
                        " WHERE ID_Stock=@id";
                    SqlCommand comando = new SqlCommand(strComando, cn);
                    comando.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        s = new Stock();
                        s.StockId = reader.GetInt32(0);
                        s.Descripcion = reader.GetString(1);
                    }
                }
                return s;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
