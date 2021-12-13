using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using ServicesManagement.Web.DAL.CallCenter;
using ServicesManagement.Web.DAL.Correos;
using ServicesManagement.Web.Helpers;
using ServicesManagement.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

using System.Web.Mvc;

namespace ServicesManagement.Web.Correos
{
    public static class Correos
    {

        /// <summary>
        /// Confirmación de Pago en Tienda Entrega en Tienda	6A
        /// </summary>
        /// <param name="OrderNo"></param>
        public static void Correo6A(int OrderNo)
        {
            var parameters = new Dictionary<string, string>();

      

            DatosGrales(ref parameters, OrderNo);
            DatosPago(ref parameters, OrderNo);
            ResumenCompra(ref parameters, OrderNo);
            DatosArticulosOrden(ref parameters, OrderNo);
            var CustomerEmail = DatosCte(ref parameters, OrderNo);


           
          



            if (string.IsNullOrEmpty(CustomerEmail))
            {
                throw new Exception("No se ha podido enviar el correo al cliente, ya que no se cuenta con correo registrado.");
            }

            MailMessage requestMessage = new MailMessage();
            requestMessage.LayoutId = 1;
            requestMessage.MailTo = CustomerEmail;

            requestMessage.Parameters = parameters;

            enviaCorreo(requestMessage);

        }
        /// <summary>
        /// Confirmación de Pago en Tienda Entrega a Domicilio
        /// </summary>
        /// <param name="OrderNo">Orden original,antes del split, sin los sufijo 10,20,30,N</param>
        public static void Correo6(int CurrentOrderNo)
        {
            var parameters = new Dictionary<string, string>();

            parameters.Add("@pedido", CurrentOrderNo.ToString()) ;
            var shipments = DALCorreos.spDatosSplitOrder_sUP(CurrentOrderNo);


            int OrderNo = int.Parse(shipments.Tables[0].Rows[0]["OrderNo"].ToString());
             DatosPago(ref parameters, OrderNo);

            var dirEntrega = DALCorreos.spDatosEntrega_sUP(OrderNo).Tables[0].Rows[0]["DireccionEnvio"].ToString();
            parameters.Add("@direccionEntrega", dirEntrega);
            int totArt = 0;

            StringBuilder tablaProductos = new StringBuilder("");
            int envio = 1;
            string urlImg = System.Configuration.ConfigurationManager.AppSettings["api_ImgBuscadorCarrito"];
            string exteImg = System.Configuration.ConfigurationManager.AppSettings["api_ExtensionImgBuscadorCarrito"];

            //Articulos
            foreach (DataTable shipment in shipments.Tables)
            {
                foreach (DataRow dr in shipment.Rows)
                {

                    #region Tabla Articulos
                    tablaProductos.Append("<table class='tg' style='width:100%'>");
                    tablaProductos.Append("<tr>");
                    tablaProductos.Append("<td class='tg-8s30' rowspan='90'></td>");
                    tablaProductos.Append($"<td class='tg-fkgn' colspan='4'> <b> Productos con envío a domicilio - Envío {envio} de {shipment.Rows.Count}</b></td>");
                    tablaProductos.Append("<td class='tg-fkgn' rowspan='90'></td>");
                    tablaProductos.Append("</tr>");
                    tablaProductos.Append("<tr>");
                    tablaProductos.Append($"<td class='tg-t0vf' colspan='4'><b> Dirección de envío </b> <br/>{dirEntrega}</td>");
                    tablaProductos.Append("</tr>");



                    var arts = DALCorreos.spDatosArticulosbyOrderId_sUP(int.Parse(dr["OrderNo"].ToString()));
                    totArt += arts.Tables[0].Rows.Count;
                    foreach (DataRow item in arts.Tables[0].Rows)
                    {
                        tablaProductos.Append("<tr>");
                        tablaProductos.Append($"<td class='tg-zv4m' rowspan='2' style='width:10%'> <img src='{string.Format("{0}{1}{2}", urlImg, item["CodeBarra"].ToString(), exteImg)}' alt='Image' width='75' height='60'></td>");
                        tablaProductos.Append($"<td style='Word-wrap:break-Word; width:70%;' rowspan='2'> {item["ProductName"].ToString()} </td>");
                        tablaProductos.Append($"<td class='tg-zv4m'>Cantidad: {item["Quantity"].ToString()} </td>");
                        tablaProductos.Append($"<td class='tg-zv4m'>${item["Price"].ToString()}</td>");
                        tablaProductos.Append("</tr>");

                        tablaProductos.Append("<tr>");
                        tablaProductos.Append("<td class='tg-zv4m' colspan='4'></td>");
                        tablaProductos.Append("</tr>");
                    }
                    envio++;
                    tablaProductos.Append("</table>");
                    #endregion

                    #region totales
           
                    #endregion
                }
            }






            parameters.Add("@tabla_articulos", tablaProductos.ToString());
            parameters.Add("@tot_arti", totArt.ToString());

            //ResumenCompra(ref parameters, OrderNo);
            //DatosArticulosOrden(ref parameters, OrderNo);
            //DatosEntrega(ref parameters, OrderNo);
            var CustomerEmail = DatosCte(ref parameters, OrderNo);

            if (string.IsNullOrEmpty(CustomerEmail))
            {
                throw new Exception("No se ha podido enviar el correo al cliente, ya que no se cuenta con correo registrado.");
            }


            MailMessage requestMessage = new MailMessage();
            requestMessage.LayoutId = 4;
            requestMessage.MailTo = CustomerEmail;
            requestMessage.Parameters = parameters;

            enviaCorreo(requestMessage);

        }
        /// <summary>
        /// Confirmación de Envío.
        /// </summary>
        /// <param name="OrderNo"></param>
        public static void Correo7(int OrderNo)
        {
            var parameters = new Dictionary<string, string>();



            DatosGrales(ref parameters, OrderNo);
            DatosPago(ref parameters, OrderNo);
            ResumenCompra(ref parameters, OrderNo);
            DatosArticulosOrden(ref parameters, OrderNo);
            DatosEntrega(ref parameters, OrderNo);
            var CustomerEmail = DatosCte(ref parameters, OrderNo);

            if (string.IsNullOrEmpty(CustomerEmail))
            {
                throw new Exception("No se ha podido enviar el correo al cliente, ya que no se cuenta con correo registrado.");
            }


            MailMessage requestMessage = new MailMessage();
            requestMessage.LayoutId = 5;
            requestMessage.MailTo = CustomerEmail;
    
            requestMessage.Parameters = parameters;

            enviaCorreo(requestMessage);

        }


