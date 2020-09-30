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

        [Display(Name = "Chi nhánh")]
        public int? Department_ID { get; set; }

        [Display(Name = "Tên")]
        [StringLength(150)]
        public string Name { get; set; }

        [Display(Name = "Mã nhân viên")]
        [StringLength(10)]
        public string Code { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }

        [Display(Name = "Ảnh")]
        [StringLength(250)]
        [DataType(DataType.Upload)]
        public string Image { get; set; }

        [Display(Name = "SĐT")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Không phải số")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10)]
        public string Phone { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        [Display(Name = "Số ngày nghỉ")]
        public int? NumberOfDayOff { get; set; }

        [Display(Name = "Trạng thái")]
        public bool Status { get; set; }

        [Display(Name = "Ghi chú")]
        [StringLength(500)]
        public string Description { get; set; }

        [Display(Name = "Đơn xin việc")]
        public bool ApplicationForm { get; set; }

        public bool CV { get; set; }

        [Display(Name = "Sổ hộ khẩu")]
        public bool HouseholdBook { get; set; }

        [Display(Name = "CMND")]
        public bool CardID { get; set; }

        [Display(Name = "Bằng cấp")]
        [StringLength(150)]
        public string Certificate { get; set; }

        [Display(Name = "Giờ vào ca")]
        [DataType(DataType.Time)]
        public TimeSpan? TimeStart { get; set; }

        [Display(Name = "Giờ tan ca")]
        [DataType(DataType.Time)]
        public TimeSpan? TimeOut { get; set; }

        [StringLength(20)]
        public string GroupID { get; set; }

        [Display(Name = "Tài khoản")]
        [StringLength(50)]
        public string Username { get; set; }

        [Display(Name = "Mật khẩu")]
        [StringLength(150)]
        public string Password { get; set; }

        [StringLength(200)]
        public string Hash { get; set; }

        [Display(Name = "Trạng thái tài khoản")]
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
