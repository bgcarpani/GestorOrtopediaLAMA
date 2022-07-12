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
//    public class ProductosSucursalesBd
//    {
//        public static List<ProductoSucursal> GetLista()
//        {
//            List<ProductoSucursal> lista = new List<ProductoSucursal>();
//            try
//            {
//                using (SqlConnection cn = ConexionBd.GetConexion())
//                {
//                    cn.Open();
//                    string strComando = "SELECT ProductoSucursalId, ProductoId, SucursalId, StockMinimo, StockMaximo, Stock, PrecioVenta, RowVersion FROM ProductosSucursales";
//                    SqlCommand comando = new SqlCommand(strComando, cn);
//                    SqlDataReader reader = comando.ExecuteReader();
//                    while (reader.Read())
//                    {
//                        ProductoSucursal p = new ProductoSucursal();
//                        p.ProductoSucursalId = reader.GetInt32(0);
//                        p.Producto = ProductosBd.GetObjeto(reader.GetInt32(1));
//                        //p.Sucursal = SucursalesBd.GetObjeto(reader.GetInt32(2));
//                        p.StockMinimo = reader.GetInt32(3);
//                        p.StockMaximo = reader.GetInt32(4);
//                        p.Stock = reader.GetInt32(5);
//                        p.PrecioVenta = reader.GetDecimal(6);
//                        p.RowVersion = (byte[])reader[7];
//                        lista.Add(p);
//                    }
//                }
//                return lista;
//            }
//            catch (Exception ex)
//            {

//                throw ex;
//            }
//        }
//        public static ProductoSucursal GetObjeto(int id)
//        {
//            ProductoSucursal p = null;
//            try
//            {
//                using (SqlConnection cn = ConexionBd.GetConexion())
//                {
//                    cn.Open();
//                    string strComando = "SELECT ProductoSucursalId, ProductoId, SucursalId, StockMinimo, "+
//                        "StockMaximo, Stock, PrecioVenta, RowVersion FROM ProductosSucursales WHERE ProductoSucursalId=@id";
//                    SqlCommand comando = new SqlCommand(strComando, cn);
//                    comando.Parameters.AddWithValue("@id", id);
//                    SqlDataReader reader = comando.ExecuteReader();
//                    if (reader.HasRows)
//                    {
//                        reader.Read();
//                        p=new ProductoSucursal();
//                        p.ProductoSucursalId = reader.GetInt32(0);
//                        p.Producto = ProductosBd.GetObjeto(reader.GetInt32(1));
//                     //   p.Sucursal = SucursalesBd.GetObjeto(reader.GetInt32(2));
//                        p.StockMinimo = reader.GetInt32(3);
//                        p.StockMaximo = reader.GetInt32(4);
//                        p.Stock = reader.GetInt32(5);
//                        p.PrecioVenta = reader.GetDecimal(6);
//                        p.RowVersion = (byte[])reader[7];

//                    }
//                }
//                return p;
//            }
//            catch (Exception ex)
//            {

//                throw ex;
//            }
//        }

//        public static void ActualizarStock(ProductoSucursal Producto, int cantidad, SqlConnection cn, SqlTransaction transaction)
//        {
//            try
//            {
//                string cadenacomando = "UPDATE ProductosSucursales SET Stock=Stock+@stock WHERE ProductoId=@id";
//                SqlCommand comando=new SqlCommand(cadenacomando,cn,transaction);
//                comando.Parameters.AddWithValue("@stock", cantidad);
//                comando.Parameters.AddWithValue("@id", Producto.ProductoId);
//                comando.ExecuteNonQuery();

//            }
//            catch (Exception ex)
//            {

//                throw ex;
//            }
//        }

//        public static void Borrar(Producto p)
//        {
//            try
//            {
//                using (SqlConnection cn = ConexionBd.GetConexion())
//                {
//                    cn.Open();
//                    string strComando = "DELETE FROM ProductosSucursales WHERE ProductoId=@id";
//                    SqlCommand comando = new SqlCommand(strComando, cn);
//                    comando.Parameters.AddWithValue("@id", p.ProductoId);
//                    comando.ExecuteNonQuery();

