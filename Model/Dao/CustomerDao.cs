using Model.EF;
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
    }
}
