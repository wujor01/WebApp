using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                return db.DailyLists.Where(x => x.Department_ID == departmentId).ToList();
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

        public long Insert(DailyList list, OrderDetail order)
        {
            if (list.Voucher_ID == 0 || list.Taxi == null)
            {
                list.Status = true;
            }
            else
            {
                list.Status = false;
            }

            db.DailyLists.Add(list);
            db.OrderDetails.Add(order);
            db.SaveChanges();

            return list.ID;
        }

        public long Update(DailyList entity, string username)
        {

            var dailyList = db.DailyLists.Find(entity.ID);


            if (entity.Voucher_ID != dailyList.Voucher_ID)
            {
                dailyList.Voucher_ID = entity.Voucher_ID;
                var order = db.OrderDetails.Where(x => x.DailyList_ID == entity.ID);
                var voucher = db.Vouchers.Find(entity.Voucher_ID);
                foreach (var item in order)
                {
                    var O = db.OrderDetails.Find(item.ID);
                    var ticket = db.Tickets.Find(item.Ticket_ID);
                    if (entity.Voucher_ID == 0)
                    {
                        O.Amount = ticket.Price;
                    }
                    else
                    {
                        O.Amount = ticket.Price * (1 - voucher.DiscountPercent / 100);
                    }
                }
                db.SaveChanges();
            }

            if (entity.Taxi.Price != 0 && dailyList.Taxi_ID == null)
            {
                Taxi taxi = new Taxi();
                taxi.Code = entity.Taxi.Code;
                taxi.Name = entity.Taxi.Name;
                taxi.NumberOfCustomers = entity.Taxi.NumberOfCustomers;
                taxi.Price = entity.Taxi.Price;
                taxi.Phone = entity.Taxi.Phone;
                taxi.Description = entity.Taxi.Description;
                db.Taxis.Add(taxi);
                db.SaveChanges();
                dailyList.Taxi_ID = taxi.ID;
            }
            else if (entity.Taxi.Price != 0 && dailyList.Taxi_ID != null)
            {
                var taxi = db.Taxis.Find(dailyList.Taxi_ID);
                taxi.Code = entity.Taxi.Code;
                taxi.Name = entity.Taxi.Name;
                taxi.NumberOfCustomers = entity.Taxi.NumberOfCustomers;
                taxi.Price = entity.Taxi.Price;
                taxi.Phone = entity.Taxi.Phone;
                taxi.Description = entity.Taxi.Description;
                db.SaveChanges();
            }
            else if (entity.Taxi.Price == 0 && dailyList.Taxi_ID != null)
            {
                var taxi = db.Taxis.Find(dailyList.Taxi_ID);
                db.Taxis.Remove(taxi);
                db.SaveChanges();
                dailyList.Taxi_ID = null;
            }
            dailyList.Total = db.OrderDetails.Where(x => x.DailyList_ID == entity.ID).Sum(x => x.Amount);

            dailyList.ModifiedBy = username;
            dailyList.ModifiedDate = DateTime.Now;
            db.SaveChanges();
            return entity.ID;
        }

        public long UpdateOrder(OrderDetail entity, string username)
        {

            var order = db.OrderDetails.Find(entity.ID);
            var dailyList = db.DailyLists.Find(order.DailyList_ID);

            order.Room_ID = entity.Room_ID;
            if (order.Employee_ID != string.Join(",", entity.SelectedIDArray).Replace(" ", ""))
            {
                string[] arrListStr = order.Employee_ID.Split(',');
                for (int i = 0; i < arrListStr.Length; i++)
                {
                    int a = Int32.Parse(arrListStr[i]);
                    var dailyEmployee = db.DailyEmployees.Find(entity.ID,a);
                    db.DailyEmployees.Remove(dailyEmployee);
                    db.SaveChanges();
                }
                

                order.Employee_ID = string.Join(",", entity.SelectedIDArray).Replace(" ", "");

                foreach (var temp in entity.SelectedIDArray)
                {
                    DailyEmployee emp = new DailyEmployee();
                    emp.Order_ID = entity.ID;
                    emp.Employee_ID = long.Parse(temp);
                    emp.Date = DateTime.Now.Date;
                    emp.Clean = 0;
                    emp.Tour = 0;
                    emp.Tour = 0;
                    db.DailyEmployees.Add(emp);
                    db.SaveChanges();
                }

            }
            
            var ticket = db.Tickets.Find(entity.Ticket_ID);

            if (entity.Ticket_ID != order.Ticket_ID)
            {
                order.Ticket_ID = entity.Ticket_ID;
                order.TimeIn = order.TimeIn;
                order.TimeOut = order.TimeIn.Value.AddMinutes(ticket.TimeTotal);

                if (dailyList.Voucher_ID == 0)
                {
                    order.Amount = ticket.Price;
                }
                else
                {
                    order.Amount = ticket.Price * (1 - dailyList.Voucher.DiscountPercent / 100);
                }

                dailyList.Total = db.OrderDetails.Where(x => x.DailyList_ID == order.DailyList_ID).Sum(x => x.Amount);
            }

            dailyList.ModifiedBy = username;
            dailyList.ModifiedDate = DateTime.Now;
            db.SaveChanges();
            return entity.ID;
        }

        public long UpdateEmp(DailyEmployee entity)
        {
            var order = db.OrderDetails.Find(entity.Order_ID);
            var emp = db.DailyEmployees.Find(entity.Order_ID,entity.Employee_ID);
            var employee = db.Employees.Find(entity.Employee_ID);
            var dep = db.Departments.Find(employee.Department.ID);
            var dailyList = db.DailyLists.Find(order.DailyList_ID);

            emp.Employee_ID = entity.Employee_ID;
            emp.Tip = entity.Tip;
            emp.Tour = dep.Tour;

            dailyList.Total = order.Amount + emp.Tour;
            db.SaveChanges();
            return entity.Order_ID;
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
                    x => x.No.ToString().Contains(searchString)
                );
            }
            if (departmentId == 0)
            {
                return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);

            }
            else
            {
                return model.OrderByDescending(x => x.CreatedDate).Where(x => x.Department_ID == departmentId).ToPagedList(page, pageSize);
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
                return model.OrderByDescending(x => x.CreatedDate).Where(x=>x.Department_ID == departmentId).ToPagedList(page, pageSize);
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

        public bool Comfirm(int id)
        {
            try
            {
                var dailyList = db.DailyLists.Find(id);
                if (dailyList.Status == false)
                {
                    dailyList.Status = true;
                }
                else
                {
                    dailyList.Status = false;
                }
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

        public long Order(OrderDetail order)
        {
            throw new NotImplementedException();
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
