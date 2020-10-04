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
            ViewBag.SelectedIDArray = new MultiSelectList(dao.ListAll("KTV",session.DepartmentID), "Code", "Code",selectedlist);
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
            ViewBag.ticketId = new SelectList(dao.ListAll(session.DepartmentID), "ID", "Name", selectedId);
        }

        public void SetViewVoucher(long? selectedId = null)
        {
            var dao = new DailyListDao();
            ViewBag.Voucher_ID = new SelectList(dao.ListAll(), "ID", "Code", selectedId);
        }

        [HasCredential(RoleID = "VIEW_CODE")]
        public ActionResult CodeIndex(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new DailyListDao();
            var model = dao.ListAllPagingCode(searchString, page, pageSize);

            ViewBag.SearchString = searchString;

            return View(model);
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

        [HttpDelete]
        [HasCredential(RoleID = "DELETE_CODE")]
        public ActionResult CodeDelete(int id)
        {
            new DailyListDao().CodeExpirated(id);

            return RedirectToAction("CodeIndex");
        }

        public ActionResult Index()
        {
            var dao = new DailyListDao();
            ViewData.['a'] = 1;
            return View(dao.ListAll());
        }

        public ActionResult SaveOrder(string request, int voucherId, string taxiCode, string taxiName, string taxiPhone, decimal taxiPrice, int taxiNumCustomer, string taxiDescription, OrderDetail[] order)
        {
            string result = "Error! Order Is Not Complete!";
            if (order != null)
            {
                WebAppDbContext db = new WebAppDbContext();

                DailyList model = new DailyList();
                model.Request = request;
                model.Voucher_ID = voucherId;
                if (model.Taxi.Price != 0)
                {
                    model.Taxi.Code = taxiCode;
                    model.Taxi.Name = taxiName;
                    model.Taxi.Phone = taxiPhone;
                    model.Taxi.Price = taxiPrice;
                    model.Taxi.NumberOfCustomers = taxiNumCustomer;
                    model.Taxi.Description = taxiDescription;
                }
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                model.CreatedDate = DateTime.Now;
                model.CreatedBy = session.UserName;
                db.DailyLists.Add(model);

                foreach (var item in order)
                {
                    OrderDetail O = new OrderDetail();
                    O.DailyList_ID = model.ID;
                    O.Room_ID = item.Room_ID;
                    O.Ticket_ID = item.Ticket_ID;
                    O.Employee_ID = item.Employee_ID;
                    O.TimeIn = DateTime.Now;
                    O.TimeOut = DateTime.Now.AddMinutes(item.Ticket.TimeTotal);
                    O.Tip = item.Tip;
                    O.Amount = item.Amount;
                    db.OrderDetails.Add(O);
                }
                db.SaveChanges();
                result = "Success! Order Is Complete!";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    
    }

}