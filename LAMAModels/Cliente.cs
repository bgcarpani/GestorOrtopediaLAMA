using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAModels
{
    public class Cliente:DatosDePersona, ICloneable
    {
        public int ClienteId { get; set; }
        public string DNI { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string Observaciones { get; set; }

        public bool Eliminado { get; set; }
        public string NombreCompleto
        {
            get { return $"{Apellido} {Nombre}"; }
        }
           

        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public override string ToString()
        {
            return $"{Apellido} {Nombre}";
        }

    }
}
