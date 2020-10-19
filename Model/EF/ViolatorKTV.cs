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

        [Display(Name = "Ngày")]
        [DataType(DataType.Date)]
        public DateTime? Datetime { get; set; }

        [Display(Name = "Mã KTV")]
        public long? Employee_ID { get; set; }

        [Display(Name = "Chi tiết")]
        [StringLength(500)]
        public string Description { get; set; }

        [Display(Name = "Xác nhận")]
        public bool? Request { get; set; }

        public string[] SelectedIDArray { get; set; }

        [Display(Name = "Giờ vào ca")]
        [DataType(DataType.Time)]
        public DateTime TimeIn { get; set; }

        [Display(Name = "Giờ kết ca")]
        [DataType(DataType.Time)]
        public DateTime TimeOut { get; set; }

        [Display(Name = "Tua")]
        public decimal Tour { get; set; }

        [Display(Name = "Trái cây")]
        public decimal Fruit { get; set; }

        [Display(Name = "Vệ sinh")]
        public decimal Elevator { get; set; }

        [Display(Name = "Tip trong ngày")]
        public decimal? TipinDate { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
