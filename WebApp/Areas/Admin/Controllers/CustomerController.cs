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
    public class CustomerController : BaseController
    {
        // GET: Admin/Customer
        [HasCredential(RoleID = "VIEW_CUSTOMER")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {

            var dao = new CustomerDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);

            ViewBag.SearchString = searchString;

            return View(model);
        }

        [HttpGet]
        [HasCredential(RoleID = "ADD_CUSTOMER")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_CUSTOMER")]
        public ActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var dao = new CustomerDao();

                //lấy id trong session đăng nhập của quản trị lưu vào phiên tạo mới user
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                customer.CreatedBy = session.UserName;
                customer.CreatedDate = DateTime.Now;

                long id = dao.Insert(customer);
                if (id > 0)
                {
                    SetAlert("Thêm khách hàng thành công", "success");
                    return RedirectToAction("Index", "Customer");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm khách hàng không thành công");
                }
            }
            SetAlert("Error", "error");
            return RedirectToAction("Index", "Customer");
        }

        [HasCredential(RoleID = "EDIT_CUSTOMER")]
        public ActionResult Edit(int id)
        {
            var customer = new CustomerDao().ViewDetail(id);
            return View(customer);
        }

        [HttpPost]
        [HasCredential(RoleID = "EDIT_CUSTOMER")]
        public ActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var dao = new CustomerDao();
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];

                long id = dao.Update(customer, session.UserName);
                if (id > 0)
                {
                    SetAlert("Sửa thông tin khách hàng thành công", "success");
                    return RedirectToAction("Index", "Customer");
                }
                else
                {
                    SetAlert("Sửa thông tin khách hàng không thành công!", "error");
                    return RedirectToAction("Index", "Customer");
                }
            }
            SetAlert("Sửa thông tin khách hàng không thành công", "error");
            return RedirectToAction("Index", "Customer");
        }

        [HttpDelete]
        [HasCredential(RoleID = "DELETE_CUSTOMER")]
        public ActionResult Delete(int id)
        {
            new CustomerDao().Delete(id);

            return RedirectToAction("Index");
        }
    }
}