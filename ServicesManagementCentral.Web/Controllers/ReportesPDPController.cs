using ServicesManagement.Web.Models.ReportesPDP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServicesManagement.Web.Controllers
{
    [Authorize]
    public class ReportesPDPController : Controller
    {
        // GET: ReportesPDP
        #region FormaPago


        public ActionResult FormaPago()
        {
            return View();
        }

        public ActionResult GetFormaPago()
        {
            try
            {
                var list = new List<ReportesPDP>(); //DataTableToModel.ConvertTo<SuppliersWHForAproval>(DALAlmacenes.SuppliersWHForAproval_sUP().Tables[0]);
                var result = new { Success = true, resp = list };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        public ActionResult CanalPago()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult Canaldecompra()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult CanalGenerarOrden()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult TipoCatalogo()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult Creditos()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult Liquidaciones()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult ReservacionesSaldo()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult PagoTienda()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult PagoLealtad()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult AutBancarias()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult AutDM()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult AutRM()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult Validaciones()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult VolTransAfiliacion()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult Reintentos()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult EstatusOrdenes()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult EstatusEntregas()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult Detenciones()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult Cancelaciones()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult IngresosAlmacen()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult EnviosEntregas()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult TotalTokens()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult BorradoTokens()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult CreacionTokens()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult Bines()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult Bancos()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult MoviemientosLealtad()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult ContraCargos()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult Aclaraciones()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult MetodosEntrega()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult Mensajerias()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult PagoTiendaEst()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

        public ActionResult VentasCatalogo()
        {
            var list = new List<ReportesPDP>();

            ViewBag.Reporte = list;


            return View();
        }

    }
}