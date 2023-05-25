using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using ServicioSincronizador.SAP;
using ServicioSincronizador.DTO;
using ServicioSincronizador.DAO;

namespace ServicioSincronizador
{
    public partial class SMCServicio : ServiceBase
    {
        private System.Timers.Timer timer;
        private string mensaje = "";
        bool flagProceso = false;
        //ConexionSBO _conexionSBO = null;

        public SMCServicio()
        {
            InitializeComponent();

            eventLogSMCServ = new EventLog();
            if (!EventLog.SourceExists("SMC_SERV"))
            {
                EventLog.CreateEventSource("SMC_SERV", "SMC_SERV_LOG");
            }
            eventLogSMCServ.Source = "SMC_SERV";
            eventLogSMCServ.Log = "SMC_SERV_LOG";

        }


        //public void onDeug()
        //{
        //    OnStart(null);
        //    timer_Elpased();
        //}


        protected override void OnStart(string[] args)
        {


            timer = new System.Timers.Timer(Convert.ToInt32(ConfigurationManager.AppSettings["Intervalo"]));
            timer.AutoReset = true;
            timer.Elapsed += new ElapsedEventHandler(timer_Elpased);
            timer.Start();


            eventLogSMCServ.WriteEntry("SMC_SERV se ha iniciado", EventLogEntryType.SuccessAudit, 3);
            //timer_Elpased();

        }

        protected override void OnStop()
        {
            timer.Stop();
            timer = null;
            GC.Collect();
            GC.WaitForFullGCComplete();
            eventLogSMCServ.WriteEntry( "SINCRONIZADOR => El servicio se ha detenido.", EventLogEntryType.SuccessAudit, 3);
        }



