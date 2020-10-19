namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Department")]
    public partial class Department
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Department()
        {
            DailyLists = new HashSet<DailyList>();
            Employees = new HashSet<Employee>();
            RevenueExpenditures = new HashSet<RevenueExpenditure>();
            Rooms = new HashSet<Room>();
            StatisticDepartments = new HashSet<StatisticDepartment>();
            Tickets = new HashSet<Ticket>();
        }

        public int ID { get; set; }

        [StringLength(50)]
        [Display(Name = "Tên chi nhánh")]
        public string Name { get; set; }

        [Display(Name = "Địa chỉ")]
        [StringLength(200)]
        public string Address { get; set; }

        public bool Status { get; set; }

        [Display(Name = "Phí mỗi Tua")]
        public decimal Tour { get; set; }

        [Display(Name = "Phí vệ sinh")]
        public decimal Clean { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DailyList> DailyLists { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employee> Employees { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RevenueExpenditure> RevenueExpenditures { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Room> Rooms { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StatisticDepartment> StatisticDepartments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
