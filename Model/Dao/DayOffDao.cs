using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class DayOffDao
    {
        WebAppDbContext db = null;
        public DayOffDao()
        {
            db = new WebAppDbContext();
        }


        public long Insert(DayOff entity)
        {       
                entity.Date = DateTime.Now;
                db.DayOffs.Add(entity);
                db.SaveChanges(); 
            return entity.ID;
        }

        public long Update(DayOff entity, string username)
        {

            var dayOff = db.DayOffs.Find(entity.ID);
            dayOff.Employee_ID = entity.Employee_ID;
            dayOff.Description = entity.Description;
            dayOff.Status = entity.Status;
            
            if (dayOff.Status == true)
            {
                var emp = db.Employees.Find(entity.Employee_ID);

                emp.NumberOfDayOff = emp.NumberOfDayOff + 1;
            }        

            if (entity.Date != null)
            {
                dayOff.Date = entity.Date;

            }
            //Ngày chỉnh sửa = Now
            dayOff.ModifiedBy = username;
            dayOff.ModifiedDate = DateTime.Now;
            db.SaveChanges();
            return entity.ID;
        }

        public bool Delete(int id)
        {
            try
            {
                var dayOff = db.DayOffs.Find(id);
                db.DayOffs.Remove(dayOff);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public DayOff ViewDetail(int id)
        {
            return db.DayOffs.Find(id);
        }

        public IEnumerable<DayOff> ListAllPaging(string searchString, int page, int pageSize, int departmentId)
        {
            IQueryable<DayOff> model = db.DayOffs;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(
                    x => x.Employee.Code.Contains(searchString) || x.Date.ToString().Contains(searchString)
                );
            }
            if (departmentId == 0)
            {
                return model.OrderByDescending(x => x.Date).ToPagedList(page, pageSize);
            }
            else
            {
                return model.OrderByDescending(x => x.Date).Where(x=>x.Employee.Department_ID == departmentId).ToPagedList(page, pageSize);
            }
        }
    }
}
