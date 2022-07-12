using LAMAModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAReportes1
{
    public class TransaccionesRpt
    {
        internal static DataSet1 GetOrdenes(List<Transaccion> trans, DateTime fechaD, DateTime fechaH, decimal total)
        {
            DataSet1 ds = new DataSet1();

            Cliente aux = new Cliente();
            aux.Nombre = "";
            aux.Apellido = "";
            foreach (var item in trans)
            {
                if (item.Cliente == null)
                {
                    item.Cliente = aux;
                }
                else if (item.Cliente.ClienteId == int.Parse(ConfigurationManager.ConnectionStrings["ConsumidorFinal"].ToString()))
                {
                    item.Cliente.Nombre = "";
                    item.Cliente.Apellido = "";
                }
            }
            foreach (var item in trans)
            {
                ds.Tables["Transacciones"].Rows.Add(item.TransaccionId,
                   item.Cliente.NombreCompleto,
                   item.FechaTransaccion,
                   fechaD.ToShortDateString(),
                   fechaH.ToShortDateString(),
                   item.Importe.ToString("C"),
                   total.ToString("C")
                    );
            }

            return ds;
        }
    }
}
