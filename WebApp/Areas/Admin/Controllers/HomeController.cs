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
            //dailylist
            var list = db.DailyLists.ToList();
            List<int> sove = new List<int>();
            List<decimal> tongtien = new List<decimal>();

            var ve = list.Select(x => x.Ticket_ID).Distinct();
            var date = list.Select(x => x.CreatedDate.Value.Date).Distinct();

            //foreach (var item in ve)
            //{
            //    sove.Add(list.Count(x => x.Ticket_ID == item));
            //    tongtien.Add(list.Where(x=>x.Ticket_ID == item).Sum(x => x.Total));
            //}
            foreach (var item in date)
            {
                sove.Add(list.Count(x => x.CreatedDate.Value.Date == item));
                tongtien.Add(list.Where(x => x.CreatedDate.Value.Date == item).Sum(x => x.Total));
            }

            var so = sove;
            ViewBag.SO = sove.ToList();
            ViewBag.TONGTIEN = tongtien.ToList();

            //

            var data = from DailyList in db.DailyLists
                   group new { DailyList.Ticket, DailyList.Room.Department, DailyList } by new
                   {
                       DailyList.Ticket.Name,
                       Column1 = DailyList.Room.Department.Name
                   } into g
                   select new
                   {
                       chinnhanh = g.Key.Column1,
                       loaive = g.Key.Name,
                       soluong = g.Count(p => p.DailyList.Ticket.ID != 0),
                       giave = (decimal?)g.Sum(p => p.DailyList.Ticket.Price),
                       tong = (decimal?)g.Sum(p => p.DailyList.Total)
                   };

            ViewBag.chinhanh = data.Select(x => x.chinnhanh).ToList();
            ViewBag.loaive = data.Select(x => x.chinnhanh).ToList();
            ViewBag.soluong = data.Select(x => x.chinnhanh).ToList();
            ViewBag.giave = data.Select(x => x.chinnhanh).ToList();
            ViewBag.tong = data.Select(x => x.chinnhanh).ToList();


            return View();
        }
    }
}