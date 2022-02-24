using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.Estafeta
{
    public class ResponseEstafetaModel
    { 
        public byte[] data { get; set; }
        public LabelPetitionResult labelPetitionResult { get; set; }
        public GeneratorSystems generatorSystems { get; set; }
    }
    public class Element
    {
        public string wayBill { get; set; }
        public string trackingCode { get; set; }
    }

    public class Result
    {
        public int code { get; set; }
        public string description { get; set; }
    }

    public class LabelPetitionResult
    {
        public int elementsCount { get; set; }
        public List<Element> elements { get; set; }
        public string destinationAddress { get; set; }
        public string referenceNumber { get; set; }
        public Result result { get; set; }
    }

    public class GeneratorSystems
    {
        public string name { get; set; }
        public string releasedDate { get; set; }
        public string version { get; set; }
    }

}