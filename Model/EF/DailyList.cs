namespace Model.EF
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

        [StringLength(50)]
        public string Employee_ID { get; set; }

        [NotMapped]
        public string[] SelectedIDArray { get; set; }

        public int? Room_ID { get; set; }

        public DateTime? TimeIn { get; set; }

        public DateTime? TimeOut { get; set; }

        public int? Ticket_ID { get; set; }

        public decimal Tip { get; set; }

        public long? Voucher_ID { get; set; }

        public long? Taxi_ID { get; set; }

        public decimal Total { get; set; }

        public bool Status { get; set; }

        [StringLength(500)]
        public string Request { get; set; }

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
