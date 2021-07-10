using ServicesManagement.Web.DAL.Embarques;
using ServicesManagement.Web.DAL.ProcesoSurtido;
using ServicesManagement.Web.Helpers;
using ServicesManagement.Web.Models.ProcesoSurtido;
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

            int OrderNo = 0;
            string UeNo = "0";


            //ViewBag.OrderNo = OrderNo;
            //ViewBag.UeNo = UeNo;

            if (Request.QueryString["OrderNo"] != null && Request.QueryString["UeNo"] != null)
            {
                OrderNo = int.Parse(Request.QueryString["OrderNo"].ToString());
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


            ViewBag.MotCan = DataTableToModel.ConvertTo<OrderFacts_UE_CancelCauses>(DALProcesoSurtido.upCorpOms_Cns_UeCancelCauses(2).Tables[0]);
            ViewBag.Header = DataTableToModel.ConvertTo<Encabezado>(DALProcesoSurtido.upCorpOms_Cns_UeNoSupplyProcessHeader(UeNo, OrderNo).Tables[0]).FirstOrDefault();
            ViewBag.PorProcesar = DataTableToModel.ConvertTo<upCorpOms_Cns_UeNoSupplyProcessSel>(DALProcesoSurtido.upCorpOms_Cns_UeNoSupplyProcessSel(UeNo, OrderNo).Tables[0]);
            Session["OrderPackages"] = DALEmbarques.upCorpOms_Cns_UeNoTracking(UeNo, OrderNo);



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