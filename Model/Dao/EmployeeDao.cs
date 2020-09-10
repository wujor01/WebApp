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

                employee.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                //logging
                return false;
            }
        }

        public Employee ViewDetail(int id)
        {
            return db.Employee.Find(id);
        }

        public bool ChangeStatus(long id)
        {
            var employee = db.Employee.Find(id);
            employee.Status = !employee.Status;
            db.SaveChanges();
            return employee.Status;
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
            catch (Exception e)
            {
                return false;
            }

        }

        //Phân trang quản lý user và thêm mục tìm kiếm theo username và email
        public IEnumerable<Employee> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Employee> model = db.Employee;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Phone.Contains(searchString));
            }

            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
    }
}
