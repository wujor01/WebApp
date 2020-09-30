namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StatisticEmployee")]
    public partial class StatisticEmployee
    {
        public long ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Datetime { get; set; }

        public long? Employee_ID { get; set; }

        public decimal? TipinDate { get; set; }

        public int? CountinDate { get; set; }

        public decimal? RevenueinDatefromEmployee { get; set; }

        [StringLength(500)]
        public string DailyList_ID { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
