using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class EmployeeDao
    {
        //Khai báo db
        WebAppDbContext db = null;
        public EmployeeDao()
        {
            db = new WebAppDbContext();
        }

        public long Insert(Employee entity)
        {
            db.Employee.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public bool Update(Employee entity)
        {
            try
            {
                var employee = db.Employee.Find(entity.ID);
                //Đểm số ngày nghỉ
                int countdayoff = db.DayOff.Where(c => c.ID == entity.ID).ToList().Count();

                employee.Name = entity.Name;
                employee.Phone = entity.Phone;
                employee.Birthday = entity.Birthday;                
                employee.Image = entity.Image;
                employee.Code = entity.Code;
                employee.Status = entity.Status;
                employee.Description = entity.Description;
                employee.NumberOfDayOff = countdayoff;
                //CV
                employee.ApplicationForm = entity.ApplicationForm;
                employee.CV = entity.CV;
                employee.HouseholdBook = entity.HouseholdBook;
                employee.CardID = entity.CardID;
                employee.Certificate = entity.Certificate;
                //Tài khoản đăng nhập hệ thống
                employee.Username = entity.Username;
                if (!string.IsNullOrEmpty(entity.Password))
                {
                    employee.Password = entity.Password;
                }
                employee.TimeStart = entity.TimeStart;
                employee.TimeOut = entity.TimeOut;
                employee.StatusAccount = entity.StatusAccount;
                //Ngày chỉnh sửa = Now
                employee.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                //logging
                return false;
            }
        }

        public Employee GetById(string userName)
        {
            return db.Employee.SingleOrDefault(x => x.Username == userName);
        }

        public Employee ViewDetail(int id)
        {
            return db.Employee.Find(id);
        }
        //chuyển các dạng dữ liệu bool về kiểu tích chọn không phải dạng list chọn
        public bool ChangeStatus(long id)
        {
            var employee = db.Employee.Find(id);
            employee.Status = !employee.Status;
            db.SaveChanges();
            return employee.Status;
        }
        public bool ChangeStatusAccount(long id)
        {
            var employee = db.Employee.Find(id);
            employee.StatusAccount = !employee.StatusAccount;
            db.SaveChanges();
            return employee.StatusAccount;
        }
        public bool ChangeApplicationForm(long id)
        {
            var employee = db.Employee.Find(id);
            employee.ApplicationForm = !employee.ApplicationForm;
            db.SaveChanges();
            return employee.ApplicationForm;
        }
        public bool ChangeCV(long id)
        {
            var employee = db.Employee.Find(id);
            employee.CV = !employee.CV;
            db.SaveChanges();
            return employee.CV;
        }
        public bool ChangeHouseholdBook(long id)
        {
            var employee = db.Employee.Find(id);
            employee.HouseholdBook = !employee.HouseholdBook;
            db.SaveChanges();
            return employee.HouseholdBook;
        }
        public bool ChangeCardID(long id)
        {
            var employee = db.Employee.Find(id);
            employee.CardID = !employee.CardID;
            db.SaveChanges();
            return employee.CardID;
        }

        public bool Delete(int id)
        {
            try
            {
                var emplyee = db.Employee.Find(id);
                db.Employee.Remove(emplyee);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool CheckUserName(string userName)
        {
            return db.Employee.Count(x => x.Username == userName) > 0;
        }
        public bool CheckEmail(string phone)
        {
            return db.Employee.Count(x => x.Phone == phone) > 0;
        }

        public int Login(string userName, string passWord)
        {
            var result = db.Employee.SingleOrDefault(x => x.Username == userName);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (result.Status == false)
                {
                    return -1;
                }
                else
                {
                    if (result.Password == passWord)
                    {
                        if (DateTime.Now.TimeOfDay > result.TimeStart && DateTime.Now.TimeOfDay < result.TimeOut)
                        {
                            return 1;
                        }
                        else
                        {
                            return -2;
                        }
                    }
                    else
                    {
                        return -3;
                    }
                }
            }
        }

    //Phân trang quản lý user và thêm mục tìm kiếm theo username và email
    public IEnumerable<Employee> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Employee> model = db.Employee;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(
                    x => x.Name.Contains(searchString) || x.Code.Contains(searchString)
                );
            }

            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        //Kiểm tra user đang nhập có quyền thực hiện các tác vụ thêm, sửa, xóa theo phân quyền

        //public List<string> GetListCredential(string userName)
        //{
        //    var user = db.User.Single(x => x.Username == userName);
        //    var data = (from a in db.Employee
        //                join b in db.UserGroups on a.UserGroupID equals b.ID
        //                join c in db.Roles on a.RoleID equals c.ID
        //                where b.ID == user.GroupID
        //                select new
        //                {
        //                    RoleID = a.RoleID,
        //                    UserGroupID = a.UserGroupID
        //                }).AsEnumerable().Select(x => new Credential()
        //                {
        //                    RoleID = x.RoleID,
        //                    UserGroupID = x.UserGroupID
        //                });
        //    return data.Select(x => x.RoleID).ToList();

        //}
    }
}
