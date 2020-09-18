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

        [HttpGet]
        //[HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create()
        {
            return View();
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
                    return RedirectToAction("Index", "ListvsTaxi");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm bảng kê không thành công");
                }
            //}
            SetAlert("Error!", "error");
            return RedirectToAction("Index", "ListvsTaxi");
        }

        //[HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(int id)
        {
            var dailyList = new DailyListDao().ViewDetail(id);
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
                    SetAlert("Sửa thông tin nhân viên thành công", "success");
                    return RedirectToAction("Index", "DailyList");
                }
                else
                {
                    SetAlert("Tài khoản hoặc mã nhân viên đã tồn tại!", "error");
                    return RedirectToAction("Index", "DailyList");
                }
            }
            SetAlert("Sửa thông tin nhân viên thất bại", "error");
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
}