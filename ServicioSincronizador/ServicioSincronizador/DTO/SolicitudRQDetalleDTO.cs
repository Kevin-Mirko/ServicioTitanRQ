using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioSincronizador.DTO
{
   public class SolicitudRQDetalleDTO
    {
        public int IdDetalle { get; set; }
        public int IdSolicitud { get; set; }
        public string ItemCode { get; set; }

        public int Numero { get; set; }

        public string Descripcion { get; set; }
        public string Almacen { get; set; }
        public string CentroCosto { get; set; }
        public decimal Cantidad { get; set; }
    }
}
