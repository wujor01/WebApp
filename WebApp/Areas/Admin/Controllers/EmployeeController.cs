﻿using System;
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

        //[HasCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new EmployeeDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);

            ViewBag.SearchString = searchString;

            return View(model);
        }

        [HttpGet]
        //[HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        //[HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create(Employee employee, HttpPostedFileBase Image)
        {
            //if (ModelState.IsValid)
            //{
                var dao = new EmployeeDao();

                //Băm mật khẩu bằng hàm MD5
                if (!string.IsNullOrEmpty(employee.Password))
                {
                    var encryptedMd5Pas = Encryptor.MD5Hash(employee.Password);
                    employee.Password = encryptedMd5Pas;
                }
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

                if (Image != null)
                {
                    //lấy đường dẫn ảnh upload lưu vào db
                var fileName = Path.GetFileName(Image.FileName);
                var folderName = "/Areas/Admin/Data/Employee/img/";
                var path = Path.Combine(Server.MapPath(folderName), fileName);
                Image.SaveAs(path);

                //Lấy chuỗi từ vị trí "Areas"-1 giống với folderName lưu vào db
                employee.Image = path.Substring(path.IndexOf("Areas")-1);
                }
                
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
            //}
            return View("Index");
        }

        //[HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(int id)
        {
            var employee = new EmployeeDao().ViewDetail(id);
            return View(employee);
        }

        [HttpPost]
        //[HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(Employee employee, HttpPostedFileBase Image)
        {
            //if (ModelState.IsValid)
            //{
                var dao = new EmployeeDao();
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                employee.ModifiedBy = session.UserName;

                if (!string.IsNullOrEmpty(employee.Password))
                {
                    var encryptedMd5Pas = Encryptor.MD5Hash(employee.Password);
                    employee.Password = encryptedMd5Pas;
                }
               
                if (Image != null)
                {
                    //lấy đường dẫn ảnh upload lưu vào db
                var fileName = Path.GetFileName(Image.FileName);
                var folderName = "/Areas/Admin/Data/Employee/img/";
                var path = Path.Combine(Server.MapPath(folderName), fileName);
                Image.SaveAs(path);

                //Lấy chuỗi từ vị trí "Areas"-1 giống với folderName lưu vào db
                employee.Image = path.Substring(path.IndexOf("Areas")-1);
                }
                

                var result = dao.Update(employee);
                if (result)
                {
                    SetAlert("Cập nhật thông tin thành công", "success");
                    return RedirectToAction("Index", "Employee");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật thông tin thành công");
                //}
            }
            return View("Index");
        }

        [HttpDelete]
        //[HasCredential(RoleID = "DELETE_USER")]
        public ActionResult Delete(int id)
        {
            new EmployeeDao().Delete(id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        //[HasCredential(RoleID = "EDIT_USER")]
        public JsonResult ChangeStatus(long id)
        {
            var result = new EmployeeDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        [HttpPost]
        //[HasCredential(RoleID = "EDIT_USER")]
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
