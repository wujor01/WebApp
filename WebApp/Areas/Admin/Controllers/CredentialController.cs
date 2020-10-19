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
    public class CredentialController : BaseController
    {
        public void SetViewBag(string[] selectedlist = null)
        {
            var dao = new CredentialDao();
            ViewBag.SelectedIDRole = new MultiSelectList(dao.ListAll(), "ID", "Name", selectedlist);
        }

        public void SetViewGroup(string selectedId = null)
        {
            var dao = new CredentialDao();
            ViewBag.UserGroupID = new SelectList(dao.ListAllGroup(), "GroupID", "Name", selectedId);
        }


        // GET: Admin/Credential
        [HasCredential(RoleID = "VIEW_ROLE")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 20)
        {
            var dao = new CredentialDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);

            ViewBag.SearchString = searchString;

            return View(model);
        }

        [HttpGet]
        [HasCredential(RoleID = "ADD_ROLE")]
        public ActionResult Create()
        {
            SetViewGroup();
            SetViewBag();
            return View();
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_ROLE")]
        public ActionResult Create(Credential credential)
        {
            if (ModelState.IsValid)
            {
                var dao = new CredentialDao();

                //lấy id trong session đăng nhập của quản trị lưu vào phiên tạo mới user

                long id = dao.Insert(credential);
                if (id > 0)
                {
                    SetAlert("Phân quyền thành công", "success");
                    return RedirectToAction("Index", "Credential");
                }
                else
                {
                    ModelState.AddModelError("", "Phân quyền không thành công");
                }
            }
            SetViewBag();
            SetAlert("Error", "error");
            return RedirectToAction("Index", "Credential");
        }
    }
}