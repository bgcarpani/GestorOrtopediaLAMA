using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LAMAModels;

namespace LAMADatabase
{
    public class DevolucionCompraDisponibleBd
    {
        public static List<DevolucionCompraDisponible> GetDisponibles(Compra compra)
        {
            List<DevolucionCompraDisponible> listaDisponible = new List<DevolucionCompraDisponible>();
            foreach (var item in compra.DetallesCompras)
            {
                DevolucionCompraDisponible disponible = new DevolucionCompraDisponible()
                {
                    DetalleCompraId = item.DetalleCompraId,
                    Producto = item.Producto,
                    CantidadOriginal = item.Cantidad,
                    Precio = item.PrecioUnidad,
                    CantidadDevuelta = DetallesDevolucionesComprasBd.GetCantidadDevuelta(compra, item.Producto),
                    Total = item.Total
                    
                };
                listaDisponible.Add(disponible);
            }
            return listaDisponible;
        }
    }
    
}
