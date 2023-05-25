using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using ServicioSincronizador.DTO;

namespace ServicioSincronizador.SAP
{
    public class ConsultasHana
    {

        public List<EntradaMercanciaDTO> ListarEntradasMercancia (string SapComanyDB)
        {
            List<EntradaMercanciaDTO> lista = new List<EntradaMercanciaDTO>();
            EntradaMercanciaDTO Items;


            Database db = DatabaseFactory.CreateDatabase("SMCHANA");
            DbCommand CommandEmpresa = db.GetStoredProcCommand(SapComanyDB + ".\"SMC_SINCRONIZADOR_ENTRADAMERCANCIA\"");

            using (IDataReader dataReader = db.ExecuteReader(CommandEmpresa))
            {
                while (dataReader.Read())
                {
                    Items = new EntradaMercanciaDTO();
                    //Items.DocEntry = dataReader["DocEntry"].ToString();
                    Items.ItemCode = dataReader["ItemCode"].ToString();
                    Items.Descripcion = dataReader["Dscription"].ToString();
                    Items.SolicitudRQ = int.Parse(dataReader["U_SMC_SOLICITUDRQ"].ToString());
                    Items.EstadoSincronizador = dataReader["U_SMC_SINCRONIZADOR"].ToString();
                    Items.Cantidad = decimal.Parse(dataReader["Quantity"].ToString());
                    Items.Almacen = dataReader["WhsCode"].ToString();
                    Items.CentroCosto = dataReader["OcrCode"].ToString();
                    lista.Add(Items);
                }
            }

            return lista;
        }




        public List<EntradaMercanciaDTO> ListarEntradasMercanciaCanceladas(string SapComanyDB)
        {
            List<EntradaMercanciaDTO> lista = new List<EntradaMercanciaDTO>();
            EntradaMercanciaDTO Items;


            Database db = DatabaseFactory.CreateDatabase("SMCHANA");
            DbCommand CommandEmpresa = db.GetStoredProcCommand(SapComanyDB + ".\"SMC_SINCRONIZADOR_ENTRADAMERCANCIA_CANCELADA\"");

            using (IDataReader dataReader = db.ExecuteReader(CommandEmpresa))
            {
                while (dataReader.Read())
                {
                    Items = new EntradaMercanciaDTO();
                    //Items.DocEntry = dataReader["DocEntry"].ToString();
                    Items.ItemCode = dataReader["ItemCode"].ToString();
                    Items.Descripcion = dataReader["Dscription"].ToString();
                    Items.SolicitudRQ = int.Parse(dataReader["U_SMC_SOLICITUDRQ"].ToString());
                    Items.EstadoSincronizador = dataReader["U_SMC_SINCRONIZADORDET"].ToString();
                    Items.Cantidad = decimal.Parse(dataReader["Quantity"].ToString());
                    Items.Almacen = dataReader["WhsCode"].ToString();
                    Items.CentroCosto = dataReader["OcrCode"].ToString();
                    lista.Add(Items);
                }
            }

            return lista;
        }



        public List<SolicitudCompraSapDTO> ListarSolicitudCompraSapCancelada(string SapComanyDB)
        {
            List<SolicitudCompraSapDTO> lista = new List<SolicitudCompraSapDTO>();
            SolicitudCompraSapDTO Items;


            Database db = DatabaseFactory.CreateDatabase("SMCHANA");
            DbCommand CommandEmpresa = db.GetStoredProcCommand(SapComanyDB + ".\"SMC_SINCRONIZADOR_SOLICITUDCOMPRA_CANCELADA\"");

            using (IDataReader dataReader = db.ExecuteReader(CommandEmpresa))
            {
                while (dataReader.Read())
                {
                    Items = new SolicitudCompraSapDTO();
                    //Items.DocEntry = dataReader["DocEntry"].ToString();
                    Items.ItemCode = dataReader["ItemCode"].ToString();
                    Items.Descripcion = dataReader["Dscription"].ToString();
                    Items.NumeroSolicitud = int.Parse(dataReader["U_SMC_SOLICITUDRQ"].ToString());
                    Items.Cantidad = decimal.Parse(dataReader["Quantity"].ToString());
                    Items.Almacen = dataReader["WhsCode"].ToString();
                    Items.CentroCosto = dataReader["OcrCode"].ToString();
                    lista.Add(Items);
                }
            }

            return lista;
        }




        public List<SalidaMercanciaDTO> ListarSalidasMercancia(string SapComanyDB)
        {
            List<SalidaMercanciaDTO> lista = new List<SalidaMercanciaDTO>();
            SalidaMercanciaDTO Items;


            Database db = DatabaseFactory.CreateDatabase("SMCHANA");
            DbCommand CommandEmpresa = db.GetStoredProcCommand(SapComanyDB + ".\"SMC_SINCRONIZADOR_SALIDAMERCANCIA\"");

            using (IDataReader dataReader = db.ExecuteReader(CommandEmpresa))
            {
                while (dataReader.Read())
                {
                    Items = new SalidaMercanciaDTO();
                    //Items.DocEntry = dataReader["DocEntry"].ToString();
                    Items.ItemCode = dataReader["ItemCode"].ToString();
                    Items.Descripcion = dataReader["Dscription"].ToString();
                    Items.SolicitudRQ = int.Parse(dataReader["U_SMC_SOLICITUDRQ"].ToString());
                    Items.EstadoSincronizador = dataReader["U_SMC_SINCRONIZADOR"].ToString();
                    Items.Cantidad = decimal.Parse(dataReader["Quantity"].ToString());
                    Items.Almacen = dataReader["WhsCode"].ToString();
                    Items.CentroCosto = dataReader["OcrCode"].ToString();
                    lista.Add(Items);
                }
            }

            return lista;
        }


