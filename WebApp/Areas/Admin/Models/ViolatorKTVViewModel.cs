using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Areas.Admin.Models
{
    public class ViolatorKTVViewModel
    {
        public long Employee_ID { get; set; }

        public int KTV_selectcount { get; set; }

        public decimal KTV_TipTotal { get; set; }

        public decimal KTV_TuaTotal { get; set; }

        public decimal KTV_TraiCayTotal { get; set; }

        public decimal KTV_VeSinhTotal { get; set; }

        public decimal KTV_TheChanTotal { get; set; }

        public int KTV_DayOffcount { get; set; }
    }   
}