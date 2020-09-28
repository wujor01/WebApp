namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Violator")]
    public partial class Violator
    {
        public long ID { get; set; }

        public long? Employee_ID { get; set; }

        public int Type_ID { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public bool? Request { get; set; }

        public decimal Loan { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual ViolatorType ViolatorType { get; set; }
    }
}
