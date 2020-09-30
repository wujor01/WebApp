using Model.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Common;

namespace WebApp.Areas.Admin.Controllers
{
    public class StatisticController : BaseController
    {
        // GET: Admin/Statistic
        [HasCredential(RoleID = "VIEW_STATISTIC")]
        public ActionResult TicketIndex(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new StatisticDao();
            dao.InsertStatisticTicketDate();
            dao.InsertStatisticTicketMonth();
            var model = dao.ListAllPaging(searchString, page, pageSize);

            ViewBag.SearchString = searchString;

            return View(model);
        }
    }
}