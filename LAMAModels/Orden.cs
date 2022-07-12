using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAModels
{
    public class Orden
    {
        public int OrdenId { get; set; }

        public Cliente Cliente { get; set; }

        public Protesis Protesis { get; set; }

        public decimal Costo { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaEntrega { get; set; }

        public string Notas { get; set; }

        public bool Entregado { get; set; }

        public bool Eliminado { get; set; }

        public DateTime FechaEliminacion { get; set; }

        public decimal Senia { get; set; }
        public int DiasEstimados { get; set; }

        public decimal ImporteOS { get; set; }
    }
}
