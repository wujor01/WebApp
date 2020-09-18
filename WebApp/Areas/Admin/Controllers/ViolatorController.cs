﻿using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Common;

namespace WebApp.Areas.Admin.Controllers
{
    public class ViolatorController : BaseController
    {
        //[HasCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new ViolatorDao();
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
        public ActionResult Create(Violator violator)
        {
            if (ModelState.IsValid)
            {
                var dao = new ViolatorDao();

                //lấy id trong session đăng nhập của quản trị lưu vào phiên tạo mới user
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                violator.CreatedBy = session.UserName;
                violator.CreatedDate = DateTime.Now;

                long id = dao.Insert(violator);
                if (id > 0)
                {
                    SetAlert("Thêm chấm công thành công", "success");
                    return RedirectToAction("Index", "Violator");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm chấm công không thành công");
                }
            }
            SetAlert("Error", "error");
            return RedirectToAction("Index", "Violator");
        }

        //[HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(int id)
        {
            var violator = new ViolatorDao().ViewDetail(id);
            return View(violator);
        }

        [HttpPost]
        //[HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(Violator violator)
        {
            if (ModelState.IsValid)
            {
                var dao = new ViolatorDao();
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                violator.ModifiedBy = session.UserName;

                long id = dao.Update(violator);
                if (id > 0)
                {
                    SetAlert("Sửa thông tin nhân viên thành công", "success");
                    return RedirectToAction("Index", "Violator");
                }
                else
                {
                    SetAlert("Tài khoản hoặc mã nhân viên đã tồn tại!", "error");
                    return RedirectToAction("Index", "Violator");
                }
            }
            SetAlert("Sửa thông tin nhân viên thất bại", "error");
            return RedirectToAction("Index", "Violator");
        }

        [HttpDelete]
        //[HasCredential(RoleID = "DELETE_USER")]
        public ActionResult Delete(int id)
        {
            new ViolatorDao().Delete(id);

            return RedirectToAction("Index");
        }
    }
}