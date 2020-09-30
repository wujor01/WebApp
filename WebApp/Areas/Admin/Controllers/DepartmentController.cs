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
    public class DepartmentController : BaseController
    {
        [HasCredential(RoleID = "VIEW_DEPARTMENT")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new DepartmentDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);

            ViewBag.SearchString = searchString;

            return View(model);
        }

        [HttpGet]
        [HasCredential(RoleID = "ADD_DEPARTMENT")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_DEPARTMENT")]
        public ActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                var dao = new DepartmentDao();

                long id = dao.Insert(department);
                if (id > 0)
                {
                    SetAlert("Thêm chấm công nhân viên thành công", "success");
                    return RedirectToAction("Index", "Department");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm chấm nhân viên công không thành công");
                }
            }
            SetAlert("Error", "error");
            return RedirectToAction("Index", "Department");
        }

        [HasCredential(RoleID = "EDIT_DEPARTMENT")]
        public ActionResult Edit(int id)
        {
            var department = new DepartmentDao().ViewDetail(id);
            return View(department);
        }

        [HttpPost]
        [HasCredential(RoleID = "EDIT_DEPARTMENT")]
        public ActionResult Edit(Department department)
        {
            if (ModelState.IsValid)
            {
                var dao = new DepartmentDao();

                long id = dao.Update(department);
                if (id > 0)
                {
                    SetAlert("Sửa thông tin nhân viên thành công", "success");
                    return RedirectToAction("Index", "Department");
                }
                else
                {
                    SetAlert("Tài khoản hoặc mã nhân viên đã tồn tại!", "error");
                    return RedirectToAction("Index", "Department");
                }
            }
            SetAlert("Sửa thông tin nhân viên thất bại", "error");
            return RedirectToAction("Index", "Department");
        }

        [HttpDelete]
        [HasCredential(RoleID = "DELETE_DEPARTMENT")]
        public ActionResult Delete(int id)
        {
            new DepartmentDao().Delete(id);

            return RedirectToAction("Index");
        }
    }
}