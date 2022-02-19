using System.Web.Mvc;
using System.Net;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Configuration;

namespace ServicesManagement.Web.Helpers
{
    public class CustomExceptionHandlerFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
          

            if (context == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (context.Exception == null)
                return;

            int status;
            string message;
            var ex = context.Exception;
            var exceptionType = ex.GetType();
            if (exceptionType == typeof(UnauthorizedAccessException))
            {
                var exAccess = (UnauthorizedAccessException)ex;
                message = exAccess.Message;
                status = (int)HttpStatusCode.Unauthorized;
            }
            else if (exceptionType == typeof(SqlException))
            {
                var exSql = (SqlException)ex;
                message = GetDbMessage(exSql);
                status = (int)HttpStatusCode.BadRequest;
            }
            else if (exceptionType == typeof(KeyNotFoundException))
            {
                var exNotFound = (KeyNotFoundException)ex;
                message = exNotFound.Message;
                status = (int)HttpStatusCode.NotFound;
            }
            else
            {
                message = ex.Message;
                status = (int)HttpStatusCode.InternalServerError;
            }



            //if (context.Exception.Data.Count == 0)
            //{


            //    var ExceptionMessage = context.Exception.Message;
            //    var ExceptionStackTrack = context.Exception.StackTrace;
            //    var ControllerName = context.RouteData.Values["controller"].ToString();
            //    var ActionName = context.RouteData.Values["action"].ToString();
            //    var ExceptionLogTime = DateTime.Now;

            //    message = GetMessage(ControllerName, ActionName);



            //    LogError(ExceptionMessage, ExceptionStackTrack, ControllerName, ActionName, ExceptionLogTime);

            //}

            if (!context.Exception.Data.Contains("negocio"))
            {

                var ExceptionMessage = context.Exception.Message;
                var ExceptionStackTrack = context.Exception.StackTrace;
                var ControllerName = context.RouteData.Values["controller"].ToString();
                var ActionName = context.RouteData.Values["action"].ToString();
                var ExceptionLogTime = DateTime.Now;

                message = GetMessage(ControllerName, ActionName);
                LogError(ExceptionMessage, ExceptionStackTrack, ControllerName, ActionName, ExceptionLogTime);
            }


            string json = string.Empty;// TODO: Json(new { success = false, message = message, status = status });
            context.ExceptionHandled = true;
            context.HttpContext.Response.Clear();
            context.HttpContext.Response.TrySkipIisCustomErrors = true;
            context.HttpContext.Response.StatusCode = status;
            context.HttpContext.Response.ContentType = "application/json";
            //context.HttpContext.Response.Write(new { success = false, message = message, status = status });
            context.HttpContext.Response.Write(message);
        }

        private string GetDbMessage(SqlException exSql)
        {
            //TODO: Remove generic from database

            return "DataBase Error see log";
        }


        private string GetMessage(string Controlador, string Accion)
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
                parametros.Add("@Controlador", Controlador);
                parametros.Add("@Accion", Accion);


                ds = Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[config].[CatalogoErrores_sUp]", false, parametros);

                return ds.Tables[0].Rows[0][0].ToString();
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

        //[config].LogErrors_iUp

        private void LogError(
            string Message, string StackTrace,
            string ControllerName, string ActionName,DateTime ExceptionLogTime
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
                parametros.Add("@Message", Message);
                parametros.Add("@StackTrace", StackTrace);
                parametros.Add("@ControllerName", ControllerName);
                parametros.Add("@ActionName", ActionName);
                parametros.Add("@ExceptionLogTime", ExceptionLogTime);


                 Soriana.FWK.FmkTools.SqlHelper.ExecuteDataSet(CommandType.StoredProcedure, "[config].[LogErrors_iUp]", false, parametros);

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