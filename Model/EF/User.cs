namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        public long ID { get; set; }

        [StringLength(50)]
        public string Username { get; set; }

        [StringLength(150)]
        public string Password { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public long? Employee_ID { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public bool Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        [StringLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

        public TimeSpan? TimeStart { get; set; }

        public TimeSpan? TimeOut { get; set; }

        [StringLength(20)]
        public string GroupID { get; set; }
    }
}
