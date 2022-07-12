using LAMAModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAReportes1
{
    public class Reportes
    {
        public static rptContratoAlquiler GetDatos(Alquiler alq, DetalleAlquiler det)
        {
            DataSet1 ds = AlquileresRpt.GetAlquileres(alq, det);
            rptContratoAlquiler rpt = new rptContratoAlquiler();
            rpt.SetDataSource(ds);
            return rpt;
        }

        public static rptContratoOrden GetDatos(Orden ord)
        {
            DataSet1 ds = OrdenesRpt.GetOrdenes(ord);
            rptContratoOrden rpt = new rptContratoOrden();
            rpt.SetDataSource(ds);
            return rpt;
        }

        public static rptCaja2 GetDatos(List<Transaccion> trans, DateTime fechaDesde, DateTime fechaHasta, decimal total)
        {
            DataSet1 ds = TransaccionesRpt.GetOrdenes(trans, fechaDesde, fechaHasta, total);
            rptCaja2 rpt = new rptCaja2();
            rpt.SetDataSource(ds);
            return rpt;
        }
    }
}