        /// <summary>
        /// Confirmación de Cancelación de Productos, Envío y/o Orden.
        /// </summary>
        /// <param name="Id_cancelacion">OrderNo/IdCancelacion, depende de opcion</param>
        /// <param name="opcion">1.IdCancelacion 2.OrderNo</param>
        public static void Correo8A(int Id_cancelacion, int opcion)
        {
            var parameters = new Dictionary<string, string>();

            int OrderNo = 0, OrderSF = 0;


            if (Id_cancelacion == 1)
            {
                PaymentsGetCancelacion(ref OrderNo, ref OrderSF, Id_cancelacion);
                DatosArticulos(ref parameters, Id_cancelacion, 2);
                TotalesImporteDev(ref parameters, Id_cancelacion);
            }
            else
            {
                OrderNo = Id_cancelacion;
                DatosArticulosOrden(ref parameters, OrderNo, 2);
                TotalesImporteDevByOrder(ref parameters, OrderNo);
            }


            DatosGrales(ref parameters, OrderNo);
            var CustomerEmail = DatosCte(ref parameters, OrderNo);

            if (string.IsNullOrEmpty(CustomerEmail))
            {
                throw new Exception("No se ha podido enviar el correo al cliente, ya que no se cuenta con correo registrado.");
            }

            MailMessage requestMessage = new MailMessage();
            requestMessage.LayoutId = 3;
            requestMessage.MailTo = CustomerEmail;

            requestMessage.Parameters = parameters;

            enviaCorreo(requestMessage);

        }


        /// <summary>
        /// Solicitud de Cancelación de Productos, Envío y/o Orden.
        /// </summary>
        public static void Correo8(int Id_cancelacion)
        {
            var parameters = new Dictionary<string, string>();

            int OrderNo = 0, OrderSF = 0;




            PaymentsGetCancelacion(ref OrderNo, ref OrderSF, Id_cancelacion);
            DatosGrales(ref parameters, OrderNo);

            DatosArticulos(ref parameters, Id_cancelacion, 2);
            TotalesImporteDev(ref parameters, Id_cancelacion);



            var CustomerEmail = DatosCte(ref parameters, OrderNo);
            if (string.IsNullOrEmpty(CustomerEmail))
            {
                throw new Exception("No se ha podido enviar el correo al cliente, ya que no se cuenta con correo registrado.");
            }

            MailMessage requestMessage = new MailMessage();
            requestMessage.LayoutId = 2;
            requestMessage.MailTo = CustomerEmail;
            requestMessage.Parameters = parameters;


            enviaCorreo(requestMessage);

        }
   


