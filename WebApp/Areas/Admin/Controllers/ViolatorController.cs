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
    public class ViolatorController : BaseController
    {
        [HasCredential(RoleID = "VIEW_VIOLATOR")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ViolatorDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);

            ViewBag.SearchString = searchString;

            return View(model);
        }

        [HttpGet]
        [HasCredential(RoleID = "ADD_VIOLATOR")]
        public ActionResult Create()
        {
            SetViewBag();
            SetTypeBag();
            return View();
        }

        public void SetViewBag(long? selectedId = null)
        {
            var dao = new EmployeeDao();
            ViewBag.Employee_ID = new SelectList(dao.ListAll("NV"), "ID", "Code", selectedId);
        }

        public void SetTypeBag(int? selectedId = null)
        {
            var dao = new ViolatorDao();
            ViewBag.Type_ID = new SelectList(dao.ListAll(), "ID", "Type", selectedId);
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_VIOLATOR")]
        public ActionResult Create(Violator violator)
        {
            if (ModelState.IsValid)
            {
                var dao = new ViolatorDao();

                //lấy id trong session đăng nhập của quản trị lưu vào phiên tạo mới user
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                violator.CreatedBy = session.UserName;
                violator.CreatedDate = DateTime.Now;

                long id = dao.Insert(violator);
                if (id > 0)
                {
                    SetAlert("Thêm chấm công nhân viên thành công", "success");
                    return RedirectToAction("Index", "Violator");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm chấm nhân viên công không thành công");
                }
            }
            SetViewBag();
            SetTypeBag();
            SetAlert("Error", "error");
            return RedirectToAction("Index", "Violator");
        }

        [HasCredential(RoleID = "EDIT_VIOLATOR")]
        public ActionResult Edit(int id)
        {
            var violator = new ViolatorDao().ViewDetail(id);
            SetViewBag();
            SetTypeBag();
            return View(violator);
        }

        [HttpPost]
        [HasCredential(RoleID = "EDIT_VIOLATOR")]
        public ActionResult Edit(Violator violator)
        {
            if (ModelState.IsValid)
            {
                var dao = new ViolatorDao();
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                violator.ModifiedBy = session.UserName;

                long id = dao.Update(violator);
                if (id > 0)
                {
                    SetAlert("Sửa thông tin nhân viên thành công", "success");
                    return RedirectToAction("Index", "Violator");
                }
                else
                {
                    SetAlert("Tài khoản hoặc mã nhân viên đã tồn tại!", "error");
                    return RedirectToAction("Index", "Violator");
                }
            }
            SetTypeBag();
            SetViewBag();
            SetAlert("Sửa thông tin nhân viên thất bại", "error");
            return RedirectToAction("Index", "Violator");
        }

        [HttpDelete]
        [HasCredential(RoleID = "DELETE_VIOLATOR")]
        public ActionResult Delete(int id)
        {
            new ViolatorDao().Delete(id);

            return RedirectToAction("Index");
        }
    }
}