using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAModels
{
    public class DevolucionCompra
    {
        public int DevolucionCompraId { get; set; }
        public DateTime FechaDevolucion { get; set; }
        public Compra Compra { get; set; }
        public decimal Total { get; set; }
        public byte[] RowVersion { get; set; }
        public List<DetalleDevolucionCompra> DetalleDevolucionCompras { get; set; } = new List<DetalleDevolucionCompra>();
    }
}
