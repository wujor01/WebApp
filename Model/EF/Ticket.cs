namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ticket")]
    public partial class Ticket
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ticket()
        {
            DailyLists = new HashSet<DailyList>();
        }

        public int ID { get; set; }

        [Display(Name = "Loại vé")]
        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "Giá")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name = "Chi nhánh")]
        public int? Department_ID { get; set; }

        [Display(Name = "Tổng thời gian")]
        [DataType(DataType.Time)]
        public double TimeTotal { get; set; }

        [Display(Name = "Trạng thái")]
        public bool Status { get; set; }

        [Display(Name = "Tiều đề")]
        [StringLength(500)]
        public string Header { get; set; }

        [Display(Name = "Nội dung")]
        [StringLength(500)]
        public string Description { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DailyList> DailyLists { get; set; }

        public virtual Department Department { get; set; }
    }
}
