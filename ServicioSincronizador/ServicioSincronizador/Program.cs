using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ServicioSincronizador
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        static void Main()
        {
            //SMCServicio a = new SMCServicio();
            //a.onDeug();

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new SMCServicio()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