        /// <summary>
        /// Solicitud de Devolucion Aceptada
        /// </summary>
        /// <param name="Id_cancelacion">Id de la Rma</param>
        public static void Correo9(int Id_cancelacion)
        {
            var parameters = new Dictionary<string, string>();

            parameters.Add("@idCancelacion", Id_cancelacion.ToString());

            int OrderNo = 0, OrderSF = 0;

            PaymentsGetCancelacion(ref OrderNo, ref OrderSF, Id_cancelacion);
            DatosGrales(ref parameters, OrderNo);
            DatosArticulos(ref parameters, Id_cancelacion, 2);
            TotalesImporteDev(ref parameters, Id_cancelacion);

            var CustomerEmail = DatosCte(ref parameters, OrderNo);

            if (string.IsNullOrEmpty(CustomerEmail))
            {
                throw new Exception("No se ha podido enviar el correo al cliente, ya que no se cuenta con correo registrado.");
            }


            MailMessage requestMessage = new MailMessage();
            requestMessage.LayoutId = 6;
            requestMessage.MailTo = CustomerEmail;
  
            requestMessage.Parameters = parameters;

            enviaCorreo(requestMessage);

        }
        /// <summary>
        /// Solicitud de Devolucion Recibida
        /// </summary>
        /// <param name="Id_cancelacion"></param>
        public static void Correo9A(int Id_cancelacion)
        {
            var parameters = new Dictionary<string, string>();

            parameters.Add("@idCancelacion", Id_cancelacion.ToString());

            int OrderNo = 0, OrderSF = 0;

            PaymentsGetCancelacion(ref OrderNo, ref OrderSF, Id_cancelacion);
            DatosGrales(ref parameters, OrderNo);
            DatosArticulos(ref parameters, Id_cancelacion, 2);
            TotalesImporteDev(ref parameters, Id_cancelacion);

            var CustomerEmail = DatosCte(ref parameters, OrderNo);

            if (string.IsNullOrEmpty(CustomerEmail))
            {
                throw new Exception("No se ha podido enviar el correo al cliente, ya que no se cuenta con correo registrado.");
            }


            MailMessage requestMessage = new MailMessage();
            requestMessage.LayoutId = 6;
            requestMessage.MailTo = CustomerEmail;
            requestMessage.Parameters = parameters;

            enviaCorreo(requestMessage);

        }
        /// <summary>
        /// Solicitud de Devolucion Apicada
        /// </summary>
        /// <param name="Id_cancelacion"></param>
        public static void Correo9B(int Id_cancelacion)
        {
            var parameters = new Dictionary<string, string>();

            parameters.Add("@idCancelacion", Id_cancelacion.ToString());

            int OrderNo = 0, OrderSF = 0;

            PaymentsGetCancelacion(ref OrderNo, ref OrderSF, Id_cancelacion);
            DatosGrales(ref parameters, OrderNo);
            DatosArticulos(ref parameters, Id_cancelacion, 2);
            TotalesImporteDev(ref parameters, Id_cancelacion);

            var CustomerEmail = DatosCte(ref parameters, OrderNo);

            if (string.IsNullOrEmpty(CustomerEmail))
            {
                throw new Exception("No se ha podido enviar el correo al cliente, ya que no se cuenta con correo registrado.");
            }


            MailMessage requestMessage = new MailMessage();
            requestMessage.LayoutId = 8;
            requestMessage.MailTo = CustomerEmail;
            requestMessage.Parameters = parameters;

            enviaCorreo(requestMessage);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id_cancelacion"></param>
        public static void Correo10A(int Id_cancelacion)
        {
            var parameters = new Dictionary<string, string>();

            parameters.Add("@idCancelacion", Id_cancelacion.ToString());

            int OrderNo = 0, OrderSF = 0;

            PaymentsGetCancelacion(ref OrderNo, ref OrderSF, Id_cancelacion);
            DatosGrales(ref parameters, OrderNo);
            DatosArticulos(ref parameters, Id_cancelacion, 2);
            TotalesImporteDev(ref parameters, Id_cancelacion);

            var CustomerEmail = DatosCte(ref parameters, OrderNo);

            if (string.IsNullOrEmpty(CustomerEmail))
            {
                throw new Exception("No se ha podido enviar el correo al cliente, ya que no se cuenta con correo registrado.");
            }


            MailMessage requestMessage = new MailMessage();
            requestMessage.LayoutId = 9;
            requestMessage.MailTo = CustomerEmail;
            requestMessage.Parameters = parameters;

            enviaCorreo(requestMessage);

        }

        /// <summary>
        /// Autorización de reebolso por Devolución (Pago En Tienda).
        /// </summary>
        /// <param name="Id_cancelacion"></param>
        public static void Correo10B(int Id_cancelacion)
        {
            var parameters = new Dictionary<string, string>();

            parameters.Add("@idCancelacion", Id_cancelacion.ToString());

            int OrderNo = 0, OrderSF = 0;

            PaymentsGetCancelacion(ref OrderNo, ref OrderSF, Id_cancelacion);
            DatosGrales(ref parameters, OrderNo);
            DatosArticulos(ref parameters, Id_cancelacion, 2);
            TotalesImporteDev(ref parameters, Id_cancelacion);

            var CustomerEmail = DatosCte(ref parameters, OrderNo);

            if (string.IsNullOrEmpty(CustomerEmail))
            {
                throw new Exception("No se ha podido enviar el correo al cliente, ya que no se cuenta con correo registrado.");
            }


            MailMessage requestMessage = new MailMessage();
            requestMessage.LayoutId = 10;
            requestMessage.MailTo = CustomerEmail;
            requestMessage.Parameters = parameters;

            enviaCorreo(requestMessage);

        }


        /// <summary>
        /// Pedido listo para Recoger en Tienda
        /// </summary>
        /// <param name="OrderNo"></param>

        public static void Correo15(int OrderNo)
        {
            var parameters = new Dictionary<string, string>();



            DatosGrales(ref parameters, OrderNo);
            DatosPago(ref parameters, OrderNo);


            #region Articulos
            StringBuilder tablaProductos = new StringBuilder("");
            var dt = DALCorreos.spDatosArticulosbyOrderId_sUP(OrderNo).Tables[0];
            string urlImg = System.Configuration.ConfigurationManager.AppSettings["api_ImgBuscadorCarrito"];
            string exteImg = System.Configuration.ConfigurationManager.AppSettings["api_ExtensionImgBuscadorCarrito"];
            int cns = 1;
            foreach (DataRow item in dt.Rows)
            {
                //var piezas = dt.Rows.Count > 1 ? "piezas" : "pieza";
                //tablaProductos.Append("<tr>");
                //tablaProductos.Append($"<td class='tg-oe15'> <img src='{item.UrlImage}' alt='Image' width='75' height='60'></td>");
                //tablaProductos.Append($"<td class='tg-oe15'> {item.Descripcion} </td>");
                //tablaProductos.Append($"<td class='tg-c1kk'>Cantidad: {item.Piezas} {piezas} </td>");
                //tablaProductos.Append($"<td class='tg-c1kk'>{item.Precio}</td>");
                //tablaProductos.Append("</tr>");
                tablaProductos.Append("<tr>");

                tablaProductos.Append($"<td class='tg-8jgo'>{cns.ToString()}</td>");
                tablaProductos.Append($"<td class='tg-8jgo'>{item["ProductId"].ToString()}</td>");
                tablaProductos.Append($"<td class='tg-8jgo'>{item["CodeBarra"].ToString()}</td>");
                tablaProductos.Append($"<td class='tg-8jgo'>{item["ProductName"].ToString()}</td>");
                tablaProductos.Append($"<td class='tg-8jgo'>{item["Quantity"].ToString()}</td>");
                tablaProductos.Append($"<td class='tg-8jgo'> <img src='{string.Format("{0}{1}{2}", urlImg, item["CodeBarra"].ToString(), exteImg)}' alt='Image' width='75' height='60'></td>");
                tablaProductos.Append("</tr>");

                cns++;
            }
            parameters.Add("@tabla_articulos", tablaProductos.ToString());
            #endregion


            var CustomerEmail = DatosCte(ref parameters, OrderNo);

            if (string.IsNullOrEmpty(CustomerEmail))
            {
                throw new Exception("No se ha podido enviar el correo al cliente, ya que no se cuenta con correo registrado.");
            }


            MailMessage requestMessage = new MailMessage();
            requestMessage.LayoutId =11;
            requestMessage.MailTo = CustomerEmail;
            requestMessage.Parameters = parameters;

            enviaCorreo(requestMessage);

        }


        /// <summary>
        /// Confirmación de Entrega en Tienda
        /// </summary>
        /// <param name="OrderNo"></param>
        public static void Correo16(int OrderNo)
        {
            var parameters = new Dictionary<string, string>();



            DatosGrales(ref parameters, OrderNo);
            DatosPago(ref parameters, OrderNo);


            #region Articulos
            StringBuilder tablaProductos = new StringBuilder("");
            var dt = DALCorreos.spDatosArticulosbyOrderId_sUP(OrderNo).Tables[3];
            string urlImg = System.Configuration.ConfigurationManager.AppSettings["api_ImgBuscadorCarrito"];
            string exteImg = System.Configuration.ConfigurationManager.AppSettings["api_ExtensionImgBuscadorCarrito"];
            int cns = 1;
            foreach (DataRow item in dt.Rows)
            {
                //var piezas = dt.Rows.Count > 1 ? "piezas" : "pieza";
                //tablaProductos.Append("<tr>");
                //tablaProductos.Append($"<td class='tg-oe15'> <img src='{item.UrlImage}' alt='Image' width='75' height='60'></td>");
                //tablaProductos.Append($"<td class='tg-oe15'> {item.Descripcion} </td>");
                //tablaProductos.Append($"<td class='tg-c1kk'>Cantidad: {item.Piezas} {piezas} </td>");
                //tablaProductos.Append($"<td class='tg-c1kk'>{item.Precio}</td>");
                //tablaProductos.Append("</tr>");
                tablaProductos.Append("<tr>");

                tablaProductos.Append($"<td class='tg-8jgo'>{cns.ToString()}</td>");
                tablaProductos.Append($"<td class='tg-8jgo'>{item["ProductId"].ToString()}</td>");
                tablaProductos.Append($"<td class='tg-8jgo'>{item["CodeBarra"].ToString()}</td>");
                tablaProductos.Append($"<td class='tg-8jgo'>{item["ProductName"].ToString()}</td>");
                tablaProductos.Append($"<td class='tg-8jgo'>{item["Quantity"].ToString()}</td>");
                tablaProductos.Append($"<td class='tg-8jgo'> <img src='{string.Format("{0}{1}{2}", urlImg, item["CodeBarra"].ToString(), exteImg)}' alt='Image' width='75' height='60'></td>");
                tablaProductos.Append("</tr>");

                cns++;
            }
            parameters.Add("@tabla_articulos", tablaProductos.ToString());
            #endregion


            var CustomerEmail = DatosCte(ref parameters, OrderNo);

            if (string.IsNullOrEmpty(CustomerEmail))
            {
                throw new Exception("No se ha podido enviar el correo al cliente, ya que no se cuenta con correo registrado.");
            }


            MailMessage requestMessage = new MailMessage();
            requestMessage.LayoutId = 12;
            requestMessage.MailTo = CustomerEmail;
            requestMessage.Parameters = parameters;

            enviaCorreo(requestMessage);

        }

        /// <summary>
        /// Confirmación de Entrega a Domicilio
        /// </summary>
        /// <param name="OrderNo"></param>
        public static void Correo16A(int OrderNo)
        {
            var parameters = new Dictionary<string, string>();



            DatosGrales(ref parameters, OrderNo);
            DatosPago(ref parameters, OrderNo);


            #region Articulos
            StringBuilder tablaProductos = new StringBuilder("");
            var dt = DALCorreos.spDatosArticulosbyOrderId_sUP(OrderNo).Tables[3];
            string urlImg = System.Configuration.ConfigurationManager.AppSettings["api_ImgBuscadorCarrito"];
            string exteImg = System.Configuration.ConfigurationManager.AppSettings["api_ExtensionImgBuscadorCarrito"];
            int cns = 1;
            foreach (DataRow item in dt.Rows)
            {
                //var piezas = dt.Rows.Count > 1 ? "piezas" : "pieza";
                //tablaProductos.Append("<tr>");
                //tablaProductos.Append($"<td class='tg-oe15'> <img src='{item.UrlImage}' alt='Image' width='75' height='60'></td>");
                //tablaProductos.Append($"<td class='tg-oe15'> {item.Descripcion} </td>");
                //tablaProductos.Append($"<td class='tg-c1kk'>Cantidad: {item.Piezas} {piezas} </td>");
                //tablaProductos.Append($"<td class='tg-c1kk'>{item.Precio}</td>");
                //tablaProductos.Append("</tr>");
                tablaProductos.Append("<tr>");

                tablaProductos.Append($"<td class='tg-8jgo'>{cns.ToString()}</td>");
                tablaProductos.Append($"<td class='tg-8jgo'>{item["ProductId"].ToString()}</td>");
                tablaProductos.Append($"<td class='tg-8jgo'>{item["CodeBarra"].ToString()}</td>");
                tablaProductos.Append($"<td class='tg-8jgo'>{item["ProductName"].ToString()}</td>");
                tablaProductos.Append($"<td class='tg-8jgo'>{item["Quantity"].ToString()}</td>");
                tablaProductos.Append($"<td class='tg-8jgo'> <img src='{string.Format("{0}{1}{2}", urlImg, item["CodeBarra"].ToString(), exteImg)}' alt='Image' width='75' height='60'></td>");
                tablaProductos.Append("</tr>");

                cns++;
            }
            parameters.Add("@tabla_articulos", tablaProductos.ToString());
            #endregion


            var CustomerEmail = DatosCte(ref parameters, OrderNo);

            if (string.IsNullOrEmpty(CustomerEmail))
            {
                throw new Exception("No se ha podido enviar el correo al cliente, ya que no se cuenta con correo registrado.");
            }


            MailMessage requestMessage = new MailMessage();
            requestMessage.LayoutId = 12;
            requestMessage.MailTo = CustomerEmail;
            requestMessage.Parameters = parameters;

            enviaCorreo(requestMessage);

        }

        /// <summary>
        /// Asignación de Orden al proveedor
        /// </summary>
        /// <param name="OrderNo"></param>
        public static void Correo12(int OrderNo)
        {
            var parameters = new Dictionary<string, string>();



            DatosProveedor(ref parameters, OrderNo);


            #region Articulos
            StringBuilder tablaProductos = new StringBuilder("");
            var dt = DALCorreos.spDatosArticulosbyOrderId_sUP(OrderNo).Tables[0];
            string urlImg = System.Configuration.ConfigurationManager.AppSettings["api_ImgBuscadorCarrito"];
            string exteImg = System.Configuration.ConfigurationManager.AppSettings["api_ExtensionImgBuscadorCarrito"];
            int cns = 1;
            foreach (DataRow item in dt.Rows)
            {
                //var piezas = dt.Rows.Count > 1 ? "piezas" : "pieza";
                //tablaProductos.Append("<tr>");
                //tablaProductos.Append($"<td class='tg-oe15'> <img src='{item.UrlImage}' alt='Image' width='75' height='60'></td>");
                //tablaProductos.Append($"<td class='tg-oe15'> {item.Descripcion} </td>");
                //tablaProductos.Append($"<td class='tg-c1kk'>Cantidad: {item.Piezas} {piezas} </td>");
                //tablaProductos.Append($"<td class='tg-c1kk'>{item.Precio}</td>");
                //tablaProductos.Append("</tr>");
                tablaProductos.Append("<tr>");

                tablaProductos.Append($"<td class='tg-8jgo'>{cns.ToString()}</td>");
                tablaProductos.Append($"<td class='tg-8jgo'>{item["ProductId"].ToString()}</td>");
                tablaProductos.Append($"<td class='tg-8jgo'>{item["CodeBarra"].ToString()}</td>");
                tablaProductos.Append($"<td class='tg-8jgo'>{item["ProductName"].ToString()}</td>");
                tablaProductos.Append($"<td class='tg-8jgo'>{item["Quantity"].ToString()}</td>");
                tablaProductos.Append($"<td class='tg-8jgo'> <img src='{string.Format("{0}{1}{2}", urlImg, item["CodeBarra"].ToString(), exteImg)}' alt='Image' width='75' height='60'></td>");
                tablaProductos.Append("</tr>");

                cns++;
            }
            parameters.Add("@tabla_articulos", tablaProductos.ToString());
            #endregion


            var CustomerEmail = DatosCte(ref parameters, OrderNo);

            if (string.IsNullOrEmpty(CustomerEmail))
            {
                throw new Exception("No se ha podido enviar el correo al cliente, ya que no se cuenta con correo registrado.");
            }


            MailMessage requestMessage = new MailMessage();
            requestMessage.LayoutId = 12;
            requestMessage.MailTo = CustomerEmail;
            requestMessage.Parameters = parameters;

            enviaCorreo(requestMessage);

        }

        /// <summary>
        /// Cancelación de Orden al proveedor
        /// </summary>
        /// <param name="OrderNo"></param>
        public static void Correo13(int OrderNo)
        {
            var parameters = new Dictionary<string, string>();


            DatosGrales(ref parameters, OrderNo);
            DatosProveedor(ref parameters, OrderNo);


            #region Articulos
            StringBuilder tablaProductos = new StringBuilder("");
            var dt = DALCorreos.spDatosArticulosbyOrderId_sUP(OrderNo).Tables[0];
            string urlImg = System.Configuration.ConfigurationManager.AppSettings["api_ImgBuscadorCarrito"];
            string exteImg = System.Configuration.ConfigurationManager.AppSettings["api_ExtensionImgBuscadorCarrito"];
            int cns = 1;
            foreach (DataRow item in dt.Rows)
            {
                //var piezas = dt.Rows.Count > 1 ? "piezas" : "pieza";
                //tablaProductos.Append("<tr>");
                //tablaProductos.Append($"<td class='tg-oe15'> <img src='{item.UrlImage}' alt='Image' width='75' height='60'></td>");
                //tablaProductos.Append($"<td class='tg-oe15'> {item.Descripcion} </td>");
                //tablaProductos.Append($"<td class='tg-c1kk'>Cantidad: {item.Piezas} {piezas} </td>");
                //tablaProductos.Append($"<td class='tg-c1kk'>{item.Precio}</td>");
                //tablaProductos.Append("</tr>");
                tablaProductos.Append("<tr>");

                tablaProductos.Append($"<td class='tg-8jgo'>{cns.ToString()}</td>");
                tablaProductos.Append($"<td class='tg-8jgo'>{item["ProductId"].ToString()}</td>");
                tablaProductos.Append($"<td class='tg-8jgo'>{item["CodeBarra"].ToString()}</td>");
                tablaProductos.Append($"<td class='tg-8jgo'>{item["ProductName"].ToString()}</td>");
                tablaProductos.Append($"<td class='tg-8jgo'>{item["Quantity"].ToString()}</td>");
                tablaProductos.Append($"<td class='tg-8jgo'> <img src='{string.Format("{0}{1}{2}", urlImg, item["CodeBarra"].ToString(), exteImg)}' alt='Image' width='75' height='60'></td>");
                tablaProductos.Append("</tr>");

                cns++;
            }
            parameters.Add("@tabla_articulos", tablaProductos.ToString());
            #endregion


            var CustomerEmail = DatosCte(ref parameters, OrderNo);

            if (string.IsNullOrEmpty(CustomerEmail))
            {
                throw new Exception("No se ha podido enviar el correo al cliente, ya que no se cuenta con correo registrado.");
            }


            MailMessage requestMessage = new MailMessage();
            requestMessage.LayoutId = 12;
            requestMessage.MailTo = CustomerEmail;
            requestMessage.Parameters = parameters;

            enviaCorreo(requestMessage);

        }

        /// <summary>
        /// Aceptación de Orden (Confirmación de Pago) al proveedor.
        /// </summary>
        /// <param name="OrderNo"></param>
        public static void Correo14(int OrderNo)
        {
            var parameters = new Dictionary<string, string>();


            DatosGrales(ref parameters, OrderNo);
            DatosProveedor(ref parameters, OrderNo);
            parameters.Add("@ordenCompra", "ordenCompra");

            #region Articulos
            StringBuilder tablaProductos = new StringBuilder("");
            var dt = DALCorreos.spDatosArticulosbyOrderId_sUP(OrderNo).Tables[0];
            string urlImg = System.Configuration.ConfigurationManager.AppSettings["api_ImgBuscadorCarrito"];
            string exteImg = System.Configuration.ConfigurationManager.AppSettings["api_ExtensionImgBuscadorCarrito"];
            int cns = 1;
            foreach (DataRow item in dt.Rows)
            {
                //var piezas = dt.Rows.Count > 1 ? "piezas" : "pieza";
                //tablaProductos.Append("<tr>");
                //tablaProductos.Append($"<td class='tg-oe15'> <img src='{item.UrlImage}' alt='Image' width='75' height='60'></td>");
                //tablaProductos.Append($"<td class='tg-oe15'> {item.Descripcion} </td>");
                //tablaProductos.Append($"<td class='tg-c1kk'>Cantidad: {item.Piezas} {piezas} </td>");
                //tablaProductos.Append($"<td class='tg-c1kk'>{item.Precio}</td>");
                //tablaProductos.Append("</tr>");
                tablaProductos.Append("<tr>");

                tablaProductos.Append($"<td class='tg-8jgo'>{cns.ToString()}</td>");
                tablaProductos.Append($"<td class='tg-8jgo'>{item["ProductId"].ToString()}</td>");
                tablaProductos.Append($"<td class='tg-8jgo'>{item["CodeBarra"].ToString()}</td>");
                tablaProductos.Append($"<td class='tg-8jgo'>{item["ProductName"].ToString()}</td>");
                tablaProductos.Append($"<td class='tg-8jgo'>{item["Quantity"].ToString()}</td>");
                tablaProductos.Append($"<td class='tg-8jgo'> <img src='{string.Format("{0}{1}{2}", urlImg, item["CodeBarra"].ToString(), exteImg)}' alt='Image' width='75' height='60'></td>");
                tablaProductos.Append("</tr>");

                cns++;
            }
            parameters.Add("@tabla_articulos", tablaProductos.ToString());
            #endregion


            var CustomerEmail = DatosCte(ref parameters, OrderNo);

            if (string.IsNullOrEmpty(CustomerEmail))
            {
                throw new Exception("No se ha podido enviar el correo al cliente, ya que no se cuenta con correo registrado.");
            }


            MailMessage requestMessage = new MailMessage();
            requestMessage.LayoutId = 12;
            requestMessage.MailTo = CustomerEmail;
            requestMessage.Parameters = parameters;

            enviaCorreo(requestMessage);

        }

        #region Metodos mapeo 
        public static void PaymentsGetCancelacion(ref int OrderNo, ref int OrderSF, int Id_cancelacion)
        {
            var ds = DALCallCenter.PaymentsGetCancelacion_sUp(Id_cancelacion);

            OrderNo = int.Parse(ds.Tables[0].Rows[0]["OrderNo"].ToString());
            OrderSF = int.Parse(ds.Tables[0].Rows[0]["OrderSF"].ToString());
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="Id_cancelacion"></param>
        /// <param name="opcion">Orden resultset 1, Cancelación o Devolución resultset 2, Surtidos resultset 3</param>
        public static void DatosArticulos(ref Dictionary<string, string> parameters, int Id_cancelacion, int opcion)
        {
            //Orden resultset 1, Cancelación o Devolución resultset 2, Surtidos resultset 3
            var ds = DALCorreos.spDatosArticulos_sUP(Id_cancelacion);
            string urlImg = System.Configuration.ConfigurationManager.AppSettings["api_ImgBuscadorCarrito"];
            string exteImg = System.Configuration.ConfigurationManager.AppSettings["api_ExtensionImgBuscadorCarrito"];

            StringBuilder tablaProductos = new StringBuilder("");


            switch (opcion)
            {
                case 1:
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {

                        var piezas = decimal.Parse(item["Quantity"].ToString()) > 1 ? "piezas" : "pieza";
                        tablaProductos.Append("<tr>");
                        tablaProductos.Append($"<td class='tg-oe15'> <img src='{string.Format("{0}{1}{2}", urlImg, item["CodeBarra"].ToString(), exteImg)}' alt='Image' width='75' height='60'></td>");
                        tablaProductos.Append($"<td class='tg-oe15'> {item["ProductName"].ToString()} </td>");
                        tablaProductos.Append($"<td class='tg-c1kk'>Cantidad: {item["Quantity"].ToString()} {piezas} </td>");
                        tablaProductos.Append($"<td class='tg-c1kk'>{item["Price"].ToString()}</td>");
                        tablaProductos.Append("</tr>");

                        parameters.Add("@totalMergeRows", (ds.Tables[0].Rows.Count * 2).ToString());
                        parameters.Add("@total_articulos", ds.Tables[0].Rows.Count.ToString());

                    }
                    break;
                case 2:
                    foreach (DataRow item in ds.Tables[1].Rows)
                    {



                      

                        tablaProductos.Append("<tr>");
                        tablaProductos.Append($"<td class='tg-zv4m' rowspan='2' style='width:10%'> <img src='{string.Format("{0}{1}{2}", urlImg, item["CodeBarra"].ToString(), exteImg)}' alt='Image' width='75' height='60'></td>");
                        tablaProductos.Append($"<td style = 'Word-wrap: break-Word; width: 70%;' class='tg-t0vf' rowspan='2'> {item["ProductName"].ToString()} <br> {item["Quantity"].ToString()} </td>");
                        tablaProductos.Append($"<td class='tg-zv4m'></td>");
                        tablaProductos.Append($"<td class='tg-zv4m' rowspan='2'> <span style='font-weight:bold'>{item["Price"].ToString()}</span></td>");
                        tablaProductos.Append("</tr>");
                        tablaProductos.Append("<tr>");
                        tablaProductos.Append("<td class='tg-zv4m'></td>");
                        tablaProductos.Append("</tr>");

                      

                    }

                    parameters.Add("@totalMergeRows", (ds.Tables[1].Rows.Count * 7).ToString());
                    parameters.Add("@tot_arti", ds.Tables[1].Rows.Count.ToString());
                    break;
                case 3:
                    foreach (DataRow item in ds.Tables[2].Rows)
                    {
                        var piezas = decimal.Parse(item["Quantity"].ToString()) > 1 ? "piezas" : "pieza";
                        tablaProductos.Append("<tr>");
                        tablaProductos.Append($"<td class='tg-oe15'> <img src='{string.Format("{0}{1}{2}", urlImg, item["CodeBarra"].ToString(), exteImg)}' alt='Image' width='75' height='60'></td>");
                        tablaProductos.Append($"<td class='tg-oe15'> {item["ProductName"].ToString()} </td>");
                        tablaProductos.Append($"<td class='tg-c1kk'>Cantidad: {item["Quantity"].ToString()} {piezas} </td>");
                        tablaProductos.Append($"<td class='tg-c1kk'>{item["Price"].ToString()}</td>");
                        tablaProductos.Append("</tr>");

                        parameters.Add("@totalMergeRows", (ds.Tables[2].Rows.Count * 2).ToString());
                        parameters.Add("@total_articulos", ds.Tables[2].Rows.Count.ToString());
                    }
                    break;

                default:
                    break;
            }

            parameters.Add("@tabla_articulos", tablaProductos.ToString());


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="OrdenNo"></param>
        /// <param name="opcion">1.Orden,2.Cancelacion</param>
        public static void DatosArticulosOrden(ref Dictionary<string, string> parameters, int OrdenNo,int opcion=1)
        {
            //Orden resultset 1, Cancelación o Devolución resultset 2, Surtidos resultset 3
            var ds = DALCorreos.spDatosArticulosbyOrderId_sUP(OrdenNo);
            string urlImg = System.Configuration.ConfigurationManager.AppSettings["api_ImgBuscadorCarrito"];
            string exteImg = System.Configuration.ConfigurationManager.AppSettings["api_ExtensionImgBuscadorCarrito"];

            StringBuilder tablaProductos = new StringBuilder("");

            switch (opcion)
            {
                case 1:
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {

               // var piezas = decimal.Parse(item["Quantity"].ToString()) > 1 ? "piezas" : "pieza";
                tablaProductos.Append("<tr>");
                tablaProductos.Append($"<td class='tg-oe15'> <img src='{string.Format("{0}{1}{2}", urlImg, item["CodeBarra"].ToString(), exteImg)}' alt='Image' width='75' height='60'></td>");
                tablaProductos.Append($"<td style='Word-wrap:break-Word;width:230px;' class='tg-oe15'> {item["ProductName"].ToString()} </td>");
                tablaProductos.Append($"<td class='tg-c1kk'>Cantidad: {item["Quantity"].ToString()}  </td>");
                tablaProductos.Append($"<td class='tg-c1kk'>{item["Price"].ToString()}</td>");
                tablaProductos.Append("</tr>");


            }

   
            parameters.Add("@tot_arti", ds.Tables[0].Rows.Count.ToString());

            parameters.Add("@tabla_articulos", tablaProductos.ToString());


        }

        public static void DatosGrales(ref Dictionary<string, string> parameters, int OrderNo)
        {
            var ds = DALCorreos.spDatosGrales_sUP(OrderNo);

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                //correo.UeNo = item["UeNo"].ToString();
                //correo.OrderNo = item["OrderNo"].ToString();
                parameters.Add("@pedido", item["OrderNo"].ToString());
                parameters.Add("@UeNo", item["UeNo"].ToString());
            }
        }

        public static string DatosCte(ref Dictionary<string, string> parameters, int OrderNo)
        {
            var ds = DALCorreos.spDatosCte_sUP(OrderNo);
            string CustomerEmail = string.Empty;
            foreach (DataRow item in ds.Tables[0].Rows)
            {

                //correo.NameReceive = item["NameReceive"].ToString();
                //correo.CustomerName = item["CustomerName"].ToString();

                parameters.Add("@customer_first_name", item["CustomerName"].ToString());
                string tarjeta = string.Empty;

                if (!string.IsNullOrEmpty(item["CustomerPersonalId"].ToString().Trim()))
                {
                    tarjeta = "<ul><span style='font-size:20px;margin:0 6px 20px 0;'><li type='disc'> Veras reflejado tu reembolso en tu <b> Tarjeta de Recompensas soriana " + item["CustomerPersonalId"].ToString() + ". </b> </li></span></ul>";
                }
                // <li type="disc">Veras reflejado tu reembolso en tu 
                //< b > Tarjeta de Recompensas soriana[1234 1234 1234 1234]. </ b > </ li ></ span ></ ul >
                parameters.Add("@tarjeta_soriana", tarjeta);

                CustomerEmail = item["CustomerEmail"].ToString().Trim();
            }

            return CustomerEmail;
        }

        public static void DatosPago(ref Dictionary<string, string> parameters, int OrderNo)
        {
            var ds = DALCorreos.spDatosPago_sUP(OrderNo);

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                parameters.Add("@numero_tienda", item["StoreNum"].ToString());
                parameters.Add("@nombre_tienda", item["Desc_UN"].ToString());
                parameters.Add("@total","$"+ item["Total"].ToString());
                parameters.Add("@cantidad", item["Total"].ToString());
                parameters.Add("@direccion_tienda", "direccion_tienda");
                parameters.Add("@horario_tienda", "horario_tienda");




                //correo.StoreNum = item["Desc_UN"].ToString();
                //correo.Desc_UN = item["Desc_UN"].ToString();
                //correo.Total = item["Total"].ToString();
            }
        }

        public static void DatosEntrega(ref Dictionary<string, string> parameters, int OrderNo)
        {
            var ds = DALCorreos.spDatosEntrega_sUP(OrderNo);

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                parameters.Add("@fechaEntrega", item["DeliveryDate"].ToString());
                parameters.Add("@direccionEntrega", item["DireccionEnvio"].ToString());

                //correo.StoreNum = item["Desc_UN"].ToString();
                //correo.Desc_UN = item["Desc_UN"].ToString();
                //correo.Total = item["Total"].ToString();
            }
        }

        public static void ResumenCompra(ref Dictionary<string, string> parameters, int OrderNo)
        {
            var ds = DALCorreos.spDatosResumenCompra_sUP(OrderNo);

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                parameters.Add("@CantidadArtDif", item["CantidadArtDif"].ToString());
                parameters.Add("@subtotal", item["subTotal"].ToString());
                parameters.Add("@efectivo_disp", item["PagoEfvo"].ToString());
                parameters.Add("@pago", item["Pago"].ToString());
                parameters.Add("@mi_ahorro", item["Ahorro"].ToString());
                parameters.Add("@envio", item["Flete"].ToString());
                //parameters.Add("@total", item["Total"].ToString());
                parameters.Add("@puntos_generados", item["PtosaOtorgar"].ToString());
                parameters.Add("@puntos_dinero", item["DeaOtorgar"].ToString());


                //0 as CantidadArtDif, 
                //0 as subTotal,
                //0 as PagoEfvo,
                //0 as Pago,
                //0 as Ahorro,
                //0 as Flete,
                //0 as Total,
                //0 as PtosaOtorgar,
                //0 as DeaOtorgar

                //correo.StoreNum = item["Desc_UN"].ToString();
                //correo.Desc_UN = item["Desc_UN"].ToString();
                //correo.Total = item["Total"].ToString();
            }
        }

