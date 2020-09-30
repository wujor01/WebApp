namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DayOff")]
    public partial class DayOff
    {
        public long ID { get; set; }

        [Display(Name = "Ngày")]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [Display(Name = "Lý do")]
        [StringLength(500)]
        public string Description { get; set; }

        [Display(Name = "Nhân viên")]
        public long? Employee_ID { get; set; }

        [Display(Name = "Trạng thái")]
        public bool Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
