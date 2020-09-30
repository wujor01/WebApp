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
            var list = db.DailyLists.ToList();
            
            foreach (var item in list)
            {
                DateTime a = item.CreatedDate.Value.Date;

                var s = db.StatisticTickets.Where(x => DbFunctions.TruncateTime(x.Datetime) == a && x.Ticket_ID == item.Ticket_ID).ToList();
                if (s.Count == 0)
                {
                    StatisticTicket sta = new StatisticTicket();
                    sta.Datetime = item.CreatedDate;
                    sta.TicketinDate = 1;
                    sta.DailyList_ID = item.ID.ToString();
                    if (item.Taxi == null)
                    {
                        sta.TicketPriceinDate = item.Total - item.Tip;
                    }
                    else
                    {
                        sta.TicketPriceinDate = item.Total - item.Tip + item.Taxi.Price;
                    }    
                    sta.Ticket_ID = item.Ticket_ID;
                    db.StatisticTickets.Add(sta);
                    db.SaveChanges();
                }
                else
                {
                    var sta = db.StatisticTickets.Where(x => DbFunctions.TruncateTime(x.Datetime) == a && x.Ticket_ID == item.Ticket_ID).Select(x => x.ID).ToList();
                    foreach (var itemid in sta)
                    {
                        var statistic = db.StatisticTickets.Find(itemid);
                        if (statistic.DailyList_ID.Contains(item.ID.ToString()) != true)
                        {
                            statistic.TicketinDate = statistic.TicketinDate + 1;
                            statistic.DailyList_ID = statistic.DailyList_ID + "," + item.ID.ToString();
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
                return model.OrderBy(x => x.Datetime).ToPagedList(page, pageSize);
        }

    }
}
