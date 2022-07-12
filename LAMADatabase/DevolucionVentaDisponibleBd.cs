using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LAMAModels;

namespace LAMADatabase
{
    public class DevolucionVentaDisponibleBd
    {
        public static List<DevolucionVentaDisponible> GetDisponibles(Venta venta)
        {
            
            List<DevolucionVentaDisponible> listaDisponible = new List<DevolucionVentaDisponible>();
            foreach (var item in venta.Detalle)
            {
                DevolucionVentaDisponible disponible = new DevolucionVentaDisponible()
                {
                    DetalleVentaId = item.DetalleVentaId,
                    Producto = item.Producto,
                    CantidadOriginal = item.Cantidad,
                    Precio = item.PrecioUnidad,
                    CantidadDevuelta = DetallesDevolucionVentasBd.GetCantidadDevuelta(venta, item.Producto),
                    Total = item.PrecioUnidad * item.Cantidad
                    
                };
                listaDisponible.Add(disponible);
            }
            return listaDisponible;
        }
    }
}
