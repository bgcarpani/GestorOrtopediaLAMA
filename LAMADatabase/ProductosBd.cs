using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LAMAModels;

namespace LAMADatabase
{
    public class ProductosBd
    {
        public static List<Producto> GetLista()
        {
            List<Producto> lista = new List<Producto>();
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string strComando = "SELECT ID_PRODUCTO, ID_Marca, Descripcion, Precio,PrecioALQ, ID_Tipo,precioAlquin FROM Productos order by DESCRIPCION";
                    SqlCommand comando = new SqlCommand(strComando, cn);
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        Producto p = new Producto();
                        p.ProductoId = reader.GetInt32(0);
                        p.Marca = MarcasBd.GetObjeto(reader.GetInt32(1));
                        p.Descripcion = reader.GetString(2);
                        p.Precio = reader.GetDecimal(3);
                        p.PrecioAlquiler = reader.GetDecimal(4);
                        p.Tipo = TiposBd.GetObjeto(reader.GetInt32(5));
                        p.PrecioAlquilerQuincena = reader.GetDecimal(6);
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

        public static List<Producto> GetLista(int selectedIndex)
        {
            List<Producto> lista = new List<Producto>();
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
                        Producto p = new Producto();
                        p = GetObjeto(reader.GetInt32(1));                
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
        public static Producto GetObjeto(int id)
        {
            Producto p = null;
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string strComando = "SELECT ID_PRODUCTO, ID_Marca, Descripcion, Precio, " +
                        "PrecioAlq, ID_Tipo, precioAlquin FROM Productos WHERE ID_PRODUCTO=@id";
                    SqlCommand comando = new SqlCommand(strComando, cn);
                    comando.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        p = new Producto();
                        p.ProductoId = reader.GetInt32(0);
                        p.Marca = MarcasBd.GetObjeto(reader.GetInt32(1));
                        p.Descripcion = reader.GetString(2);
                        p.Precio = reader.GetDecimal(3);
                        p.PrecioAlquiler = reader.GetDecimal(4);
                        p.Tipo = TiposBd.GetObjeto(reader.GetInt32(5));
                        p.PrecioAlquilerQuincena = reader.GetDecimal(6);
                    }
                }
                return p;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static void Borrar(Producto p)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string strComando = "DELETE FROM Productos WHERE ID_PRODUCTO=@id";
                    SqlCommand comando = new SqlCommand(strComando, cn);
                    comando.Parameters.AddWithValue("@id", p.ProductoId);
                    comando.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        public static void Editar(Producto p)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string strComando = "UPDATE Productos SET ID_Marca = @marca, Descripcion = @desc, Precio = @precio, PrecioAlq = @precioAlq, ID_Tipo = @idTipo, precioAlquin = @prec WHERE ID_PRODUCTO=@id";
                    SqlCommand comando = new SqlCommand(strComando, cn);
                    comando.Parameters.AddWithValue("@marca", p.Marca.MarcaId);
                    comando.Parameters.AddWithValue("@desc", p.Descripcion);
                    comando.Parameters.AddWithValue("@precio", p.Precio);
                    comando.Parameters.AddWithValue("@precioAlq", p.PrecioAlquiler);
                    comando.Parameters.AddWithValue("@idTipo", p.Tipo.TipoId);
                    comando.Parameters.AddWithValue("@id", p.ProductoId);
                    comando.Parameters.AddWithValue("@prec", p.PrecioAlquilerQuincena);
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

        internal static void BajarStock(Producto producto, int cantidad, SqlConnection cn, SqlTransaction transaction)
        {
            try
            {
                string cadenacomando = "UPDATE Productos SET Cantidad=Cantidad-@stock WHERE ID_PRODUCTO=@id";
                SqlCommand comando = new SqlCommand(cadenacomando, cn, transaction);
                comando.Parameters.AddWithValue("@stock", cantidad);
                comando.Parameters.AddWithValue("@id", producto.ProductoId);
                comando.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static void Agregar(Producto p)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string strComando = "INSERT INTO Productos (ID_Marca, Descripcion, Precio, PrecioAlq, ID_Tipo, precioAlquin) " +
                        " VALUES(@marca, @desc, @precio, @precioAlq, @idTipo, @prec)";
                    SqlCommand comando = new SqlCommand(strComando, cn);
                    comando.Parameters.AddWithValue("@marca", p.Marca.MarcaId);
                    comando.Parameters.AddWithValue("@desc", p.Descripcion);
                    comando.Parameters.AddWithValue("@precio", p.Precio);
                    comando.Parameters.AddWithValue("@precioAlq", p.PrecioAlquiler);
                    comando.Parameters.AddWithValue("@idTipo", p.Tipo.TipoId);
                    comando.Parameters.AddWithValue("@prec", p.PrecioAlquilerQuincena);
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

        //internal static void ActualizarStock(Producto producto, int cantidad, SqlConnection cn, SqlTransaction transaction)
        //{
        //    try
        //    {
        //        string cadenacomando = "UPDATE Productos SET Cantidad=Cantidad+@stock WHERE ID_PRODUCTO=@id";
        //        SqlCommand comando = new SqlCommand(cadenacomando, cn, transaction);
        //        comando.Parameters.AddWithValue("@stock", cantidad);
        //        comando.Parameters.AddWithValue("@id", producto.ProductoId);
        //        comando.ExecuteNonQuery();

        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        public static List<Producto> GetLista(Marca marca)
        {
            List<Producto> lista = new List<Producto>();
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string strComando = "SELECT ID_PRODUCTO, ID_Marca, Descripcion, Precio, PrecioAlq, ID_Tipo, precioAlquin" +
                       " FROM Productos WHERE ID_Marca = @marca";
                    SqlCommand comando = new SqlCommand(strComando, cn);
                    comando.Parameters.AddWithValue("@marca", marca.MarcaId);
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        Producto p = new Producto();
                        p.ProductoId = reader.GetInt32(0);
                        p.Marca = MarcasBd.GetObjeto(reader.GetInt32(1));
                        p.Descripcion = reader.GetString(2);
                        p.Precio = reader.GetDecimal(3);
                        p.PrecioAlquiler = reader.GetDecimal(4);
                        p.Tipo = TiposBd.GetObjeto(reader.GetInt32(5));
                        p.PrecioAlquilerQuincena = reader.GetDecimal(6);
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

        public static void CargarCombo(ref ComboBox cboProducto)
        {
            List<Producto> lista = GetLista();
            Producto defaultProductoSucural = new Producto
            {
                    Marca = new Marca(),
                    Descripcion = "<Seleccione Producto>"
            };
            lista.Insert(0, defaultProductoSucural);
            cboProducto.DataSource = lista;
            cboProducto.ValueMember = "ProductoId";
            cboProducto.DisplayMember = "NombreProducto";
            cboProducto.SelectedIndex = 0;
        }

        public static void CargarCombo(ref ComboBox cboProducto, int selectedIndex)
        {

                List<Producto> lista = GetLista(selectedIndex);
                Producto defaultProductoSucural = new Producto
                {
                    Marca = new Marca(),
                    Descripcion = "<Seleccione Producto>"
                };
                lista.Insert(0, defaultProductoSucural);
                cboProducto.DataSource = lista;
                cboProducto.ValueMember = "ProductoId";
                cboProducto.DisplayMember = "NombreProducto";
                cboProducto.SelectedIndex = 0;
        }

        public static void GenerarAumento(Aumento au)
        {
            try
            {  
                decimal aumento = (au.porcentaje / 100) + 1;
                NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
                string aum = aumento.ToString(nfi);
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string strComando = $"update PRODUCTOS set";
                    if (au.precioVenta && au.precioAlquiler)
                    {
                        strComando = strComando + $" PRECIO = PRECIO * {aum},  PRECIOAlq = PRECIOAlq * {aum}, PRECIOALQUIN = PRECIOALQUIN * {aum}";
                    }
                    else
                    {
                        if (au.precioVenta)
                        {
                            strComando = strComando + $" PRECIO = PRECIO * {aum}";
                        }
                        if (au.precioAlquiler)
                        {
                            strComando = strComando + $" PRECIOAlq = PRECIOAlq * {aum}, PRECIOALQUIN = PRECIOALQUIN * {aum}";
                        }
                    }
                    strComando = strComando + " WHERE ID_TIPO=@idTipo";
                    SqlCommand comando = new SqlCommand(strComando, cn);
                    comando.Parameters.AddWithValue("@idTipo", au.tipo.TipoId);
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
    }
}
