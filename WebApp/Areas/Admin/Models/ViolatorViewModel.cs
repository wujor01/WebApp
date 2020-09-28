using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.Models
{
    public class ViolatorViewModel
    {
        public long Employee_ID { get; set; }

        public int Violator_DayOffcount { get; set; }

        public decimal Violator_DayOffloantotal { get; set; }
    }
}