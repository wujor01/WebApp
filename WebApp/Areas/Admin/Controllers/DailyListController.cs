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
        public void SetViewDepartment(int? selectedId = null)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];

            var dao = new DepartmentDao();
            ViewBag.Department_ID = new SelectList(dao.ListDepartment(session.DepartmentID), "ID", "Name", selectedId);
        }

        public void SetViewBag(string[] selectedlist = null)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];

            var dao = new EmployeeDao();
            ViewBag.SelectedIDArray = new MultiSelectList(dao.ListAll("KTV",session.DepartmentID), "ID", "Code",selectedlist);
        }

        public void SetViewRoom(int? selectedID = null)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];

            var dao = new EmployeeDao();
            ViewBag.Room_ID = new SelectList(dao.ListRoomAll(session.DepartmentID), "ID", "Name", selectedID);
        }

        public void SetViewCustomer(long? selectedId = null)
        {
            var dao = new CustomerDao();
            ViewBag.Customer_ID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }

        public void SetViewTicket(long? selectedId = null)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];

            var dao = new TicketDao();
            ViewBag.Ticket_ID = new SelectList(dao.ListAll(session.DepartmentID), "ID", "Name", selectedId);
        }

        public void SetViewVoucher(long? selectedId = null)
        {
            var dao = new DailyListDao();
            ViewBag.Voucher_ID = new SelectList(dao.ListAll(), "ID", "Code", selectedId);
        }

        [HasCredential(RoleID = "VIEW_LIST")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            var dao = new DailyListDao();
            var model = dao.ListAllPaging(searchString, page, pageSize, session.DepartmentID);

            ViewBag.SearchString = searchString;

            return View(model);
        }

        [HasCredential(RoleID = "VIEW_LIST")]
        public ActionResult TaxiIndex(string searchString, int page = 1, int pageSize = 10)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];

            var dao = new DailyListDao();
            var model = dao.ListAllPagingTaxi(searchString, page, pageSize, session.DepartmentID);

            ViewBag.SearchString = searchString;

            return View(model);
        }

        [HasCredential(RoleID = "VIEW_CODE")]
        public ActionResult CodeIndex(string searchString, int page = 1, int pageSize = 10)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];

            var dao = new DailyListDao();
            var model = dao.ListAllPagingCode(searchString, page, pageSize);

            ViewBag.SearchString = searchString;

            return View(model);
        }

        [HasCredential(RoleID = "VIEW_LIST")]
        public ActionResult Detail(int id)
        {
            var dailyList = new DailyListDao().ViewDetail(id);
            return View(dailyList);
        }

        [HasCredential(RoleID = "VIEW_CODE")]
        public ActionResult CodeDetail(int id)
        {
            var dailyList = new DailyListDao().ViewCodeDetail(id);
            return View(dailyList);
        }

        [HttpGet]
        [HasCredential(RoleID = "ADD_CODE")]
        public ActionResult CodeCreate()
        {
            return View();
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_CODE")]
        public ActionResult CodeCreate(Voucher voucher)
        {
            var dao = new DailyListDao();

            //lấy id trong session đăng nhập của quản trị lưu vào phiên tạo mới user
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            voucher.CreatedBy = session.UserName;
            voucher.CreatedDate = DateTime.Now;

            long id = dao.CodeInsert(voucher);
            if (id > 0)
            {
                SetAlert("Tạo code thành công", "success");
                return RedirectToAction("CodeDetail/"+voucher.ID, "DailyList");
            }
            else
            {
                ModelState.AddModelError("", "Tạo code không thành công");
            }
            SetAlert("Error!", "error");
            return RedirectToAction("Index", "DailyList");
        }

        [HttpGet]
        [HasCredential(RoleID = "ADD_LIST")]
        public ActionResult Create()
        {
            SetViewDepartment();
            SetViewTicket();
            SetViewVoucher();
            SetViewBag();
            SetViewCustomer();
            SetViewRoom();
            return View();
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_LIST")]
        public ActionResult Create(DailyList list)
        {
            //if (ModelState.IsValid)
            //{
                list.Employee_ID = string.Join(",", list.SelectedIDArray);


                var dao = new DailyListDao();

            //lấy id trong session đăng nhập của quản trị lưu vào phiên tạo mới user
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                list.CreatedBy = session.UserName;
                list.CreatedDate = DateTime.Now;

            long id = dao.Insert(list);
                if (id > 0)
                {
                    SetAlert("Thêm bảng kê thành công", "success");
                    return RedirectToAction("Detail/"+list.ID, "DailyList");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm bảng kê không thành công");
                }
            //}
            SetViewVoucher();
            SetViewDepartment();
            SetViewCustomer();
            SetViewBag();
            SetViewTicket();
            SetViewRoom();
            SetAlert("Error!", "error");
            return RedirectToAction("Index", "DailyList");
        }

        [HttpGet]
        [HasCredential(RoleID = "EDIT_LIST")]
        public ActionResult Edit(int id)
        {
            var dailyList = new DailyListDao().ViewDetail(id);
            SetViewDepartment();
            SetViewCustomer();
            SetViewBag();
            SetViewTicket();
            SetViewVoucher();
            SetViewRoom();
            return View(dailyList);
        }

        [HttpPost]
        [HasCredential(RoleID = "EDIT_LIST")]
        public ActionResult Edit(DailyList dailyList)
        {
            if (ModelState.IsValid)
            {
                var dao = new DailyListDao();
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];

                long id = dao.Update(dailyList,session.UserName);
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
            SetViewDepartment();
            SetViewTicket();
            SetViewCustomer();
            SetViewBag();
            SetViewVoucher();
            SetViewRoom();
            SetAlert("Error", "error");
            return RedirectToAction("Index", "DailyList");
        }

        [HttpDelete]
        [HasCredential(RoleID = "DELETE_LIST")]
        public ActionResult Delete(int id)
        {
            new DailyListDao().Delete(id);

            return RedirectToAction("Index");
        }

        [HttpDelete]
        [HasCredential(RoleID = "DELETE_CODE")]
        public ActionResult CodeDelete(int id)
        {
            new DailyListDao().CodeExpirated(id);

            return RedirectToAction("CodeIndex");
        }
    }

}