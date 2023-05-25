using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ServicioSincronizador.DTO;

namespace ServicioSincronizador.DAO
{
    public class ConsultasSQL
    {

        public List<SolicitudRQDetalleDTO> ObtenerSolicitudDetalle(int IdSociedad)
        {
            List<SolicitudRQDetalleDTO> lstSolicitudRQDetalleDTO = new List<SolicitudRQDetalleDTO>();
            using (SqlConnection cn = new Conexion().conectar())
            {
                try
                {
                    cn.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SMC_ListarSolicitudRQDetalle", cn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@IdSociedad", IdSociedad);
                    SqlDataReader drd = da.SelectCommand.ExecuteReader();
                    while (drd.Read())
                    {
                        SolicitudRQDetalleDTO oSolicitudRQDetalleDTO = new SolicitudRQDetalleDTO();
                        oSolicitudRQDetalleDTO.IdDetalle = int.Parse(drd["IdDetalle"].ToString());
                        oSolicitudRQDetalleDTO.Numero = int.Parse(drd["Numero"].ToString());
                        oSolicitudRQDetalleDTO.ItemCode = drd["IdArticulo"].ToString();
                        oSolicitudRQDetalleDTO.Descripcion = drd["Descripcion"].ToString();
                        oSolicitudRQDetalleDTO.Almacen = drd["IdAlmacen"].ToString();
                        oSolicitudRQDetalleDTO.CentroCosto = drd["IdCentroCostos"].ToString();
                        oSolicitudRQDetalleDTO.Cantidad = decimal.Parse(drd["CantidadNecesaria"].ToString());
                        lstSolicitudRQDetalleDTO.Add(oSolicitudRQDetalleDTO);
                    }
                    drd.Close();


                }
                catch (Exception ex)
                {
                }
            }
            return lstSolicitudRQDetalleDTO;
        }




        public List<SolicitudRQDetalleDTO> ObtenerSolicitudDetalleSincronizado(int IdSociedad)
        {
            List<SolicitudRQDetalleDTO> lstSolicitudRQDetalleDTO = new List<SolicitudRQDetalleDTO>();
            using (SqlConnection cn = new Conexion().conectar())
            {
                try
                {
                    cn.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SMC_ListarSolicitudRQDetalleSincronizados", cn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand.Parameters.AddWithValue("@IdSociedad", IdSociedad);
                    SqlDataReader drd = da.SelectCommand.ExecuteReader();
                    while (drd.Read())
                    {
                        SolicitudRQDetalleDTO oSolicitudRQDetalleDTO = new SolicitudRQDetalleDTO();
                        oSolicitudRQDetalleDTO.IdDetalle = int.Parse(drd["IdDetalle"].ToString());
                        oSolicitudRQDetalleDTO.Numero = int.Parse(drd["Numero"].ToString());
                        oSolicitudRQDetalleDTO.ItemCode = drd["IdArticulo"].ToString();
                        oSolicitudRQDetalleDTO.Descripcion = drd["Descripcion"].ToString();
                        oSolicitudRQDetalleDTO.Almacen = drd["IdAlmacen"].ToString();
                        oSolicitudRQDetalleDTO.CentroCosto = drd["IdCentroCostos"].ToString();
                        oSolicitudRQDetalleDTO.Cantidad = decimal.Parse(drd["CantidadNecesaria"].ToString());
                        lstSolicitudRQDetalleDTO.Add(oSolicitudRQDetalleDTO);
                    }
                    drd.Close();


                }
                catch (Exception ex)
                {
                }
            }
            return lstSolicitudRQDetalleDTO;
        }


