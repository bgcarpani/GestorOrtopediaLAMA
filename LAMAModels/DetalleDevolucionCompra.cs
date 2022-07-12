﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAModels
{
    public class DetalleDevolucionCompra
    {
        public int DetalleDevolucionCompraId { get; set; }
        public DevolucionCompra DevolucionCompra { get; set; }
        public Producto Producto { get; set; }
        public Kardex Kardex { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Total { get; set; }
    }
}
