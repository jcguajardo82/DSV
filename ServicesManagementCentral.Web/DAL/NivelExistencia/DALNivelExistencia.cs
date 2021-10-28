using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ServicesManagement.Web.DAL.NivelExistencia
{
    public class DALNivelExistencia
    {
        public static DataSet upCorpOMS_Cns_UeNoStockLevels(DateTime? FecIni,DateTime? FecFin, int? IdOwner, int? IdTienda)
        {
            // update - 2021-09-23
            DataSet ds = new DataSet();

            string conection = ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]];
            if (System.Configuration.ConfigurationManager.AppSettings["flagConectionDBEcriptado"].ToString().Trim().Equals("1"))
            {
                conection = Soriana.FWK.FmkTools.Seguridad.Desencriptar(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings["AmbienteSC"]]);
            }

            try
            {
                Soriana.FWK.FmkTools.SqlHelper.connection_Name(ConfigurationManager.ConnectionStrings["Connection_DM"].ConnectionString);
                System.Collections.Hashtable parametros = new System.Collections.Hashtable();

                parametros.Add("@FechaIni", FecIni);
                parametros.Add("@FechaFin", FecFin);
                parametros.Add("@IdOwner", IdOwner);
                parametros.Add("@IdTienda", IdTienda);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOMS_Cns_UeNoStockLevels]", false,parametros);

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