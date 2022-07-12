using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAModels
{
    public class DetalleAlquiler
    {
        public int DetalleAlquilerId { get; set; }

        public Alquiler Alquiler { get; set; }

        public Producto Producto { get; set; }

        public int Cantidad { get; set; }

        public Stock Stock{ get; set; }

        public override string ToString()
        {
            return $"{Producto.Marca.Descripcion} {Producto.Descripcion}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is DetalleAlquiler))
            {
                return false;
            }

            return Producto.ProductoId == ((DetalleAlquiler)obj).Producto.ProductoId;
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
