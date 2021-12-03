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


        public static void Correo6(int OrderNo)
        {
            var parameters = new Dictionary<string, string>();

      

            DatosGrales(ref parameters, OrderNo);
            DatosPago(ref parameters, OrderNo);
            var CustomerEmail = DatosCte(ref parameters, OrderNo);



            if (string.IsNullOrEmpty(CustomerEmail))
            {
                throw new Exception("No se ha podido enviar el correo al cliente, ya que no se cuenta con correo registrado.");
            }

            MailMessage requestMessage = new MailMessage();
            requestMessage.LayoutId = 2;
            requestMessage.MailTo = "Agonzalez@itechdev.com.mx;jcguajardo@itechdev.com.mx";
            requestMessage.Parameters = parameters;

            var requestMail = JsonConvert.SerializeObject(requestMessage);

            SendMessage(requestMail);


            //var html = replacelayout(requestMessage, 2);

            //Console.Write(html);

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
            var CustomerEmail = DatosCte(ref parameters, OrderNo);
            DatosArticulos(ref parameters, Id_cancelacion, 2);
            TotalesImporteDev(ref parameters, Id_cancelacion);


            if (string.IsNullOrEmpty(CustomerEmail))
            {
                throw new Exception("No se ha podido enviar el correo al cliente, ya que no se cuenta con correo registrado.");
            }

            MailMessage requestMessage = new MailMessage();
            requestMessage.LayoutId = 2;
            requestMessage.MailTo = "Agonzalez@itechdev.com.mx;jcguajardo@itechdev.com.mx";
            requestMessage.Parameters = parameters;

            var requestMail = JsonConvert.SerializeObject(requestMessage);

            SendMessage(requestMail);


            //var html = replacelayout(requestMessage, 2);

            //Console.Write(html);

        }
        /// <summary>
        /// Confirmación de Cancelación de Productos, Envío y/o Orden.

        /// </summary>
        public static void Correo8A(int Id_cancelacion)
        {
            var parameters = new Dictionary<string, string>();

            int OrderNo = 0, OrderSF = 0;




            PaymentsGetCancelacion(ref OrderNo, ref OrderSF, Id_cancelacion);
            DatosGrales(ref parameters, OrderNo);
            var CustomerEmail = DatosCte(ref parameters, OrderNo);
            DatosArticulos(ref parameters, Id_cancelacion, 2);
            TotalesImporteDev(ref parameters, Id_cancelacion);


            if (string.IsNullOrEmpty(CustomerEmail))
            {
                throw new Exception("No se ha podido enviar el correo al cliente, ya que no se cuenta con correo registrado.");
            }

            MailMessage requestMessage = new MailMessage();
            requestMessage.LayoutId = 3;
            requestMessage.MailTo = "Agonzalez@itechdev.com.mx;jcguajardo@itechdev.com.mx";
            requestMessage.Parameters = parameters;

            var requestMail = JsonConvert.SerializeObject(requestMessage);

            SendMessage(requestMail);


            //var html = replacelayout(requestMessage, 3);

            //Console.Write(html);

        }




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
                        //var piezas = decimal.Parse(item["Quantity"].ToString()) > 1 ? "piezas" : "pieza";
                        //tablaProductos.Append("<tr>");
                        //tablaProductos.Append($"<td class='tg-oe15'> <img src='{string.Format("{0}{1}{2}", urlImg, item["CodeBarra"].ToString(), exteImg)}' alt='Image' width='75' height='60'></td>");
                        //tablaProductos.Append($"<td class='tg-oe15'> {item["ProductName"].ToString()} </td>");
                        //tablaProductos.Append($"<td class='tg-c1kk'>Cantidad: {item["Quantity"].ToString()} {piezas} </td>");
                        //tablaProductos.Append($"<td class='tg-c1kk'>{item["Price"].ToString()}</td>");
                        //tablaProductos.Append("</tr>");


                        parameters.Add("@totalMergeRows", (ds.Tables[1].Rows.Count * 7).ToString());
                        parameters.Add("@total_articulos", ds.Tables[1].Rows.Count.ToString());

                        tablaProductos.Append("<tr>");
                        tablaProductos.Append($"<td class='tg-zv4m' rowspan='2'> <img src='{string.Format("{0}{1}{2}", urlImg, item["CodeBarra"].ToString(), exteImg)}' alt='Image' width='75' height='60'></td>");
                        tablaProductos.Append($"<td class='tg-t0vf' rowspan='2'> {item["ProductName"].ToString()} <br> {item["Quantity"].ToString()} </td>");
                        tablaProductos.Append($"<td class='tg-zv4m'>Puntos a descontar: [ <span style='color:#FE0000'>Numeros</span>]</td>");
                        tablaProductos.Append($"<td class='tg-zv4m' rowspan='2'>[ <span style='font-weight:bold'>Numero total</span>]</td>");
                        tablaProductos.Append("</tr>");
                        tablaProductos.Append("<tr>");
                        tablaProductos.Append("<td class='tg-zv4m'>Puntos a devolver: [ <span style='color:#BBBE2F'>Numeros</span>]</td>");
                        tablaProductos.Append("</tr>");
                        //< tr >
                        //<td class='tg-zv4m' rowspan='2'><img src = 'C:\Users\josera\Downloads\html\html\producto.jpg' alt='prueba' width='72' height='54'></td>
                        //<td class='tg-t0vf' rowspan='2'>[Nombre del producto]<br>[Dato]</td>
                        //<td class='tg-zv4m'>Puntos a descontar: [ <span style='color:#FE0000'>Numeros</span>]</td>
                        //<td class='tg-zv4m' rowspan='2'>[ <span style='font-weight:bold'>Numero total</span>]</td>
                        //</tr>
                        //<tr>
                        //<td class='tg-zv4m'>Puntos a devolver: [ <span style='color:#BBBE2F'>Numeros</span>]</td>
                        //</tr>

                    }
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
        public static void DatosGrales(ref Dictionary<string, string> parameters, int OrderNo)
        {
            var ds = DALCorreos.spDatosGrales_sUP(OrderNo);

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                //correo.UeNo = item["UeNo"].ToString();
                //correo.OrderNo = item["OrderNo"].ToString();
                parameters.Add("@pedido", item["OrderNo"].ToString());
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
                parameters.Add("@total", item["Total"].ToString());
                //correo.StoreNum = item["Desc_UN"].ToString();
                //correo.Desc_UN = item["Desc_UN"].ToString();
                //correo.Total = item["Total"].ToString();
            }
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

    }

    public class MailMessage
    {
        public int LayoutId { get; set; }
        public string MailTo { get; set; }
        public Dictionary<string, string> Parameters { get; set; }


    }


}