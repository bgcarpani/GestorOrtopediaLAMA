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
    public class ProductoStocksBd
    {

        public static List<ProductoStock> GetLista(int selectedIndex)
        {
            List<ProductoStock> lista = new List<ProductoStock>();
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string strComando = "SELECT * FROM Producto_STOCK Where ID_STOCK = @id";
                    SqlCommand comando = new SqlCommand(strComando, cn);
                    comando.Parameters.AddWithValue("@id", selectedIndex);
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        ProductoStock p = new ProductoStock();
                        p.ProductoStockId = reader.GetInt32(0);
                        p.Stock = StocksBd.GetObjeto(reader.GetInt32(1));
                        p.Producto = ProductosBd.GetObjeto(reader.GetInt32(2));
                        p.StockDisponible = reader.GetInt32(3);
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
        public static List<ProductoStock> GetListaCompleta()
        {
            List<ProductoStock> lista = new List<ProductoStock>();
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string strComando = "SELECT * FROM Producto_STOCK";
                    SqlCommand comando = new SqlCommand(strComando, cn);
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        ProductoStock p = new ProductoStock();
                        p.ProductoStockId = reader.GetInt32(0);
                        p.Stock = StocksBd.GetObjeto(reader.GetInt32(1));
                        p.Producto = ProductosBd.GetObjeto(reader.GetInt32(2));
                        p.StockDisponible = reader.GetInt32(3);
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

        public static List<ProductoStock> GetLista()
        {
            List<ProductoStock> lista = new List<ProductoStock>();
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "SELECT * FROM PRODUCTO_STOCK ORDER BY Stock";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        ProductoStock v = new ProductoStock();
                        v.ProductoStockId = reader.GetInt32(0);
                        v.Stock = StocksBd.GetObjeto(reader.GetInt32(1));
                        v.Producto = ProductosBd.GetObjeto(reader.GetInt32(2));
                        v.StockDisponible = reader.GetInt32(3);
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

        public static void BajarStock(int stockId, int cantidad, int productoId, SqlConnection cn = null, SqlTransaction transaction = null)
        {
            bool commit = false;
            if (cn == null)
            {
                cn = ConexionBd.GetConexion();
                cn.Open();
            }
            if (transaction == null)
            {
                commit = true;
                transaction = cn.BeginTransaction();
            }
            try
            {
                string cadenacomando = "UPDATE Producto_Stock SET Stock=Stock-@cantidad WHERE ID_Stock=@id and ID_Producto=@prod";
                SqlCommand comando = new SqlCommand(cadenacomando, cn, transaction);
                comando.Parameters.AddWithValue("@cantidad", cantidad);
                comando.Parameters.AddWithValue("@id", stockId);
                comando.Parameters.AddWithValue("@prod", productoId);
                comando.ExecuteNonQuery();
                if (commit)
                {
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
        }



        public static void MoverStock(int stockIdDesde, int stockIdHasta, int cantidad, int productoId)
        {

            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                using (var transaction = cn.BeginTransaction())
                {
                    try
                    {
                        var lista = GetListaCompleta();
                        BajarStock(stockIdDesde, cantidad, productoId, cn, transaction);
                        lista = lista.Where(s => s.Stock.StockId == stockIdHasta && s.Producto.ProductoId == productoId).ToList();
                        if (lista.Any())
                        {
                            SubirStock(stockIdHasta, cantidad, productoId, cn, transaction);
                        }
                        else
                        {
                            Agregar(stockIdHasta, cantidad, productoId);
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }

                }
            }

        }

        public static void SubirStock(int stockId, int cantidad, int productoId, SqlConnection cn = null, SqlTransaction transaction = null)
        {
            try
            {
                string cadenacomando = "UPDATE Producto_Stock SET Stock=Stock+@cantidad WHERE ID_Stock=@id and ID_Producto=@prod";
                SqlCommand comando = new SqlCommand(cadenacomando, cn, transaction);
                comando.Parameters.AddWithValue("@cantidad", cantidad);
                comando.Parameters.AddWithValue("@id", stockId);
                comando.Parameters.AddWithValue("@prod", productoId);
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static void CargarCombo(ref ComboBox cboProducto, int selectedIndex)
        {

            List<ProductoStock> lista = GetLista(selectedIndex);
            ProductoStock defaultps = new ProductoStock();
            defaultps.Stock = new Stock();
            defaultps.Producto = new Producto();
            defaultps.Producto.Marca = new Marca();
            defaultps.Producto.Descripcion = "<Seleccione Producto>";

            lista.Insert(0, defaultps);
            cboProducto.DataSource = lista;
            cboProducto.DisplayMember = "Producto";
            cboProducto.ValueMember = "ProductoStockId";
            cboProducto.SelectedIndex = 0;
        }

        internal static void Agregar(int stockId, int cantidad, int productoId)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string strComando = "INSERT INTO Producto_STOCK (ID_STOCK, ID_PRODUCTO, STOCK) " +
                        " VALUES(@idsto, @prod, @stock)";
                    SqlCommand comando = new SqlCommand(strComando, cn);
                    comando.Parameters.AddWithValue("@idsto", stockId);
                    comando.Parameters.AddWithValue("@prod", productoId);
                    comando.Parameters.AddWithValue("@stock", cantidad);
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

        public static void EliminarPorProducto(Producto prod)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string strComando = "DELETE FROM Producto_Stock WHERE ID_PRODUCTO=@id";
                    SqlCommand comando = new SqlCommand(strComando, cn);
                    comando.Parameters.AddWithValue("@id", prod.ProductoId);
                    comando.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }


}
