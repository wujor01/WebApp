using System;
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
            if (ModelState.IsValid)
            {
                var dao = new EmployeeDao();

                //lấy id trong session đăng nhập của quản trị lưu vào phiên tạo mới user
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                employee.CreatedBy = session.UserName;
                employee.CreatedDate = DateTime.Now;
                //lấy đường dẫn ảnh upload lưu vào db
                var fileName = Path.GetFileName(Image.FileName);
                var folderName = "/Areas/Admin/Data/Employee/img/";
                var path = Path.Combine(Server.MapPath(folderName), fileName);
                Image.SaveAs(path);
                //Lấy chuỗi từ vị trí "Areas" -1 giống với folderName lưu vào db
                employee.Image = path.Substring(path.IndexOf("Areas")-1);
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
            }
            return View("Index");
        }
            
    }
}
