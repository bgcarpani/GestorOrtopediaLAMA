using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAModels
{
    public class ProductoSucursal:ICloneable
    {
        public int ProductoSucursalId { get; set; }
        public Producto Producto { get; set; }
        public int StockMinimo { get; set; }
        public int StockMaximo { get; set; }
        public decimal PrecioVenta { get; set; }

        public int Stock { get; set; }
        public byte[] RowVersion { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
