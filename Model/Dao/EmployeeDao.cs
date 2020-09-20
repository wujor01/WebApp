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
            if (CheckUserName(entity.Username) == false && CheckCode(entity.Code) == false)
            {
                db.Employees.Add(entity);
                db.SaveChanges();               
            }
            return entity.ID;
        }

        public long Update(Employee entity)
        {
            var employee = db.Employees.Find(entity.ID);

            //Đểm số ngày nghỉ
            int countdayoff = db.DayOffs.Where(c => c.Employee_ID == entity.ID).ToList().Count();
            employee.Name = entity.Name;
            employee.Phone = entity.Phone;
            if (!string.IsNullOrEmpty(entity.Birthday.ToString()))
            {
                employee.Birthday = entity.Birthday;
            }
            
            employee.Image = entity.Image;
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
                return 1;
        }

        public Employee GetById(string userName)
        {
            return db.Employees.SingleOrDefault(x => x.Username == userName);
        }

        public Employee ViewDetail(int id)
        {
            return db.Employees.Find(id);
        }
        //chuyển các dạng dữ liệu bool về kiểu tích chọn không phải dạng list chọn
        public bool ChangeStatus(long id)
        {
            var employee = db.Employees.Find(id);
            employee.Status = !employee.Status;
            db.SaveChanges();
            return employee.Status;
        }
        public bool ChangeStatusAccount(long id)
        {
            var employee = db.Employees.Find(id);
            employee.StatusAccount = !employee.StatusAccount;
            db.SaveChanges();
            return employee.StatusAccount;
        }
        public bool ChangeApplicationForm(long id)
        {
            var employee = db.Employees.Find(id);
            employee.ApplicationForm = !employee.ApplicationForm;
            db.SaveChanges();
            return employee.ApplicationForm;
        }
        public bool ChangeCV(long id)
        {
            var employee = db.Employees.Find(id);
            employee.CV = !employee.CV;
            db.SaveChanges();
            return employee.CV;
        }
        public bool ChangeHouseholdBook(long id)
        {
            var employee = db.Employees.Find(id);
            employee.HouseholdBook = !employee.HouseholdBook;
            db.SaveChanges();
            return employee.HouseholdBook;
        }
        public bool ChangeCardID(long id)
        {
            var employee = db.Employees.Find(id);
            employee.CardID = !employee.CardID;
            db.SaveChanges();
            return employee.CardID;
        }

        public bool Delete(int id)
        {
            try
            {
                var employee = db.Employees.Find(id);
                db.Employees.Remove(employee);
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
            return db.Employees.Count(x => x.Username == userName) > 0;
        }
        public bool CheckCode(string code)
        {
            return db.Employees.Count(x => x.Code == code) > 0;
        }

        public int Login(string userName, string passWord)
        {
            var result = db.Employees.SingleOrDefault(x => x.Username == userName);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (result.StatusAccount == false)
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
                        else if (result.TimeOut < result.TimeStart)
                        {
                            if (DateTime.Now.TimeOfDay > result.TimeStart && DateTime.Now.TimeOfDay > result.TimeOut)
                            {
                                return 1;
                            }
                            return -2;
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
    public IEnumerable<Employee> ListAllPaging(string searchString, int page, int pageSize, int departmentid)
        {
            IQueryable<Employee> model = db.Employees;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(
                    x => x.Name.Contains(searchString) || x.Code.Contains(searchString)
                );
            }

            if (departmentid == 0)
            {
                return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);

            }
            else
            {
                return model.OrderByDescending(x => x.CreatedDate).Where(x=>x.Department_ID == departmentid).ToPagedList(page, pageSize);
            }
        }

        //Kiểm tra user đang nhập có quyền thực hiện các tác vụ thêm, sửa, xóa theo phân quyền

        public List<string> GetListCredential(string userName)
        {
            var user = db.Employees.Single(x => x.Username == userName);
            var data = (from a in db.Credentials
                        join b in db.UserGroups on a.UserGroupID equals b.GroupID
                        join c in db.Roles on a.RoleID equals c.ID
                        where b.GroupID == user.GroupID
                        select new
                        {
                            RoleID = a.RoleID,
                            UserGroupID = a.UserGroupID
                        }).AsEnumerable().Select(x => new Credential()
                        {
                            RoleID = x.RoleID,
                            UserGroupID = x.UserGroupID
                        });
            return data.Select(x => x.RoleID).ToList();

        }

        public List<Employee> ListAll(string position)
        {
            return db.Employees.Where(x => x.Status == true && x.Code.StartsWith(position) == true).ToList();
        }

        public List<Department> ListDepartment()
        {
            return db.Departments.ToList();
        }
    }
}
