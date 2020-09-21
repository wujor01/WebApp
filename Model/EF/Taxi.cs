namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Taxi")]
    public partial class Taxi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Taxi()
        {
            DailyLists = new HashSet<DailyList>();
        }

        public long ID { get; set; }

        [Display(Name = "Tên")]
        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "Ngày")]
        [DataType(DataType.Date)]
        public DateTime? DateTime { get; set; }

        [Display(Name = "Mã Grab-Taxi-CTV")]
        [StringLength(50)]
        public string Code { get; set; }

        [Display(Name = "Số khách")]
        public int? NumberOfCustomers { get; set; }

        [Display(Name = "% Hoa hồng")]
        [StringLength(50)]
        public string Commission { get; set; }

        [Display(Name = "Số tiền")]
        public decimal? Price { get; set; }

        [Display(Name = "SĐT")]
        [StringLength(10)]
        public string Phone { get; set; }

        [Display(Name = "Ghi chú")]
        [StringLength(500)]
        public string Description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DailyList> DailyLists { get; set; }
    }
}
