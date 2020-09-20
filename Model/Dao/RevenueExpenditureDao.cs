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

        public long Update(RevenueExpenditure entity)
        {
            var revenueExpenditure = db.RevenueExpenditures.Find(entity.ID);
            revenueExpenditure.Contents = entity.Contents;
            revenueExpenditure.Type_ID = entity.Type_ID;
            revenueExpenditure.Description = entity.Description;
            revenueExpenditure.Money = revenueExpenditure.Money;

            //Ngày chỉnh sửa = Now
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

        public IEnumerable<RevenueExpenditure> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<RevenueExpenditure> model = db.RevenueExpenditures;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(
                    x => x.Contents.Contains(searchString) || x.Money.ToString().Contains(searchString)
                );
            }

            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public List<ReExType> ListAll()
        {
            return db.ReExTypes.ToList();
        }
    }
}
