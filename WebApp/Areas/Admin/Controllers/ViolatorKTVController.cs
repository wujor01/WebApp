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
    public class ViolatorKTVController : BaseController
    {
        //[HasCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ViolatorKTVDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);

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
        public ActionResult Create(ViolatorKTV violatorKTV)
        {
            if (ModelState.IsValid)
            {
                var dao = new ViolatorKTVDao();

                //lấy id trong session đăng nhập của quản trị lưu vào phiên tạo mới user
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                violatorKTV.CreatedBy = session.UserName;
                violatorKTV.CreatedDate = DateTime.Now;

                long id = dao.Insert(violatorKTV);
                if (id > 0)
                {
                    SetAlert("Thêm chấm công nhân viên thành công", "success");
                    return RedirectToAction("Index", "ViolatorKTV");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm chấm nhân viên công không thành công");
                }
            }
            SetViewBag();
            SetAlert("Error", "error");
            return RedirectToAction("Index", "ViolatorKTV");
        }

        //[HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(int id)
        {
            var violatorKTV = new ViolatorKTVDao().ViewDetail(id);
            SetViewBag();
            return View(violatorKTV);
        }

        [HttpPost]
        //[HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(ViolatorKTV violatorKTV)
        {
            if (ModelState.IsValid)
            {
                var dao = new ViolatorKTVDao();
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                violatorKTV.ModifiedBy = session.UserName;

                long id = dao.Update(violatorKTV);
                if (id > 0)
                {
                    SetAlert("Sửa thông tin nhân viên thành công", "success");
                    return RedirectToAction("Index", "ViolatorKTV");
                }
                else
                {
                    SetAlert("Tài khoản hoặc mã nhân viên đã tồn tại!", "error");
                    return RedirectToAction("Index", "ViolatorKTV");
                }
            }
            SetViewBag();
            SetAlert("Sửa thông tin nhân viên thất bại", "error");
            return RedirectToAction("Index", "ViolatorKTV");
        }

        [HttpDelete]
        //[HasCredential(RoleID = "DELETE_USER")]
        public ActionResult Delete(int id)
        {
            new ViolatorKTVDao().Delete(id);

            return RedirectToAction("Index");
        }
    }
}