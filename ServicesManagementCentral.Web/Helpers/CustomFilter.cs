using System.Web.Mvc;

namespace ServicesManagement.Web.Helpers
{
    public class CustomExceptionHandlerFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception.Message != "Error de validacion")
            {
                filterContext.ExceptionHandled = true;
                filterContext.Result = new ViewResult()
                {
                    ViewName = "Error"
                };
            }
        }
    }
}