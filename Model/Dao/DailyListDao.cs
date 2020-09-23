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
                return db.Vouchers.OrderBy(x => x.Code).Where(x => x.Status == true).ToList();
        }

        public long CodeInsert(Voucher voucher)
        {           
            var rand = new Random();
            string r = rand.Next(100000, 1000000).ToString();
            voucher.Status = true;
            voucher.DiscountPercent = 20;
            voucher.ExpirationDate = DateTime.Now.AddDays(7);
            do
            {              
                voucher.Code = string.Concat("NV", r);
                db.Vouchers.Add(voucher);
            } while (db.Vouchers.Where(x => x.Code == voucher.Code).ToList().Count > 0);

            db.SaveChanges();
            return voucher.ID;
        }

        public long Insert(DailyList list)
        {
            if (list.Tip == null)
            {
                list.Tip = 0;
            }
            if (list.Discount == null)
            {
                list.Discount = 0;
            }

            var ticket = db.Tickets.Find(list.Ticket_ID);
            var voucher = db.Vouchers.Find(list.Voucher_ID);

            if (list.Taxi.Code == null)
            {
                if (list.Discount == 0)
                {
                    list.Status = true;
                }
                //lưu vào db
                db.DailyLists.Add(list);
                db.SaveChanges();

                if (voucher.DiscountPercent != 0)
                {
                    list.Total = ticket.Price * voucher.DiscountPercent / 100 - list.Discount + list.Tip;
                }
                else
                {
                    list.Total = ticket.Price - list.Discount - list.Taxi.Price + list.Tip;
                }

                var dailylist = db.DailyLists.Find(list.ID);
                if (list.Voucher_ID != 0)
                {
                    voucher.Status = false;
                    voucher.ModifiedBy = list.CreatedBy;
                    voucher.ModifiedDate = list.CreatedDate;
                }
                //tìm trong db để sửa lại Taxi.Code nề null và xóa bảng null taxi
                var taxi = db.Taxis.Find(dailylist.Taxi_ID);
                db.Taxis.Remove(taxi);
                dailylist.Taxi_ID = null;
                db.SaveChanges();
            }
            else
            {
                if (list.Discount == 0 && list.Taxi.Price == null)
                {
                    list.Status = true;
                }
                if (list.Taxi.Price == null)
                {
                    list.Taxi.Price = 0;
                }
                if (voucher.DiscountPercent != 0)
                {
                    list.Total = ticket.Price * voucher.DiscountPercent / 100 - list.Discount - list.Taxi.Price + list.Tip;
                }
                else
                {
                    list.Total = ticket.Price - list.Discount - list.Taxi.Price + list.Tip;
                }

                if (list.Voucher_ID != 0)
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
            if (entity.Tip == null)
            {
                entity.Tip = 0;
            }
            
            if (entity.Discount == null)
            {
                entity.Discount = 0;
            }
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
            dailyList.Customer_ID = entity.Customer_ID;
            dailyList.Ticket_ID = entity.Ticket_ID;
            dailyList.Voucher_ID = entity.Voucher_ID;

            var ticket = db.Tickets.Find(entity.Ticket_ID);
            var voucher = db.Vouchers.Find(entity.Voucher_ID);


            if (entity.Taxi.Code != null)
            {
                if (entity.Taxi.Price == null)
                {
                    entity.Taxi.Price = 0;
                }
                if (voucher.DiscountPercent != 0)
                {
                    dailyList.Total = ticket.Price * voucher.DiscountPercent / 100 - entity.Discount - entity.Taxi.Price + entity.Tip;
                }
                else
                {
                    dailyList.Total = ticket.Price - entity.Discount - entity.Taxi.Price + entity.Tip;
                }
                dailyList.Taxi = entity.Taxi;
            }
            else
            {
                if (voucher.DiscountPercent != 0)
                {
                    dailyList.Total = ticket.Price * voucher.DiscountPercent / 100 - entity.Discount + entity.Tip;
                }
                else
                {
                    dailyList.Total = ticket.Price - entity.Discount + entity.Tip;
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

        public IEnumerable<Voucher> ListAllPagingCode(string searchString, int page, int pageSize)
        {
            IQueryable<Voucher> model = db.Vouchers;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(
                    x => x.Code.Contains(searchString) || x.ExpirationDate.ToString().Contains(searchString)
                );
            }
                return model.Where(x=>x.ID > 0).OrderByDescending(x => x.Status).ToPagedList(page, pageSize);
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
