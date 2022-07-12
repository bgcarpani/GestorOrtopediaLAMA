using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMADatabase
{
    public class ConexionBd
    {
        public static SqlConnection GetConexion()
        {
            //Definicion del string de conexión
            //Se especifica el servidor, la base de datos y el tipo de seguridad
            //string stringDeConexion = @"Data Source=DESKTOP-EU6IKG7\SQLEXPRESS01;Initial Catalog=DBLAMA_new;Connection Timeout=120;Trusted_Connection=True;";
            string stringDeConexion = ConfigurationManager.ConnectionStrings["Conexion"].ToString();
            //Creo un objeto SqlConnection y establezco la conexion
            SqlConnection cn=new SqlConnection(stringDeConexion);
            //retorno la conexion
            return cn;
        }
    }
}
