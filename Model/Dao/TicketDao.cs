using Model.EF;
using PagedList;
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

        public long Insert(Ticket entity)
        {
            db.Tickets.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public Ticket ViewDetail(int id)
        {
            return db.Tickets.Find(id);
        }

        public long Update(Ticket entity, string username)
        {
            var ticket = db.Tickets.Find(entity.ID);
            ticket.Department_ID = entity.Department_ID;
            ticket.Name = entity.Name;
            ticket.Price = entity.Price;
            ticket.Header = entity.Header;
            ticket.Description = entity.Description;
            ticket.TimeTotal = entity.TimeTotal;
            ticket.Status = entity.Status;

            //Ngày chỉnh sửa = Now
            ticket.ModifiedBy = username;
            ticket.ModifiedDate = DateTime.Now;
            db.SaveChanges();
            return entity.ID;
        }

        public List<Ticket> ListAll(int departmentId)
        {
            if (departmentId == 0)
            {
                return db.Tickets.Where(x => x.Status == true).OrderBy(x => x.ID).ToList();
            }
            else
            {
                return db.Tickets.Where(x => x.Status == true || x.ID == 0).OrderBy(x => x.ID).Where(x => x.Department_ID == departmentId || x.Department_ID == null).ToList();
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var ticket = db.Tickets.Find(id);
                ticket.Status = false;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public IEnumerable<Ticket> ListAllPaging(string searchString, int page, int pageSize, int departmentId)
        {
            IQueryable<Ticket> model = db.Tickets;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(
                    x => x.Name.Contains(searchString) || x.Department.Name.ToString().Contains(searchString)
                );
            }
            if (departmentId == 0)
            {
                return model.Where(x => x.Status == true).OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);

            }
            else
            {
                return model.Where(x => x.Status == true).OrderByDescending(x => x.CreatedDate).Where(x => x.Department_ID == departmentId).ToPagedList(page, pageSize);
            }
        }
    }
}
