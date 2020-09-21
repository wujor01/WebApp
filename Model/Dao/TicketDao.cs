using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class TicketDao
    {
        WebAppDbContext db = null;
        public TicketDao()
        {
            db = new WebAppDbContext();
        }

        public List<Ticket> ListAll(int departmentId)
        {
            if (departmentId == 0)
            {
                return db.Tickets.OrderBy(x => x.Price).ToList();
            }
            else
            {
                return db.Tickets.OrderBy(x => x.Price).Where(x => x.Department_ID == departmentId).ToList();
            }
        }
    }
}
