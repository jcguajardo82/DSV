using ServicesManagement.Web.Helpers;
using ServicesManagement.Web.Models.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ServicesManagement.Web.Controllers
{
    public class ConfigController : Controller
    {

        // GET: Config


        #region Menus
        public ActionResult Menus()
        {
            //if (Session["Id_Num_UN"] == null)
            //{
            //    return RedirectToAction("Index", "Ordenes");
            //}
            ViewBag.Menu = DataTableToModel.ConvertTo<Menu>(DALConfig.Menu_sUP().Tables[0]);

            return View();
        }

        public ActionResult AddMenus(string menuId, string descripcion, string descripcionCorta, string padreId
            , string accion, string controlador, string icono, string habilitado)
        {
            try
            {
                if (int.Parse(menuId) == 0)
                {
                    DALConfig.Menu_iUP(int.Parse(menuId), descripcion, descripcionCorta, int.Parse(padreId), icono, controlador, accion, bool.Parse(habilitado));
                }
                else
                {
                    DALConfig.Menu_uUP(int.Parse(menuId), descripcion, descripcionCorta, icono, controlador, accion, bool.Parse(habilitado));
                }
                var result = new { Success = true };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult MenusByIdMenu(string menuId)
        {
            try
            {
                var list = DataTableToModel.ConvertTo<Menu>(DALConfig.MenuByIdMenu_sUP(int.Parse(menuId)).Tables[0]).First();


                var result = new { Success = true, resp = list, };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion

        #region Roles
        public ActionResult Roles()
        {

            return View();
        }

        public ActionResult ListRoles()
        {
            try
            {
                var list = DataTableToModel.ConvertTo<Rol>(DALConfig.roles_sUP(true).Tables[0]);

                var result = new { Success = true, resp = list };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetRol(string idRol)
        {
            try
            {
                var list = DataTableToModel.ConvertTo<Rol>(DALConfig.rolesByIdRol_sUP(int.Parse(idRol)).Tables[0]).FirstOrDefault();

                var result = new { Success = true, resp = list };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult AddRol(string idRol, string nombreRol, string descripcion, string activo)
        {
            try
            {
                //roles_uUP
                if (int.Parse(idRol) != 0)
                {
                    DALConfig.roles_uUP(int.Parse(idRol), nombreRol, descripcion, bool.Parse(activo));
                }
                else
                {
                    DALConfig.roles_iUP(nombreRol, descripcion, bool.Parse(activo));
                }


                var result = new { Success = true };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult GetMenuRol(string idRol)
        {
            try
            {
                var list = DataTableToModel.ConvertTo<MenuRolConfig>(DALConfig.MenuRolConfig_sUp(int.Parse(idRol)).Tables[0]);

                var padre = list.Where(x => x.padreId == 0);

                ViewBag.Menu = list;

                var result = new { Success = true, resp = list, padre = padre };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult SetMenuRol(string menuId, string rolId, string activo)
        {
            try
            {

                DALConfig.MenuRolConfig_iUp(int.Parse(rolId), bool.Parse(activo), int.Parse(menuId));

                var result = new { Success = true };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion

        #region Usuarios
        public ActionResult Usuarios()
        {
            //if (Session["Id_Num_UN"] == null)
            //{
            //    return RedirectToAction("Index", "Ordenes");
            //}
            return View();
        }

        public ActionResult ListUsuarios()
        {
            try
            {
                var list = DataTableToModel.ConvertTo<Usuario>(DALConfig.Usuarios_sUP().Tables[0]);

                var result = new { Success = true, resp = list };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetUsuario(string idUsuario)
        {
            try
            {
                var list = DataTableToModel.ConvertTo<Usuario>(DALConfig.UsuariosById_sUP(int.Parse(idUsuario)).Tables[0]).FirstOrDefault();

                var result = new { Success = true, resp = list };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                var result = new { Success = false, Message = x.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult AddUsuario(string idUsuario, string nombre, string activo, string usuario, string rol)
        {
            try
            {
                if (int.Parse(idUsuario) == 0)
                {
                    DALConfig.Usuarios_iUP(nombre, bool.Parse(activo), "sys", usuario, rol);
                }
                else
                {
                    DALConfig.Usuarios_uUP(int.Parse(idUsuario), nombre, bool.Parse(activo), "sys", usuario, rol);
                }
                var result = new { Success = true };
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