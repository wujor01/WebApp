using Model.Dao;
using Model.EF;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model;
using WebApp.Common;
using System.Text;
using System.Data.Entity;

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
        public ActionResult Index(int page = 1, int pageSize = 16)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            var dao = new RoomDao();
            string searchString = null;
            var model = dao.ListAllPaging(searchString, page, pageSize, session.DepartmentID);

            ViewBag.SearchString = searchString;

            List<TimerModel> list = new List<TimerModel>();

            WebAppDbContext db = new WebAppDbContext();

            var rooms = model;

            foreach (var item in rooms)
            {
                DateTime now = DateTime.Now;

                foreach (var temp in item.OrderDetails.Where(x=>x.DailyList.Status == true).OrderByDescending(x=>x.ID).Take(1))
                {
                    if (temp.DailyEmployees.Sum(x=>x.Tip) < 1)
                    {
                        list.Add(new TimerModel
                        {
                            Name = item.ID,
                            ReleaseDateTime = temp.TimeOut.Value.Subtract(new DateTime(1970, 1, 1).AddHours(7)).TotalMilliseconds,
                            Message = string.Concat(item.Name, " hết giờ")
                        });
                    }
                }
            }

            ViewBag.TimerList = list;
            return View(model);
        }        
    }
}