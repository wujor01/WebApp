using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Model.Dao
{
    public class DailyListDao
    {
        WebAppDbContext db = null;
        public DailyListDao()
        {
            db = new WebAppDbContext();
        }

        public long CodeInsert(Voucher voucher)
        {           
            var rand = new Random();
            string r = rand.Next(100000, 1000000).ToString();
            voucher.Status = true;
            voucher.DiscountPercent = 20;
            voucher.ExpirationDate = DateTime.Now.AddDays(14);
            do
            {              
                voucher.Code = string.Concat("MV ", r);
                db.Vouchers.Add(voucher);
            } while (db.Vouchers.Where(x => x.Code == voucher.Code).ToList().Count > 0);

            db.SaveChanges();
            return voucher.ID;
        }

        public IEnumerable<Voucher> ListAllPagingCode(string searchString, int page, int pageSize)
        {
            IQueryable<Voucher> model = db.Vouchers;

            foreach (var item in model)
            {
                if (item.ExpirationDate < DateTime.Now)
                {
                    CodeExpirated(item.ID);
                }
                    
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(
                    x => x.Code.Contains(searchString) || x.ExpirationDate.ToString().Contains(searchString)
                );
            }
                return model.Where(x=> DateTime.Today < x.ExpirationDate && x.ID > 99).OrderByDescending(x => x.ExpirationDate).ToPagedList(page, pageSize);
        }

        public bool CodeExpirated(long id)
        {
            try
            {
                var voucher = db.Vouchers.Find(id);
                voucher.Expirated = true;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public Voucher ViewCodeDetail(int ID)
        {
            return db.Vouchers.Find(ID);
        }

        public IEnumerable<DailyList> ListAll()
        {
            return db.DailyLists.ToList();
        }
    }
}
