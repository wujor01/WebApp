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
    public class RoomController : BaseController
    {
        [HasCredential(RoleID = "VIEW_TICKET")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            var dao = new RoomDao();
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

            var dao = new DeparmentDao();
            ViewBag.Department_ID = new SelectList(dao.ListDepartment(session.DepartmentID), "ID", "Name", selectedId);
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_TICKET")]
        public ActionResult Create(Room room)
        {
            if (ModelState.IsValid)
            {
                var dao = new RoomDao();

                //lấy id trong session đăng nhập của quản trị lưu vào phiên tạo mới user
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                room.CreatedBy = session.UserName;
                room.CreatedDate = DateTime.Now;

                long id = dao.Insert(room);
                if (id > 0)
                {
                    SetAlert("Thêm chấm công nhân viên thành công", "success");
                    return RedirectToAction("Index", "Room");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm chấm nhân viên công không thành công");
                }
            }
            SetViewDepartment();
            SetAlert("Error", "error");
            return RedirectToAction("Index", "Room");
        }

        [HasCredential(RoleID = "EDIT_TICKET")]
        public ActionResult Edit(int id)
        {
            var room = new RoomDao().ViewDetail(id);
            SetViewDepartment();
            return View(room);
        }

        [HttpPost]
        [HasCredential(RoleID = "EDIT_TICKET")]
        public ActionResult Edit(Room room)
        {
            if (ModelState.IsValid)
            {
                var dao = new RoomDao();
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];

                long id = dao.Update(room, session.UserName);
                if (id > 0)
                {
                    SetAlert("Sửa thông tin nhân viên thành công", "success");
                    return RedirectToAction("Index", "Room");
                }
                else
                {
                    SetAlert("Tài khoản hoặc mã nhân viên đã tồn tại!", "error");
                    return RedirectToAction("Index", "Room");
                }
            }
            SetViewDepartment();
            SetAlert("Sửa thông tin nhân viên thất bại", "error");
            return RedirectToAction("Index", "Room");
        }

        [HttpDelete]
        [HasCredential(RoleID = "DELETE_TICKET")]
        public ActionResult Delete(int id)
        {
            new RoomDao().Delete(id);

            return RedirectToAction("Index");
        }
    }
}