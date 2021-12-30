using ServicesManagement.Web.DAL.ProcesoSurtido;
using ServicesManagement.Web.Helpers;
using ServicesManagement.Web.Models.ProcesoSurtido;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServicesManagement.Web.Controllers
{
    public class CancelacionSupervisorController : Controller
    {
        // GET: CancelacionSupervisor
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ConsultaDetalleCancelacion(string order)
        {

            if (string.IsNullOrEmpty(order))
            {
                return RedirectToAction("OrdenSeleccionada", "Ordenes");
            }
            else if (Session["Id_Num_UN"] == null && Session["ordenACancelar"] == null)
            {
                return RedirectToAction("Index", "Ordenes");
            }
            else

            {
                DataSet ds;

                ds = DALServicesM.GetOrdersByOrderNo(order);

                var ueType = ds.Tables[0].Rows[0]["UeType"].ToString();

                int IdOwner = 0;

                if (ueType.Equals("SETC"))
                    IdOwner = 1;
                if (ueType.Equals("DST"))
                    IdOwner = 2;
                if (ueType.Equals("CEDIS") || ueType.Equals("DSV"))
                    return RedirectToAction("OrdenSeleccionada", "Ordenes", new { Ueno = order });

                ViewBag.MotCanCD = DataTableToModel.ConvertTo<OrderFacts_UE_CancelCauses>(DALProcesoSurtido.upCorpOms_Cns_UeCancelCauses(IdOwner).Tables[0]);
                Session["OrderSelected"] = ds;
                Session["OrderStatus"] = ds.Tables[0].Rows[0]["StatusDescription"].ToString();

            }
            return View();

        }

        public ActionResult GetOrderDetalle(string order, string cancelacion)
        {
            try
            {
                if (!string.IsNullOrEmpty(order))
                {
                    System.Data.DataSet ds = DALServicesM.GetOrdersByOrderNo(order);

                    Session["OrderSelected"] = ds;

                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        if (cancelacion != null)
                            Session["ordenACancelar"] = order;
                        else
                            Session["ordenACancelar"] = null;

                        var result = new { Success = true, message = Url.Action("ConsultaDetalleCancelacion", "CancelacionSupervisor") };

                        return Json(result, JsonRequestBehavior.AllowGet);

                        //RedirectToAction("ConsultaDetalle", "Ordenes", new { order = order });
                    }
                    else
                    {
                        var result = new { Success = false, Message = "Alta exitosa" };

                        return Json(result, JsonRequestBehavior.AllowGet);
                    }

                }
            }
            catch
            {
                var result = new { Success = false, Message = "Error" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return View();

        }
    }
}