        public int ActualizarSincronizadoDetalle(int IdDetalle)
        {
            TransactionOptions transactionOptions = default(TransactionOptions);
            transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            transactionOptions.Timeout = TimeSpan.FromSeconds(60.0);
            TransactionOptions option = transactionOptions;
            using (SqlConnection cn = new Conexion().conectar())
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, option))
                {
                    try
                    {
                        cn.Open();
                        SqlDataAdapter da = new SqlDataAdapter("SMC_ActualizarSincronizadoDetalle", cn);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.Parameters.AddWithValue("@IdDetalle", IdDetalle);
                        int rpta = Convert.ToInt32(da.SelectCommand.ExecuteScalar());
                        transactionScope.Complete();
                        return rpta;
                    }
                    catch (Exception ex)
                    {
                        return -1;
                    }
                }
            }
        }



        public int ActualizarSincronizadoDetalleCancelado(int IdDetalle)
        {
            TransactionOptions transactionOptions = default(TransactionOptions);
            transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            transactionOptions.Timeout = TimeSpan.FromSeconds(60.0);
            TransactionOptions option = transactionOptions;
            using (SqlConnection cn = new Conexion().conectar())
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, option))
                {
                    try
                    {
                        cn.Open();
                        SqlDataAdapter da = new SqlDataAdapter("SMC_ActualizarSincronizadoDetalleCancelado", cn);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.Parameters.AddWithValue("@IdDetalle", IdDetalle);
                        int rpta = Convert.ToInt32(da.SelectCommand.ExecuteScalar());
                        transactionScope.Complete();
                        return rpta;
                    }
                    catch (Exception ex)
                    {
                        return -1;
                    }
                }
            }
        }







        public int ValidaActualizaEstadoCabecera(int IdDetalle,int IdSociedad)
        {
            TransactionOptions transactionOptions = default(TransactionOptions);
            transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            transactionOptions.Timeout = TimeSpan.FromSeconds(60.0);
            TransactionOptions option = transactionOptions;
            using (SqlConnection cn = new Conexion().conectar())
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, option))
                {
                    try
                    {
                        cn.Open();
                        SqlDataAdapter da = new SqlDataAdapter("SMC_ValidaActualizaEstadoCabecera", cn);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.Parameters.AddWithValue("@IdDetalle", IdDetalle);
                        da.SelectCommand.Parameters.AddWithValue("@IdSociedad", IdSociedad);
                        int rpta = Convert.ToInt32(da.SelectCommand.ExecuteScalar());
                        transactionScope.Complete();
                        return rpta;
                    }
                    catch (Exception ex)
                    {
                        return -1;
                    }
                }
            }
        }





        public int ValidaActualizaEstadoCabeceraCancelado(int IdDetalle, int IdSociedad)
        {
            TransactionOptions transactionOptions = default(TransactionOptions);
            transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            transactionOptions.Timeout = TimeSpan.FromSeconds(60.0);
            TransactionOptions option = transactionOptions;
            using (SqlConnection cn = new Conexion().conectar())
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, option))
                {
                    try
                    {
                        cn.Open();
                        SqlDataAdapter da = new SqlDataAdapter("SMC_ValidaActualizaEstadoCabeceraCancelado", cn);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.Parameters.AddWithValue("@IdDetalle", IdDetalle);
                        da.SelectCommand.Parameters.AddWithValue("@IdSociedad", IdSociedad);
                        int rpta = Convert.ToInt32(da.SelectCommand.ExecuteScalar());
                        transactionScope.Complete();
                        return rpta;
                    }
                    catch (Exception ex)
                    {
                        return -1;
                    }
                }
            }
        }




        public int ValidarItemSolicitudCancelada(int IdDetalle, int IdSociedad)
        {
            TransactionOptions transactionOptions = default(TransactionOptions);
            transactionOptions.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            transactionOptions.Timeout = TimeSpan.FromSeconds(60.0);
            TransactionOptions option = transactionOptions;
            using (SqlConnection cn = new Conexion().conectar())
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, option))
                {
                    try
                    {
                        cn.Open();
                        SqlDataAdapter da = new SqlDataAdapter("SMC_ValidarItemsSolicitudCancelada", cn);
                        da.SelectCommand.CommandType = CommandType.StoredProcedure;
                        da.SelectCommand.Parameters.AddWithValue("@idSolicitudRQDetalle", IdDetalle);
                        da.SelectCommand.Parameters.AddWithValue("@IdSociedad", IdSociedad);
                        int rpta = Convert.ToInt32(da.SelectCommand.ExecuteScalar());
                        transactionScope.Complete();
                        return rpta;
                    }
                    catch (Exception ex)
                    {
                        return -1;
                    }
                }
            }
        }






        public List<SociedadDTO> ObtenerSociedades()
        {
            List<SociedadDTO> lstSociedadDTO = new List<SociedadDTO>();
            using (SqlConnection cn = new Conexion().conectar())
            {
                try
                {
                    cn.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SMC_ListarSociedades", cn);
                    da.SelectCommand.CommandType = CommandType.StoredProcedure;
                    SqlDataReader drd = da.SelectCommand.ExecuteReader();
                    while (drd.Read())
                    {
                        SociedadDTO oSociedadDTO = new SociedadDTO();
                        oSociedadDTO.Id = drd["Id"].ToString();
                        oSociedadDTO.Bd = drd["NombreBd"].ToString();
                        oSociedadDTO.AlmacenIntermedio = (drd["AlmacenIntermedio"].ToString() == null)?"": drd["AlmacenIntermedio"].ToString();
                        lstSociedadDTO.Add(oSociedadDTO);
                    }
                    drd.Close();


                }
                catch (Exception ex)
                {
                }
            }
            return lstSociedadDTO;
        }




    }
}
