using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioSincronizador.DTO
{
    public class ValidarSincronizadorDTO
    {

        public string DocEntry { get; set; }
        public string ContadorTerminados { get; set; }
        public string ContadorTotales { get; set; }
        public string Tipo { get; set; }

    }
}
