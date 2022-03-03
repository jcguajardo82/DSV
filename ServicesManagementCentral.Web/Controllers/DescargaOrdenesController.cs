using Newtonsoft.Json;
using ServicesManagement.Web.Models.DescargaOrdenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ServicesManagement.Web.Controllers
{
    public class DescargaOrdenesController : Controller
    {
        // GET: DescargaOrdenes
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GenerarLink(string json)
        {
            string _link = string.Empty;
            try
            {
                //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                Soriana.FWK.FmkTools.RestResponse r2 = Soriana.FWK.FmkTools.RestClient.RequestRest(Soriana.FWK.FmkTools.HttpVerb.POST, System.Configuration.ConfigurationSettings.AppSettings["DescargaOrdenes_API"], "", json);

                if (r2.message.Contains("url"))
                {
                    DescargaOrdenesResponse response = JsonConvert.DeserializeObject<DescargaOrdenesResponse>(r2.message);
                    _link = response.url;
                }
                else
                    throw new Exception("Ocurrio un error al generar el archivo");

                var result = new { Success = true, link= _link };
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