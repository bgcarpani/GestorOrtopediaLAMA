using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAModels
{
    public class Proveedor : ICloneable
    {
        public int ProveedorId { get; set; }
        public Localidad Localidad { get; set; }
        public string RazonSocial { get; set; }
        public string Direccion  { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Web { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}
