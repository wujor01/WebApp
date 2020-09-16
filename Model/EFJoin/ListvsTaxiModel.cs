﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.Models
{
    public class ListvsTaxiModel
    {
        public long ID { get; set; }

        [StringLength(10)]
        public string Employee_Code { get; set; }

        [StringLength(10)]
        public string Room { get; set; }

        public DateTime? TimeIn { get; set; }

        public DateTime? TimeOut { get; set; }

        public decimal? Ticket { get; set; }

        public decimal? Tip { get; set; }

        public decimal? Code { get; set; }

        public decimal? Voucher { get; set; }

        public long Taxi_ID { get; set; }

        [StringLength(50)]
        public string Taxi_Name { get; set; }

        public DateTime? Taxi_DateTime { get; set; }

        [StringLength(50)]
        public string Taxi_Code { get; set; }

        public int? Taxi_NumberOfCustomers { get; set; }

        public decimal? Taxi_Commission { get; set; }

        public decimal? Taxi_Price { get; set; }

        [StringLength(10)]
        public string Taxi_Phone { get; set; }

        [StringLength(500)]
        public string Taxi_Description { get; set; }

        public decimal? Total { get; set; }

        public bool Status { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }
    }
}