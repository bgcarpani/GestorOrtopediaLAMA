using LAMAModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAReportes1
{
    public class OrdenesRpt
    {
        public static DataSet1 GetOrdenes(Orden ord)
        {
            string nombreCompleto = "";
            if (ord.Cliente.Eliminado)
            {
                nombreCompleto = ord.Cliente.Nombre+ " " + ord.Cliente.Apellido;
            }
            else
            {
                nombreCompleto = ord.Cliente.Apellido + ", " + ord.Cliente.Nombre;
            }
            DataSet1 ds = new DataSet1();
            
            ds.Tables["OrdenCliente"].Rows.Add(
                   ord.OrdenId,
                   nombreCompleto,
                   ord.Cliente.ClienteId,
                   ord.Cliente.DNI,
                   ord.Cliente.Domicilio.Direccion,
                   ord.Cliente.Domicilio.Localidad.Descripcion,
                   ord.Cliente.Domicilio.Provincia.NombreProvincia,
                   ord.Cliente.Domicilio.CodigoPostal,
                   ord.Cliente.TelefonoMovil);

            ds.Tables["OrdenProtesis"].Rows.Add(
                ord.Protesis.ProtesisId,
                ord.Protesis.Tipo,
                ord.Protesis.Descripcion,
                ord.FechaEntrega.ToShortDateString(),
                ord.Senia,
                ord.Protesis.Descripcion,
                ord.Costo);

            return ds;
        }
    }
}
