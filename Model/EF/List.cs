namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("List")]
    public partial class List
    {
        public long ID { get; set; }

        [StringLength(10)]
        public string Room { get; set; }

        public DateTime? TimeIn { get; set; }

        public DateTime? TimeOut { get; set; }

        public decimal? Fare { get; set; }

        public decimal? Tip { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        public long? Taxi_ID { get; set; }

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
