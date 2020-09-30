namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StatisticDepartment")]
    public partial class StatisticDepartment
    {
        public long ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Datetime { get; set; }

        public int? Deparment_ID { get; set; }

        public decimal? TotalListinDate { get; set; }

        public int? TicketinDate { get; set; }

        public decimal? TicketPriceinDate { get; set; }

        public decimal? TipinDate { get; set; }

        public decimal? RevenueinDate { get; set; }

        public decimal? ExpenditureinDate { get; set; }

        public decimal? RevenueinDatefromEmployee { get; set; }

        [StringLength(500)]
        public string DailyList_ID { get; set; }

        [StringLength(100)]
        public string REEX_ID { get; set; }

        [StringLength(100)]
        public string StatisticTicket_ID { get; set; }

        [StringLength(500)]
        public string StatisticEmpyee_ID { get; set; }

        public virtual Department Department { get; set; }
    }
}
