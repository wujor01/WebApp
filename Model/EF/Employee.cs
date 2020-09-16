namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Employee")]
    public partial class Employee
    {
        public long ID { get; set; }

        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Birthday { get; set; }

        [StringLength(250)]
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

        public bool ApplicationForm { get; set; }

        public bool CV { get; set; }

        public bool HouseholdBook { get; set; }

        public bool CardID { get; set; }

        [StringLength(150)]
        public string Certificate { get; set; }

        public TimeSpan? TimeStart { get; set; }

        public TimeSpan? TimeOut { get; set; }

        [StringLength(20)]
        public string GroupID { get; set; }

        [StringLength(50)]
        public string Username { get; set; }

        [StringLength(150)]
        public string Password { get; set; }

        public bool StatusAccount { get; set; }
    }
}
