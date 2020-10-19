using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ViolatorKTVDao
    {
        WebAppDbContext db = null;
        public ViolatorKTVDao()
        {
            db = new WebAppDbContext();
        }

        public long Insert(ViolatorKTV entity)
        {
            int a = 0;
            string[] arrEmpId = string.Join(",", entity.SelectedIDArray).Replace(" ", "").Split(',');
            for (int i = 0; i < arrEmpId.Length; i++)
            {
                entity.Employee_ID = Int32.Parse(arrEmpId[i]);
                entity.TimeOut = entity.TimeIn.AddHours(12);
                db.ViolatorKTVs.Add(entity);
                db.SaveChanges();
                a = 1;
            }
            return a;
        }

        public long Update(ViolatorKTV entity,string username)
        {
            var violatorKTV = db.ViolatorKTVs.Find(entity.ID);
            violatorKTV.Employee_ID = entity.Employee_ID;
            violatorKTV.Tour = entity.Tour;
            violatorKTV.Fruit = entity.Fruit;
            violatorKTV.Description = entity.Description;

            //Ngày chỉnh sửa = Now
            violatorKTV.ModifiedBy = username;
            violatorKTV.ModifiedDate = DateTime.Now;
            db.SaveChanges();
            return entity.ID;
        }

        public bool Delete(int id)
        {
            try
            {
                var violatorKTV = db.ViolatorKTVs.Find(id);
                db.ViolatorKTVs.Remove(violatorKTV);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public ViolatorKTV ViewDetail(int id)
        {
            return db.ViolatorKTVs.Find(id);
        }

        public IEnumerable<ViolatorKTV> ListAllPaging(string searchString, int page, int pageSize, int departmentId)
        {
            IQueryable<ViolatorKTV> model = db.ViolatorKTVs;
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
                return model.OrderByDescending(x => x.CreatedDate).Where(x=>x.Employee.Department_ID == departmentId).ToPagedList(page, pageSize);
            }
        }
    }
}