//                }
//            }
//            catch (Exception ex)
//            {

//                throw ex;
//            }


//        }
//        public static void Editar(ProductoSucursal p)
//        {
//            try
//            {
//                using (SqlConnection cn = ConexionBd.GetConexion())
//                {
//                    cn.Open();
//                    string strComando = "UPDATE ProductosSucursales SET ProductoId=@prod, SucursalId=@suc, " +
//                        "StockMinimo=@min , StockMaximo=@max, Stock=@stock PrecioVenta=@venta, " +
//                        "WHERE ProductoId=@id and RowVersion=@row";
//                    SqlCommand comando = new SqlCommand(strComando, cn);
//                    comando.Parameters.AddWithValue("@prod", p.Producto.ProductoId);
//                    //comando.Parameters.AddWithValue("@suc", p.Sucursal.SucursalId);
//                    comando.Parameters.AddWithValue("@min", p.StockMinimo);
//                    comando.Parameters.AddWithValue("@max", p.StockMaximo);
//                    comando.Parameters.AddWithValue("@stock", p.Stock);
//                    comando.Parameters.AddWithValue("@venta", p.PrecioVenta);
//                    comando.Parameters.AddWithValue("@id", p.ProductoSucursalId);
//                    comando.Parameters.AddWithValue("@row", p.RowVersion);
//                    int cantidad = comando.ExecuteNonQuery();
//                    if (cantidad == 0)
//                    {
//                        throw new Exception("Registro no encontrado o modificado por otro usuario");
//                    }
//                    else
//                    {
//                        strComando = "SELECT RowVersion FROM ProductosSucursales WHERE ProductoId=@id";
//                        comando = new SqlCommand(strComando, cn);
//                        comando.Parameters.AddWithValue("@id", p.ProductoSucursalId);
//                        p.RowVersion = (byte[])comando.ExecuteScalar();
//                    }

//                }

//            }
//            catch (Exception ex)
//            {

//                throw ex;
//            }

//        }
//        public static void Agregar(ProductoSucursal p)
//        {
//            try
//            {
//                using (SqlConnection cn = ConexionBd.GetConexion())
//                {
//                    cn.Open();
//                    string strComando = "INSERT INTO ProductosSucursales (ProductoId, SucursalId, StockMinimo, StockMaximo, Stock, PrecioVenta) " +
//                        " VALUES(@prod, @suc, @max, @min, @stock, @venta)";
//                    SqlCommand comando = new SqlCommand(strComando, cn);
//                    comando.Parameters.AddWithValue("@prod", p.Producto.ProductoId);
//                   // comando.Parameters.AddWithValue("@suc", p.Sucursal.SucursalId);
//                    comando.Parameters.AddWithValue("@min", p.StockMinimo);
//                    comando.Parameters.AddWithValue("@max", p.StockMaximo);
//                    comando.Parameters.AddWithValue("@stock", p.Stock);
//                    comando.Parameters.AddWithValue("@venta", p.PrecioVenta);
//                    comando.ExecuteNonQuery();
//                    strComando = "SELECT @@IDENTITY";
//                    comando = new SqlCommand(strComando, cn);
//                    int id = (int)(decimal)comando.ExecuteScalar();
//                }

//            }
//            catch (Exception ex)
//            {

//                throw ex;
//            }
//        }


