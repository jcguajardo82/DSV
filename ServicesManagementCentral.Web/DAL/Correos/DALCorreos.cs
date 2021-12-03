using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.DAL.Correos
{
    public class DALCorreos
    {
        public static DataSet spDatosArticulos_sUP(int Id_cancelacion)
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
                parametros.Add("@Id_cancelacion", Id_cancelacion); // parametro Admin fijo


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[Correo].[spDatosArticulos_sUP]", false, parametros);

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

        public static DataSet spDatosCte_sUP(int @rderNo)
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
                parametros.Add("@OrderNo", @rderNo); // parametro Admin fijo


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[Correo].[spDatosCte_sUP]", false, parametros);

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

        public static DataSet spDatosEntrega_sUP(int @rderNo)
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
                parametros.Add("@OrderNo", @rderNo); // parametro Admin fijo


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[Correo].[spDatosEntrega_sUP]", false, parametros);

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

        public static DataSet spDatosGrales_sUP(int @rderNo)
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
                parametros.Add("@OrderNo", @rderNo); // parametro Admin fijo


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[Correo].[spDatosGrales_sUP]", false, parametros);

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

        public static DataSet spDatosPago_sUP(int @rderNo)
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
                parametros.Add("@OrderNo", @rderNo); // parametro Admin fijo


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[Correo].[spDatosPago_sUP]", false, parametros);

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

        public static DataSet spTotalesImporteDev_sUp(int Id_cancelacion)
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
                parametros.Add("@Id_cancelacion", Id_cancelacion); // parametro Admin fijo


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[Correo].[spTotalesImporteDev_sUp]", false, parametros);

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


        //Correo.EmailLayout_iUp

        public static DataSet EmailLayout_iUp(string Subject,string Message)
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
                parametros.Add("@Message", Message); 
                parametros.Add("@Subject", Subject); 


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[Correo].[EmailLayout_iUp]", false, parametros);

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

        public static DataSet EmailLayout_sUp(int Id)
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
                parametros.Add("@Id", Id);


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[Correo].[EmailLayout_sUp]", false, parametros);

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