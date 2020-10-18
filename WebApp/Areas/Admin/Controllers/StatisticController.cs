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

            var dep = db.OrderDetails.Select(x => x.Ticket.Department.Name).Distinct().ToList();
            foreach (var item in dep)
            {

                ////Tổng ngày hôm nay
                ticketTotalinDate.Add(db.OrderDetails.Where(x => x.Ticket.Department.Name == item && x.DailyList.CreatedDate.Day == DateTime.Today.Day && x.DailyList.CreatedDate.Month == DateTime.Today.Month && x.DailyList.CreatedDate.Year == DateTime.Today.Year).Sum(x => x.Amount));

                //Tổng tháninnày
                ticketPriceinMonth.Add(db.OrderDetails.Where(x => x.Ticket.Department.Name == item && x.DailyList.CreatedDate.Month == DateTime.Today.Month && x.DailyList.CreatedDate.Year == DateTime.Today.Year).Sum(x => x.Amount));
                           
                //Tổng năm iny
                ticketPriceinYear.Add(db.OrderDetails.Where(x => x.Ticket.Department.Name == item && x.DailyList.CreatedDate.Month == DateTime.Today.Month && x.DailyList.CreatedDate.Year == DateTime.Today.Year).Sum(x => x.Amount));

                //Tổng trước đến nay
                ticketCount.Add(db.OrderDetails.Where(x => x.Ticket.Department.Name == item ).Sum(x => x.Amount));

                ////TổngCount hôm nay
                ticketCountinDate.Add(db.OrderDetails.Where(x => x.Ticket.Department.Name == item && DbFunctions.TruncateTime(x.DailyList.CreatedDate) == DateTime.Today).Count());

                //TổngCount tháng này
                ticketCountinMonth.Add(db.OrderDetails.Where(x => x.Ticket.Department.Name == item && x.DailyList.CreatedDate.Month == DateTime.Today.Month && x.DailyList.CreatedDate.Year == DateTime.Today.Year).Count());
                           
                //TổngCount năm
                ticketCountinYear.Add(db.OrderDetails.Where(x => x.Ticket.Department.Name == item && x.DailyList.CreatedDate.Year == DateTime.Today.Year).Count());

                //TổngCount từ trước đến nay
                ticketCount.Add(db.OrderDetails.Where(x => x.Ticket.Department.Name == item ).Count());

            }

            ViewBag.ticketCount = ticketCountinMonth;
            ViewBag.deparmentName = dep;
            ViewBag.ticketPrice = ticketPriceinMonth;

            //Emp
            List<decimal> empPrice = new List<decimal>();
            List<decimal> empPriceinDate = new List<decimal>();
            List<decimal> empTotalinDate = new List<decimal>();
            List<decimal> empPriceinMonth = new List<decimal>();
            List<decimal> empPriceinYear = new List<decimal>();

            List<decimal> empCount = new List<decimal>();
            List<decimal> empCountinDate = new List<decimal>();
            List<decimal> empCountinMonth = new List<decimal>();
            List<decimal> empCountinYear = new List<decimal>();

            var emp = db.DailyEmployees.Where(x => x.Employee.Department_ID == 4).Select(x => x.Employee.Code).Distinct().ToList();
            foreach (var item in emp)
            {
                empPriceinMonth.Add(db.DailyEmployees.Where(x => x.Employee.Code == item && x.Date.Month == DateTime.Today.Month && x.Date.Year == DateTime.Today.Year).Sum(x => x.Tip));

                empCountinMonth.Add(db.DailyEmployees.Where(x => x.Employee.Code == item && x.Date.Month == DateTime.Today.Month && x.Date.Year == DateTime.Today.Year).Count());

            }

            ViewBag.empCount = empCountinMonth;
            ViewBag.empName = emp;
            ViewBag.empPrice = empPriceinMonth;

            return View();
        }

        [HasCredential(RoleID = "VIEW_STATISTIC")]
        public ActionResult GetDataEmp()
        {

            db = new WebAppDbContext();
            //
            List<decimal> empPrice = new List<decimal>();
            List<decimal> empPriceinDate = new List<decimal>();
            List<decimal> empTotalinDate = new List<decimal>();
            List<decimal> empPriceinMonth = new List<decimal>();
            List<decimal> empPriceinYear = new List<decimal>();

            List<decimal> empCount = new List<decimal>();
            List<decimal> empCountinDate = new List<decimal>();
            List<decimal> empCountinMonth = new List<decimal>();
            List<decimal> empCountinYear = new List<decimal>();

            var dep = db.DailyEmployees.Where(x=>x.Employee.Department_ID == 4).Select(x => x.Employee.Code).Distinct().ToList();
            foreach (var item in dep)
            {
                empPriceinMonth.Add(db.DailyEmployees.Where(x => x.Employee.Code == item && x.Date.Month == DateTime.Today.Month && x.Date.Year == DateTime.Today.Year).Sum(x => x.Tip));

                empCountinMonth.Add(db.DailyEmployees.Where(x => x.Employee.Code == item && x.Date.Month == DateTime.Today.Month && x.Date.Year == DateTime.Today.Year).Count());

            }

            ViewBag.ticketCount = empCountinMonth;
            ViewBag.deparmentName = dep;
            ViewBag.ticketPrice = empPriceinMonth;

            return View();
        }
    }
}