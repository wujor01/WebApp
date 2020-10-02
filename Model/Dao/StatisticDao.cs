using Model.EF;
using Model.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using System.Data.Entity;

namespace Model.Dao
{
    public class StatisticDao
    {
        WebAppDbContext db = null;
        public StatisticDao()
        {
            db = new WebAppDbContext();
        }

        public int InsertStatisticTicketDate()
        {
            if (db.StatisticTickets.Count() != 0)
            {
                var all = from c in db.StatisticTickets select c;
                db.StatisticTickets.RemoveRange(all);
                db.SaveChanges();
            }

            var list = db.DailyLists.ToList();
            
            foreach (var item in list)
            {
                DateTime a = item.CreatedDate.Value.Date;

                var s = db.StatisticTickets.Where(x => DbFunctions.TruncateTime(x.Datetime) == a && x.Ticket_ID == item.Ticket_ID).ToList();
                if (s.Count == 0)
                {
                    StatisticTicket sta = new StatisticTicket();
                    sta.Datetime = (DateTime)item.CreatedDate;
                    sta.TicketinDate = 1;
                    sta.Employee_ID = "'" + item.Employee_ID + "'";
                    if (item.Taxi == null)
                    {
                        sta.TicketPriceinDate = item.Total - item.Tip;
                    }
                    else
                    {
                        sta.TicketPriceinDate = item.Total - item.Tip + item.Taxi.Price;
                    }    
                    sta.Ticket_ID = (int)item.Ticket_ID;
                    db.StatisticTickets.Add(sta);
                    db.SaveChanges();
                }
                else
                {
                    var sta = db.StatisticTickets.Where(x => DbFunctions.TruncateTime(x.Datetime) == a && x.Ticket_ID == item.Ticket_ID).Select(x => x.ID).ToList();
                    foreach (var itemid in sta)
                    {
                        var statistic = db.StatisticTickets.Find(itemid);
                        if (statistic.Employee_ID.Contains(item.ID.ToString()) != true)
                        {
                            statistic.TicketinDate = statistic.TicketinDate+ 1;
                            statistic.Employee_ID = statistic.Employee_ID + ",'" + item.Employee_ID + "'";
                            if (item.Taxi == null)
                            {
                                statistic.TicketPriceinDate = statistic.TicketPriceinDate + (item.Total - item.Tip);
                            }
                            else
                            {
                                statistic.TicketPriceinDate = statistic.TicketPriceinDate + (item.Total - item.Tip + item.Taxi.Price);
                            }
                            db.SaveChanges();
                        }
                    }
                }
            }
            return 1;
        }

        public IEnumerable<StatisticTicket> ListAllPaging(string searchString, int page, int pageSize)
        {

            IQueryable<StatisticTicket> model = db.StatisticTickets;

            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(
                    x => x.Ticket.Name.Contains(searchString) || x.Datetime.ToString().Contains(searchString)
                    || x.Ticket.Department.Name.Contains(searchString)
                );
            }
                return model.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }

    }
}
