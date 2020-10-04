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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employee()
        {
            DayOffs = new HashSet<DayOff>();
            StatisticEmployees = new HashSet<StatisticEmployee>();
            Violators = new HashSet<Violator>();
            ViolatorKTVs = new HashSet<ViolatorKTV>();
        }

        public long ID { get; set; }

        public int? Department_ID { get; set; }

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

        [StringLength(200)]
        public string Hash { get; set; }

        public bool StatusAccount { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DayOff> DayOffs { get; set; }

        public virtual Department Department { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StatisticEmployee> StatisticEmployees { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Violator> Violators { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ViolatorKTV> ViolatorKTVs { get; set; }
    }
}
