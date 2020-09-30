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
    public class TicketController : BaseController
    {
        // GET: Admin/Ticket
        [HasCredential(RoleID = "VIEW_TICKET")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            var dao = new TicketDao();
            var model = dao.ListAllPaging(searchString, page, pageSize, session.DepartmentID);

            ViewBag.SearchString = searchString;

            return View(model);
        }

        [HttpGet]
        [HasCredential(RoleID = "ADD_TICKET")]
        public ActionResult Create()
        {
            SetViewDepartment();
            return View();
        }

        public void SetViewDepartment(int? selectedId = null)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];

            var dao = new DepartmentDao();
            ViewBag.Department_ID = new SelectList(dao.ListDepartment(session.DepartmentID), "ID", "Name", selectedId);
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_TICKET")]
        public ActionResult Create(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                var dao = new TicketDao();

                //lấy id trong session đăng nhập của quản trị lưu vào phiên tạo mới user
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                ticket.CreatedBy = session.UserName;
                ticket.CreatedDate = DateTime.Now;

                long id = dao.Insert(ticket);
                if (id > 0)
                {
                    SetAlert("Thêm chấm công nhân viên thành công", "success");
                    return RedirectToAction("Index", "Ticket");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm chấm nhân viên công không thành công");
                }
            }
            SetViewDepartment();
            SetAlert("Error", "error");
            return RedirectToAction("Index", "Ticket");
        }

        [HasCredential(RoleID = "EDIT_TICKET")]
        public ActionResult Edit(int id)
        {
            var ticket = new TicketDao().ViewDetail(id);
            SetViewDepartment();
            return View(ticket);
        }

        [HttpPost]
        [HasCredential(RoleID = "EDIT_TICKET")]
        public ActionResult Edit(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                var dao = new TicketDao();
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];

                long id = dao.Update(ticket, session.UserName);
                if (id > 0)
                {
                    SetAlert("Sửa thông tin nhân viên thành công", "success");
                    return RedirectToAction("Index", "Ticket");
                }
                else
                {
                    SetAlert("Tài khoản hoặc mã nhân viên đã tồn tại!", "error");
                    return RedirectToAction("Index", "Ticket");
                }
            }
            SetViewDepartment();
            SetAlert("Sửa thông tin nhân viên thất bại", "error");
            return RedirectToAction("Index", "Ticket");
        }

        [HttpDelete]
        [HasCredential(RoleID = "DELETE_TICKET")]
        public ActionResult Delete(int id)
        {
            new TicketDao().Delete(id);

            return RedirectToAction("Index");
        }
    }
}