using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Admin.Models;
using WebApp.Common;

namespace WebApp.Areas.Admin.Controllers
{
    public class StatisticController : BaseController
    {
        // GET: Admin/Statistic
        WebAppDbContext db = null;

        [HasCredential(RoleID = "VIEW_STATISTIC")]
        public ActionResult TicketIndex(string searchString, int page = 1, int pageSize = 10)
        {
            var statisticdao = new StatisticDao();
            statisticdao.InsertStatisticTicketDate();

            var dao = new StatisticDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            ViewBag.SearchString = searchString;

            return View(model);
        }
        
        public ActionResult GetDataDailyList()
        {

            db = new WebAppDbContext();
            //
            List<decimal> ticketPrice = new List<decimal>();
            List<decimal> ticketPriceinDate = new List<decimal>();
            List<decimal> ticketTotalinDate = new List<decimal>();
            List<decimal> ticketPriceinMonth = new List<decimal>();
            List<decimal> ticketPriceinYear = new List<decimal>();

            List<decimal> ticketCount = new List<decimal>();
            List<decimal> ticketCountinDate = new List<decimal>();
            List<decimal> ticketCountinMonth = new List<decimal>();
            List<decimal> ticketCountinYear = new List<decimal>();

            List<int> empCount = new List<int>();

            var dep = db.DailyLists.Select(x => x.Ticket.Department.Name).Distinct().ToList();
            foreach (var item in dep)
            {
                //Tổng ngày hôm nay
                ticketPriceinDate.Add(db.DailyLists.Where(x=>x.Ticket.Department.Name == item && DbFunctions.TruncateTime(x.CreatedDate.Value) == DateTime.Today).Sum(x => x.PricewithVoucher));
                
                //Tổng ngày hôm nay
                ticketTotalinDate.Add(db.DailyLists.Where(x => x.Ticket.Department.Name == item && DbFunctions.TruncateTime(x.CreatedDate.Value) == DateTime.Today).Sum(x => x.Total - x.Tip));

                //Tổng tháninnày
                ticketPriceinMonth.Add(db.DailyLists.Where(x => x.Ticket.Department.Name == item && x.CreatedDate.Value.Month == DateTime.Today.Month && x.CreatedDate.Value.Year == DateTime.Today.Year).Sum(x => x.PricewithVoucher));
                           
                //Tổng năm iny
                ticketPriceinYear.Add(db.DailyLists.Where(x => x.Ticket.Department.Name == item && x.CreatedDate.Value.Month == DateTime.Today.Month && x.CreatedDate.Value.Year == DateTime.Today.Year).Sum(x => x.PricewithVoucher));

                //Tổng trước đến nay
                ticketCount.Add(db.DailyLists.Where(x => x.Ticket.Department.Name == item ).Sum(x => x.PricewithVoucher));

                //TổngCount hôm nay
                ticketCountinDate.Add(db.DailyLists.Where(x => x.Ticket.Department.Name == item && DbFunctions.TruncateTime(x.CreatedDate.Value) == DateTime.Today).Count());
                           
                //TổngCount tháng này
                ticketCountinMonth.Add(db.DailyLists.Where(x => x.Ticket.Department.Name == item && x.CreatedDate.Value.Month == DateTime.Today.Month && x.CreatedDate.Value.Year == DateTime.Today.Year).Count());
                           
                //TổngCount năm
                ticketCountinYear.Add(db.DailyLists.Where(x => x.Ticket.Department.Name == item && x.CreatedDate.Value.Year == DateTime.Today.Year).Count());

                //TổngCount từ trước đến nay
                ticketCount.Add(db.DailyLists.Where(x => x.Ticket.Department.Name == item ).Count());

            }

            var emp = db.Employees.Select(x => x.Code).Distinct().ToList();

            foreach (var item in emp)
            {
                empCount.Add(db.DailyLists.Select(x => x.Employee_ID.Contains("'"+item+"'")).Count());
            }

            ViewBag.empCount = empCount;
            ViewBag.deparmentName = dep;
            ViewBag.ticketPrice = ticketPriceinDate;

        }
    }
}