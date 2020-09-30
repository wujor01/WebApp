namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Customer")]
    public partial class Customer
    {
        public long ID { get; set; }

        [Display(Name ="Tên khách hàng")]
        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "CMND")]
        [StringLength(50)]
        public string CardID { get; set; }

        [Display(Name = "SĐT")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Không phải số")]
        [StringLength(10)]
        public string Phone { get; set; }

        [Display(Name = "Ghi chú")]
        [StringLength(500)]
        public string Description { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }
    }
}
