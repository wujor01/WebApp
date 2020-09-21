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
            if (list.Tip == null)
            {
                list.Tip = 0;
            }
            if (list.Code == null)
            {
                list.Code = 0;
            }
            if (list.Discount == null)
            {
                list.Discount = 0;
            }   
            if (list.Taxi.Code == null)
            {
                if (list.Code == 0 && list.Discount == 0)
                {
                    list.Status = true;
                }
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
                if (list.Code == 0 && list.Discount == 0 && list.Taxi.Price == null)
                {
                    list.Status = true;
                }
                if (list.Taxi.Price == null)
                {
                    list.Taxi.Price = 0;
                }
                db.DailyLists.Add(list);
                db.SaveChanges();
            }
            return list.ID;
        }

        public long Update(DailyList entity)
        {

            var dailyList = db.DailyLists.Find(entity.ID);
            if (entity.Tip == null)
            {
                entity.Tip = 0;
            }
            if (entity.Code == null)
            {
                entity.Code = 0;
            }
            if (entity.Discount == null)
            {
                entity.Discount = 0;
            }
            dailyList.Code = entity.Code;
            dailyList.Description = entity.Description;
            dailyList.Employee_ID = entity.Employee_ID;
            dailyList.Room = entity.Room;
            dailyList.Status = entity.Status;
            dailyList.Ticket = entity.Ticket;
            dailyList.TimeIn = entity.TimeIn;
            dailyList.TimeOut = entity.TimeOut;
            dailyList.Tip = entity.Tip;
            dailyList.Total = entity.Total;
            dailyList.Discount = entity.Discount;

            if (entity.Taxi.Code != null)
            {
                if (entity.Taxi.Price == null)
                {
                    entity.Taxi.Price = 0;
                }
                dailyList.Taxi = entity.Taxi;
            }

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
        public IEnumerable<DailyList> ListAllPaging(string searchString, int page, int pageSize, int departmentId)
        {
            IQueryable<DailyList> model = db.DailyLists;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(
                    x => x.Employee.Code.Contains(searchString) || x.ID.ToString().Contains(searchString)
                );
            }
            if (departmentId == 0)
            {
                return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);

            }
            else
            {
                return model.OrderByDescending(x => x.CreatedDate).Where(x => x.Employee.Department_ID == departmentId).ToPagedList(page, pageSize);
            }
        }

        public IEnumerable<DailyList> ListAllPagingTaxi(string searchString, int page, int pageSize, int departmentId)
        {
            IQueryable<DailyList> model = db.DailyLists;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(
                    x => x.Taxi.Code.Contains(searchString) || x.Taxi.Phone.ToString().Contains(searchString)
                );
            }
            if (true)
            {
                return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
            }
            else
            {
                return model.OrderByDescending(x => x.CreatedDate).Where(x=>x.Employee.Department_ID == departmentId).ToPagedList(page, pageSize);
            }
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
