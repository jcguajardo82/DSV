using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ServicesManagement.Web.DAL.DALHistorialRMA
{
    public class DALHistorialRMA
    {
        #region Historial RMA

        public static DataSet upCorpOms_Cns_OrdersByDates(DateTime FecIni,DateTime FecFin, int? OrderId)
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


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[dbo].[upCorpOms_Cns_OrdersByDates]", false, parametros);

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