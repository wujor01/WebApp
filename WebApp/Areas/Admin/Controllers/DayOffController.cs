using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Common;

namespace WebApp.Areas.Admin.Controllers
{
    public class DayOffController : BaseController
    {
        public void SetViewBag(long? selectedId = null)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];

            var dao = new EmployeeDao();
            ViewBag.Employee_ID = new SelectList(dao.ListAll("",session.DepartmentID), "ID", "Code", selectedId);
        }



        [HasCredential(RoleID = "VIEW_DAYOFF")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];

            var dao = new DayOffDao();
            var model = dao.ListAllPaging(searchString, page, pageSize, session.DepartmentID);

            ViewBag.SearchString = searchString;

            return View(model);
        }

        [HttpGet]
        [HasCredential(RoleID = "ADD_DAYOFF")]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_DAYOFF")]
        public ActionResult Create(DayOff dayOff)
        {
            if (ModelState.IsValid)
            {
                var dao = new DayOffDao();
                //lấy id trong session đăng nhập của quản trị lưu vào phiên tạo mới user
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                dayOff.CreatedBy = session.UserName;
                dayOff.CreatedDate = DateTime.Now;

                long id = dao.Insert(dayOff);
                if (id > 0)
                {
                    SetAlert("Thêm xin nghỉ thành công", "success");
                    return RedirectToAction("Index", "DayOff");
                }
                else
                {
                    ModelState.AddModelError("", "Nhân viên không tồn tại");
                }
            }
            SetViewBag();
            SetAlert("Error", "error");
            return RedirectToAction("Index", "DayOff");
        }
        //hiển thị dropdown chọn mã nhân viên từ id

        [HasCredential(RoleID = "EDIT_DAYOFF")]
        public ActionResult Edit(int id)
        {
            var dayOff = new DayOffDao().ViewDetail(id);
            SetViewBag();
            return View(dayOff);
        }

        [HttpPost]
        [HasCredential(RoleID = "EDIT_DAYOFF")]
        public ActionResult Edit(DayOff dayOff)
        {
            if (ModelState.IsValid)
            {
                var dao = new DayOffDao();
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];

                long id = dao.Update(dayOff, session.UserName);
                if (id > 0)
                {
                    SetAlert("Sửa thông tin nhân viên thành công", "success");
                    return RedirectToAction("Index", "DayOff");
                }
                else
                {
                    SetAlert("Tài khoản hoặc mã nhân viên đã tồn tại!", "error");
                    return RedirectToAction("Index", "DayOff");
                }
            }
            SetViewBag();
            SetAlert("Sửa thông tin nhân viên thất bại", "error");
            return RedirectToAction("Index", "DayOff");
        }

        [HttpDelete]
        [HasCredential(RoleID = "DELETE_DAYOFF")]
        public ActionResult Delete(int id)
        {
            new DayOffDao().Delete(id);

            return RedirectToAction("Index");
        }

        
    }
}