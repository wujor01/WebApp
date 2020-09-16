using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ListvsTaxiDao
    {
        WebAppDbContext db = null;
        public ListvsTaxiDao()
        {
            db = new WebAppDbContext();
        }

        public long Insert(DailyList list, Taxi taxi)
        {
            if (taxi.Code != null)
            {
                db.Taxi.Add(taxi);
                db.SaveChanges();
            }
            db.DailyList.Add(list);
            db.SaveChanges();
            return list.ID;
        }


        //Phân trang quản lý user và thêm mục tìm kiếm theo username và email
        public IEnumerable<DailyList> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<DailyList> model = db.DailyList;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(
                    x => x.Employee_Code.Contains(searchString) || x.Room.Contains(searchString)
                );
            }

            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
    }
}
