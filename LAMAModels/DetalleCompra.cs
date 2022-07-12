using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace LAMAModels
{
    public class DetalleCompra
    {
        public int DetalleCompraId { get; set; }
        public Compra Compra { get; set; }
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnidad { get; set; }
        public decimal Total { get; set; }
        public Kardex Kardex { get; set; }

        public override string ToString()
        {
            return $"{Producto.Marca.Descripcion} " +
                   $"{Producto.Descripcion}";
        }
        public override bool Equals(object obj)
            {
                //       
                // See the full list of guidelines at
                //   http://go.microsoft.com/fwlink/?LinkID=85237  
                // and also the guidance for operator== at
                //   http://go.microsoft.com/fwlink/?LinkId=85238
                //

                if (obj == null || !(obj is DetalleCompra))
                {
                    return false;
                }

                return this.Producto.ProductoId == ((DetalleCompra) obj).Producto.ProductoId;
            }

            // override object.GetHashCode
            public override int GetHashCode()
            {
                return this.Producto.ProductoId;
            }
    }
}
