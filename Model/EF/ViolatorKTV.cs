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

        [Display(Name = "Mã nhân viên")]
        public long? Employee_ID { get; set; }

        [Display(Name = "Vi phạm")]
        [StringLength(500)]
        public string Description { get; set; }

        [Display(Name ="Có phép")]
        public bool? Request { get; set; }

        [Display(Name = "Tua")]
        public decimal? Tour { get; set; }

        [Display(Name = "Trái cây")]
        public decimal? Fruit { get; set; }

        [Display(Name = "VSCSVC Thang máy")]
        public decimal? Elevator { get; set; }

        [Display(Name = "Thế chân")]
        public decimal? Substitution { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
