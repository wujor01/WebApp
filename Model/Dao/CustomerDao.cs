using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class CustomerDao
    {
        WebAppDbContext db = null;
        public CustomerDao()
        {
            db = new WebAppDbContext();
        }

        public List<Customer> ListAll()
        {
            return db.Customers.OrderByDescending(x => x.ID).ToList();
        }

        public long Insert(Customer entity)
        {
            db.Customers.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public Customer ViewDetail(int id)
        {
            return db.Customers.Find(id);
        }

        public long Update(Customer entity, string username)
        {
            var customer = db.Customers.Find(entity.ID);
            customer.Name = entity.Name;
            customer.Phone = entity.Phone;
            customer.CardID = entity.CardID;
            customer.Description = entity.Description;

            //Ngày chỉnh sửa = Now
            customer.ModifiedBy = username;
            customer.ModifiedDate = DateTime.Now;
            db.SaveChanges();
            return entity.ID;
        }

        public bool Delete(int id)
        {
            try
            {
                var customer = db.Customers.Find(id);
                db.Customers.Remove(customer);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public IEnumerable<Customer> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Customer> model = db.Customers;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(
                    x => x.Name.Contains(searchString) || x.Phone.ToString().Contains(searchString)
                );
            }
                return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);          
        }
    }
}
