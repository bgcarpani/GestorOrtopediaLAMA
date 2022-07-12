using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAModels
{
    public class DevolucionVentaDisponible
    {
        public int DetalleVentaId { get; set; }
        public Venta Venta { get; set; }

        public Producto Producto { get; set; }
        public int CantidadOriginal { get; set; }
        public int CantidadDevuelta { get; set; }
        public int CantidadDisponible { get { return CantidadOriginal - CantidadDevuelta; } }
        public decimal Precio { get; set; }
        public decimal Total { get; set; }
        public Kardex Kardex { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
