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
    public class SituacionesIvaBd
    {
        public static List<SituacionIva> GetLista()
        {
            var lista = new List<SituacionIva>();
            using (var conexion = ConexionBd.GetConexion())
            {
                conexion.Open();
                string cadenaComando = "SELECT SituacionIvaId, Descripcion, RowVersion FROM SituacionesIva";
                SqlCommand comando = new SqlCommand(cadenaComando, conexion);
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    SituacionIva situacionIva = new SituacionIva
                    {
                        SituacionIvaId = reader.GetInt32(0),
                        Descripcion = reader.GetString(1),
                        RowVersion = (byte[])reader[2]
                    };
                    lista.Add(situacionIva);
                }
                return lista;



            }
        }


        public static void Agregar(SituacionIva situacionIva)
        {
            try
            {
                using (var conexion = ConexionBd.GetConexion())
                {
                    conexion.Open();
                    string cadenaComando = "INSERT INTO SituacionesIva (Descripcion) VALUES (@desc)";
                    SqlCommand comando = new SqlCommand(cadenaComando, conexion);
                    comando.Parameters.AddWithValue("@desc", situacionIva.Descripcion);
                    comando.ExecuteNonQuery();
                    cadenaComando = "SELECT @@IDENTITY";
                    comando = new SqlCommand(cadenaComando, conexion);
                    int id = (int)(decimal)comando.ExecuteScalar();
                    situacionIva.SituacionIvaId = id;

                }

            }
            catch (Exception ex)
            {

                if (ex.Message.Contains("IX_SituacionesIva_Descripcion"))
                {
                    throw new Exception("Situacion de Iva existente");
                }
            }
        }

        public static void Editar(SituacionIva situacionIva)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "UPDATE SituacionesIva SET Descripcion=@nomb WHERE SituacionIvaId=@id AND RowVersion=@row";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@nomb", situacionIva.Descripcion);
                    comando.Parameters.AddWithValue("@id", situacionIva.SituacionIvaId);
                    comando.Parameters.AddWithValue("@row", situacionIva.RowVersion);
                    int cantidad = comando.ExecuteNonQuery();
                    if (cantidad == 0)
                    {
                        throw new Exception("Registro no encontrado o modificado por otro usuario");
                    }
                    else
                    {
                        cadenaComando = "SELECT RowVersion FROM SituacionesIva WHERE SituacionIvaId=@id";
                        comando = new SqlCommand(cadenaComando, cn);
                        comando.Parameters.AddWithValue("@id", situacionIva.SituacionIvaId);
                        situacionIva.RowVersion = (byte[])comando.ExecuteScalar();
                    }
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_SituacionesIva_Descripcion"))
                {
                    throw new Exception("Situacion de Iva existente");
                }
                throw ex;
            }
        }

        public static void Borrar(SituacionIva situacionIva)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "DELETE FROM SituacionesIva WHERE SituacionIvaId=@id";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@id", situacionIva.SituacionIvaId);
                    comando.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public static SituacionIva GetObjeto(int id)
        {
            SituacionIva situacionIva = null;
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "SELECT SituacionIvaId, Descripcion, RowVersion FROM SituacionesIva WHERE SituacionIvaId=@id";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = comando.ExecuteReader(CommandBehavior.SingleRow);
                    if (reader.HasRows)
                    {
                        reader.Read();
                        situacionIva = new SituacionIva();
                        situacionIva.SituacionIvaId = reader.GetInt32(0);
                        situacionIva.Descripcion = reader.GetString(1);
                        situacionIva.RowVersion = (byte[])reader[2];

                    }
                    return situacionIva;

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public static void CargarCombo(ref ComboBox cbo)
        {
            var situaciones = SituacionesIvaBd.GetLista();
            var defaultSituacion = new SituacionIva() { Descripcion = "<Seleccione Situación Iva>" };
            situaciones.Insert(0, defaultSituacion);
            cbo.DataSource = situaciones;
            cbo.DisplayMember = "Descripcion";
            cbo.ValueMember = "SituacionIvaId";
            cbo.SelectedIndex = 0;

        }

    }
}
