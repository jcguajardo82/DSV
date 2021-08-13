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
    }
}