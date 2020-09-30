using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class DepartmentDao
    {
        WebAppDbContext db = null;
        public DepartmentDao()
        {
            db = new WebAppDbContext();
        }

        public List<Department> ListDepartment(int departmentId)
        {
            if (departmentId == 0)
            {
                return db.Departments.ToList();
            }
            else
            {
                return db.Departments.Where(x => x.ID == departmentId && x.Status == true).ToList();
            }
        }

        public long Insert(Department entity)
        {
            db.Departments.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public Department ViewDetail(int id)
        {
            return db.Departments.Find(id);
        }

        public long Update(Department entity)
        {
            var department = db.Departments.Find(entity.ID);
            department.Name = entity.Name;
            department.Address = entity.Address;
            department.Status = entity.Status;

            db.SaveChanges();
            return entity.ID;
        }

        public bool Delete(int id)
        {
            try
            {
                var department = db.Departments.Find(id);
                department.Status = false;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public IEnumerable<Department> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Department> model = db.Departments;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(
                    x => x.Name.Contains(searchString) || x.Address.ToString().Contains(searchString)
                );
            }
                return model.Where(x => x.Status==true).OrderBy(x => x.ID).ToPagedList(page, pageSize);
        }

    }
}
