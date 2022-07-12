using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAModels
{
    public class DetalleDevolucionVenta
    {
        public int DetalleDevolucionVentaId { get; set; }
        public DevolucionVenta DevolucionVenta { get; set; }
        public Producto Producto { get; set; }
        public Kardex Kardex { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Total { get; set; }
    }
}
