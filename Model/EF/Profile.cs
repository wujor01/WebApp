namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Profile")]
    public partial class Profile
    {
        public long ID { get; set; }

        public bool? ApplicationForm { get; set; }

        public bool? CV { get; set; }

        public bool? HouseholdBook { get; set; }

        public bool? CardID { get; set; }

        [StringLength(150)]
        public string Qualification { get; set; }

        public long? Employee_ID { get; set; }

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
