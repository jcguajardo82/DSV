using ServicesManagement.Web.DAL.Embarques;
using ServicesManagement.Web.DAL.ProcesoSurtido;
using ServicesManagement.Web.Helpers;
using ServicesManagement.Web.Models.ProcesoSurtido;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ServicesManagement.Web.Controllers
{
    [Authorize]
    public class ProcesoReenvioMciaController : Controller
    {
        // GET: SolicitudGuiasReenvio


        public ActionResult ProcesoReenvioMcia()
        {

            int? OrderNo = null;
            string UeNo = "0";


            //ViewBag.OrderNo = OrderNo;
            //ViewBag.UeNo = UeNo;

            if (Request.QueryString["UeNo"] != null)
            {
                if (Request.QueryString["OrderNo"] != null)
                {
                    OrderNo = int.Parse(Request.QueryString["OrderNo"].ToString());
                }
                UeNo = Request.QueryString["UeNo"].ToString();
            }
            else
            {
                var msj = new msj();
                msj.mensaje = "No existe la consignacion.";

                ViewBag.Mensaje = msj;

                return View();
            }

            var ds = DALProcesoSurtido.upCorpOms_Cns_UeNoSupplyProcess(UeNo, OrderNo);
            ViewBag.OrderNo = OrderNo;
            ViewBag.UeNo = UeNo;


            ViewBag.MotCan = DataTableToModel.ConvertTo<OrderFacts_UE_CancelCauses>(DALProcesoSurtido.upCorpOms_Cns_UeCancelCauses(4).Tables[0]);
            var enc = DataTableToModel.ConvertTo<Encabezado>(DALProcesoSurtido.upCorpOms_Cns_UeNoSupplyProcessHeader(UeNo, OrderNo).Tables[0]).FirstOrDefault();
            ViewBag.Header = enc;
            ViewBag.PorProcesar = DataTableToModel.ConvertTo<upCorpOms_Cns_UeNoSupplyProcessSel>(DALProcesoSurtido.upCorpOms_Cns_UeNoSupplyProcessSel(UeNo, OrderNo).Tables[0]);
            Session["OrderPackages"] = DALEmbarques.upCorpOms_Cns_UeNoTracking(UeNo, OrderNo);

            var dsVehiculos = (DALProcesoSurtido.upCorpOms_Cns_UeNoConsigmentsVehicles(UeNo, OrderNo, enc.idSupplierWH, enc.IdSupplierWHCode).Tables[0]);

            if (dsVehiculos.Columns.Count > 1)
            {
                ViewBag.Vehiculos = DataTableToModel.ConvertTo<upCorpOms_Cns_UeNoConsigmentsVehicles>(dsVehiculos);
            }
            else {
                ViewBag.Vehiculos = new List<upCorpOms_Cns_UeNoConsigmentsVehicles>();
            }
            if (ds.Tables.Count == 1)
            {
                ViewBag.Mensaje = DataTableToModel.ConvertTo<msj>(ds.Tables[0]).FirstOrDefault();
            }
            else
            {
                var list = DataTableToModel.ConvertTo<upCorpOms_Cns_UeNoSupplyProcess>(ds.Tables[0]);

                var totSup = list.Where(x => x.Suplido == true).ToList();


                if (list.Count == totSup.Count)
                {
                    ViewBag.ArtSup = true;
                }

                ViewBag.UeNoSupplyProcess = list;
                ViewBag.Encabezado = DataTableToModel.ConvertTo<upCorpOms_Cns_UeNoSupplyProcess>(ds.Tables[1]);
            }

            return View();
        }
    }
}