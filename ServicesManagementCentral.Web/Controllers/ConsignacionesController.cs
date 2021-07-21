using System;
using ServicesManagement.Web.DAL.Consignaciones;
using ServicesManagement.Web.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServicesManagement.Web.Models.Consignaciones;
using System.Data;
using System.IO;

namespace ServicesManagement.Web.Controllers
{
    [Authorize]
    public class ConsignacionesController : Controller
    {

        #region Consignaciones Administrador
        public ActionResult Consignaciones()
        {
            return View();
        }

        public ActionResult GetconsignacionesAdm()
        {
            try
            {
                var list = DataTableToModel.ConvertTo<ConsignacionesAdm>(DALConsignacionesAdm.upCorpAlmacen_Cns_Consigments(User.Identity.Name).Tables[0]);
                var result = new { Success = true, resp = list };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult GetDetalleConsignacionAdm(string consignacion)
        {
            try
            {
                List<DetalleConsignaciones> list = new List<DetalleConsignaciones>();
                var ds = DALConsignacionesAdm.upCorpAlmacen_Cns_ConsigmentsDetail(consignacion);

                if (ds.Tables.Count > 0)
                    list = DataTableToModel.ConvertTo<DetalleConsignaciones>(ds.Tables[0]);

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

        #region Consignaciones CEDIS
        public ActionResult Consignaciones_CEDIS()
        {

            return View();

        }


        public ActionResult GetconsignacionesCEDIS()
        {
            try
            {
                var list = DataTableToModel.ConvertTo<ConsignacionesCEDIS>(DALConsignacionesCEDIS.upCorpAlmacen_Cns_ConsigmentsCEDIS(User.Identity.Name).Tables[0]);
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

        #region Consignaciones Proveedor Pendientes
        public ActionResult Consignaciones_Proveedores()
        {

            return View();

        }
        public ActionResult GetconsignacionesProveedor()
        {
            try
            {
                var list = DataTableToModel.ConvertTo<ConsignacionesProveedor>(DALConsignacionesProveedor.upCorpAlmacen_Cns_ConsigmentsProveedor(User.Identity.Name).Tables[0]);
                var result = new { Success = true, resp = list };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public FileResult ExcelConsignacionesProveedor(string op)

        {


            var d = DALConsignacionesProveedor.upCorpAlmacen_Cns_ConsigmentsProveedor(User.Identity.Name).Tables[0];

            string nombreArchivo = "EnvíoPedidosPendientes";

            //Excel to create an object file

            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

            //Add a sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");


            //Here you can set a variety of styles seemingly font color backgrounds, but not very convenient, there is not set
            //Sheet1 head to add the title of the first row
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

            row1.CreateCell(0).SetCellValue("Numero de Pedido");
            row1.CreateCell(1).SetCellValue(" Fecha creacion del pedido");
            row1.CreateCell(2).SetCellValue("Hora creacion de pedido");
            row1.CreateCell(3).SetCellValue(" Fecha Pago Autorizado");
            row1.CreateCell(4).SetCellValue("Fecha Limite Entrega a la Paqueteria");
            row1.CreateCell(5).SetCellValue("Nombre del cliente");
            row1.CreateCell(6).SetCellValue("Estatus del Pedido Surtido");
            row1.CreateCell(7).SetCellValue("Estatus del Pedido Envio");
            row1.CreateCell(8).SetCellValue("No.de orden de compra(DSV)");
            row1.CreateCell(9).SetCellValue("Estatus de Orden de Compra(DSV)");
            row1.CreateCell(10).SetCellValue("No.Guía");
            row1.CreateCell(11).SetCellValue("Vigencia Guía");
            row1.CreateCell(12).SetCellValue("Fecha Recolección");

            //The data is written progressively sheet1 each row

            for (int i = 0; i < d.Rows.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(d.Rows[i]["Consignacion"].ToString());
                rowtemp.CreateCell(1).SetCellValue(d.Rows[i]["FechaCreacion"].ToString());
                rowtemp.CreateCell(2).SetCellValue(d.Rows[i]["HoraCreacion"].ToString());
                rowtemp.CreateCell(3).SetCellValue(d.Rows[i]["FechaPago"].ToString());
                rowtemp.CreateCell(4).SetCellValue(d.Rows[i]["FechaLimite"].ToString());
                rowtemp.CreateCell(5).SetCellValue(d.Rows[i]["NombreCliente"].ToString());
                rowtemp.CreateCell(6).SetCellValue(d.Rows[i]["EstatusConsignacionAlmacen"].ToString());
                rowtemp.CreateCell(7).SetCellValue(d.Rows[i]["EstatusConsignacionEntrega"].ToString());
                rowtemp.CreateCell(8).SetCellValue(d.Rows[i]["NroOrdenCompra"].ToString());
                rowtemp.CreateCell(9).SetCellValue(d.Rows[i]["EstatusOrdenCompra"].ToString());
                rowtemp.CreateCell(10).SetCellValue(d.Rows[i]["GuiaEnvio"].ToString());
                rowtemp.CreateCell(11).SetCellValue(d.Rows[i]["GuiaVig"].ToString());
                rowtemp.CreateCell(12).SetCellValue(d.Rows[i]["FechaEntrega"].ToString());

            }

            //  Write to the client 

            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            book.Write(ms);

            ms.Seek(0, SeekOrigin.Begin);

            DateTime dt = DateTime.Now;

            string dateTime = dt.ToString("yyyyMMddHHmmssfff");

            string fileName = nombreArchivo + "_" + dateTime + "." + op;

            return File(ms, "application/vnd.ms-excel", fileName);

        }
        #endregion


        #region Consignaciones Proveedor concluidos
        public ActionResult Consignaciones_ProveedoresConcluidos()
        {

            return View();

        }


        public ActionResult GetconsignacionesProveedorConcluidos()
        {
            try
            {
                var list = DataTableToModel.ConvertTo<ConsignacionesProveedor>(DALConsignacionesProveedor.upCorpAlmacen_Cns_ConsigmentsProveedorConcluido(User.Identity.Name).Tables[0]);
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

    }
}