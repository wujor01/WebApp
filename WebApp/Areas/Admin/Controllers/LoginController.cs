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
                var result = dao.Login(model.UserName, Encryptor.MD5Hash(model.PassWord));
                if (result == 1)
                {
                    var user = dao.GetById(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.Username;
                    userSession.UserID = user.ID;
                    userSession.GroupID = user.GroupID;
                    var listCredentials = dao.GetListCredential(model.UserName);

                    Session.Add(CommonConstants.SESSION_CREDENTIALS, listCredentials);
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    return RedirectToAction("Index", "Home");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Account not exist!");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Account was  blocked!");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Time Out!");
                }
                else if (result == -3)
                {
                    ModelState.AddModelError("", "Wrong Password!");
                }
                else
                {
                    ModelState.AddModelError("","Login unsuccessful!");
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