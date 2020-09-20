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
            db.ViolatorKTVs.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public long Update(ViolatorKTV entity)
        {
            var violatorKTV = db.ViolatorKTVs.Find(entity.ID);
            violatorKTV.Employee_ID = entity.Employee_ID;
            violatorKTV.Tour = entity.Tour;
            violatorKTV.Fruit = entity.Fruit;
            violatorKTV.Description = entity.Description;

            //Ngày chỉnh sửa = Now
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

        public IEnumerable<ViolatorKTV> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<ViolatorKTV> model = db.ViolatorKTVs;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(
                    x => x.Employee.Code.Contains(searchString) || x.ID.ToString().Contains(searchString)
                );
            }

            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
    }
}
