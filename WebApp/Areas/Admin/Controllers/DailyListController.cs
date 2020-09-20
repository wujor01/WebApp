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
    public class DailyListController : BaseController
    {
        //[HasCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new DailyListDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);

            ViewBag.SearchString = searchString;

            return View(model);
        }

        public ActionResult TaxiIndex(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new DailyListDao();
            var model = dao.ListAllPagingTaxi(searchString, page, pageSize);

            ViewBag.SearchString = searchString;

            return View(model);
        }

        [HttpGet]
        //[HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        public void SetViewBag(long? selectedId = null)
        {
            var dao = new EmployeeDao();
            ViewBag.Employee_ID = new SelectList(dao.ListAll(), "ID", "Code", selectedId);
        }

        [HttpPost]
        //[HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create(DailyList list)
        {
            //if (ModelState.IsValid)
            //{
                var dao = new DailyListDao();

            //lấy id trong session đăng nhập của quản trị lưu vào phiên tạo mới user
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                list.CreatedBy = session.UserName;
                list.CreatedDate = DateTime.Now;

            long id = dao.Insert(list);
                if (id > 0)
                {
                    SetAlert("Thêm bảng kê thành công", "success");
                    return RedirectToAction("Index", "DailyList");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm bảng kê không thành công");
                }
            //}
            SetViewBag();
            SetAlert("Error!", "error");
            return RedirectToAction("Index", "DailyList");
        }

        //[HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(int id)
        {
            var dailyList = new DailyListDao().ViewDetail(id);
            SetViewBag();
            return View(dailyList);
        }

        [HttpPost]
        //[HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(DailyList dailyList)
        {
            if (ModelState.IsValid)
            {
                var dao = new DailyListDao();
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                dailyList.ModifiedBy = session.UserName;

                long id = dao.Update(dailyList);
                if (id > 0)
                {
                    SetAlert("Sửa thông tin bảng kê thành công", "success");
                    return RedirectToAction("Index", "DailyList");
                }
                else
                {
                    SetAlert("Error!", "error");
                    return RedirectToAction("Index", "DailyList");
                }
            }
            SetViewBag();
            SetAlert("Error", "error");
            return RedirectToAction("Index", "DailyList");
        }

        [HttpDelete]
        //[HasCredential(RoleID = "DELETE_USER")]
        public ActionResult Delete(int id)
        {
            new DailyListDao().Delete(id);

            return RedirectToAction("Index");
        }
    }

}