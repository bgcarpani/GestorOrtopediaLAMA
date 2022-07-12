using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAModels
{
    public class DevolucionVentaDevuelto
    {

        public Venta Venta { get; set; }
        public ProductoSucursal ProductoSucursal { get; set; }
        public int CantidadADevolver { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Total { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is DevolucionVentaDevuelto))
            {
                return false;
            }
            return this.ProductoSucursal == ((DevolucionVentaDevuelto)obj).ProductoSucursal;
        }

        public override int GetHashCode()
        {
            return this.ProductoSucursal.Producto.Descripcion.GetHashCode();
        }
    }
}
