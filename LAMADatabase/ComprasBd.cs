using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LAMAModels;

namespace LAMADatabase
{
    public class ComprasBd
    {
        public static List<Compra> GetLista()
        {
            List<Compra> lista = new List<Compra>();
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string strComando = "SELECT ID_COMPRA, ID_PROVEEDOR, FECHA_COMPRA, IMPORTE FROM Compras order by Fecha_Compra desc";
                    SqlCommand comando = new SqlCommand(strComando, cn);
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        Compra compra = new Compra();
                        compra.CompraId = reader.GetInt32(0);
                        compra.Proveedor = ProveedoresBd.GetObjeto(reader.GetInt32(1));
                        compra.FechaCompra = reader.GetDateTime(2);                      
                        compra.Total = reader.GetDecimal(3);

                        lista.Add(compra);
                    }
                }
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public static void Agregar(Compra compra)
        {
            using (var cn=ConexionBd.GetConexion())
            {
                cn.Open();
                using (var transaction=cn.BeginTransaction())
                {
                    try
                    {
                        #region Grabar encabezado de compra
                        string cadenaComando =
                            "INSERT INTO Compras (Id_Proveedor, FECHA_COMPRA, IMPORTE) VALUES (@prov, @fec, @tot)";
                        SqlCommand comando = new SqlCommand(cadenaComando, cn, transaction);
                        int proveedorId = 0;
                        if (compra.Proveedor == null)
                        {
                            proveedorId = 1;
                        }
                        else
                        {
                            proveedorId = compra.Proveedor.ProveedorId;
                        }
                        comando.Parameters.AddWithValue("@prov", proveedorId);
                        comando.Parameters.AddWithValue("@fec", compra.FechaCompra);
                        comando.Parameters.AddWithValue("@tot", compra.Total);
                        comando.ExecuteNonQuery();
                        cadenaComando = "SELECT @@IDENTITY";
                        comando=new SqlCommand(cadenaComando,cn,transaction);
                        int id = (int)(decimal)comando.ExecuteScalar();
                        compra.CompraId = id;
                        #endregion Grabar encabezado de compra
                        #region Grabar detalle de compra
                        List<ProductoStock> productos = ProductoStocksBd.GetLista(1);

                        foreach (var detallesCompra in compra.DetallesCompras)
                        {
                            detallesCompra.Compra = compra;
                            Kardex kardex = KardexBd.UltimoKardex(detallesCompra.Producto, cn, transaction);
                            if (kardex == null)
                            {
                                kardex = new Kardex();
                                kardex.Producto = detallesCompra.Producto;
                                kardex.FechaMovimiento = compra.FechaCompra;
                                kardex.Movimiento = $"CO {compra.CompraId}";
                                kardex.Entrada = detallesCompra.Cantidad;
                                kardex.Salida = 0;
                                kardex.Saldo = detallesCompra.Cantidad;
                                kardex.UltimoCosto = detallesCompra.PrecioUnidad;
                                kardex.CostoPromedio = detallesCompra.PrecioUnidad;
                            }
                            else
                            {
                                int NuevoSaldo = kardex.Saldo + detallesCompra.Cantidad;
                                decimal NuevoPromedio = ((kardex.CostoPromedio * (decimal)kardex.Saldo) + (detallesCompra.PrecioUnidad * (decimal)detallesCompra.Cantidad)) / (decimal)NuevoSaldo;
                                kardex.Producto = detallesCompra.Producto;
                                kardex.FechaMovimiento = compra.FechaCompra;
                                kardex.Movimiento = $"CO {compra.CompraId}";
                                kardex.Entrada = detallesCompra.Cantidad;
                                kardex.Salida = 0;
                                kardex.Saldo = NuevoSaldo;
                                kardex.UltimoCosto = detallesCompra.PrecioUnidad;
                                kardex.CostoPromedio = NuevoPromedio;

                            }
                            KardexBd.Agregar(kardex, cn, transaction);
                            detallesCompra.Kardex = kardex;
                            TransaccionesBd.Agregar(2, int.Parse(ConfigurationManager.ConnectionStrings["ConsumidorFinal"].ToString()), -(compra.Total), cn, transaction, $"COMPRA-{compra.CompraId}");
                            DetallesComprasBd.Agregar(detallesCompra,cn,transaction);
                         
                            if (!ExisteProductoEnStock(productos, detallesCompra.Producto.ProductoId))
                            {
                                ProductoStocksBd.Agregar(1, detallesCompra.Cantidad, detallesCompra.Producto.ProductoId);
                            }
                            else
                            {
                                ProductoStocksBd.SubirStock(1, detallesCompra.Cantidad, detallesCompra.Producto.ProductoId, cn, transaction);
                            }
                        }

                        #endregion Grabar detalle de compra
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

        public static void Editar(Compra p)
        {
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                using (var transaction = cn.BeginTransaction())
                {
                    try
                    {

                        string cadenaComando = "UPDATE Compras Set ID_PROVEEDOR=@prov, FECHA_COMPRA=@fec, IMPORTE=@tot WHERE ID_COMPRA=@id";
                        SqlCommand comando = new SqlCommand(cadenaComando, cn, transaction);
                        comando.Parameters.AddWithValue("@id", p.CompraId);
                        comando.Parameters.AddWithValue("@prov", p.Proveedor.ProveedorId);
                        comando.Parameters.AddWithValue("@fec", p.FechaCompra);
                        comando.Parameters.AddWithValue("@tot", p.Total);
                        comando.ExecuteNonQuery();


                        foreach (var detallesCompra in p.DetallesCompras)
                        {
                            detallesCompra.Compra = p;
                            Kardex kardex = KardexBd.UltimoKardex(detallesCompra.Producto, cn, transaction);
                            int kardexAEliminar = kardex.KardexId;
                            if (kardex == null)
                            {
                                kardex = new Kardex();
                                kardex.Producto = detallesCompra.Producto;
                                kardex.FechaMovimiento = p.FechaCompra;
                                kardex.Movimiento = $"CO {p.CompraId}";
                                kardex.Entrada = detallesCompra.Cantidad;
                                kardex.Salida = 0;
                                kardex.Saldo = detallesCompra.Cantidad;
                                kardex.UltimoCosto = detallesCompra.PrecioUnidad;
                                kardex.CostoPromedio = detallesCompra.PrecioUnidad;
                            }
                            else
                            {
                                int NuevoSaldo = kardex.Saldo + detallesCompra.Cantidad;
                                decimal NuevoPromedio = ((kardex.CostoPromedio * (decimal)kardex.Saldo) + (detallesCompra.PrecioUnidad * (decimal)detallesCompra.Cantidad)) / (decimal)NuevoSaldo;
                                kardex.Producto = detallesCompra.Producto;
                                kardex.FechaMovimiento = p.FechaCompra;
                                kardex.Movimiento = $"CO {p.CompraId}";
                                kardex.Entrada = detallesCompra.Cantidad;
                                kardex.Salida = 0;
                                kardex.Saldo = NuevoSaldo;
                                kardex.UltimoCosto = detallesCompra.PrecioUnidad;
                                kardex.CostoPromedio = NuevoPromedio;

                            }
                            KardexBd.Borrar(kardexAEliminar, cn, transaction);
                            KardexBd.Agregar(kardex, cn, transaction);
                            detallesCompra.Kardex = kardex;
                            DetallesComprasBd.Eliminar(p.CompraId, true);
                            detallesCompra.Compra = p;
                            DetallesComprasBd.Agregar(detallesCompra, cn, transaction);
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

        public static void Borrar(Compra compra)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string strComando = "DELETE FROM Compras WHERE ID_Compra=@id";
                    SqlCommand comando = new SqlCommand(strComando, cn);
                    comando.Parameters.AddWithValue("@id", compra.CompraId);
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

        public static Compra GetObjeto(int compraId)
        {
            Compra p = null;
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComnando = "SELECT * FROM Compras WHERE ID_COMPRA=@id";
                    SqlCommand comando = new SqlCommand(cadenaComnando, cn);
                    comando.Parameters.AddWithValue("@id", compraId);
                    SqlDataReader reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        p = new Compra();
                        p.CompraId = reader.GetInt32(0);
                        p.Proveedor = ProveedoresBd.GetObjeto(reader.GetInt32(1));
                        p.FechaCompra = reader.GetDateTime(2);
                        p.Total = reader.GetDecimal(3);
                    }

                    return p;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static bool ExisteProductoEnStock(List<ProductoStock> productos, int productoId)
        {
            return productos.Where(p => p.Producto.ProductoId == productoId).Any();
        }
    }
}
