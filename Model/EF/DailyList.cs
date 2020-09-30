﻿namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DailyList")]
    public partial class DailyList
    {
        public long ID { get; set; }

        [Display(Name = "Mã KTV")]
        [StringLength(50)]
        public string Employee_ID { get; set; }

        [Display(Name = "Phòng")]
        public int? Room_ID { get; set; }

        public string[] SelectedIDArray { get; set; }

        [Display(Name = "Giờ vào")]
        public DateTime? TimeIn { get; set; }

        [Display(Name = "Giờ ra")]
        public DateTime? TimeOut { get; set; }

        [Display(Name = "Loại vé")]
        public int? Ticket_ID { get; set; }

        public decimal Tip { get; set; }

        [Display(Name = "Code/Voucher")]
        public long? Voucher_ID { get; set; }

        public long? Taxi_ID { get; set; }

        [Display(Name = "Tổng tiền")]
        public decimal Total { get; set; }

        [Display(Name = "Trạng thái")]
        public bool Status { get; set; }

        [Display(Name = "Yêu cầu")]
        [StringLength(500)]
        public string Request { get; set; }

        [Display(Name = "Ghi chú")]
        [StringLength(500)]
        public string Description { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        public virtual Room Room { get; set; }

        public virtual Taxi Taxi { get; set; }

        public virtual Ticket Ticket { get; set; }

        public virtual Voucher Voucher { get; set; }
    }
}
