using LAMAModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAReportes1
{
    public class AlquileresRpt
    {
        public static DataSet1 GetAlquileres(Alquiler alquiler, DetalleAlquiler det)
        {
            DataSet1 ds = new DataSet1();
            ds.Tables["Alquiler"].Rows.Add(
                    alquiler.AlquilerId,
                    alquiler.Cliente.Nombre+" "+alquiler.Cliente.Apellido,
                    alquiler.FechaDesde.ToShortDateString(),
                    alquiler.FechaHasta.ToShortDateString(),
                    alquiler.FechaDevolucion,
                    alquiler.Importe,
                    alquiler.Cliente.DNI,
                    alquiler.Cliente.Domicilio.Direccion);
            //foreach (var item in alquiler.Detalle)
            //{
            ds.Tables["Detalle_Alquiler"].Rows.Add(
                det.Alquiler,
                det.Producto.Marca.Descripcion+" "+det.Producto.Descripcion,
                det.Cantidad,
                det.Producto.ProductoId);
            //}
            
            return ds;
        }
    }
}
