using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models
{
    public class ResponseModels
    {

        public string Error { get; set; }
        public string[] Messages { get; set; }
        public string Reference { get; set; }
        public Labels[] Labels { get; set; }
    }

    public class Labels
    {
        public string[] Folios { get; set; }
        public byte[] PDF { get; set; }
    }
    public class EstafetaRequestModel
    {
        //Contenido
        //Centro de costos
        //public string costCenter { get; set; } = "CCtos";
        ////Ocurre
        //public bool deliveryToEstafetaOffice { get; set; } = false;
        ////En caso de envio a otro pais, solo siglasWS de generación de guías Estafeta Versión 2 Fecha: 31/01/2011
        ////Estafeta Mexicana S.A.de C.V. 36
        //public string destinationCountryId { get; set; } = "MX";
        ////Tipo de envio 1{get; set;} =SOBRE 4{get; set;} =PAQUETE
        //public int parcelTypeId { get; set; } = 4;
        //Referencia
        public string Courier { get; set; } = "EST";
        public string Reference { get; set; } = "Referencia";
        public int NumberLabels { get; set; } = 1;
        public decimal Volume { get; set; } = 1;
        //Peso
        public decimal Weight { get; set; } = 1;
        public string Content { get; set; } = "Documentos";
        public bool CustomerPickUp { get; set; } = false;
        public string AditionalInfo { get; set; } = "Informacion adicional";
        public string ServiceType { get; set; } = "70";
        //Numero de oficina que corresponde al cliente
        //public string officeNum { get; set; } = "421";
        ////Documento de retorno
        //public bool returnDocument { get; set; } = true;
        ////Servicio del documento de retorno
        //public string serviceTypeIdDocRet { get; set; } = "50";
        ////Fecha de vigencia
        //public string effectiveDate { get; set; } = "20110525";
        
        public AddressModel Destination { get; set; }

        public AddressModel Origin { get; set; }

    }


    public class AddressModel
    {


        public string Address1 { get; set; } = "public string addr1";
        public string Address2 { get; set; } = "public string Addr2";
        public string City { get; set; } = "Ciudad";
        public string ContactName { get; set; } = "Cliente";
        public string CorporateName { get; set; } = "Corporate";
        public string CustomerNumber { get; set; } = "1234568";
        public string Neighborhood { get; set; } = "neighborhood";
        public string PhoneNumber { get; set; } = "1111111";
        public string State { get; set; } = "Mexico";
        //Código postal destino
        public string ZipCode { get; set; } = "01000";


    }
}