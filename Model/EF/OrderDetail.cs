namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.CompilerServices;

    [Table("OrderDetail")]
    public partial class OrderDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrderDetail()
        {
            DailyEmployees = new HashSet<DailyEmployee>();
        }

        public long ID { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name ="HĐ")]
        public string No { get; set; }

        [Display(Name = "Mã bảng kê")]
        public long DailyList_ID { get; set; }

        [Display(Name = "Phòng")]
        public int Room_ID { get; set; }

        public string[] SelectedIDArray { get; set; }
        
        [Display(Name = "Vé")]
        public int Ticket_ID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "KTV_ID")]
        public string empId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Mã KTV")]
        public string Employee_ID { get; set; }

        [Display(Name = "Giờ vào")]
        [DataType(DataType.Time)]
        public DateTime TimeIn { get; set; }

        [Display(Name = "Giờ ra")]
        [DataType(DataType.Time)]
        public DateTime TimeOut { get; set; }

        [Display(Name = "Tổng tiền")]
        public decimal Amount { get; set; }

        [Display(Name = "Trạng thái")]
        public bool Status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DailyEmployee> DailyEmployees { get; set; }

        public virtual DailyList DailyList { get; set; }

        public virtual Room Room { get; set; }

        public virtual Ticket Ticket { get; set; }
    }
}
