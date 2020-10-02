using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.Models
{
    public class ListViewModel
    {
        public int DepartmentName { get; set; }

        public int TicketName { get; set; }

        public int TicketPrice { get; set; }

        public decimal TicketCount { get; set; }

    }
}