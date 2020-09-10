namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserGroup")]
    public partial class UserGroup
    {
        [Key]
        [StringLength(20)]
        public string GroupID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }
    }
}
