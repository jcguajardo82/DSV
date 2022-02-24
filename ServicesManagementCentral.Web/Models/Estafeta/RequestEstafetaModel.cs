using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.Estafeta
{
    public class RequestEstafetaModel
    {
        public LabelDefinition labelDefinition { get; set; }
    }
    public class Identification
    {
        public string customerNumber { get; set; }
        public string suscriberId { get; set; }
    }

    public class Merchandise
    {
        public double merchandiseValue { get; set; }
        public string currency { get; set; }
        public string productServiceCode { get; set; }
        public double merchandiseQuantity { get; set; }
        public string measurementUnitCode { get; set; }
        public string tariffFraction { get; set; }
        public string UUIDExteriorTrade { get; set; }
        public bool isInternational { get; set; }
        public bool isImport { get; set; }
        public bool isHazardousMaterial { get; set; }
        public string hazardousMaterialCode { get; set; }
        public string packagingCode { get; set; }
    }

    public class Merchandises
    {
        public decimal totalGrossWeight { get; set; }
        public string weightUnitCode { get; set; }
        public List<Merchandise> merchandise { get; set; }
    }

    public class ItemDescription
    {
        public int parcelId { get; set; }
        public decimal weight { get; set; }
        public int height { get; set; }
        public int length { get; set; }
        public int width { get; set; }
        public Merchandises merchandises { get; set; }
    }

    public class Address
    {
        public bool bUsedCode { get; set; }
        public string roadTypeCode { get; set; }
        public string roadTypeAbbName { get; set; }
        public string roadName { get; set; }
        public string townshipCode { get; set; }
        public string townshipName { get; set; }
        public string settlementTypeCode { get; set; }
        public string settlementTypeAbbName { get; set; }
        public string settlementName { get; set; }
        public string stateCode { get; set; }
        public string stateAbbName { get; set; }
        public string zipCode { get; set; }
        public string countryCode { get; set; }
        public string countryName { get; set; }
        public string addressReference { get; set; }
        public string betweenRoadName1 { get; set; }
        public string betweenRoadName2 { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string externalNum { get; set; }
        public string indoorInformation { get; set; }
        public string nave { get; set; }
        public string platform { get; set; }
        public string localityCode { get; set; }
        public string localityName { get; set; }
    }

    public class Contact
    {
        public string cellPhone { get; set; }
        public string contactName { get; set; }
        public string corporateName { get; set; }
        public string email { get; set; }
        public object phoneExt { get; set; }
        public string telephone { get; set; }
    }

    public class HomeAddress
    {
        public Address address { get; set; }
        public Contact contact { get; set; }
    }

    public class Destination
    {
        public object deliveryPUDOCode { get; set; }
        public HomeAddress homeAddress { get; set; }
        public bool isDeliveryToPUDO { get; set; }
    }
    public class Residence
    {
        public Contact contact { get; set; }
        public Address address { get; set; }
    }

    public class Notified
    {
        public string notifiedTaxIdCode { get; set; }
        public string notifiedTaxCountry { get; set; }
        public Residence residence { get; set; }
    }

    public class Origin
    {
        public Address address { get; set; }
        public Contact contact { get; set; }
    }

    public class Location
    {
        public bool isDRAAlternative { get; set; }
        public Origin origin { get; set; }
        public Destination destination { get; set; }
        public Notified notified { get; set; }
    }

    public class Insurance
    {
        public string contentDescription { get; set; }
        public double declaredValue { get; set; }
    }

    public class ReturnDocument
    {
        public string type { get; set; }
        public string serviceId { get; set; }
    }

    public class ServiceConfiguration
    {
        public int quantityOfLabels { get; set; }
        public string serviceTypeId { get; set; }
        public string salesOrganization { get; set; }
        public string effectiveDate { get; set; }
        public string originZipCodeForRouting { get; set; }
        public bool isInsurance { get; set; }
        public Insurance insurance { get; set; }
        public bool isReturnDocument { get; set; }
        public ReturnDocument returnDocument { get; set; }
    }

    public class WayBillDocument
    {
        public string aditionalInfo { get; set; }
        public string content { get; set; }
        public string costCenter { get; set; }
        public string customerShipmentId { get; set; }
        public string groupShipmentId { get; set; }
        public string referenceNumber { get; set; }
    }

    public class LabelDefinition
    {
        public ItemDescription itemDescription { get; set; }
        public Location location { get; set; }
        public ServiceConfiguration serviceConfiguration { get; set; }
        public WayBillDocument wayBillDocument { get; set; }
    }

    public class SystemInformation
    {
        public string id { get; set; }
        public string name { get; set; }
        public string version { get; set; }
    }
}