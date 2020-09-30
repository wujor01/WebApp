namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StatisticTicket")]
    public partial class StatisticTicket
    {
        public long ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Datetime { get; set; }

        public int? Ticket_ID { get; set; }

        public int? TicketinDate { get; set; }

        public decimal? TicketPriceinDate { get; set; }

        [StringLength(500)]
        public string DailyList_ID { get; set; }

        public virtual Ticket Ticket { get; set; }
    }
}
