using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServicesManagementCentral.Web.Controllers.Api;
using ServicesManagement.Web.Models;
using ServicesManagement.Web.DAL.ActualizacionesInventario;
using ServicesManagement.Web.Helpers;

namespace ServicesManagement.Web.Controllers
{
    [Authorize]
    public class ActualizacionesInventarioController : Controller
    {
        // GET: SolicitudGuiasReenvio
        public ActionResult ActualizacionesInventario()
        {
            return View();
        }

        public ActionResult NiveldeServicio()
        {
            return View();
        }

        public ActionResult NotasdeCredito()
        {
            return View();
        }
    }
}