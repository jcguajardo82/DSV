using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
namespace ServicesManagement.Web.DAL.ReenvioMcia
{
    public class DALReenvioMciaProveedores
    {


        public static DataSet upCorpOMS_Cns_UeNoReShipment()
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
                parametros.Add("@idOwner", 4); // parametro Proveedor fijo
                parametros.Add("@Seccion", 0); // parametro Seccion fijo
                //parametros.Add("@Consignacion", "101187901-3"); // parametro Proveedor fijo


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOMS_Cns_UeNoReShipment]", false, parametros);
                //ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOMS_Cns_UeNoReshipmentDetail]", false, parametros);

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

        public static DataSet upCorpOMS_Cns_UeNoReshipmentDetail(string consignacion)
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
                parametros.Add("@Consignacion", consignacion);
                // parametros.Add("@Consignacion", "101187901-3"); // parametro Proveedor fijo

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOMS_Cns_UeNoReshipmentDetail]", false, parametros);

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

        public static DataSet upCorpOMS_Cns_UeNoReShipmentConcluidos()
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
                parametros.Add("@idOwner", 4); // parametro Proveedor fijo
                parametros.Add("@Seccion", 1); // parametro Seccion fijo
                //parametros.Add("@Consignacion", "101187901-3"); // parametro Proveedor fijo


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOMS_Cns_UeNoReShipment]", false, parametros);
                //ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOMS_Cns_UeNoReshipmentDetail]", false, parametros);

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

        public static DataSet upCorpOMS_Cns_UeNoReshipmentDetailconcluidos(string consignacion)
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
                parametros.Add("@Consignacion", consignacion);
                // parametros.Add("@Consignacion", "101187901-3"); // parametro Proveedor fijo

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOMS_Cns_UeNoReshipmentDetail]", false, parametros);

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