using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAModels
{
    public class DetalleSalida
    {
        public int DetalleSalidaId { get; set; }
        public Salida Salida { get; set; }
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }
        public Kardex Kardex { get; set; }
        public string Motivo { get; set; }
        public byte[] RowVersion { get; set; }

        public override string ToString()
        {
            return $"{Producto.Marca.Descripcion}" +
                   $"{Producto.Descripcion}";
        }

        public decimal Total()
        {
            return Cantidad* Producto.Precio;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is DetalleSalida))
            {
                return false;
            }

            return Producto.ProductoId == ((DetalleSalida)obj).Producto.ProductoId;
        }
    }
}
