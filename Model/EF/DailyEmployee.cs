namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DailyEmployee")]
    public partial class DailyEmployee
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Order_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Employee_ID { get; set; }

        public decimal Tip { get; set; }

        [Display(Name = "Tua")]
        public decimal Tour { get; set; }

        [Display(Name = "Vệ sinh")]
        public decimal Clean { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual OrderDetail OrderDetail { get; set; }
    }
}
