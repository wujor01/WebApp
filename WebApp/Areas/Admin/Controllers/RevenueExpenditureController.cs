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
    public class RevenueExpenditureController : BaseController
    {
        public void SetViewBag(int? selectedId = null)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];

            var dao = new DepartmentDao();
            ViewBag.Department_ID = new SelectList(dao.ListDepartment(session.DepartmentID), "ID", "Name", selectedId);
        }

        public void SetTypeBag(int? selectedId = null)
        {
            var dao = new RevenueExpenditureDao();
            ViewBag.Type_ID = new SelectList(dao.ListAll(), "ID", "Type", selectedId);
        }

        [HasCredential(RoleID = "VIEW_REEX")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];

            var dao = new RevenueExpenditureDao();
            var model = dao.ListAllPaging(searchString, page, pageSize, session.DepartmentID);

            ViewBag.SearchString = searchString;

            return View(model);
        }

        [HttpGet]
        [HasCredential(RoleID = "ADD_REEX")]
        public ActionResult Create()
        {
            SetViewBag();
            SetTypeBag();
            return View();
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_REEX")]
        public ActionResult Create(RevenueExpenditure revenueExpenditure)
        {
            if (ModelState.IsValid)
            {
                var dao = new RevenueExpenditureDao();

                //lấy id trong session đăng nhập của quản trị lưu vào phiên tạo mới user
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                revenueExpenditure.CreatedBy = session.UserName;
                revenueExpenditure.CreatedDate = DateTime.Now;

                long id = dao.Insert(revenueExpenditure);
                if (id > 0)
                {
                    SetAlert("Thêm chấm công nhân viên thành công", "success");
                    return RedirectToAction("Index", "RevenueExpenditure");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm chấm nhân viên công không thành công");
                }
            }
            SetViewBag();
            SetTypeBag();
            SetAlert("Error", "error");
            return RedirectToAction("Index", "RevenueExpenditure");
        }

        [HasCredential(RoleID = "EDIT_REEX")]
        public ActionResult Edit(int id)
        {
            var revenueExpenditure = new RevenueExpenditureDao().ViewDetail(id);
            SetViewBag();
            SetTypeBag();
            return View(revenueExpenditure);
        }

        [HttpPost]
        [HasCredential(RoleID = "EDIT_REEX")]
        public ActionResult Edit(RevenueExpenditure revenueExpenditure)
        {
            if (ModelState.IsValid)
            {
                var dao = new RevenueExpenditureDao();
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];

                long id = dao.Update(revenueExpenditure, session.UserName);
                if (id > 0)
                {
                    SetAlert("Sửa thông tin nhân viên thành công", "success");
                    return RedirectToAction("Index", "RevenueExpenditure");
                }
                else
                {
                    SetAlert("Tài khoản hoặc mã nhân viên đã tồn tại!", "error");
                    return RedirectToAction("Index", "RevenueExpenditure");
                }
            }
            SetViewBag();
            SetTypeBag();
            SetAlert("Sửa thông tin nhân viên thất bại", "error");
            return RedirectToAction("Index", "RevenueExpenditure");
        }

        [HttpDelete]
        [HasCredential(RoleID = "DELETE_REEX")]
        public ActionResult Delete(int id)
        {
            new RevenueExpenditureDao().Delete(id);

            return RedirectToAction("Index");
        }
    }
}