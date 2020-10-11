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

        public void SetViewDepartment(int? selectedId = null)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];

            var dao = new DepartmentDao();
            ViewBag.Department_ID = new SelectList(dao.ListDepartment(session.DepartmentID), "ID", "Name", selectedId);
        }
        [HasCredential(RoleID = "VIEW_LIST")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 50)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            var dao = new RoomDao();
            var model = dao.ListAllPaging(searchString, page, pageSize, session.DepartmentID);

            ViewBag.SearchString = searchString;

            return View(model);
        }

        public ActionResult GetDataDailyList()
        {
            return View();
        }
    }
}