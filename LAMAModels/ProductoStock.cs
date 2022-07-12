using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAModels
{
    public class ProductoStock
    {
        public int ProductoStockId { get; set; }
        public Stock Stock { get; set; }
        public Producto Producto { get; set; }
        public int StockDisponible { get; set; }
    }
}
