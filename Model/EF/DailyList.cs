namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DailyList")]
    public partial class DailyList
    {
        public long ID { get; set; }

        [Display(Name = "Mã KTV")]
        public long? Employee_ID { get; set; }

        [Display(Name = "Phòng")]
        [StringLength(10)]
        public string Room { get; set; }

        [Display(Name = "Giờ vào")]
        [DataType(DataType.Time)]
        public TimeSpan? TimeIn { get; set; }

        [Display(Name = "Giờ ra")]
        [DataType(DataType.Time)]
        public TimeSpan? TimeOut { get; set; }

        [Display(Name = "Giá vé")]
        public decimal? Ticket { get; set; }

        public decimal? Tip { get; set; }

        public decimal? Code { get; set; }

        public decimal? Voucher { get; set; }

        public long? Taxi_ID { get; set; }

        public decimal? Total { get; set; }

        [Display(Name = "Xác nhận")]
        public bool Status { get; set; }

        [Display(Name = "Yêu cầu")]
        [StringLength(500)]
        public string Description { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual Taxi Taxi { get; set; }
    }
}
