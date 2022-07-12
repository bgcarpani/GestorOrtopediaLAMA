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
    public class FormasDePagoBd
    {
        public static List<FormaDePago> GetLista()
        {
            List<FormaDePago> lista = new List<FormaDePago>();
            try
            {
                using (var conexion = ConexionBd.GetConexion())
                {
                    conexion.Open();
                    string cadenaComando = "SELECT FormaDePagoId, Descripcion, RowVersion FROM FormasDePago";
                    SqlCommand comando = new SqlCommand(cadenaComando, conexion);
                    SqlDataReader reader = comando.ExecuteReader();
                    while (reader.Read())
                    {
                        FormaDePago pago = new FormaDePago()
                        {
                            FormaDePagoId = reader.GetInt32(0),
                            Descripcion = reader.GetString(1),
                            RowVersion = (byte[])reader[2],
                        };
                        lista.Add(pago);
                    }
                    return lista;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void Agregar(FormaDePago pago)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "INSERT INTO FormasDePago (Descripcion) VALUES (@desc)";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@desc", pago.Descripcion);
                    comando.ExecuteNonQuery();
                    cadenaComando = "SELECT @@IDENTITY";
                    comando = new SqlCommand(cadenaComando, cn);
                    int id = (int)(decimal)comando.ExecuteScalar();

                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_FormasDePagoId_Descripcion"))
                {
                    throw new Exception("Registro repetido");
                }
            }
        }

        public static void Borrar(int Id)
        {
            using (SqlConnection cn = ConexionBd.GetConexion())
            {
                cn.Open();
                string cadenaComando = "DELETE FROM FormasDePago WHERE FormaDePagoId=@id";
                SqlCommand comando = new SqlCommand(cadenaComando, cn);
                comando.Parameters.AddWithValue("@id", Id);
                comando.ExecuteNonQuery();
            }
        }

        public static void Editar(FormaDePago pago)
        {
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "UPDATE FormasDePago SET Descripcion=@nomb WHERE FormaDepagoId=@id AND RowVersion=@row";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@nomb", pago.Descripcion);
                    comando.Parameters.AddWithValue("@id", pago.FormaDePagoId);
                    comando.Parameters.AddWithValue("@row", pago.RowVersion);
                    int cantidad = comando.ExecuteNonQuery();
                    if (cantidad == 0)
                    {
                        throw new Exception("Registro no encontrado o modificado por otro usuario");
                    }
                    else
                    {
                        cadenaComando = "SELECT RowVersion FROM FormasDePago WHERE FormaDePagoId=@id";
                        comando = new SqlCommand(cadenaComando, cn);
                        comando.Parameters.AddWithValue("@id", pago.FormaDePagoId);
                        pago.RowVersion = (byte[])comando.ExecuteScalar();
                    }
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IX_FormasDePago_Descripcion"))
                {
                    throw new Exception("Forma de pago existente");
                }
                throw ex;
            }
        }

        public static FormaDePago GetObjeto(int id)
        {
            FormaDePago pago = null;
            try
            {
                using (SqlConnection cn = ConexionBd.GetConexion())
                {
                    cn.Open();
                    string cadenaComando = "SELECT FormaDePagoId, Descripcion, RowVersion FROM FormasDePago WHERE FormaDePagoId=@id";
                    SqlCommand comando = new SqlCommand(cadenaComando, cn);
                    comando.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = comando.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        pago = new FormaDePago();
                        pago.FormaDePagoId = reader.GetInt32(0);
                        pago.Descripcion = reader.GetString(1);
                        pago.RowVersion = (byte[])reader[2];

                    }
                    return pago;

                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public static void CargarCombo(ref ComboBox cboForma)
        {
            var forma = GetLista();
            var defaultForma = new FormaDePago() {Descripcion = "<Seleccione forma de pago>"};
            forma.Insert(0, defaultForma);
            cboForma.DataSource = forma;
            cboForma.DisplayMember = "Descripcion";
            cboForma.ValueMember = "FormaDePagoId";
            cboForma.SelectedIndex = 0;
        }
    }
}
