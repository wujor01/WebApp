using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Admin.Models;
using WebApp.Common;

namespace WebApp.Areas.Admin.Controllers
{
    public class ListvsTaxiController : BaseController
    {
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        // GET: Admin/List
        [HttpPost]
        public ActionResult Create(ListvsTaxiModel model)
        {
            var dao = new ListvsTaxiDao();
            //List<DailyList> list = db.DailyList.ToList();
            //ViewBag.dailylist = new SelectList(list, "");
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            DailyList dailylist = new DailyList();
            Taxi taxi = new Taxi();

            taxi.ID = model.Taxi_ID;
            taxi.Name = model.Taxi_Name;
            taxi.NumberOfCustomers = model.Taxi_NumberOfCustomers;
            taxi.Commission = model.Taxi_Commission;
            taxi.Price = model.Taxi_Price;
            taxi.Phone = model.Taxi_Phone;
            taxi.Description = model.Taxi_Description;

            dailylist.ID = model.ID;
            dailylist.Employee_Code = model.Employee_Code;
            dailylist.Room = model.Room;
            dailylist.TimeIn = model.TimeIn;
            dailylist.TimeOut = model.TimeOut;
            dailylist.Ticket = model.Ticket;
            dailylist.Tip = model.Tip;
            dailylist.Code = model.Code;
            dailylist.Voucher = model.Voucher;
            dailylist.Taxi_ID = model.Taxi_ID;
            dailylist.Total = (dailylist.Ticket - dailylist.Code - dailylist.Voucher + dailylist.Tip);
            dailylist.Description = model.Description;
            dailylist.CreatedDate = DateTime.Now;
            dailylist.CreatedBy = session.UserName;

            long id = dao.Insert(dailylist, taxi);
            //if (id > 0)
            //{
            //    SetAlert("Thêm bảng kê thành công", "success");
            //    return RedirectToAction("Index", "ListvsTaxi");
            //}
            //else
            //{
            //    ModelState.AddModelError("", "Thêm bảng kê không thành công");
            //}

            return View(model);
        }

        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ListvsTaxiDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);

            ViewBag.SearchString = searchString;

            return View(model);
        }

    }
}