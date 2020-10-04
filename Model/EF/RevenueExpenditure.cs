namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RevenueExpenditure")]
    public partial class RevenueExpenditure
    {
        public long ID { get; set; }

        public int? Department_ID { get; set; }

        public int Type_ID { get; set; }

        [StringLength(500)]
        public string Contents { get; set; }

        public decimal Money { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        public virtual Department Department { get; set; }

        public virtual ReExType ReExType { get; set; }
    }
}
