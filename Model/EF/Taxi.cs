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
        public long ID { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public DateTime? DateTime { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        public int? NumberOfCustomers { get; set; }

        public decimal? Commission { get; set; }

        public decimal? Price { get; set; }

        [StringLength(10)]
        public string Phone { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
    }
}
