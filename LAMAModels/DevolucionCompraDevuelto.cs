using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAModels
{
    public class DevolucionCompraDevuelto
    {
        public Compra Compra { get; set; }
        public ProductoSucursal ProductoSucursal { get; set; }
        public int CantidadADevolver { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Total { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is DevolucionCompraDevuelto))
            {
                return false;
            }
            return this.ProductoSucursal == ((DevolucionCompraDevuelto)obj).ProductoSucursal;
        }

        public override int GetHashCode()
        {
            return this.ProductoSucursal.Producto.Descripcion.GetHashCode();
        }
    }
}
