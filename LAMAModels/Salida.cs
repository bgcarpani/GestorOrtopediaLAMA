using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAModels
{
    public class Salida
    {
        public int SalidaId { get; set; }
        public DateTime FechaSalida { get; set; }
        public decimal Total { get; set; }
        public byte[] RowVersion { get; set; }
        public List<DetalleSalida> DetalleSalidas { get; set; }

        public Salida()
        {
            DetalleSalidas = new List<DetalleSalida>();
        }
    }
}
