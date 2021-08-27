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



        #region Dal Alta RMA       

        public static DataSet up_CorpTMS_Sel_TipoVehiculo()
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


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "dbo.up_CorpTMS_Sel_TipoVehiculo", false);

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
        public static DataSet GastosV_sUp()
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


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "tms.Gastos_sUp", false);

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
        public static DataSet Gastos_sUp()
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


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "tms.GastoVehiculo_sUp", false);

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


        public static DataSet MotivosRMA_sUp()
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
                //parametros.Add("@Id_Vehiculo", Id_Vehiculo);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "MotivosRMA_sUp", false);

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

        public static DataSet GastoVehiculoById_sUP(int IdConsecutivo)
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
                parametros.Add("@IdConsecutivo", IdConsecutivo);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "tms.GastoVehiculoById_sUP", false, parametros);

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

        public static DataSet MotivosRMA_iUP(int IdPadre, string descripcionmotivo, string created_user)
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
                parametros.Add("@Id_Padre ", IdPadre);
                parametros.Add("@Descripcion_Motivo ", descripcionmotivo);
                //parametros.Add("@FecGasto", FecGasto);
                //parametros.Add("@Kilometraje", Kilometraje);
                //parametros.Add("@CantidadGasto", CantidadGasto);
                //parametros.Add("@created_user", created_user);
                parametros.Add("@UserId ", created_user);

                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "MotivosRMA_iUP", false, parametros);

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


        public static DataSet MotivosRMA_uUp(int IdPadre, string descripcionmotivo, int IdMotivo, string created_user)
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
                /*parametros.Add("@Id_Vehiculo", Id_Vehiculo);*/
                //parametros.Add("@IdGasto", IdGasto);
                parametros.Add("@Id_Padre ", IdPadre);
                parametros.Add("@Descripcion_Motivo ", descripcionmotivo);
                parametros.Add("@Id_Motivo ", IdMotivo);
                parametros.Add("@Modified_User ", created_user);
                parametros.Add("@Bit_Eliminado ", 1);


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "MotivosRMA_uUp", false, parametros);

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


        public static DataSet DeleteGastoVehiculo_uUp(int IdConsecutivo, string FecGasto, int Kilometraje, decimal CantidadGasto, string created_user)
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
                /*parametros.Add("@Id_Vehiculo", Id_Vehiculo);*/
                //parametros.Add("@IdGasto", IdGasto);
                parametros.Add("@FecGasto", FecGasto);
                parametros.Add("@Kilometraje", Kilometraje);
                parametros.Add("@CantidadGasto", CantidadGasto);
                parametros.Add("@created_user", created_user);
                parametros.Add("@IdConsecutivo", IdConsecutivo);
                parametros.Add("@activo", 0);


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "tms.GastoVehiculo_uUp", false, parametros);

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

        public static DataSet DeleteGastoVehiculo_uUp2(int IdConsecutivo, string created_user, int Kilometraje, decimal CantidadGasto, DateTime FecGasto)
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
                /*parametros.Add("@Id_Vehiculo", Id_Vehiculo);*/
                //parametros.Add("@IdGasto", IdGasto);
                parametros.Add("@FecGasto", FecGasto);
                parametros.Add("@Kilometraje", Kilometraje);
                parametros.Add("@CantidadGasto", CantidadGasto);
                parametros.Add("@created_user", created_user);
                parametros.Add("@IdConsecutivo", IdConsecutivo);
                parametros.Add("@activo", 0);


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "tms.GastoVehiculo_uUp", false, parametros);

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

        //GastoVehiculo_dUp
        public static DataSet MotivosRMA_dUP(int IdMotivo, string modified_user)
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

                parametros.Add("@modified_user", modified_user);
                //parametros.Add("@Id_Padre ", IdPadre);
                parametros.Add("@Id_Motivo ", IdMotivo);



                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "MotivosRMA_dUP", false, parametros);

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