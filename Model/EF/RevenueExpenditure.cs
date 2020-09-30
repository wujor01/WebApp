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

        [Display(Name ="Chi nhánh")]
        public int? Department_ID { get; set; }

        [Display(Name = "Loại")]
        public int Type_ID { get; set; }

        [Display(Name = "Nội dung")]
        [StringLength(500)]
        public string Contents { get; set; }

        [Display(Name = "Số tiền")]
        [DataType(DataType.Currency)]
        public decimal Money { get; set; }

        [Display(Name ="Ghi chú")]
        [StringLength(500)]
        public string Description { get; set; }

        [Display(Name = "Ngày")]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        public virtual ReExType ReExType { get; set; }
    }
}
