using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.EFView
{
    public class DailyListViews
    {
        private string _chinnhanh;
        private string _loaive;
        private long? _soluong;
        private decimal? _giave;
        private decimal? _tong;
        public DailyListViews(
            string Achinnhanh, string Aloaive, long? Asoluong, decimal? Agiave,
            decimal? Atong)
        {
            _chinnhanh = Achinnhanh;
            _loaive = Aloaive;
            _soluong = Asoluong;
            _giave = Agiave;
            _tong = Atong;
        }
        public string chinnhanh { get { return _chinnhanh; } }
        public string loaive { get { return _loaive; } }
        public long? soluong { get { return _soluong; } }
        public decimal? giave { get { return _giave; } }
        public decimal? tong { get { return _tong; } }
    }
    public class DailyListList : List<DailyListViews>
    {
        WebAppDbContext db = null;
        public DailyListList(DailyListViews dailylist)
        {
            db = new WebAppDbContext();
            var query =
                from DailyList in db.DailyLists
                group new { DailyList.Ticket, DailyList.Room.Department, DailyList } by new
                {
                    DailyList.Ticket.Name,
                    Column1 = DailyList.Room.Department.Name
                } into g
                select new
                {
                    chinnhanh = g.Key.Column1,
                    loaive = g.Key.Name,
                    soluong = g.Count(p => p.DailyList.Ticket.ID != 0),
                    giave = (decimal?)g.Sum(p => p.DailyList.Ticket.Price),
                    tong = (decimal?)g.Sum(p => p.DailyList.Total)
                };
            foreach (var r in query)
                Add(new DailyListViews(
                    r.chinnhanh, r.loaive, r.soluong, r.giave, r.tong));
        }
    }

}
