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

        [Display(Name = "Mã NV")]
        public long? Employee_ID { get; set; }

        [Display(Name = "Loại")]
        public int Type_ID { get; set; }

        [Display(Name = "Chi tiết")]
        [StringLength(500)]
        public string Description { get; set; }

        [Display(Name = "Xác nhận")]
        public bool? Request { get; set; }

        [Display(Name = "Số tiền")]
        [DataType(DataType.Currency)]
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
