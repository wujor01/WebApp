namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ViolatorKTV")]
    public partial class ViolatorKTV
    {
        public long ID { get; set; }

        public long? Employee_ID { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public bool? Request { get; set; }

        public decimal Tour { get; set; }

        public decimal Fruit { get; set; }

        public decimal Elevator { get; set; }

        public decimal Substitution { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
