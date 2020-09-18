using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ViolatorDao
    {
        WebAppDbContext db = null;
        public ViolatorDao()
        {
            db = new WebAppDbContext();
        }

        public long Insert(Violator entity)
        {
            db.Violators.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public long Update(Violator entity)
        {
            var violator = db.Violators.Find(entity.ID);
            violator.Code = entity.Code;
            violator.Type = entity.Type;
            violator.Loan = entity.Loan;
            violator.Description = entity.Description;

            //Ngày chỉnh sửa = Now
            violator.ModifiedDate = DateTime.Now;
            db.SaveChanges();
            return entity.ID;
        }

        public bool Delete(int id)
        {
            try
            {
                var violator = db.Violators.Find(id);
                db.Violators.Remove(violator);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public Violator ViewDetail(int id)
        {
            return db.Violators.Find(id);
        }

        public IEnumerable<Violator> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Violator> model = db.Violators;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(
                    x => x.Code.Contains(searchString) || x.Type.Contains(searchString)
                );
            }

            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }
    }
}
