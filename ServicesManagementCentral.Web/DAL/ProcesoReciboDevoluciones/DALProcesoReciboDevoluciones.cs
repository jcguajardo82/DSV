using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.DAL.ProcesoReciboDevoluciones
{
    public class DALProcesoReciboDevoluciones
    {
        public static DataSet upCorpOMS_Cns_UeNoDevolEvidencia(string UeNo)
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
                parametros.Add("@UeNo", UeNo);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOMS_Cns_UeNoDevolEvidencia]", false, parametros);

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
        public static DataSet upCorpOms_Ins_UeNoDevolEvidencia(string idUeNo, int idOrderNo, string User, byte[] Evidence)
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
                parametros.Add("@UeNo", idUeNo);
                parametros.Add("@OrderNo", idOrderNo);
                parametros.Add("@Evidence", Evidence);
                parametros.Add("@CreationUser", User);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOms_Ins_UeNoDevolEvidencia]", false, parametros);

                return ds;

                //// Objetos de conexión y comando
                //System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings["Connection_DEV"].ConnectionString);
                //System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();

                //// Estableciento propiedades
                //cmd.Connection = conn;
                ////cmd.CommandText = "INSERT INTO dbo.OrderFacts_UE_Kardex_Returns VALUES (@UeNo, @OrderNo, @Evidence, @CreationUser)";
                //cmd.CommandText = "Exec [dbo].[upCorpOms_Ins_UeNoDevolEvidencia] @UeNo, @OrderNo, @Evidence, @CreationUser";

                //// Creando los parámetros necesarios
                //cmd.Parameters.Add("@UeNo", System.Data.SqlDbType.NVarChar);
                //cmd.Parameters.Add("@OrderNo", System.Data.SqlDbType.Int);
                //cmd.Parameters.Add("@Evidence", System.Data.SqlDbType.Image);
                //cmd.Parameters.Add("@CreationUser", System.Data.SqlDbType.NVarChar);

                //// Asignando los valores a los atributos
                //cmd.Parameters["@UeNo"].Value = idUeNo;
                //cmd.Parameters["@OrderNo"].Value = idOrderNo;
                //cmd.Parameters["@Evidence"].Value = Evidence;
                //cmd.Parameters["@CreationUser"].Value = User;

                //conn.Open();
                //cmd.ExecuteNonQuery();
                //conn.Close();

                //return ds;
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
        public static DataSet upCorpOMS_Cns_UeNoDevolProcess(int idOwner,string usuario,DateTime FecIni,DateTime FecFin)
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
                parametros.Add("@IdOwner", idOwner);
                parametros.Add("@usuario", usuario);
                parametros.Add("@fechaIni", FecIni);
                parametros.Add("@fechafin", FecFin);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOMS_Cns_UeNoDevolProcess]", false, parametros);

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
        public static DataSet upCorpOMS_Cns_UeNoDevolDetail(string UeNo)
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
                parametros.Add("@UeNo", UeNo);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOMS_Cns_UeNoDevolDetail]", false, parametros);

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
        public static DataSet upCorpOms_Cns_UeCondCauses(int idOwner)
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
                parametros.Add("@idOwner ", idOwner);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOms_Cns_UeCondCauses]", false, parametros);

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
        public static DataSet upCorpOms_Cns_UeDevolCauses(int idOwner)
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
                parametros.Add("@idOwner", idOwner);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOms_Cns_UeDevolCauses]", false, parametros);

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
        public static DataSet upCorpOMS_Ins_UeNoDevolProcess(string UeNo, int OrderNo, string IdTrackingService, string TrackingType, decimal Barcode, string CreationId,
            string PackageCondition, string ReturnedComment)
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
                parametros.Add("@UeNo", UeNo);
                parametros.Add("@OrderNo", OrderNo);
                parametros.Add("@IdTrackingService", IdTrackingService);
                parametros.Add("@TrackingType", TrackingType);
                parametros.Add("@Barcode", Barcode);
                parametros.Add("@CreationId", CreationId);
                parametros.Add("@PackageCondition", PackageCondition);
                parametros.Add("@ReturnedComment", ReturnedComment);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOMS_Ins_UeNoDevolProcess]", false, parametros);

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
    }
}