        private void timer_Elpased(object sender, ElapsedEventArgs ev)
        //private void timer_Elpased()
        {
            if (flagProceso) return;

            flagProceso = true;

            try
            {
                ConsultasHana hana = new ConsultasHana();
                ConsultasSQL sql = new ConsultasSQL();
                List<BaseDatos> Bd = new List<BaseDatos>();

                var Sociedades = sql.ObtenerSociedades();

                //Bd.Add(new BaseDatos
                //{
                //    Id = "7",
                //    Nombre = "SBO_TITAN_LUIS"
                //}
                //);

                List<EntradaMercanciaDTO> EntradaSAP = new List<EntradaMercanciaDTO>();
                List<SalidaMercanciaDTO> SalidaSAP = new List<SalidaMercanciaDTO>();
                List<TranferenciasDTO> TranferenciaSAP = new List<TranferenciasDTO>();
                List<SolicitudRQDetalleDTO> SolicitudDetalleRQ = new List<SolicitudRQDetalleDTO>();



                List<EntradaMercanciaDTO> EntradaCanceladaSAP = new List<EntradaMercanciaDTO>();
                List<SolicitudCompraSapDTO> SolicitudCompraSapCancelada = new List<SolicitudCompraSapDTO>();
                List<SolicitudRQDetalleDTO> SolicitudDetalleRQSincronizado = new List<SolicitudRQDetalleDTO>();

               
                foreach (var item in Sociedades) // obtengo datos por sociedad
                {

                    EntradaSAP = hana.ListarEntradasMercancia(item.Bd); //entradas SAP
                    SalidaSAP = hana.ListarSalidasMercancia(item.Bd); //salida SAP
                    TranferenciaSAP = hana.ListarTranferencia(item.Bd, item.AlmacenIntermedio); //Tranferencias SAP
                    SolicitudDetalleRQ = sql.ObtenerSolicitudDetalle(int.Parse(item.Id)); //solicitudrq SQL


                    //validando items entradas de mercancia con items addon
                    for (int i = 0; i < EntradaSAP.Count(); i++)
                    {

                        for (int j = 0; j < SolicitudDetalleRQ.Count; j++)
                        {
                            if (EntradaSAP[i].SolicitudRQ == SolicitudDetalleRQ[j].Numero) //valida el numero de mi RQ
                            {
                                if (EntradaSAP[i].ItemCode == SolicitudDetalleRQ[j].ItemCode &&  //valida los datos iguales itemscode,almacen,cc
                                                                                                 //EntradaSAP[i].Almacen == SolicitudDetalleRQ[j].Almacen &&
                                    EntradaSAP[i].CentroCosto == SolicitudDetalleRQ[j].CentroCosto)
                                {
                                    if (EntradaSAP[i].Cantidad >= SolicitudDetalleRQ[j].Cantidad) //valida sus cantidades
                                    {
                                        int rpt = sql.ActualizarSincronizadoDetalle(SolicitudDetalleRQ[j].IdDetalle);
                                        int rpt2 = hana.ActualizarSincronizadorDetalle(item.Bd, SolicitudDetalleRQ[j].Numero, SolicitudDetalleRQ[j].ItemCode, EntradaSAP[i].Almacen, SolicitudDetalleRQ[j].CentroCosto, "E");

                                        int rpt3 = sql.ValidaActualizaEstadoCabecera(SolicitudDetalleRQ[j].IdDetalle, int.Parse(item.Id));
                                    }
                                }
                            }
                        }
                    }





                    //validando items entradas de mercancia con items addon
                    for (int i = 0; i < SalidaSAP.Count(); i++)
                    {

                        for (int j = 0; j < SolicitudDetalleRQ.Count; j++)
                        {
                            if (SalidaSAP[i].SolicitudRQ == SolicitudDetalleRQ[j].Numero) //valida el numero de mi RQ
                            {
                                if (SalidaSAP[i].ItemCode == SolicitudDetalleRQ[j].ItemCode &&  //valida los datos iguales itemscode,almacen,cc
                                                                                                //EntradaSAP[i].Almacen == SolicitudDetalleRQ[j].Almacen &&
                                    SalidaSAP[i].CentroCosto == SolicitudDetalleRQ[j].CentroCosto)
                                {
                                    if (SalidaSAP[i].Cantidad >= SolicitudDetalleRQ[j].Cantidad) //valida sus cantidades
                                    {
                                        int rpt = sql.ActualizarSincronizadoDetalle(SolicitudDetalleRQ[j].IdDetalle);
                                        int rpt2 = hana.ActualizarSincronizadorDetalle(item.Bd, SolicitudDetalleRQ[j].Numero, SolicitudDetalleRQ[j].ItemCode, SalidaSAP[i].Almacen, SolicitudDetalleRQ[j].CentroCosto, "S");

                                        int rpt3 = sql.ValidaActualizaEstadoCabecera(SolicitudDetalleRQ[j].IdDetalle, int.Parse(item.Id));
                                    }
                                }
                            }
                        }
                    }







                    //validando items tranferencia de items con items addon
                    for (int i = 0; i < TranferenciaSAP.Count(); i++)
                    {
                        for (int j = 0; j < SolicitudDetalleRQ.Count; j++)
                        {
                            if (TranferenciaSAP[i].SolicitudRQ == SolicitudDetalleRQ[j].Numero) //valida el numero de mi RQ
                            {
                                if (TranferenciaSAP[i].ItemCode == SolicitudDetalleRQ[j].ItemCode &&  //valida los datos iguales itemscode,almacen,cc
                                    TranferenciaSAP[i].Almacen == SolicitudDetalleRQ[j].Almacen &&
                                    TranferenciaSAP[i].CentroCosto == SolicitudDetalleRQ[j].CentroCosto)
                                {
                                    if (TranferenciaSAP[i].Cantidad == SolicitudDetalleRQ[j].Cantidad) //valida sus cantidades
                                    {
                                        int rpt = sql.ActualizarSincronizadoDetalle(SolicitudDetalleRQ[j].IdDetalle);
                                        int rpt2 = hana.ActualizarSincronizadorDetalle(item.Bd, SolicitudDetalleRQ[j].Numero, SolicitudDetalleRQ[j].ItemCode, SolicitudDetalleRQ[j].Almacen, SolicitudDetalleRQ[j].CentroCosto, "T");

                                        int rpt3 = sql.ValidaActualizaEstadoCabecera(SolicitudDetalleRQ[j].IdDetalle, int.Parse(item.Id));
                                    }
                                }
                            }
                        }
                    }





                    var Validacion = hana.ValidarSincronizadorDetalle(item.Bd, item.AlmacenIntermedio);

                    for (int i = 0; i < Validacion.Count(); i++)
                    {
                        if (Validacion[i].Tipo == "T")
                        {
                            if (Validacion[i].ContadorTerminados == Validacion[i].ContadorTotales)
                            {
                                int Tranfer = hana.ActualizarSincronizadorCabecera(item.Bd, Validacion[i].DocEntry, "T");
                            }
                        }


                        if (Validacion[i].Tipo == "E")
                        {
                            if (Validacion[i].ContadorTerminados == Validacion[i].ContadorTotales)
                            {
                                int Entrada = hana.ActualizarSincronizadorCabecera(item.Bd, Validacion[i].DocEntry, "E");
                            }
                        }


                        if (Validacion[i].Tipo == "S")
                        {
                            if (Validacion[i].ContadorTerminados == Validacion[i].ContadorTotales)
                            {
                                int Entrada = hana.ActualizarSincronizadorCabecera(item.Bd, Validacion[i].DocEntry, "S");
                            }
                        }

                    }









                    //cancelaciones
                    EntradaCanceladaSAP = hana.ListarEntradasMercanciaCanceladas(item.Bd);
                    SolicitudCompraSapCancelada = hana.ListarSolicitudCompraSapCancelada(item.Bd);
                    SolicitudDetalleRQSincronizado = sql.ObtenerSolicitudDetalleSincronizado(int.Parse(item.Id)); //solicitudrq SQL


                    //entradas canceladas
                    for (int i = 0; i < EntradaCanceladaSAP.Count; i++)
                    {
                        for (int j = 0; j < SolicitudDetalleRQSincronizado.Count; j++)
                        {
                            if (EntradaCanceladaSAP[i].SolicitudRQ == SolicitudDetalleRQSincronizado[j].Numero) //valida el numero de mi RQ
                            {
                                if (EntradaCanceladaSAP[i].ItemCode == SolicitudDetalleRQSincronizado[j].ItemCode &&  //valida los datos iguales itemscode,almacen,cc
                                                                                                          //EntradaSAP[i].Almacen == SolicitudDetalleRQ[j].Almacen &&
                                    EntradaCanceladaSAP[i].CentroCosto == SolicitudDetalleRQSincronizado[j].CentroCosto)
                                {
                                    if (EntradaCanceladaSAP[i].Cantidad == SolicitudDetalleRQSincronizado[j].Cantidad) //valida sus cantidades
                                    {
                                        //actualiza sincronizador a 0
                                        int rpt = sql.ActualizarSincronizadoDetalleCancelado(SolicitudDetalleRQSincronizado[j].IdDetalle);
                                        int rpt2 = hana.ActualizarSincronizadorDetalleCancelado(item.Bd, SolicitudDetalleRQSincronizado[j].Numero, SolicitudDetalleRQSincronizado[j].ItemCode, EntradaCanceladaSAP[i].Almacen, SolicitudDetalleRQSincronizado[j].CentroCosto, "E");

                                        int rpt3 = sql.ValidaActualizaEstadoCabeceraCancelado(SolicitudDetalleRQSincronizado[j].IdDetalle, int.Parse(item.Id));
                                    }
                                }
                            }
                        }
                    }





                    for (int i = 0; i < SolicitudCompraSapCancelada.Count; i++)
                    {
                        for (int j = 0; j < SolicitudDetalleRQSincronizado.Count; j++)
                        {
                            if (SolicitudCompraSapCancelada[i].NumeroSolicitud == SolicitudDetalleRQSincronizado[j].Numero) //valida el numero de mi RQ
                            {
                                if (SolicitudCompraSapCancelada[i].ItemCode == SolicitudDetalleRQSincronizado[j].ItemCode &&  //valida los datos iguales itemscode,almacen,cc
                                                                                                                              //EntradaSAP[i].Almacen == SolicitudDetalleRQ[j].Almacen &&
                                    SolicitudCompraSapCancelada[i].CentroCosto == SolicitudDetalleRQSincronizado[j].CentroCosto)
                                {
                                    if (SolicitudCompraSapCancelada[i].Cantidad == SolicitudDetalleRQSincronizado[j].Cantidad) //valida sus cantidades
                                    {
                                        //actualiza sincronizador a 0
                                        int rpt = sql.ActualizarSincronizadoDetalleCancelado(SolicitudDetalleRQSincronizado[j].IdDetalle);
                                        int rpt2 = sql.ValidarItemSolicitudCancelada(SolicitudDetalleRQSincronizado[j].IdDetalle, int.Parse(item.Id));
                                        ////int rpt2 = hana.ActualizarSincronizadorDetalle(item.Bd, SolicitudDetalleRQ[j].Numero, SolicitudDetalleRQ[j].ItemCode, EntradaSAP[i].Almacen, SolicitudDetalleRQ[j].CentroCosto, "E");

                                        ////int rpt3 = sql.ValidaActualizaEstadoCabecera(SolicitudDetalleRQ[j].IdDetalle, int.Parse(item.Id));
                                    }
                                }
                            }
                        }
                    }
                    






                }


               

         






              
            }
            catch (Exception e)
            {
                string err = e.ToString();
            }

            flagProceso = false;
        }


        private void eventLogSMCServ_EntryWritten(object sender, EntryWrittenEventArgs e)
        {

        }

    }
}