        public List<TranferenciasDTO> ListarTranferencia(string SapComanyDB,string AlmacenIntermedio)
        {
            List<TranferenciasDTO> lista = new List<TranferenciasDTO>();
            TranferenciasDTO Items;


            Database db = DatabaseFactory.CreateDatabase("SMCHANA");
            DbCommand CommandEmpresa = db.GetStoredProcCommand(SapComanyDB + ".\"SMC_SINCRONIZADOR_TRANFERENCIA\"('" + AlmacenIntermedio + "')");

            using (IDataReader dataReader = db.ExecuteReader(CommandEmpresa))
            {
                while (dataReader.Read())
                {
                    Items = new TranferenciasDTO();
                    //Items.DocEntry = dataReader["DocEntry"].ToString();
                    Items.ItemCode = dataReader["ItemCode"].ToString();
                    Items.Descripcion = dataReader["Dscription"].ToString();
                    Items.SolicitudRQ = int.Parse(dataReader["U_SMC_SOLICITUDRQ"].ToString());
                    Items.EstadoSincronizador = dataReader["U_SMC_SINCRONIZADOR"].ToString();
                    Items.Cantidad = decimal.Parse(dataReader["Quantity"].ToString());
                    Items.Almacen = dataReader["WhsCode"].ToString();
                    Items.CentroCosto = dataReader["OcrCode"].ToString();
                    lista.Add(Items);
                }
            }

            return lista;
        }








        public int ActualizarSincronizadorDetalle(string SapComanyDB, int SolicitudRQ,string ItemCode,string Almacen,string CentroCosto,string Tipo)
        {

            Database db = DatabaseFactory.CreateDatabase("SMCHANA");
            DbCommand CommandEmpresa = db.GetStoredProcCommand(SapComanyDB + ".\"SMC_ACTUALIZAR_SINCRONIZADORDET\" ('" + SolicitudRQ + "','" + ItemCode + "','" + Almacen + "','" + CentroCosto + "','" + Tipo + "')");
            int Resultado = 0;
            Resultado = db.ExecuteNonQuery(CommandEmpresa);

            if (Resultado > 0)
            {
                Resultado = 1;
            }
            else
            {
                Resultado = 0;
            }

            return Resultado;
        }



        public int ActualizarSincronizadorDetalleCancelado(string SapComanyDB, int SolicitudRQ, string ItemCode, string Almacen, string CentroCosto, string Tipo)
        {

            Database db = DatabaseFactory.CreateDatabase("SMCHANA");
            DbCommand CommandEmpresa = db.GetStoredProcCommand(SapComanyDB + ".\"SMC_ACTUALIZAR_SINCRONIZADORDET_CANCELADO\" ('" + SolicitudRQ + "','" + ItemCode + "','" + Almacen + "','" + CentroCosto + "','" + Tipo + "')");
            int Resultado = 0;
            Resultado = db.ExecuteNonQuery(CommandEmpresa);

            if (Resultado > 0)
            {
                Resultado = 1;
            }
            else
            {
                Resultado = 0;
            }

            return Resultado;
        }



        public int ActualizarSincronizadorCabecera(string SapComanyDB,string DocEntry, string Tipo)
        {

            Database db = DatabaseFactory.CreateDatabase("SMCHANA");
            DbCommand CommandEmpresa = db.GetStoredProcCommand(SapComanyDB + ".\"SMC_ACTUALIZAR_SINCRONIZADORCAB\" ('" + DocEntry + "','" + Tipo + "')");
            int Resultado = 0;
            Resultado = db.ExecuteNonQuery(CommandEmpresa);

            if (Resultado > 0)
            {
                Resultado = 1;
            }
            else
            {
                Resultado = 0;
            }

            return Resultado;
        }




        public List<ValidarSincronizadorDTO> ValidarSincronizadorDetalle(string SapComanyDB, string AlmacenIntermedio)
        {
            List<ValidarSincronizadorDTO> lista = new List<ValidarSincronizadorDTO>();
            ValidarSincronizadorDTO valida;
;
            Database db = DatabaseFactory.CreateDatabase("SMCHANA");
            DbCommand CommandEmpresa = db.GetStoredProcCommand(SapComanyDB + ".\"SMC_VALIDAR_SINCRONIZADORDET\" ('" + AlmacenIntermedio + "')");

            using (IDataReader dataReader = db.ExecuteReader(CommandEmpresa))
            {
                while (dataReader.Read())
                {
                    valida = new ValidarSincronizadorDTO();
                    valida.DocEntry = dataReader["DocEntry"].ToString();
                    valida.ContadorTerminados = dataReader["ContadorTerminados"].ToString();
                    valida.ContadorTotales = dataReader["ContadorTotales"].ToString();
                    valida.Tipo = dataReader["Tipo"].ToString();
                    lista.Add(valida);
                }
            }
            return lista;
        }






    }
}
