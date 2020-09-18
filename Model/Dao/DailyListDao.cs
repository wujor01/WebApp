using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class DailyListDao
    {
        WebAppDbContext db = null;
        public DailyListDao()
        {
            db = new WebAppDbContext();
        }

        public long Insert(DailyList list)
        {
            if (list.Taxi.Code == null)
            {
                //lưu vào db
                db.DailyLists.Add(list);
                db.SaveChanges();
                //tìm trong db để sửa lại Taxi.Code nề null và xóa bảng null taxi
                var dailylist = db.DailyLists.Find(list.ID);
                var taxi = db.Taxis.Find(dailylist.Taxi_ID);
                db.Taxis.Remove(taxi);
                dailylist.Taxi_ID = null;
                db.SaveChanges();
            }
            else
            {
                db.DailyLists.Add(list);
                db.SaveChanges();
            }
            return list.ID;
        }

        public long Update(DailyList entity)
        {
            var dailyList = db.DailyLists.Find(entity.ID);
            dailyList.Code = entity.Code;
            dailyList.Description = entity.Description;
            dailyList.Employee_Code = entity.Employee_Code;
            dailyList.Room = entity.Room;
            dailyList.Status = entity.Status;
            dailyList.Ticket = entity.Ticket;
            dailyList.TimeIn = entity.TimeIn;
            dailyList.TimeOut = entity.TimeOut;
            dailyList.Tip = entity.Tip;
            dailyList.Total = entity.Total;
            dailyList.Voucher = entity.Voucher;

            //Ngày chỉnh sửa = Now
            dailyList.ModifiedDate = DateTime.Now;
            db.SaveChanges();
            return entity.ID;
        }

        public DailyList ViewDetail(int list_ID)
        {
            return db.DailyLists.Find(list_ID);
        }

        //Phân trang quản lý user và thêm mục tìm kiếm theo username và email
        public IEnumerable<DailyList> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<DailyList> model = db.DailyLists;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(
                    x => x.Employee_Code.Contains(searchString) || x.Room.Contains(searchString)
                );
            }

            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public bool Delete(int id)
        {
            try
            {
                var dailyList = db.DailyLists.Find(id);
                db.DailyLists.Remove(dailyList);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
