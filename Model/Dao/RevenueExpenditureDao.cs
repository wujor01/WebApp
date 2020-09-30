using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class RevenueExpenditureDao
    {
        WebAppDbContext db = null;
        public RevenueExpenditureDao()
        {
            db = new WebAppDbContext();
        }

        public long Insert(RevenueExpenditure entity)
        {
            db.RevenueExpenditures.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public long Update(RevenueExpenditure entity,string username)
        {
            var revenueExpenditure = db.RevenueExpenditures.Find(entity.ID);
            revenueExpenditure.Contents = entity.Contents;
            revenueExpenditure.Type_ID = entity.Type_ID;
            revenueExpenditure.Description = entity.Description;
            revenueExpenditure.Money = entity.Money;

            //Ngày chỉnh sửa = Now
            revenueExpenditure.ModifiedBy = username;
            revenueExpenditure.ModifiedDate = DateTime.Now;
            db.SaveChanges();
            return entity.ID;
        }

        public bool Delete(int id)
        {
            try
            {
                var revenueExpenditure = db.RevenueExpenditures.Find(id);
                db.RevenueExpenditures.Remove(revenueExpenditure);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public RevenueExpenditure ViewDetail(int id)
        {
            return db.RevenueExpenditures.Find(id);
        }

        public IEnumerable<RevenueExpenditure> ListAllPaging(string searchString, int page, int pageSize, int departmentId)
        {
            IQueryable<RevenueExpenditure> model = db.RevenueExpenditures;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(
                    x => x.Contents.Contains(searchString) || x.ID.ToString().Contains(searchString)
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

        public List<ReExType> ListAll()
        {
            return db.ReExTypes.ToList();
        }
    }
}
