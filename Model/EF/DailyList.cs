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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DailyList()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public long ID { get; set; }

        [StringLength(20)]
        public string No { get; set; }

        public int? Department_ID { get; set; }

        public long? Voucher_ID { get; set; }

        public long? Taxi_ID { get; set; }

        public decimal PricewithVoucher { get; set; }

        public decimal Total { get; set; }

        public bool Status { get; set; }

        [StringLength(500)]
        public string Request { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        public virtual Department Department { get; set; }

        public virtual Taxi Taxi { get; set; }

        public virtual Voucher Voucher { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
