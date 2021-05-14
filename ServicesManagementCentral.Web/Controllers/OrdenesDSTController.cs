using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServicesManagement.Web.Controllers
{
    public class OrdenesDSTController : Controller
    {
        // GET: OrdenesDST

        public ActionResult OrdenSeleccionada()
        {
            if (Session["Id_Num_UN"] != null)
            {

                string un = Session["Id_Num_UN"].ToString();

                Session["listaOrdersSurtir"] = DALServicesM.GetListaSurtir(un);
                Session["listaOrdersEmbarcar"] = DALServicesM.GetListaEmbarcar(un);
                Session["listTrans"] = DALServicesM.GetCarriersUN(int.Parse(un));

            }
            else
            {
                return RedirectToAction("Index", "Ordenes");
            }

            return View();

        }

        public ActionResult OrdenDetalle(string order)
        {

            if (!string.IsNullOrEmpty(order) & Session["Id_Num_UN"] != null)
            {
                Session["OrderSelected"] = DALServicesM.upCorpOms_Cns_OrdersByOrderNoInit(order);
                Session["listS"] = DALServicesM.GetSurtidores(Session["Id_Num_UN"].ToString());
            }
            else
            {
                return RedirectToAction("Index", "Ordenes");
            }

            return View();
        }

        public ActionResult ConsultaDetalle(string order)
        {

            if (string.IsNullOrEmpty(order))
            {
                return RedirectToAction("OrdenSeleccionada", "OrdenesDST");
            }
            else if (Session["Id_Num_UN"] == null)
            {
                return RedirectToAction("Index", "Ordenes");
            }
            else

            {
                Session["OrderSelected"] = DALServicesM.GetOrdersByOrderNo(order);
            }
            return View();

        }

        public ActionResult Embarque(string order)
        {
            if (string.IsNullOrEmpty(order))
            {
                return RedirectToAction("OrdenSeleccionada", "OrdenesDST");
            }
            else if (Session["Id_Num_UN"] == null)
            {
                return RedirectToAction("Index", "Ordenes");
            }
            else
            {
                Session["OrderSelected"] = DALServicesM.GetOrdersByOrderNo(order);
            }
            return View();

        }
    }
}