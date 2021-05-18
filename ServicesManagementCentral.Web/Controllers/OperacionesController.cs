using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServicesManagement.Web.Controllers
{
    [Authorize]
    public class OperacionesController : Controller
    {
        // GET: Operaciones
        public ActionResult Operaciones()
        {
            return View();
        }


        #region AltaProveedor

        #endregion

        #region TotalOrdenes
        public ActionResult TotalOrdenes()
        {
            return View();
        }
        #endregion

        #region ProcesoSurtido
        public ActionResult ProcesoSurtido()
        {

            return View();
        }
        #endregion

        #region ProcesoReenvioMcia
        public ActionResult ProcesoReenvioMcia()
        {

            return View();
        }
        #endregion

        #region EstatusGuiaEnvio
        public ActionResult EstatusGuiaEnvio()
        {

            return View();
        }
        #endregion

        #region ProcesoReciboDevoluciones
        public ActionResult ProcesoReciboDevoluciones()
        {

            return View();
        }
        #endregion

        #region RenviodeMercancia
        public ActionResult RenviodeMercancia()
        {

            return View();
        }
        #endregion

        #region SolicitudGuiasReenvio
        public ActionResult SolicitudGuiasReenvio()
        {

            return View();
        }
        #endregion
    }
}