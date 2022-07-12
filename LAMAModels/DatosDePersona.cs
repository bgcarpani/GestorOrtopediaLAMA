using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAMAModels
{
    public class DatosDePersona
    {
        public Domicilio Domicilio { get; set; }
        public string TelefonoMovil { get; set; }

        public string TelefonoFijo { get; set; }

        public string TelefonoTrabajo { get; set; }
        public string CorreoElectronico { get; set; }

        public int Fax { get; set; }

        public int ID_Tipo { get; set; }

        public DateTime FechaNac { get; set; }

    }
}
