using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAModels
{
    public class Producto
    {
        public int ProductoId { get; set; }

        public string NombreProducto
        {
            get { return $"{Marca.Descripcion} {Descripcion}"; }
        }

        public Marca Marca { get; set; }

        public Tipo Tipo { get; set; }
        public string Descripcion { get; set; }

        public decimal Precio { get; set; }
        public decimal PrecioAlquiler { get; set; }

        public decimal PrecioAlquilerQuincena { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"{Descripcion} {Marca.Descripcion}";
        }
    }
}
