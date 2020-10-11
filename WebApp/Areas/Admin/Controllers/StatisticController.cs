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

        [HasCredential(RoleID = "VIEW_STATISTIC")]
        public ActionResult EmpIndex(string searchString, int page = 1, int pageSize = 10)
        {
            var statisticdao = new StatisticDao();
            statisticdao.InsertStatisticEmpDate();

            var dao = new StatisticDao();
            var model = dao.ListAllPagingKTV(searchString, page, pageSize);
            ViewBag.SearchString = searchString;

            return View(model);
        }

        [HasCredential(RoleID = "VIEW_STATISTIC")]
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

            var dep = db.OrderDetails.Select(x => x.Ticket.Department.Name).Distinct().ToList();
            foreach (var item in dep)
            {

                ////Tổng ngày hôm nay
                ticketTotalinDate.Add(db.OrderDetails.Where(x => x.Ticket.Department.Name == item && DbFunctions.TruncateTime(x.DailyList.CreatedDate.Value) == DateTime.Today).Sum(x => x.Amount));

                //Tổng tháninnày
                ticketPriceinMonth.Add(db.OrderDetails.Where(x => x.Ticket.Department.Name == item && x.DailyList.CreatedDate.Value.Month == DateTime.Today.Month && x.DailyList.CreatedDate.Value.Year == DateTime.Today.Year).Sum(x => x.Amount));
                           
                //Tổng năm iny
                ticketPriceinYear.Add(db.OrderDetails.Where(x => x.Ticket.Department.Name == item && x.DailyList.CreatedDate.Value.Month == DateTime.Today.Month && x.DailyList.CreatedDate.Value.Year == DateTime.Today.Year).Sum(x => x.Amount));

                //Tổng trước đến nay
                ticketCount.Add(db.OrderDetails.Where(x => x.Ticket.Department.Name == item ).Sum(x => x.Amount));

                ////TổngCount hôm nay
                ticketCountinDate.Add(db.OrderDetails.Where(x => x.Ticket.Department.Name == item && DbFunctions.TruncateTime(x.DailyList.CreatedDate.Value) == DateTime.Today).Count());

                //TổngCount tháng này
                ticketCountinMonth.Add(db.OrderDetails.Where(x => x.Ticket.Department.Name == item && x.DailyList.CreatedDate.Value.Month == DateTime.Today.Month && x.DailyList.CreatedDate.Value.Year == DateTime.Today.Year).Count());
                           
                //TổngCount năm
                ticketCountinYear.Add(db.OrderDetails.Where(x => x.Ticket.Department.Name == item && x.DailyList.CreatedDate.Value.Year == DateTime.Today.Year).Count());

                //TổngCount từ trước đến nay
                ticketCount.Add(db.OrderDetails.Where(x => x.Ticket.Department.Name == item ).Count());

            }

            var emp = db.Employees.Select(x => x.Code).Distinct().ToList();

            foreach (var item in emp)
            {
                empCount.Add(db.OrderDetails.Select(x => x.Employee_ID.Contains("'"+item+"'")).Count());
            }

            ViewBag.ticketCount = ticketCountinMonth;
            ViewBag.deparmentName = dep;
            ViewBag.ticketPrice = ticketPriceinMonth;

            return View();
        }
    }
}