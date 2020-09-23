namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Voucher")]
    public partial class Voucher
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Voucher()
        {
            DailyLists = new HashSet<DailyList>();
        }

        public long ID { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        [Display(Name ="% Giảm giá")]
        public decimal? DiscountPercent { get; set; }

        [Display(Name = "Giảm giá tối đa")]
        public decimal? Max { get; set; }

        [Display(Name = "Trạng thái")]
        public bool Status { get; set; }

        [Display(Name = "Ngày hết hạn")]
        [DataType(DataType.Date)]
        public DateTime? ExpirationDate { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DailyList> DailyLists { get; set; }
    }
}