        public static void DatosProveedor(ref Dictionary<string, string> parameters, int OrderNo)
        {
            parameters.Add("@nombreProveedor", "nombre del proveedor");

            //var ds = DALCorreos.spDatosResumenCompra_sUP(OrderNo);

            //foreach (DataRow item in ds.Tables[0].Rows)
            //{
            //    parameters.Add("@CantidadArtDif", item["CantidadArtDif"].ToString());
            //    parameters.Add("@subtotal", item["subTotal"].ToString());
            //    parameters.Add("@efectivo_disp", item["PagoEfvo"].ToString());
            //    parameters.Add("@pago", item["Pago"].ToString());
            //    parameters.Add("@mi_ahorro", item["Ahorro"].ToString());
            //    parameters.Add("@envio", item["Flete"].ToString());
            //    //parameters.Add("@total", item["Total"].ToString());
            //    parameters.Add("@puntos_generados", item["PtosaOtorgar"].ToString());
            //    parameters.Add("@puntos_dinero", item["DeaOtorgar"].ToString());


            //    //0 as CantidadArtDif, 
            //    //0 as subTotal,
            //    //0 as PagoEfvo,
            //    //0 as Pago,
            //    //0 as Ahorro,
            //    //0 as Flete,
            //    //0 as Total,
            //    //0 as PtosaOtorgar,
            //    //0 as DeaOtorgar

            //    //correo.StoreNum = item["Desc_UN"].ToString();
            //    //correo.Desc_UN = item["Desc_UN"].ToString();
            //    //correo.Total = item["Total"].ToString();
            //}
        }
        public static void TotalesImporteDev(ref Dictionary<string, string> parameters, int Id_cancelacion)
        {
            var ds = DALCorreos.spTotalesImporteDev_sUp(@Id_cancelacion);

            foreach (DataRow item in ds.Tables[0].Rows)
            {


                parameters.Add("@subTotal", "$" + item["subTotal"].ToString());
                parameters.Add("@envio", "$" + item["Flete"].ToString());
                parameters.Add("@total", "$" + item["Total"].ToString());

                //correo.TotalProductosDev = item["TotalProductos"].ToString();
                //correo.subTotalDev = item["subTotal"].ToString();
                //correo.FleteDev = item["Flete"].ToString();
                //correo.Total = item["Total"].ToString();
                //correo.OrderSF = item["OrderSF"].ToString();
                //correo.OrderNo = item["OrderNo"].ToString();

            }
        }

