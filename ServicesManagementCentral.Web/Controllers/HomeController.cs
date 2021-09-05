using ServicesManagement.Web.DAL.NotificacionesSis;
using System.Web.Mvc;

namespace ServicesManagement.Web.Controllers
{
    /// <summary>
    /// home controller
    /// </summary>

    [Authorize]
    public class HomeController : BaseController
    {
        public HomeController() : base(System.Web.HttpContext.Current)
        {

        }

        /// <summary>
        /// home index view
        /// </summary>
        //public ActionResult Index()
        //{
        //    //if (Login == null)
        //    //{
        //    //    return RedirectToAction("Login", "Security");
        //    //}

        //    if (Login != null)
        //    {
        //        return RedirectToAction("Index", "Ordenes");
        //    }

        //    return RedirectToAction("Index", "CPanel");
        //    //return RedirectToAction("Index", "Ordenes");
        //    //  return View();
        //}


        public ActionResult Index()
        {
            //if (Login == null)
            //{
            //    return RedirectToAction("Login", "Security");
            //}

            if (Login != null)
            {
                if (Login.Username.Trim().Equals("omsmercurio") && Login.Password.Trim().Equals("$oriana2021#"))
                {
                    return RedirectToAction("Index", "Ordenes");
                }
                else
                {

//                    var notificaciones = DALNotificacionesSis.ImagenNotificacion_CnsbyDates().Tables[0].Rows.Count;

//                    if (notificaciones == 0)
//                    {
                        if (Session["loginTienda"] != null)
                        {
                            Session["Id_Num_UN"] = Session["loginTienda"].ToString();
                            Session["Desc_Num_UN"] = "";

                            System.Data.DataSet ds = DALServicesM.GetUN();

                            foreach (System.Data.DataRow r in ds.Tables[0].Rows)
                            {
                                if (Session["loginTienda"].ToString().Trim().Equals(r[0].ToString().Trim()))
                                {
                                    Session["Desc_Num_UN"] = r[1].ToString();
                                }
                            }

                            return RedirectToAction("OrdenSeleccionada", "Ordenes");
                            //return RedirectToAction("Index", "Ordenes");
                        }
                        else
                        {
                            return RedirectToAction("Index", "CPanel");
                        }
//                    }
//                    else {
//                        return RedirectToAction("NotificacionSis", "NotificacionesSis");
//                    }
                    //Session["userFail"] = "Usuario o Password incorrecto";
                    //return RedirectToAction("Login", "Security");
                }
            }

            return RedirectToAction("Index", "CPanel");
            //return RedirectToAction("Index", "Ordenes");
            //  return View();
        }


    }
}