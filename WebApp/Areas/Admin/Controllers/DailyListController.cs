﻿using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApp.Areas.Admin.Models;
using WebApp.Common;

namespace WebApp.Areas.Admin.Controllers
{
    public class DailyListController : BaseController
    {
        WebAppDbContext db = new WebAppDbContext();

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

        public void SetViewEmp(string[] selectedlist = null)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];

            var dao = new EmployeeDao();
            ViewBag.Employee_ID = new SelectList(dao.ListAll("KTV", session.DepartmentID), "ID", "Code", selectedlist);
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

        public ActionResult SaveOrder(
            long Voucher_ID, string Request, string Description,
            string Code, string Name, int NumberOfCustomers, decimal Price, string Phone, string Taxi_Description,
            OrderDetail[] order)
        {
            string result = "Error! Order Is Not Complete!";

            DailyList model = new DailyList();
            model.Voucher_ID = Voucher_ID;
            model.Request = Request;
            model.Description = Description;
            model.PricewithVoucher = 0;
            model.Total = 0;
            if (Price != 0)
            {
                Taxi taxi = new Taxi();
                taxi.Code = Code;
                taxi.Name = Name;
                taxi.NumberOfCustomers = NumberOfCustomers;
                taxi.Price = Price;
                taxi.Phone = Phone;
                taxi.Description = Taxi_Description;
                db.Taxis.Add(taxi);
                db.SaveChanges();

                model.Taxi_ID = taxi.ID;
            }

            var session = (UserLogin)Session[CommonConstants.USER_SESSION];

            model.Department_ID = session.DepartmentID;
            model.CreatedDate = DateTime.Now;
            model.CreatedBy = session.UserName;
            db.DailyLists.Add(model);
            db.SaveChanges();

            var voucher = db.Vouchers.Find(model.Voucher_ID);
            foreach (var item in order)
            {
                OrderDetail O = new OrderDetail();
                O.Room_ID = item.Room_ID;
                O.Ticket_ID = item.Ticket_ID;
                var ticket = db.Tickets.Find(item.Ticket_ID);
                O.Employee_ID = string.Join(",", item.SelectedIDArray).Replace(" ", "");

                if (model.Voucher_ID == 0)
                {
                    O.Amount = ticket.Price;
                }
                else
                {
                    O.Amount = ticket.Price * (1 - voucher.DiscountPercent / 100);
                }
                O.TimeIn = DateTime.Now;
                O.TimeOut = DateTime.Now.AddMinutes(ticket.TimeTotal);
                O.DailyList_ID = model.ID;
                model.PricewithVoucher = model.PricewithVoucher + O.Amount;
                db.OrderDetails.Add(O);
                db.SaveChanges();
                foreach (var temp in item.SelectedIDArray)
                {
                    DailyEmployee emp = new DailyEmployee();
                    emp.Order_ID = O.ID;
                    emp.Employee_ID = long.Parse(temp);
                    emp.Date = DateTime.Now.Date;
                    emp.Clean = 0;
                    emp.Tour = 0;
                    emp.Tour = 0;
                    db.DailyEmployees.Add(emp);
                    db.SaveChanges();
                }
            }
            model.Total = model.PricewithVoucher;
            db.SaveChanges();
            result = "Success! Order Is Complete!";

        return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HasCredential(RoleID = "EDIT_LIST")]
        public ActionResult Comfirm(int id)
        {
            new DailyListDao().Comfirm(id);
            SetAlert("Đổi trạng thái thành công", "success");
            return RedirectToAction("Index");
        }

        [HttpGet]
        [HasCredential(RoleID = "EDIT_LIST")]
        public ActionResult EditDailyList(int id)
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
        public ActionResult EditDailyList(DailyList dailyList)
        {
            var dao = new DailyListDao();
            SetViewDepartment();
            SetViewTicket();
            SetViewCustomer();
            SetViewBag();
            SetViewVoucher();
            SetViewRoom();
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

        [HttpGet]
        [HasCredential(RoleID = "EDIT_LIST")]
        public ActionResult EditOrder(int id)
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
        public ActionResult EditOrder(OrderDetail order)
        {
            var dao = new DailyListDao();
            SetViewDepartment();
            SetViewTicket();
            SetViewCustomer();
            SetViewBag();
            SetViewVoucher();
            SetViewRoom();
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            if (order.Room_ID == 0 || order.Ticket_ID == 0 || order.SelectedIDArray == null)
            {
                SetAlert("Thiếu thông tin!", "error");
                return View();
            }
            
            long id = dao.UpdateOrder(order, session.UserName);
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

        [HttpGet]
        public ActionResult EditEmp(long orderId, long empId)
        {
            var emp = db.DailyEmployees.Find(orderId,empId);
            SetViewEmp();
            return View(emp);
        }

        [HttpPost]
        public ActionResult EditEmp(DailyEmployee emp)
        {
            var dao =new DailyListDao();
            long id = dao.UpdateEmp(emp);
            SetViewEmp();
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