        //[Correo].[spTotalesImporteDevByOrder_sUp]
        public static void TotalesImporteDevByOrder(ref Dictionary<string, string> parameters, int OrderNo)
        {
            var ds = DALCorreos.spTotalesImporteDevByOrder_sUp(OrderNo);

            foreach (DataRow item in ds.Tables[0].Rows)
            {


                parameters.Add("@subTotal", "$" + item["subTotal"].ToString());
                parameters.Add("@envio", "$" + item["Flete"].ToString());
                parameters.Add("@total", "$" + item["Total"].ToString());

                //correo.TotalProductosDev = item["TotalProductos"].ToString();
                //correo.subTotalDev = item["subTotal"].ToString();
                //correo.FleteDev = item["Flete"].ToString();
                //correo.Total = item["Total"].ToString();
                //correo.OrderSF = item["OrderSF"].ToString();
                //correo.OrderNo = item["OrderNo"].ToString();

            }
        }

        private static void SendMessage(string mensaje)
        {

            string ServiceBusConnectionString = ConfigurationManager.ConnectionStrings["ServiceBus"].ConnectionString;
            string QueueName = "mail";//ConfigurationManager.AppSettings["stockQueueName"];

            var queueClient = QueueClient.CreateFromConnectionString(ServiceBusConnectionString, QueueName);
            BrokeredMessage message = new BrokeredMessage(mensaje);
            queueClient.Send(message);
        }

