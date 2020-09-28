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

            var query = db.DailyLists.Include("Ticket")
                .GroupBy(p => p.Ticket.Name)
                .Select(g => new { name = g.Key, count = g.SingleOrDefault().Total });
            
            return Json(query,JsonRequestBehavior.AllowGet);
        }
    }
}