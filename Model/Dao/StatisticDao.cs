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

            var list = db.OrderDetails.ToList();

            foreach (var item in list)
            {
                DateTime a = item.DailyList.CreatedDate.Value.Date;

                var s = db.StatisticTickets.Where(x => DbFunctions.TruncateTime(x.Datetime) == a && x.Ticket_ID == item.Ticket_ID).ToList();
                if (s.Count == 0)
                {
                    StatisticTicket sta = new StatisticTicket();
                    sta.Datetime = (DateTime)item.DailyList.CreatedDate;
                    sta.TicketinDate = 1;
                    sta.Employee_ID = "'" + item.Employee_ID + "'";
                    if (item.DailyList.Taxi == null)
                    {
                        sta.TicketPriceinDate = item.Amount;
                    }
                    else
                    {
                        sta.TicketPriceinDate = item.Amount + item.DailyList.Taxi.Price;
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
                            statistic.TicketinDate = statistic.TicketinDate + 1;
                            statistic.Employee_ID = statistic.Employee_ID + ",'" + item.Employee_ID + "'";

                                statistic.TicketPriceinDate = statistic.TicketPriceinDate + item.Amount;

                            db.SaveChanges();
                        }
                    }
                }
            }
            return 1;
        }

        public int InsertStatisticEmpDate()
        {
            if (db.StatisticEmployees.Count() != 0)
            {
                var all = from c in db.StatisticEmployees select c;
                db.StatisticEmployees.RemoveRange(all);
                db.SaveChanges();
            }

            var list = db.DailyEmployees.ToList();

            foreach (var item in list)
            {
                DateTime a = item.Date;

                var s = db.StatisticEmployees.Where(x => DbFunctions.TruncateTime(x.Datetime) == a && x.Employee_ID == item.Employee_ID).ToList();
                if (s.Count == 0)
                {
                    StatisticEmployee sta = new StatisticEmployee();
                    sta.Datetime = item.Date;
                    sta.Employee_ID = item.Employee_ID;
                    sta.CountinDate = 1;
                    sta.TipinDate = item.Tip;
                    sta.TourinDate = item.Tour;
                    sta.CleaninDate = item.Employee.Department.Clean;
                    db.StatisticEmployees.Add(sta);
                    db.SaveChanges();
                }
                else
                {
                    var sta = db.StatisticEmployees.Where(x => DbFunctions.TruncateTime(x.Datetime) == a && x.Employee_ID == item.Employee_ID).Select(x => x.ID).ToList();
                    foreach (var itemid in sta)
                    {
                        var statistic = db.StatisticEmployees.Find(itemid);
                        statistic.CountinDate = statistic.CountinDate + 1;
                        statistic.TipinDate = statistic.TipinDate + item.Tip;
                        statistic.TourinDate = statistic.TourinDate + item.Tour;
                        db.SaveChanges();
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

        public IEnumerable<StatisticEmployee> ListAllPagingKTV(string searchString, int page, int pageSize)
        {

            IQueryable<StatisticEmployee> model = db.StatisticEmployees;

            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(
                    x => x.Employee.Code.Contains(searchString) || x.Datetime.ToString().Contains(searchString)
                    || x.Employee.Department.Name.Contains(searchString)
                );
            }
            return model.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }
    }
}
