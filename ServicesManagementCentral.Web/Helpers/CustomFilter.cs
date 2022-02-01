using System.Web.Mvc;
using System.Net;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;

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



            if (context.Exception.Data.Count == 0)
            {
                message = "Error personalizado";

                var ExceptionMessage = context.Exception.Message;
                var ExceptionStackTrack = context.Exception.StackTrace;
                var ControllerName = context.RouteData.Values["controller"].ToString();
                var ActionName = context.RouteData.Values["action"].ToString();
                var ExceptionLogTime = DateTime.Now;



                //Consulto diccionario de errores mandado contolador y accion, obtendria el mensaje perzonalizado

                //guardo en log
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
    }
}