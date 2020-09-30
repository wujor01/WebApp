using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Model.Dao
{
    public class DailyListDao
    {
        WebAppDbContext db = null;
        public DailyListDao()
        {
            db = new WebAppDbContext();
        }

        public List<Voucher> ListAll()
        {
                return db.Vouchers.OrderByDescending(x => x.ExpirationDate).Where(x => x.Status == true && x.ExpirationDate > DateTime.Today).ToList();
        }

        public List<DailyList> DailyListAll(int departmentId)
        {
            if (departmentId == 0)
            {
                return db.DailyLists.ToList();
            }
            else
            {
                return db.DailyLists.Where(x => x.Room.Department_ID == departmentId).ToList();
            }
        }

        public long CodeInsert(Voucher voucher)
        {           
            var rand = new Random();
            string r = rand.Next(100000, 1000000).ToString();
            voucher.Status = true;
            voucher.DiscountPercent = 20;
            voucher.ExpirationDate = DateTime.Now.AddDays(14);
            do
            {              
                voucher.Code = string.Concat("MV ", r);
                db.Vouchers.Add(voucher);
            } while (db.Vouchers.Where(x => x.Code == voucher.Code).ToList().Count > 0);

            db.SaveChanges();
            return voucher.ID;
        }

        public long Insert(DailyList list)
        {

            var ticket = db.Tickets.Find(list.Ticket_ID);
            var voucher = db.Vouchers.Find(list.Voucher_ID);

            list.TimeIn = DateTime.Now;
            list.TimeOut = DateTime.Now.Add(TimeSpan.FromMinutes(ticket.TimeTotal));

            if (list.Taxi.Price == 0)
            {

                if (voucher.DiscountPercent !=0)
                {
                    list.Status = false;
                    list.Total = ticket.Price * (1 - voucher.DiscountPercent / 100) + list.Tip;
                }
                else
                {
                    list.Total = ticket.Price + list.Tip;
                }

                //lưu vào db
                db.DailyLists.Add(list);
                db.SaveChanges();

                var dailylist = db.DailyLists.Find(list.ID);
                if (voucher.ID > 99)
                {
                    voucher.Status = false;
                    voucher.ModifiedBy = list.CreatedBy;
                    voucher.ModifiedDate = list.CreatedDate;
                }
                //tìm trong db để sửa lại Taxi.Code về null và xóa bảng null taxi
                var taxi = db.Taxis.Find(dailylist.Taxi_ID);
                db.Taxis.Remove(taxi);
                dailylist.Taxi_ID = null;
                db.SaveChanges();
            }
            else
            {
                if (voucher.DiscountPercent != 0)
                {
                    list.Status = false;
                    list.Total = ticket.Price * (1 - voucher.DiscountPercent / 100) - list.Taxi.Price + list.Tip;
                }
                else
                {
                    list.Total = ticket.Price - list.Taxi.Price + list.Tip;
                }

                if (voucher.ID > 99 )
                {
                    voucher.Status = false;
                    voucher.ModifiedBy = list.CreatedBy;
                    voucher.ModifiedDate = list.CreatedDate;
                }
                db.DailyLists.Add(list);
                db.SaveChanges();
            }
            return list.ID;
        }

        public long Update(DailyList entity, string username)
        {

            var dailyList = db.DailyLists.Find(entity.ID);
            
            dailyList.Description = entity.Description;
            //dailyList.Employee_ID = entity.Employee_ID;
            dailyList.Room_ID = entity.Room_ID;
            dailyList.Status = entity.Status;
            dailyList.Tip = entity.Tip;
            dailyList.Ticket_ID = entity.Ticket_ID;
            dailyList.Voucher_ID = entity.Voucher_ID;

            var ticket = db.Tickets.Find(entity.Ticket_ID);
            var voucher = db.Vouchers.Find(entity.Voucher_ID);


            if (entity.Taxi.Price != 0)
            {
                if (voucher.ID != 0)
                {
                    dailyList.Total = ticket.Price * (1 - voucher.DiscountPercent / 100) - entity.Taxi.Price + entity.Tip;
                }
                else
                {
                    dailyList.Total = ticket.Price - entity.Taxi.Price + entity.Tip;
                }
                dailyList.Taxi = entity.Taxi;
            }
            else
            {
                if (voucher.DiscountPercent != 0)
                {
                    dailyList.Total = ticket.Price * (1 - voucher.DiscountPercent / 100) + entity.Tip;
                }
                else
                {
                    dailyList.Total = ticket.Price + entity.Tip;
                }
            }
            //Ngày chỉnh sửa = Now
            dailyList.ModifiedBy = username;
            dailyList.ModifiedDate = DateTime.Now;
            db.SaveChanges();
            return entity.ID;
        }

        public DailyList ViewDetail(int list_ID)
        {
            return db.DailyLists.Find(list_ID);
        }

        public Voucher ViewCodeDetail(int ID)
        {
            return db.Vouchers.Find(ID);
        }

        //Phân trang quản lý user và thêm mục tìm kiếm theo username và email
        public IEnumerable<DailyList> ListAllPaging(string searchString, int page, int pageSize, int departmentId)
        {
            IQueryable<DailyList> model = db.DailyLists; 

            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(
                    x => x.Room.Name.Contains(searchString) || x.ID.ToString().Contains(searchString)
                );
            }
            if (departmentId == 0)
            {
                return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);

            }
            else
            {
                return model.OrderByDescending(x => x.CreatedDate).Where(x => x.Room.Department_ID == departmentId).ToPagedList(page, pageSize);
            }
        }

        public IEnumerable<DailyList> ListAllPagingTaxi(string searchString, int page, int pageSize, int departmentId)
        {
            IQueryable<DailyList> model = db.DailyLists;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(
                    x => x.ID.ToString().Contains(searchString) || x.Taxi.Phone.ToString().Contains(searchString)
                );
            }
            if (departmentId == 0)
            {
                return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
            }
            else
            {
                return model.OrderByDescending(x => x.CreatedDate).Where(x=>x.Room.Department_ID == departmentId).ToPagedList(page, pageSize);
            }
        }

        public IEnumerable<Voucher> ListAllPagingCode(string searchString, int page, int pageSize)
        {
            IQueryable<Voucher> model = db.Vouchers;

            foreach (var item in model)
            {
                if (item.ExpirationDate < DateTime.Now)
                {
                    CodeExpirated(item.ID);
                }
                    
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(
                    x => x.Code.Contains(searchString) || x.ExpirationDate.ToString().Contains(searchString)
                );
            }
                return model.Where(x=> DateTime.Today < x.ExpirationDate && x.ID > 99).OrderByDescending(x => x.ExpirationDate).ToPagedList(page, pageSize);
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

        public bool CodeExpirated(long id)
        {
            try
            {
                var voucher = db.Vouchers.Find(id);
                voucher.Expirated = true;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public List<Employee> ListAll(string position, int departmentId)
        {
            if (departmentId == 0)
            {
                return db.Employees.Where(x => x.Status == true && x.Code.StartsWith(position) == true).ToList();

            }
            else
            {
                return db.Employees.Where(x => x.Status == true && x.Code.StartsWith(position) == true && x.Department_ID == departmentId).ToList();
            }
        }
    }
}
