using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAModels
{
    public class Alquiler:ICloneable
    {
        public int AlquilerId { get; set; }

        public Cliente Cliente { get; set; }
        public DateTime FechaDesde { get; set; }

        public DateTime FechaHasta { get; set; }

        public int Cantidad { get; set; }

        public string Observacion { get; set; }

        public bool EstaEnUso { get; set; }

        public decimal Importe { get; set; }

        public decimal ImporteOS { get; set; }

        public DateTime FechaDevolucion { get; set; }
        public List<DetalleAlquiler> Detalle { get; set; }

        public Alquiler(int alquilerId)
        {
            AlquilerId = alquilerId;
        }
        public Alquiler()
        {
            Detalle = new List<DetalleAlquiler>();
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
