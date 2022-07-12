using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LAMAModels;

namespace LAMADatabase
{
    public class TiposDeProductosBd
    {
        public static List<TipoDeProducto> GetLista()
        {
            var lista = new List<TipoDeProducto>();
            using (var conexion = ConexionBd.GetConexion())
            {
                conexion.Open();
                string cadenaComando = "SELECT TipoDeProductoId, Descripcion, RowVersion FROM TiposDeProductos";
                SqlCommand comando = new SqlCommand(cadenaComando, conexion);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    TipoDeProducto tipoProducto = new TipoDeProducto
                    {
                        TipoDeProductoId = reader.GetInt32(0),
                        Descripcion = reader.GetString(1),
                        RowVersion = (byte[])reader[2]
                    };
                    lista.Add(tipoProducto);
                }
                return lista;



            }
        }


        public static void Agregar(TipoDeProducto tipoProducto)
        {
            try
            {
                using (var conexion = ConexionBd.GetConexion())
                {
                    conexion.Open();
                    string cadenaComando = "INSERT INTO TiposDeProductos (Descripcion) VALUES (@desc)";
                    SqlCommand comando = new SqlCommand(cadenaComando, conexion);
                    comando.Parameters.AddWithValue("@desc", tipoProducto.Descripcion);
                    comando.ExecuteNonQuery();
                    cadenaComando = "SELECT @@IDENTITY";
                    comando = new SqlCommand(cadenaComando, conexion);
                    int id = (int)(decimal)comando.ExecuteScalar();
                    tipoProducto.TipoDeProductoId = id;

                }

            }
            catch (Exception ex)
            {

                if (ex.Message.Contains("IX_TiposDeProductos_Descripcion"))
                {
                    throw new Exception("Tipo de Producto existente");
                }
            }
        }

        public static void Editar(TipoDeProducto tipoProducto)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "UPDATE TiposDeProductos SET Descripcion=@nomb WHERE TipoDeProductoId=@id AND RowVersion=@row";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@nomb", tipoProducto.Descripcion);
                    comando.Parameters.AddWithValue("@id", tipoProducto.TipoDeProductoId);
                    comando.Parameters.AddWithValue("@row", tipoProducto.RowVersion);
                    int cantidad = comando.ExecuteNonQuery();
                    if (cantidad == 0)
                    {
                        throw new Exception("Registro no encontrado o modificado por otro usuario");
                    }
                    else
                    {
                        cadenaComando = "SELECT RowVersion FROM TiposDeProductos WHERE TipoDeProductoId=@id";
                        comando = new SqlCommand(cadenaComando, cn);
                        comando.Parameters.AddWithValue("@id", tipoProducto.TipoDeProductoId);
                        tipoProducto.RowVersion = (byte[])comando.ExecuteScalar();
                    }
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_TiposDeProductos_Descripcion"))
                {
                    throw new Exception("Tipo de Producto existente");
                }
                throw ex;
            }
        }

        public static void Borrar(TipoDeProducto tipoProducto)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "DELETE FROM TiposDeProductos WHERE TipoDeProductoId=@id";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@id", tipoProducto.TipoDeProductoId);
                    comando.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public static TipoDeProducto GetObjeto(int id)
        {
            TipoDeProducto tipoProducto = null;
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "SELECT TipoDeProductoId, Descripcion, RowVersion FROM TiposDeProductos WHERE TipoDeProductoId=@id";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = comando.ExecuteReader(CommandBehavior.SingleRow);
                    if (reader.HasRows)
                    {
                        reader.Read();
                        tipoProducto = new TipoDeProducto();
                        tipoProducto.TipoDeProductoId = reader.GetInt32(0);
                        tipoProducto.Descripcion = reader.GetString(1);
                        tipoProducto.RowVersion = (byte[])reader[2];

                    }
                    return tipoProducto;

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public static void CargarCombo(ref ComboBox cbo)
        {
            cbo.DataSource = null;
            var tipos = TiposDeProductosBd.GetLista();
            var defaultTipo= new TipoDeProducto() { Descripcion = "<Seleccione Tipo>" };
            tipos.Insert(0, defaultTipo);
            cbo.DataSource = tipos;
            cbo.DisplayMember = "Descripcion";
            cbo.ValueMember = "TipoDeProductoId";
            cbo.SelectedIndex = 0;

        }

    }
}
