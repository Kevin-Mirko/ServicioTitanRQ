using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioSincronizador.DTO
{
    public class SalidaMercanciaDTO
    {
        public string DocEntry { get; set; }
        public string ItemCode { get; set; }
        public string Descripcion { get; set; }
        public int SolicitudRQ { get; set; }
        public string Almacen { get; set; }
        public string CentroCosto { get; set; }
        public string EstadoSincronizador { get; set; }

        public decimal Cantidad { get; set; }

    }
}
