using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;
using WebApp.Common;

namespace WebApp.Areas.Admin.Controllers
{
    public class EmployeeController : BaseController
    {
        // GET: Admin/Employee

        [HasCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new EmployeeDao();
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];

            var model = dao.ListAllPaging(searchString, page, pageSize, session.DepartmentID);

            ViewBag.SearchString = searchString;

            return View(model);
        }

        public void SetViewBag(int? selectedId = null)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];

            var dao = new DeparmentDao();
            ViewBag.Department_ID = new SelectList(dao.ListDepartment(session.DepartmentID), "ID", "Name", selectedId);
        }

        [HttpGet]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create()
        {
            SetViewBag();
            return View();        
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var dao = new EmployeeDao();

                //mã nhân viên khi lưu vào db luôn là HOA
                if (!string.IsNullOrEmpty(employee.Code))
                {
                    var upperCode = employee.Code.ToUpper();
                    employee.Code = upperCode;
                }

                //tài khoản luôn là chữ thường khi lưu
                if (!string.IsNullOrEmpty(employee.Username))
                {
                    var lowerUserName = employee.Username.ToLower();
                    employee.Username = lowerUserName;
                }

                //lấy id trong session đăng nhập của quản trị lưu vào phiên tạo mới user
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                employee.CreatedBy = session.UserName;
                employee.CreatedDate = DateTime.Now;
                int deparmentId = session.DepartmentID;
                
                long id = dao.Insert(employee);
                if (id > 0)
                {
                    SetAlert("Thêm nhân viên thành công", "success");
                    return RedirectToAction("Index", "Employee");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm nhân viên không thành công");
                }
            }
            SetViewBag();
            SetAlert("Tài khoản hoặc mã nhân viên đã tồn tại!", "error");
            return RedirectToAction("Index", "Employee");
        }

        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(int id)
        {
            var employee = new EmployeeDao().ViewDetail(id);
            SetViewBag();
            return View(employee);
        }

        [HttpPost]
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var dao = new EmployeeDao();
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];

                long id = dao.Update(employee, session.UserName);
                if (id > 0)
                {
                    SetAlert("Sửa thông tin nhân viên thành công", "success");
                    return RedirectToAction("Index", "Employee");
                }
                else
                {
                    SetAlert("Tài khoản hoặc mã nhân viên đã tồn tại!", "error");
                    return RedirectToAction("Index", "Employee");
                }
            }
            SetViewBag();
            SetAlert("Sửa thông tin nhân viên thất bại", "error");
            return RedirectToAction("Index", "Employee");
        }

        [HttpDelete]
        [HasCredential(RoleID = "DELETE_USER")]
        public ActionResult Delete(int id)
        {
            var dao = new EmployeeDao();

            bool status = dao.Delete(id);
            if (status)
            {
                SetAlert("Xóa nhân viên thành công", "success");
            }
            else
            {
                SetAlert("Xóa nhân viên thất bại", "error");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [HasCredential(RoleID = "EDIT_USER")]
        public JsonResult ChangeStatus(long id)
        {
            var result = new EmployeeDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        [HttpPost]
        [HasCredential(RoleID = "EDIT_USER")]
        public JsonResult ChangeStatusAccount(long id)
        {
            var result = new EmployeeDao().ChangeStatusAccount(id);
            return Json(new
            {
                status = result
            });
        }
    }
}
