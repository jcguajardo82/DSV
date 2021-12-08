using ServicesManagement.Web.DAL.Correos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServicesManagement.Web.Controllers
{
    public class EnvioCorreosController : Controller
    {
        // GET: EnvioCorreos
        public ActionResult Index()
        {
          ViewBag.Plantillas =  DALCorreos.EmailLayoutAll_sUp().Tables[0];
   
            return View();
        }

        public ActionResult EnvioCorreo(int Orden,int id)
        {
            try
            {
                switch (id)
                {
                    //1   Confirmación de Pago en Tienda Entrega en Tienda    6A
                    case 1:
                        Correos.Correos.Correo6A(Orden);
                        break;
                    //2   Solicitud de Cancelación    8
                    case 2:
                        Correos.Correos.Correo8(Orden);
                        break;
                    //3   Confirmación de Cancelación de Productos, Envío y/ o Orden. 8A
                    case 3:
                        Correos.Correos.Correo8A(Orden);
                        break;
                    case 4:
                        Correos.Correos.Correo6(Orden);
                        break;
                    case 5:
                        Correos.Correos.Correo7(Orden);
                        break;
                    case 6:
                        Correos.Correos.Correo9(Orden);
                        break;
                    case 7:
                        Correos.Correos.Correo9A(Orden);
                        break;
                    case 8:
                        Correos.Correos.Correo9B(Orden);
                        break;
                    case 9:
                        Correos.Correos.Correo10A(Orden);
                        break;
                    case 10:
                        Correos.Correos.Correo10B(Orden);
                        break;
                    case 11:
                        Correos.Correos.Correo15(Orden);
                        break;
                    case 12:
                        Correos.Correos.Correo16(Orden);
                        break;
                    case 13:
                        Correos.Correos.Correo16A(Orden);
                        break;
                    case 14:
                        Correos.Correos.Correo12(Orden);
                        break;
                    case 15:
                        Correos.Correos.Correo13(Orden);
                        break;
                    case 16:
                        Correos.Correos.Correo14(Orden);
                        break;
                    default:
                        break;
                }
                var result = new
                {
                    Success = true
                    
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
}