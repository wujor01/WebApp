using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.Models
{
    public class DailyListViewModel
    {
        public int Department_ID { get; set; }

        public int Ticket_ID { get; set; }

        public int Ticket_count { get; set; }

        public decimal Ticket_total { get; set; }

        public decimal Taxi_total { get; set; }

        public decimal Voucher_total { get; set; }
    }
}