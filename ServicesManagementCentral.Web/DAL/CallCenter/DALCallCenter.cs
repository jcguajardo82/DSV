using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ServicesManagement.Web.DAL.CallCenter
{
    public class DALCallCenter
    {
        public static DataSet up_Corp_cns_EstatusRma(DateTime FecIni, DateTime FecFin, int? OrderId)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@FecIni", FecIni);
                parametros.Add("@FecFin", FecFin);
                if (OrderId != null)
                    parametros.Add("@OrderId", OrderId);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[up_Corp_cns_EstatusRma]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        public static DataSet tbl_OrdenFotosRMA_sUp( int ordenRMA)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@ordenRMA", ordenRMA);
               

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[tbl_OrdenFotosRMA_sUp]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        #region Historial
        public static DataSet upCorpOms_Cns_OrdersByHistorical(int OrderNo)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@OrderNo", OrderNo);


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOms_Cns_OrdersByHistorical]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        public static DataSet up_Corp_ins_tbl_OrdenCancelada(string orderId
            , string accion, string origen, int clientId, string clientEmail, string clientPhone, int? EstatusRma, int? ProcesoAut
            , int? IdTSolicitud, int? IdTmovimiento, string jsonRequest = "")
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@orderId", orderId);
                parametros.Add("@accion", accion);
                parametros.Add("@origen", origen);
                parametros.Add("@clientId", clientId);
                parametros.Add("@clientEmail", clientEmail);
                parametros.Add("@clientPhone", clientPhone);
                parametros.Add("@jsonRequest", jsonRequest);
                if (EstatusRma != null)
                    parametros.Add("@EstatusRma", EstatusRma);
                if (ProcesoAut != null)
                    parametros.Add("@ProcesoAut", ProcesoAut);
                if (IdTSolicitud != null)
                    parametros.Add("@IdTSolicitud", IdTSolicitud);
                if (IdTmovimiento != null)
                    parametros.Add("@IdTmovimiento", IdTmovimiento);



                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[up_Corp_ins_tbl_OrdenCancelada]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }


        public static DataSet up_Corp_ins_tbl_OrdenCancelada_Detalle(int Id_cancelacion
            , string orderId, string shipmentId, int position, decimal newProductQuantity, decimal productId, string cancellationReason = "", string cancellationComment = "")
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@Id_cancelacion", Id_cancelacion);
                parametros.Add("@orderId", orderId);
                parametros.Add("@shipmentId", shipmentId);
                parametros.Add("@position", position);
                parametros.Add("@newProductQuantity", newProductQuantity);
                parametros.Add("@cancellationReason", cancellationReason);
                parametros.Add("@cancellationComment", cancellationComment);
                parametros.Add("@productId", productId);


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[up_Corp_ins_tbl_OrdenCancelada_Detalle]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }


        public static DataSet up_Corp_ins_tbl_OrdenRetorno_Detalle(int Id_cancelacion
         , string orderId, string shipmentId, int position, int newProductQuantity, decimal productId, string returnReason = ""
            , string returnComment = "", bool isBonusProduct = false, decimal parentProductID = 0)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@Id_cancelacion", Id_cancelacion);
                parametros.Add("@orderId", orderId);
                parametros.Add("@shipmentId", shipmentId);
                parametros.Add("@position", position);
                parametros.Add("@newProductQuantity", newProductQuantity);
                parametros.Add("@isBonusProduct", isBonusProduct);
                parametros.Add("@parentProductID", parentProductID);
                parametros.Add("@returnReason", returnReason);
                parametros.Add("@returnComment", returnComment);
                parametros.Add("@productId", productId);









                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[up_Corp_ins_tbl_OrdenRetorno_Detalle]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }




        public static DataSet up_Corp_cns_OrderInfo(int OrderNo)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@OrderNo", OrderNo);


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[up_Corp_cns_OrderInfo]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        public static DataSet MotivosRMAById_sUp(int Id_Padre)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@Id_Padre", Id_Padre);


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[MotivosRMAById_sUp]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }



        public static DataSet Catalogo_Checklist_iUP(int Id_Cancelacion, int Id_Pregunta, int ProductId, bool Respuesta)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@Id_Cancelacion", Id_Cancelacion);
                parametros.Add("@Id_Pregunta", Id_Pregunta);
                parametros.Add("@ProductId", ProductId);
                parametros.Add("@Respuesta", Respuesta);


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[Catalogo_Checklist_iUP]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        #endregion


        #region HistoriaSF
        public static DataSet upCorpOms_Cns_OrdersByDatesSF(DateTime FecIni, DateTime FecFin, int? OrderId)
        {
            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);

                System.Collections.Hashtable parametros = new System.Collections.Hashtable();
                /*parametros.Add("@ViewType", 1);*/ // parametro admin fijo
                /*parametros.Add("@Seccion", 0);*/ // parametro admin fijo
                /* parametros.Add("@usuario", usuario);*/ // parametro admin fijo
                parametros.Add("@fechaini", FecIni); // parametro admin fijo
                parametros.Add("@fechafin", FecFin); // parametro admin fijo

                if (OrderId != null)
                    parametros.Add("@OrderId", OrderId);


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOms_Cns_OrdersByDatesSF_V2]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }


        public static DataSet upCorpOms_Cns_OrdersByHistoricalSF(int OrderNo, int Id_Cancelacion)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@OrderNo", OrderNo);
                parametros.Add("@Id_Cancelacion", Id_Cancelacion);


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOms_Cns_OrdersByHistoricalSF]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }


        public static DataSet tbl_OrdenCancelada_uUp(int Id_Cancelacion, int IdTSolicitud,int IdTmovimiento)
        {

          DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@Id_Cancelacion", Id_Cancelacion);
                parametros.Add("@IdTSolicitud", IdTSolicitud);
                parametros.Add("@IdTmovimiento", IdTmovimiento);
               

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[tbl_OrdenCancelada_uUp]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }
        #endregion

        #region Alta Pedido

        public static DataSet DirCte_iUp(
            int Id_Num_Cte, int Id_Num_DirTipo,int Ids_Num_Edo,
            string Calle, string Nom_DirCTe, string Num_Ext, string Num_Int,
            string Ciudad, string Cod_Postal, string Colonia, string Telefono,
            string Latitud="", string Longitud=""
            )
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@Id_Num_Cte", Id_Num_Cte);

                parametros.Add("@Id_Num_DirTipo", Id_Num_DirTipo);
                parametros.Add("@Ids_Num_Edo", Ids_Num_Edo);
                parametros.Add("@Calle", Calle);
                parametros.Add("@Nom_DirCTe", Nom_DirCTe);
                parametros.Add("@Num_Ext", Num_Ext);
                parametros.Add("@Num_Int", Num_Int);
                parametros.Add("@Ciudad", Ciudad);
                parametros.Add("@Cod_Postal", Cod_Postal);
                parametros.Add("@Colonia", Colonia);
                parametros.Add("@Telefono", Telefono);
                parametros.Add("@Latitud", Latitud);
                parametros.Add("@Longitud", Longitud);



                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[carrito].[DirCte_iUp]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        public static DataSet Email_iUp(int Id_Num_Cte,string Id_Email,string Cve_Acceso = "")
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@Id_Num_Cte", Id_Num_Cte);
                parametros.Add("@Id_Email", Id_Email);
                parametros.Add("@Cve_Acceso", Cve_Acceso);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[carrito].[Email_iUp]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        public static DataSet Cte_iUp( string Nom_Cte,string Ap_Paterno,string Ap_Materno, string Pregunta = "",string Cve_Respuesta = "")
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@Nom_Cte", Nom_Cte);
                parametros.Add("@Ap_Materno", Ap_Materno);
                parametros.Add("@Ap_Paterno", Ap_Paterno);
                parametros.Add("@Pregunta", Pregunta);
                parametros.Add("@Cve_Respuesta", Cve_Respuesta);







                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[carrito].[Cte_iUp]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        public static DataSet GetClientByPhoneEmail(string Criterio)
        {
        
            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@Criterio", Criterio);


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[carrito].[GetClientByPhoneEmail]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        public static DataSet GetClientByName(string Criterio)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@Criterio", Criterio);


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[carrito].[GetClientByName]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        public static DataSet GetDirCteIdNumCte(int Id_Num_Cte)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@Id_Num_Cte", Id_Num_Cte);


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[carrito].[GetDirDirCteIdNumCte]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        public static DataSet GetDirDirCteIdDirCTe(int Id_Cnsc_DirCTe)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@Id_Cnsc_DirCTe", Id_Cnsc_DirCTe);


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[carrito].[GetDirDirCteIdDirCTe]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        public static DataSet Estado_d_sUp(int Id_Num_Pais)
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@Id_Num_Pais", Id_Num_Pais);


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[carrito].[Estado_d_sUp]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        public static DataSet Car_iUp(int Id_Num_UN,int Id_Num_Apl=21, int Id_Num_Visita=0)
        {

         DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@Id_Num_Visita", Id_Num_Visita);
                parametros.Add("@Id_Num_Apl", Id_Num_Apl);
                parametros.Add("@Id_Num_UN", Id_Num_UN);


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[carrito].[Car_iUp]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        public static DataSet ArtCar_d_iUp(
            int Id_Num_Car, int id_Num_Sku ,  decimal Cant_Unidades, 
            decimal Precio_VtaNormal, decimal Precio_VtaOferta, decimal Dcto,
            int Id_Num_ArtCar_Tipo = 8
            )

        {


            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@Id_Num_Car", Id_Num_Car);
                parametros.Add("@id_Num_Sku", id_Num_Sku);
                parametros.Add("@Cant_Unidades", Cant_Unidades);
                parametros.Add("@Precio_VtaNormal", Precio_VtaNormal);
                parametros.Add("@Precio_VtaOferta", Precio_VtaOferta);
                parametros.Add("@Dcto", Dcto);
                parametros.Add("@Id_Num_ArtCar_Tipo", Id_Num_ArtCar_Tipo);


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[carrito].[ArtCar_d_iUp]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }


        public static DataSet ArtCar_Obser_iUp(int Id_Num_Car, int id_Num_Sku , string Desc_ArtCarObser )
        {

            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@Id_Num_Car", Id_Num_Car);
                parametros.Add("@id_Num_Sku", id_Num_Sku);
                parametros.Add("@Desc_ArtCarObser", Desc_ArtCarObser);


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[carrito].[ArtCar_Obser_iUp]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        public static DataSet CarObs_iUp(int Id_Num_Car, string Desc_CarObs)
        {


           DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@Id_Num_Car", Id_Num_Car);
                parametros.Add("@Desc_CarObs", Desc_CarObs);


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[carrito].[CarObs_iUp]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        public static DataSet Orden_iUp(int Id_Num_Car, int Id_Num_Cte, string id_Num_SrvEntrega)
        {

         DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@Id_Num_Car", Id_Num_Car);
                parametros.Add("@Id_Num_Cte", Id_Num_Cte);
                parametros.Add("@id_Num_SrvEntrega", id_Num_SrvEntrega);


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[carrito].[Orden_iUp]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        public static DataSet CalEntrega_iUp(int id_Num_SrvEntrega, int Id_Num_Un, int Id_Num_Orden,DateTime Fec_Entrega)
        {





           DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@id_Num_SrvEntrega", id_Num_SrvEntrega);
                parametros.Add("@Id_Num_Un", Id_Num_Un);
                parametros.Add("@Fec_Entrega", Fec_Entrega);


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[carrito].[Orden_iUp]", false, parametros);

                return ds;
            }
            catch (SqlException ex)
            {

                throw ex;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }
        
        #endregion
    }
}