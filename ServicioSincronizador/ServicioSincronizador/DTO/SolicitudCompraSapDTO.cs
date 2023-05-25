using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioSincronizador.DTO
{
    public class SolicitudCompraSapDTO
    {
        public string ItemCode { get; set; }
        public string Descripcion { get; set; }
        public int NumeroSolicitud { get; set; }
        public decimal Cantidad { get; set; }
        public string Almacen { get; set; }
        public string CentroCosto { get; set; }
    }
}
