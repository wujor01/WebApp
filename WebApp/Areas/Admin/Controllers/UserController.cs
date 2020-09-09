using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebApp.Common;

namespace WebApp.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        // GET: Admin/User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid) 
            {
            var dao = new UserDao();

            var encryptedMd5Pas = Encryptor.MD5Hash(user.Password);
            user.Password = encryptedMd5Pas;

                //lấy id trong session đăng nhập của mod lưu vào phiên tạo mới user
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                user.CreatedBy = session.UserName;
                user.CreatedDate = DateTime.Now;
            long id = dao.Insert(user);
                if (id >0)
                {
                SetAlert("Thêm user thành công", "success");
                return RedirectToAction("Index", "User");
                }
                else
                {
                ModelState.AddModelError("", "Thêm user không thành công");
                }
            }
            return View("Index");
        }
    }
}