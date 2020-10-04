namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrderDetail")]
    public partial class OrderDetail
    {
        public long ID { get; set; }

        public long DailyList_ID { get; set; }

        public int Room_ID { get; set; }

        public int Ticket_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Employee_ID { get; set; }

        public DateTime? TimeIn { get; set; }

        public DateTime? TimeOut { get; set; }

        public decimal? Tip { get; set; }

        public decimal Amount { get; set; }

        public virtual DailyList DailyList { get; set; }

        public virtual Room Room { get; set; }

        public virtual Ticket Ticket { get; set; }
    }
}