        private static string replacelayout(MailMessage mailMessage, int id)
        {

            var message = DALCorreos.EmailLayout_sUp(id).Tables[0].Rows[0]["Message"].ToString();
            if (mailMessage.Parameters != null)
            {
                foreach (KeyValuePair<string, string> item in mailMessage.Parameters)
                {
                    //string subject = layout.Subject.Replace(item.Key, item.Value);
                    //layout.Subject = subject;
                    message = message.Replace(item.Key, item.Value);
                }
            }

            return message;

        }
        #endregion

        public static void enviaCorreo(MailMessage requestMessage)
        {

            //Header
          var header=  DALCorreos.EmailLayout_sUp(17).Tables[0].Rows[0]["Message"].ToString();
          var footer=  DALCorreos.EmailLayout_sUp(18).Tables[0].Rows[0]["Message"].ToString();


            requestMessage.Parameters.Add("@header", header);
            requestMessage.Parameters.Add("@footer", footer);

            var html = replacelayout(requestMessage, requestMessage.LayoutId);

            ////Console.Write(html);

            //requestMessage.MailTo = "petkstillo@gmail.com";
            requestMessage.MailTo = "josera@soriana.com";


            var requestMail = JsonConvert.SerializeObject(requestMessage);

            //SendMessage(requestMail);



        }

    }

    public class MailMessage
    {
        public int LayoutId { get; set; }
        public string MailTo { get; set; }
        public Dictionary<string, string> Parameters { get; set; }


    }


}