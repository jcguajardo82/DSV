using ServicesManagement.Web.DAL.NivelExistencia;
using ServicesManagement.Web.Helpers;
using ServicesManagement.Web.Models.NivelExistencia;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;


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
            ViewBag.IdOwner = 0;
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

            if (Request.QueryString["IdOwner"] != null)
            {
                IdOwner = int.Parse(Request.QueryString["IdOwner"].ToString());
                ViewBag.IdOwner = IdOwner;
                Session["IdOwnerNiveles"] = IdOwner;
            }

            if (Request.QueryString["IdTienda"] != null)
            {
                IdTienda = Request.QueryString["IdTienda"].ToString();
                ViewBag.IdTienda = IdTienda;
                Session["IdTiendaNiveles"] = IdTienda;
            }

            //list = DataTableToModel.ConvertTo<upCorpOMS_Cns_UeNoTotalsByOrder>(
            //    DALNivelExistencia.upCorpOMS_Cns_UeNoStockLevels(FecIni, FecFin, IdOwner, IdTienda).Tables[0]);

            //ViewBag.Orders = list;

            return View();
        }
        [HttpPost]
        public ActionResult GetNiveles()
        {
            try
            {
                //ViewBag.FecIni = DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");
                //ViewBag.FecFin = DateTime.Now.ToString("yyyy/MM/dd");
                //ViewBag.IdOwner = 0;
                //ViewBag.IdTienda = 0;


                List<upCorpOMS_Cns_UeNoTotalsByOrder> list = new List<upCorpOMS_Cns_UeNoTotalsByOrder>();

                DateTime FecIni = DateTime.Now.AddDays(-7), FecFin = DateTime.Now;
                int IdOwner = 0;
                string IdTienda = string.Empty;
                bool parameters = false;

                if (Session["IdOwnerNiveles"] != null)
                {
                    IdOwner = int.Parse(Session["IdOwnerNiveles"].ToString());
                    parameters = true;
                }

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

                    IQueryable<upCorpOMS_Cns_UeNoTotalsByOrder> query = from row in DALNivelExistencia.upCorpOMS_Cns_UeNoStockLevels(IdOwner, IdTienda).Tables[0].AsEnumerable().AsQueryable()
                                                                        select new upCorpOMS_Cns_UeNoTotalsByOrder()
                                                                        {
                                                                            EAN = (row["EAN"].ToString()),
                                                                            SKU = (row["SKU"].ToString()),
                                                                            Descripcion = row["Descripcion"].ToString(),
                                                                            Categoria = (row["Categoria"].ToString()),
                                                                            NroProveedor = row["NroProveedor"].ToString(),
                                                                            TipoAlmacen = row["TipoAlmacen"].ToString(),
                                                                            NombreProveedor = row["NombreProveedor"].ToString(),
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
                                                                            EstatusProducto = row["EstatusProducto"].ToString(),
                                                                            CostoMaterial = row["CostoMaterial"].ToString(),
                                                                            FechaCreacion = row["FechaCreacion"].ToString(),
                                                                            NroProvOrigen = row["NroProvOrigen"].ToString(),
                                                                            NomProvOrigen = row["NomProvOrigen"].ToString()
                                                                        };




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
    }
}