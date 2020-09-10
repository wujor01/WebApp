namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    [Table("Employee")]
    public partial class Employee
    {
        public long ID { get; set; }

        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Upload File")]
        [Required(ErrorMessage = "Please choose file to upload.")]
        public string Image { get; set; }

        [StringLength(10)]
        public string Phone { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        public int? NumberOfDayOff { get; set; }

        public bool Status { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
    }
}
