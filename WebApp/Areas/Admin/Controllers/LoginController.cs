using Model.Dao;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using WebApp.Areas.Admin.Models;
using WebApp.Common;

namespace WebApp.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new EmployeeDao();
                var result = dao.Login(model.UserName, model.PassWord);
                if (result == 1)
                {
                    var user = dao.GetById(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.Username;
                    userSession.UserID = user.ID;
                    userSession.GroupID = user.GroupID;
                    userSession.DepartmentID = user.Department.ID;

                    var listCredentials = dao.GetListCredential(model.UserName);

                    Session.Add(CommonConstants.SESSION_CREDENTIALS, listCredentials);
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    if (user.GroupID == "MEMBER")
                    {
                        return RedirectToAction("SelectKTV", "Employee");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại!");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đang bị khóa!");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Ngoài ca làm việc!");
                }
                else if (result == -3)
                {
                    ModelState.AddModelError("", "Sai mật khẩu hoặc tài khoản!");
                }
                else
                {
                    ModelState.AddModelError("","Đăng nhập không thành công!");
                }
            }
            return View("Index");
        }
        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}