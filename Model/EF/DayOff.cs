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

        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [Display(Name = "Ghi Chú")]
        [StringLength(500)]
        public string Description { get; set; }

        [Display(Name ="Mã nhân viên")]
        public long? Employee_ID { get; set; }

        [Display(Name = "Xác nhận")]
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
