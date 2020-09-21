using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class DeparmentDao
    {
        WebAppDbContext db = null;
        public DeparmentDao()
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
                return db.Departments.Where(x => x.ID == departmentId).ToList();
            }
        }
    }
}
