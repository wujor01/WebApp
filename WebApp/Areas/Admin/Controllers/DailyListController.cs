using Model;
using Model.Dao;
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

        public void SetViewBagKTV(string[] selectedlist = null)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];

            var dao = new EmployeeDao();
            ViewBag.SelectedIDArray = new MultiSelectList(dao.ListAllKTV("KTV", session.DepartmentID), "ID", "Code", selectedlist);
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

        [HttpGet]
        [HasCredential(RoleID = "VIEW_LIST")]
        public ActionResult Invoice(int id)
        {
            WebAppDbContext db = new WebAppDbContext();
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            var dep = db.Departments.Where(x=>x.ID == session.DepartmentID);

            string name = dep.Select(x => x.Name).First().ToString();
            string address = dep.Select(x => x.Address).First().ToString();

            List<InvoiceModel> model = new List<InvoiceModel>();
            var order = db.OrderDetails.Where(x => x.ID == id);

            foreach (var item in order)
            {
                model.Add(new InvoiceModel { 
                    Description = item.Ticket.Name,
                    Quality = 1,
                    Price = item.Ticket.Price
                });
                if (item.DailyList.Voucher_ID != 0 && item.DailyList.Voucher_ID < 100)
                {
                    model.Add(new InvoiceModel
                    {
                        Description = "Voucher",
                        Quality = 1,
                        Price = item.Amount - item.Ticket.Price
                    });
                }
                else if (item.DailyList.Voucher_ID != 0 && item.DailyList.Voucher_ID > 100)
                {
                    model.Add(new InvoiceModel
                    {
                        Description = "Code",
                        Quality = 1,
                        Price = item.Amount - item.Ticket.Price
                    });
                }

                foreach (var temp in item.DailyEmployees)
                {
                    model.Add(new InvoiceModel
                    {
                        Description = "Tip " + temp.Employee.Code.Trim(),
                        Price = temp.Tip
                    });
                }
            }
            long listId = order.Select(x => x.DailyList_ID).First();
            //ViewBag.No = db.DailyLists.Where(x => x.ID == id).Select(x => x.No).First().ToString();
            if (db.OrderDetails.Where(x=>x.DailyList_ID == listId).Count() == 1)
            {
                ViewBag.No = order.Select(x => x.No).First().ToString();
            }
            else
            {
                ViewBag.No = order.Select(x => x.No).First().ToString() + "-" + order.Select(x => x.ID).First().ToString();
            }
            ViewBag.Room = order.Select(x => x.Room.Name).First().ToString();
            string timeIn = order.Select(x => x.TimeIn).First().ToString();
            string timeOut = order.Select(x => x.TimeOut).First().ToString();
            ViewBag.Date = Convert.ToDateTime(timeIn).ToString("dd-MM-yyyy");
            ViewBag.TimeIn = Convert.ToDateTime(timeIn).ToString("H:mm");
            ViewBag.TimeOut = Convert.ToDateTime(timeOut).ToString("H:mm");
            ViewBag.Total = model.Sum(x=>x.Price);
            ViewBag.Invoice = model;
            ViewBag.Null = model.Where(x => x.Price == 0).Count();
            ViewBag.Name = "Massage " + name;
            ViewBag.Address = address;

            return View();
        }

        [HasCredential(RoleID = "VIEW_LIST")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 6)
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
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            DailyList model = new DailyList();
            model.Status = true;
            foreach (var item in order)
            {
                if (item.Room_ID == 0 || item.Ticket_ID == 0)
                {
                    SetAlert("Lỗi khi tạo phòng và vé", "error");
                    return RedirectToAction("Index");
                }
            }
                if (Voucher_ID != 0)
            {
                model.Status = false;
            }
            model.Voucher_ID = Voucher_ID;
            var voucher = db.Vouchers.Find(Voucher_ID);
            if (Voucher_ID != 0 && Voucher_ID >= 100)
            {
                voucher.Status = false;
                db.SaveChanges();
            }
            model.Request = Request;
            model.Description = Description;
            model.PricewithVoucher = 0;
            model.Total = 0;
            
            if (Price != 0)
            {
                model.Status = false;
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

            model.Department_ID = session.DepartmentID;
            model.CreatedDate = DateTime.Now;
            model.CreatedBy = session.UserName;
            db.DailyLists.Add(model);
            db.SaveChanges();

            foreach (var item in order)
            {
                OrderDetail O = new OrderDetail();
                O.Room_ID = item.Room_ID;
                var room = db.Rooms.Find(O.Room_ID);
                room.Status = false;
                db.SaveChanges();
                O.Ticket_ID = item.Ticket_ID;
                var ticket = db.Tickets.Find(item.Ticket_ID);
                DateTime dt = DateTime.Today;
                string date = dt.ToString("yyyy-MM-dd");
                if (db.OrderDetails.Where(x => DbFunctions.TruncateTime(x.TimeIn) == dt).Count() == 0)
                {
                    O.No = date.Replace("-", "") + "-" + session.DepartmentID.ToString()
                    + 1.ToString("D3");
                }
                else
                {
                    string Str = null;
                    foreach (var temp in db.OrderDetails.Where(x => DbFunctions.TruncateTime(x.TimeIn) == dt).OrderByDescending(x => x.ID).Take(1))
                    {
                        Str = temp.No;
                    }
                    string Str1 = Str.Substring(11);
                    O.No = date.Replace("-", "") + "-" + session.DepartmentID.ToString()
                    + (Int32.Parse(Str1) + 1).ToString("D3");
                }

                string[] arrEmpId = string.Join(",", item.SelectedIDArray).Replace(" ", "").Split(',');
                O.empId = string.Join(",", item.SelectedIDArray);

                List<string> emplist = new List<string>();

                foreach (var temp in arrEmpId)
                {
                    int id = Convert.ToInt32(temp);
                    var e = db.Employees.Find(id);
                    e.OnAir = true;
                    db.SaveChanges();
                    emplist.Add(e.Code);
                }

                O.Employee_ID = string.Join(",", emplist).Replace(" ", "");

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
                foreach (var temp in arrEmpId)
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

        [HasCredential(RoleID = "VIEW_LIST")]
        public ActionResult Detail(int id)
        {

            var dailyList = new DailyListDao().ViewDetail(id);
            return View(dailyList);
        }

        [HttpGet]
        [HasCredential(RoleID = "EDIT_LIST")]
        public ActionResult Payment(int id)
        {
            var order = new DailyListDao().ViewDetailOrder(id);
            SetViewDepartment();
            SetViewCustomer();
            SetViewBag();
            SetViewTicket();
            SetViewVoucher();
            SetViewRoom();
            return View(order);
        }

        [HttpGet]
        [HasCredential(RoleID = "EDIT_LIST")]
        public ActionResult EditOrder(int id)
        {
            var dailyList = new DailyListDao().ViewDetail(id);
            var order = db.OrderDetails.Find(id);
            foreach (var temp in order.DailyEmployees)
            {
                var e = db.Employees.Find(temp.Employee_ID);
                e.OnAir = false;
                db.SaveChanges();
            }
            var room = db.Rooms.Find(order.Room_ID);
            room.Status = true;
            db.SaveChanges();

            SetViewDepartment();
            SetViewCustomer();
            SetViewBag();
            SetViewTicket();
            SetViewVoucher();
            SetViewRoom();

            room.Status = false;
            db.SaveChanges();
            foreach (var temp in order.DailyEmployees)
            {
                var e = db.Employees.Find(temp.Employee_ID);
                e.OnAir = true;
                db.SaveChanges();
            }
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
                return RedirectToAction("Payment/" + emp.Order_ID, "DailyList");
            }
            else
            {
                SetAlert("Error!", "error");
                return RedirectToAction("Index", "DailyList");
            }
            
        }

        [HttpGet]
        [HasCredential(RoleID = "DELETE_LIST")]
        public ActionResult Delete(int id)
        {
            var dao = new DailyListDao().Delete(id);

            if (dao == true)
            {
                SetAlert("Xóa thành công!", "success");
                return RedirectToAction("Index");
            }
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