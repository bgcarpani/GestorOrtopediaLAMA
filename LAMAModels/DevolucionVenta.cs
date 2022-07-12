using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAModels
{
    public class DevolucionVenta
    {
        public int DevolucionVentaId { get; set; }
        public DateTime FechaDevolucion { get; set; }
        public Venta Venta { get; set; }
        public decimal Total { get; set; }
        public byte[] RowVersion { get; set; }
        public List<DetalleDevolucionVenta> DetalleDevolucionVentas { get; set; } = new List<DetalleDevolucionVenta>();

    }
}