//        public static List<ProductoSucursal> GetLista(Producto producto)
//        {
//            List<ProductoSucursal> lista = new List<ProductoSucursal>();
//            try
//            {
//                using (SqlConnection cn = ConexionBd.GetConexion())
//                {
//                    cn.Open();
//                    string strComando = "SELECT ProductoSucursalId, ProductoId, SucursalId, StockMinimo, "+
//                        "StockMaximo, Stock, PrecioVenta, RowVersion FROM ProductosSucursales WHERE ProductoId=@prod";
//                    SqlCommand comando = new SqlCommand(strComando, cn);
//                    comando.Parameters.AddWithValue("@prod", producto.ProductoId);
//                    SqlDataReader reader = comando.ExecuteReader();
//                    while (reader.Read())
//                    {
//                        ProductoSucursal p = new ProductoSucursal();
//                        p.ProductoSucursalId = reader.GetInt32(0);
//                        p.Producto = ProductosBd.GetObjeto(reader.GetInt32(1));
//                     //   p.Sucursal = SucursalesBd.GetObjeto(reader.GetInt32(2));
//                        p.StockMinimo = reader.GetInt32(3);
//                        p.StockMaximo = reader.GetInt32(4);
//                        p.Stock = reader.GetInt32(5);
//                        p.PrecioVenta = reader.GetDecimal(6);
//                        p.RowVersion = (byte[])reader[7];
//                        lista.Add(p);
//                    }
//                }
//                return lista;
//            }
//            catch (Exception ex)
//            {

//                throw ex;
//            }
//        }

//        //public static void CargarCombo(ref ComboBox cboProducto, Sucursal sucursal)
//        //{
//        //    List<ProductoSucursal> lista = ProductosSucursalesBd.GetLista(sucursal);
//        //    ProductoSucursal defaultProductoSucural = new ProductoSucursal
//        //    {

//        //        Producto = new Producto
//        //        {
//        //            Marca=new Marca(),
//        //            TipoDeProducto = new TipoDeProducto(),
//        //            Descripcion = "<Seleccione Producto>"
//        //        }
//        //    };
//        //    lista.Insert(0, defaultProductoSucural);
//        //    cboProducto.DataSource = lista;
//        //    cboProducto.DisplayMember = "Producto";
//        //    cboProducto.ValueMember = "ProductoSucursalId";
//        //    cboProducto.SelectedIndex = 0;
//        //}

//        //private static List<ProductoSucursal> GetLista(Sucursal sucursal)
//        //{
//        //    List<ProductoSucursal> lista = new List<ProductoSucursal>();
//        //    try
//        //    {
//        //        using (SqlConnection cn = ConexionBd.GetConexion())
//        //        {
//        //            cn.Open();
//        //            string strComando = "SELECT ProductoSucursalId, ProductoId, SucursalId, StockMinimo, " +
//        //                "StockMaximo, Stock, PrecioVenta, RowVersion FROM ProductosSucursales WHERE SucursalId=@suc";
//        //            SqlCommand comando = new SqlCommand(strComando, cn);
//        //            comando.Parameters.AddWithValue("@suc", sucursal.SucursalId);
//        //            SqlDataReader reader = comando.ExecuteReader();
//        //            while (reader.Read())
//        //            {
//        //                ProductoSucursal p = new ProductoSucursal();
//        //                p.ProductoSucursalId = reader.GetInt32(0);
//        //                p.Producto = ProductosBd.GetObjeto(reader.GetInt32(1));
//        //                //p.Sucursal = SucursalesBd.GetObjeto(reader.GetInt32(2));
//        //                p.StockMinimo = reader.GetInt32(3);
//        //                p.StockMaximo = reader.GetInt32(4);
//        //                p.Stock = reader.GetInt32(5);
//        //                p.PrecioVenta = reader.GetDecimal(6);
//        //                p.RowVersion = (byte[])reader[7];
//        //                lista.Add(p);
//        //            }
//        //        }
//        //        return lista;
//        //    }
//        //    catch (Exception ex)
//        //    {

//        //        throw ex;
//        //    }
//        //}

//        public static void BajarStock(ProductoSucursal productoSucursal, int cantidad, SqlConnection cn, SqlTransaction transaction)
//        {
//            try
//            {
//                string cadenacomando = "UPDATE ProductosSucursales SET Stock=Stock-@stock WHERE ProductoSucursalId=@id";
//                SqlCommand comando = new SqlCommand(cadenacomando, cn, transaction);
//                comando.Parameters.AddWithValue("@stock", cantidad);
//                comando.Parameters.AddWithValue("@id", productoSucursal.ProductoSucursalId);
//                comando.ExecuteNonQuery();

//            }
//            catch (Exception ex)
//            {

//                throw ex;
//            }
//        }
////    }
}
