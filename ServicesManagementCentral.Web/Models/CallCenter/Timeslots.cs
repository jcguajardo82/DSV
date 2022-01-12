using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServicesManagement.Web.Models.CallCenter
{

    public class ResponseTimeSlot
    {
        public TimeSlots Timeslots { get; set; }
    }
    public class TimeSlots
    {
        public List<SlotWeeks> SlotWeeks { get; set; }
    }
    public class SlotWeeks
    {
        public string SlotWeekName { get; set; }
        public List<SlotsDays> SlotsDays { get; set; }

    }

    public class SlotsDays
    {
        public string SlotDayName { get; set; }
        public List<Slot> Slots { get; set; }
    }
    public class Slot
    {
        public string SlotName { get; set; }
        public string SlotDateTime { get; set; }

       
        public string slotEnabled { get; set; }
        public string SlotPrice { get; set; }

    }
}