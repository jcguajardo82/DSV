using ServicesManagement.Web.DAL.NivelExistencia;
using ServicesManagement.Web.Helpers;
using ServicesManagement.Web.Models.NivelExistencia;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Ajax.Utilities;

namespace ServicesManagement.Web.Controllers
{
    [Authorize]
    public class NivelExistenciaController : Controller
    {
        // GET: NivelExistencia - 2021-09-23
        public string draw = "";
        public string start = "";
        public string length = "";
        public string sortColumn = "";
        public string sortColumnDir = "";
        public string searchValue = "";
        public int pageSize, skip, recordsTotal;

        public ActionResult NivelExistencia()
        {
            ViewBag.FecIni = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");
            ViewBag.FecFin = DateTime.Now.ToString("yyyy/MM/dd");
            ViewBag.IdOwner = "";
            ViewBag.IdTienda = "";


            //List<upCorpOMS_Cns_UeNoTotalsByOrder> list = new List<upCorpOMS_Cns_UeNoTotalsByOrder>();

            //DateTime FecIni = DateTime.Now.AddDays(-7), FecFin = DateTime.Now;
            int IdOwner = 0;
            string IdTienda = string.Empty;

            //if (Request.QueryString["FecIni"] != null)
            //{
            //    FecIni = Convert.ToDateTime(Request.QueryString["FecIni"].ToString());
            //    ViewBag.FecIni = FecIni.ToString("yyyy/MM/dd");
            //}

            //if (Request.QueryString["FecFin"] != null)
            //{
            //    FecFin = Convert.ToDateTime(Request.QueryString["FecFin"].ToString());
            //    ViewBag.FecFin = FecFin.ToString("yyyy/MM/dd");
            //}
            Session["IdOwnerNiveles"] = null;
            Session["IdTiendaNiveles"] = null;

            //if (Request.QueryString["IdOwner"] != null)
            //{
            //    IdOwner = int.Parse(Request.QueryString["IdOwner"].ToString());
            //    ViewBag.IdOwner = IdOwner;
            //    Session["IdOwnerNiveles"] = IdOwner;
            //}

            if (Request.QueryString["IdTienda"] != null)
            {
                IdTienda = Request.QueryString["IdTienda"].ToString();
                
                Session["IdTiendaNiveles"] = IdTienda;
                List<JsonData> lstJson = JsonConvert.DeserializeObject<List<JsonData>>(Request.QueryString["IdTienda"]);
                Session["lstJsonNiveles"] = lstJson;

                var owners="";
                var tiendas = "";
                foreach (var item in lstJson)
                {
                    tiendas += item.IdTienda + "-"+item.IdOwner + "-" + item.Nombre + "-" + item.NombreProveedor + ",";
                }

                foreach (var item in lstJson.DistinctBy(x =>x.IdOwner))
                {
                    owners += item.IdOwner + ",";
                }
                ViewBag.IdTienda = tiendas;
                ViewBag.IdOwner = owners;
            }
            Session["lstNiveles"] = null;

            return View();
        }
        [HttpPost]
        public ActionResult GetNiveles()
        {
            try
            {
                List<upCorpOMS_Cns_UeNoTotalsByOrder> list = new List<upCorpOMS_Cns_UeNoTotalsByOrder>();

                DateTime FecIni = DateTime.Now.AddDays(-7), FecFin = DateTime.Now;
                int IdOwner = 0;
                string IdTienda = string.Empty;
                bool parameters = false;

                //if (Session["IdOwnerNiveles"] != null)
                //{
                //    IdOwner = int.Parse(Session["IdOwnerNiveles"].ToString());
                //    parameters = true;
                //}

                if (Session["IdTiendaNiveles"] != null)
                {
                    IdTienda = Session["IdTiendaNiveles"].ToString();
                    parameters = true;
                }

                List<upCorpOMS_Cns_UeNoTotalsByOrder> lst = new List<upCorpOMS_Cns_UeNoTotalsByOrder>();

                //logistica datatable
                var draw = Request.Form.GetValues("draw").FirstOrDefault();
                var start = Request.Form.GetValues("start").FirstOrDefault();
                var length = Request.Form.GetValues("length").FirstOrDefault();
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault().ToLower();

                
                #region Se Obtienen Filtros Por Columna
                var EAN = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault().ToLower();
                var SKU = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault().ToLower();
                var Descripcion = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault().ToLower();
                var Categoria = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault().ToLower();
                var NroProveedor = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault().ToLower();
                var TipoAlmacen = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault().ToLower();
                var NombreProveedor = Request.Form.GetValues("columns[6][search][value]").FirstOrDefault().ToLower();
                var NombreAlmacen = Request.Form.GetValues("columns[7][search][value]").FirstOrDefault().ToLower();
                var NivelExistencia = Request.Form.GetValues("columns[8][search][value]").FirstOrDefault().ToLower();




                #endregion

                if (parameters)
                {
                    pageSize = length != null ? Convert.ToInt32(length) : 0;
                    skip = start != null ? Convert.ToInt32(start) : 0;
                    recordsTotal = 0;
                    List<JsonData> lstJson = new List<JsonData>();
                    
                    IQueryable <upCorpOMS_Cns_UeNoTotalsByOrder> query = null;
                    if (Session["lstNiveles"] == null)
                    {
                        lstJson = (List<JsonData>)Session["lstJsonNiveles"];
                        query = from row in DALNivelExistencia.upCorpOMS_Cns_UeNoStockLevelsV2(IdTienda).Tables[0].AsEnumerable().AsQueryable()
                                                                            select new upCorpOMS_Cns_UeNoTotalsByOrder()
                                                                            {
                                                                                EAN = (row["EAN"].ToString()),
                                                                                SKU = (row["SKU"].ToString()),
                                                                                Descripcion = row["Descripcion"].ToString(),
                                                                                Categoria = (row["Categoria"].ToString()),
                                                                                NroProveedor = row["NroProveedor"].ToString(),
                                                                                TipoAlmacen = row["TipoAlmacen"].ToString(),
                                                                                NombreProveedor =  lstJson.Where(x => x.IdTienda == int.Parse(row["NroProveedor"].ToString())).FirstOrDefault().NombreProveedor,
                                                                                NivelExistencia = row["NivelExistencia"].ToString(),
                                                                                InvReservado = row["InvReservado"].ToString(),
                                                                                InvVenta = row["InvVenta"].ToString(),
                                                                                InvSeguridad = row["InvSeguridad"].ToString(),
                                                                                TipoArticulo = row["TipoArticulo"].ToString(),
                                                                                Largo = (row["Largo"].ToString()),
                                                                                Alto = row["Alto"].ToString(),
                                                                                Ancho = row["Ancho"].ToString(),
                                                                                Peso = row["Peso"].ToString(),
                                                                                PesoVol = row["PesoVol"].ToString(),
                                                                                PesoReal = row["PesoReal"].ToString(),
                                                                                UnidadLargo = (row["UnidadLargo"].ToString()),
                                                                                UnidadAlto = row["UnidadAlto"].ToString(),
                                                                                UnidadAncho = row["UnidadAncho"].ToString(),
                                                                                UnidadPeso = row["UnidadPeso"].ToString(),
                                                                                UnidadPesoVol = row["UnidadPesoVol"].ToString(),
                                                                                UnidadPesoReal = row["UnidadPesoReal"].ToString(),
                                                                                EstatusProducto = row["EstatusProducto"].ToString(),
                                                                                CostoMaterial = row["CostoMaterial"].ToString(),
                                                                                FechaCreacion = row["FechaCreacion"].ToString(),
                                                                                NroProvOrigen = row["NroProvOrigen"].ToString(),
                                                                                NomProvOrigen = row["NomProvOrigen"].ToString(),
                                                                                NombreAlmacen = lstJson.Where(x => x.IdTienda == int.Parse(row["NroProveedor"].ToString())).FirstOrDefault().Nombre
                                                                            };
                        Session["lstNiveles"] = query;
                    }
                    else
                    {
                        query = (IQueryable<upCorpOMS_Cns_UeNoTotalsByOrder>)Session["lstNiveles"];
                    }



                    if (searchValue != "")
                        query = query.Where(d => d.EAN.ToString().ToLower().Contains(searchValue)
                        || d.SKU.ToLower().Contains(searchValue)
                        || d.Descripcion.ToString().ToLower().Contains(searchValue)
                        || d.Categoria.ToLower().Contains(searchValue)
                        || d.NroProveedor.ToLower().Contains(searchValue)
                        || d.TipoAlmacen.ToLower().Contains(searchValue)
                        || d.NombreProveedor.ToLower().Contains(searchValue)
                        || d.NivelExistencia.ToLower().Contains(searchValue)
                        || d.InvReservado.ToLower().Contains(searchValue)
                        );


                    //Filter By Columns
                    #region Filter By Columns
                    if (!string.IsNullOrEmpty(EAN))
                    {
                        query = query.Where(a => a.EAN.ToString().ToLower().Contains(EAN));
                    }
                    if (!string.IsNullOrEmpty(SKU))
                    {
                        query = query.Where(a => a.SKU.ToLower().Contains(SKU));
                    }

                    if (!string.IsNullOrEmpty(Descripcion))
                    {
                        query = query.Where(a => a.Descripcion.ToString().ToLower().Contains(Descripcion));
                    }
                    if (!string.IsNullOrEmpty(Categoria))
                    {
                        query = query.Where(a => a.Categoria.ToLower().Contains(Categoria));
                    }

                    if (!string.IsNullOrEmpty(NroProveedor))
                    {
                        query = query.Where(a => a.NroProveedor.ToLower().Contains(NroProveedor));
                    }

                    if (!string.IsNullOrEmpty(TipoAlmacen))
                    {
                        query = query.Where(a => a.TipoAlmacen.ToLower().Contains(TipoAlmacen));
                    }

                    if (!string.IsNullOrEmpty(NombreProveedor))
                    {
                        query = query.Where(a => a.NombreProveedor.ToLower().Contains(NombreProveedor));
                    }
                    if (!string.IsNullOrEmpty(NombreAlmacen))
                    {
                        query = query.Where(a => a.NombreAlmacen.ToLower().Contains(NombreAlmacen));
                    }

                    if (!string.IsNullOrEmpty(NivelExistencia))
                    {
                        query = query.Where(a => a.NivelExistencia.ToLower().Contains(NivelExistencia));
                    }

                    #endregion

                    //Sorting    
                    //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    //{
                    //    query = query.OrderBy(sortColumn + " " + sortColumnDir);

                    //}

                    recordsTotal = query.Count();

                    lst = query.Skip(skip).Take(pageSize).ToList();

                }
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = lst });
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }
        public FileResult Excel()
        {
            List<upCorpOMS_Cns_UeNoTotalsByOrder> lst = new List<upCorpOMS_Cns_UeNoTotalsByOrder>();

            var query = (IQueryable<upCorpOMS_Cns_UeNoTotalsByOrder>)Session["lstNiveles"];

            lst = query.ToList();
            string nombreArchivo = "NivelesExistencia";

            //Excel to create an object file

            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();

            //Add a sheet
            NPOI.SS.UserModel.ISheet sheet1 = book.CreateSheet("Sheet1");


            //Here you can set a variety of styles seemingly font color backgrounds, but not very convenient, there is not set
            //Sheet1 head to add the title of the first row
            NPOI.SS.UserModel.IRow row1 = sheet1.CreateRow(0);

            row1.CreateCell(0).SetCellValue("EAN/Codigo de barras");
            row1.CreateCell(1).SetCellValue("ID (#numero material SAP)");
            row1.CreateCell(2).SetCellValue("Nombre del Producto");
            row1.CreateCell(3).SetCellValue("Grupo de Categorías");
            row1.CreateCell(4).SetCellValue("No. Proveedor");
            row1.CreateCell(5).SetCellValue("T. Almacen");
            row1.CreateCell(6).SetCellValue("Nombre Proveedor");
            row1.CreateCell(7).SetCellValue("Nombre Almacen");
            row1.CreateCell(8).SetCellValue("Nivel de Existencias");
            row1.CreateCell(9).SetCellValue("Inventario Reservado");
            row1.CreateCell(10).SetCellValue("Inventario disponible para la venta");
            row1.CreateCell(11).SetCellValue("Stock de seguridad");
            row1.CreateCell(12).SetCellValue("Tipo de Articulo");
            row1.CreateCell(13).SetCellValue("Largo");
            row1.CreateCell(14).SetCellValue("Unidad de Medida Largo");
            row1.CreateCell(15).SetCellValue("Alto");
            row1.CreateCell(16).SetCellValue("Unidad de Medida Alto");
            row1.CreateCell(17).SetCellValue("Ancho");
            row1.CreateCell(18).SetCellValue("Unidad de Medida Ancho");
            row1.CreateCell(19).SetCellValue("Peso");
            row1.CreateCell(20).SetCellValue("Unidad de Medida Peso");
            row1.CreateCell(21).SetCellValue("Peso Volumetrico");
            row1.CreateCell(22).SetCellValue("Unidad de Medida Peso Volumetrico");
            row1.CreateCell(23).SetCellValue("Peso Real");
            row1.CreateCell(24).SetCellValue("Unidad de Medida Peso Real");
            row1.CreateCell(25).SetCellValue("Estatus del codigo");
            row1.CreateCell(26).SetCellValue("Costo del Material");
            row1.CreateCell(27).SetCellValue("Fecha de cracion del Material");
            row1.CreateCell(28).SetCellValue("Num de proveedor que surte producto");
            row1.CreateCell(29).SetCellValue("Nom de proveedor que surte producto");


            //The data is written progressively sheet1 each row

            for (int i = 0; i < lst.Count; i++)
            {
                NPOI.SS.UserModel.IRow rowtemp = sheet1.CreateRow(i + 1);
                rowtemp.CreateCell(0).SetCellValue(lst[i].EAN.ToString());
                rowtemp.CreateCell(1).SetCellValue(lst[i].SKU.ToString());
                rowtemp.CreateCell(2).SetCellValue(lst[i].Descripcion.ToString());
                rowtemp.CreateCell(3).SetCellValue(lst[i].Categoria.ToString());
                rowtemp.CreateCell(4).SetCellValue(lst[i].NroProveedor.ToString());
                rowtemp.CreateCell(5).SetCellValue(lst[i].TipoAlmacen.ToString());
                rowtemp.CreateCell(6).SetCellValue(lst[i].NombreProveedor.ToString());
                rowtemp.CreateCell(7).SetCellValue(lst[i].NombreAlmacen != null ? lst[i].NombreAlmacen.ToString() : "");
                rowtemp.CreateCell(8).SetCellValue(lst[i].NivelExistencia.ToString());
                rowtemp.CreateCell(9).SetCellValue(lst[i].InvReservado.ToString());
                rowtemp.CreateCell(10).SetCellValue(lst[i].InvVenta.ToString());
                rowtemp.CreateCell(11).SetCellValue(lst[i].InvSeguridad.ToString());
                rowtemp.CreateCell(12).SetCellValue(lst[i].TipoArticulo.ToString());
                rowtemp.CreateCell(13).SetCellValue(lst[i].Largo.ToString());
                rowtemp.CreateCell(14).SetCellValue(lst[i].UnidadLargo.ToString());
                rowtemp.CreateCell(15).SetCellValue(lst[i].Alto.ToString());
                rowtemp.CreateCell(16).SetCellValue(lst[i].UnidadAlto.ToString());
                rowtemp.CreateCell(17).SetCellValue(lst[i].Ancho.ToString());
                rowtemp.CreateCell(18).SetCellValue(lst[i].UnidadAncho.ToString());
                rowtemp.CreateCell(19).SetCellValue(lst[i].Peso.ToString());
                rowtemp.CreateCell(20).SetCellValue(lst[i].UnidadPeso.ToString());
                rowtemp.CreateCell(21).SetCellValue(lst[i].PesoVol.ToString());
                rowtemp.CreateCell(22).SetCellValue(lst[i].UnidadPesoVol.ToString());
                rowtemp.CreateCell(23).SetCellValue(lst[i].PesoReal.ToString());
                rowtemp.CreateCell(24).SetCellValue(lst[i].UnidadPesoReal.ToString());
                rowtemp.CreateCell(25).SetCellValue(lst[i].EstatusProducto.ToString());
                rowtemp.CreateCell(26).SetCellValue(lst[i].CostoMaterial.ToString());
                rowtemp.CreateCell(27).SetCellValue(lst[i].FechaCreacion.ToString());
                rowtemp.CreateCell(28).SetCellValue(lst[i].NroProvOrigen.ToString());
                rowtemp.CreateCell(29).SetCellValue(lst[i].NomProvOrigen.ToString());

            }

            //  Write to the client 

            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            book.Write(ms);

            ms.Seek(0, SeekOrigin.Begin);

            DateTime dt = DateTime.Now;

            string dateTime = dt.ToString("yyyyMMddHHmmssfff");

            string fileName = nombreArchivo + "_" + dateTime + ".xls";

            return File(ms, "application/vnd.ms-excel", fileName);

        }
    }
}