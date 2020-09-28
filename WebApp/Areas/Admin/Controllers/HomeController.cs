using Model.Dao;
using Model.EF;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Admin.Models;
using WebApp.Common;

namespace WebApp.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        WebAppDbContext db = new WebAppDbContext();

        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetDataDailyList()
        {
            var list = db.DailyLists.ToList();
            List<int> sove = new List<int>();
            List<decimal> tongtien = new List<decimal>();

            var ve = list.Select(x => x.Ticket_ID).Distinct();
            var ve1 = list.Select(x => x.Ticket.Name).Distinct();

            foreach (var item in ve)
            {
                sove.Add(list.Count(x => x.Ticket_ID == item));
                tongtien.Add(list.Where(x=>x.Ticket_ID == item).Sum(x => x.Total));
            }

            var so = sove;
            ViewBag.VE = ve1;
            ViewBag.SO = sove.ToList();
            ViewBag.TONGTIEN = tongtien.ToList();
            return View();
        }
    }
}