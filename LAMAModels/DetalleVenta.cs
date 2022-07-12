using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAModels
{
    public class DetalleVenta: ICloneable
    {
        public int DetalleVentaId { get; set; }
        public Venta Venta { get; set; }
        public Producto Producto { get; set; }
        public decimal PrecioUnidad { get; set; }

        public int Cantidad { get; set; }

        public decimal Total { get; set; }

        public Kardex Kardex { get; set; }

        public bool Devuelto { get; set; }


        public override string ToString()
        {
            return $"{Producto.Marca.Descripcion} {Producto.Descripcion}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is DetalleVenta))
            {
                return false;
            }

            return Producto.ProductoId == ((DetalleVenta)obj).Producto.ProductoId;
        }

        public override int GetHashCode()
        {
            return Producto.ProductoId;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
