using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.Models
{
    public class TaxiViewModel
    {
        public int Department_ID { get; set; }

        public int Taxi_count { get; set; }

        public int Taxi_Customercount { get; set; }
    }
}