using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManagementDispatch.Models;

namespace ManagementDispatch.Controllers
{
    [AuthorzireBusiness]
    public class AdminController : Controller
    {
        DataBaseDataContext _data = new DataBaseDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult InfoAdminLogin()
        {
            NhanVien admin = (NhanVien)Session["Admin"];
            if (admin != null)
            {
                var getInfoAdmin = _data.NhanViens.First(k => k.IDNhanVien == admin.IDNhanVien);
                return PartialView(getInfoAdmin);
            }
            else
            {
                return View("LoginAdmin");
            }
        }
        public ActionResult ProfileAdmin()
        {
            NhanVien admin = (NhanVien)Session["Admin"];

            var getInfoAdmin = _data.NhanViens.First(k => k.IDNhanVien == admin.IDNhanVien);
            return View(getInfoAdmin);

        }
        [HttpGet]
        public ActionResult EditProfile()
        {
            NhanVien admin = (NhanVien)Session["Admin"];

            var getInfoAdmin = _data.NhanViens.First(k => k.IDNhanVien == admin.IDNhanVien);
            return View(getInfoAdmin);

        }
        [HttpPost]
        public ActionResult EditProfile(FormCollection formCollection)
        {
            NhanVien admin = (NhanVien)Session["Admin"];
            var getInfoAdmin = _data.NhanViens.First(k => k.IDNhanVien == admin.IDNhanVien);
            string name = formCollection["hoten"];
            string email = formCollection["email"];
            DateTime birthday = DateTime.Parse(formCollection["ngaysinh"]);
            string tel = formCollection["sdt"];
            getInfoAdmin.HoTen = name;
            getInfoAdmin.NgaySinh = birthday.Date;
            getInfoAdmin.SDT = tel;
            getInfoAdmin.Email = email;

            UpdateModel(getInfoAdmin);
            _data.SubmitChanges();
            return View("ProfileAdmin");

        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            NhanVien admin = (NhanVien)Session["Admin"];

            var getInfoAdmin = _data.NhanViens.First(k => k.IDNhanVien == admin.IDNhanVien);
            return View(getInfoAdmin);

        }
        [HttpPost]
        public ActionResult ChangePassword(FormCollection formCollection)
        {
            NhanVien admin = (NhanVien)Session["Admin"];
            var getInfoAdmin = _data.NhanViens.First(k => k.IDNhanVien == admin.IDNhanVien);
            string oldpassword = formCollection["oldpassword"];
            string newpassword = formCollection["newpassword"];
            string confirmpassword = formCollection["confirmpassword"];

            if (oldpassword != getInfoAdmin.Password)
                ViewBag.Validate = "Sai mật khẩu cũ";
            if (newpassword == confirmpassword)
                ViewBag.Validate = "Mật khẩu mới không trùng khớp";
            if (oldpassword == getInfoAdmin.Password && newpassword == confirmpassword)
            {
                getInfoAdmin.Password = newpassword;
            }

            UpdateModel(getInfoAdmin);
            _data.SubmitChanges();
            return View("ProfileAdmin");

        }

        //---------------------------
        public ActionResult ListAdmin()
        {
            var getListAdmin = _data.NhanViens.ToList();
            return View(getListAdmin);
        }

        [HttpGet]
        public ActionResult AddAdmin()
        {
            ViewBag.IDPhongBan = new SelectList(_data.PhongBans.ToList().OrderBy(n => n.TenPhongBan), "IDPhongBan", "TenPhongBan");
            return View();
        }
        [HttpPost]
        public ActionResult AddAdmin(NhanVien item, FormCollection formCollection)
        {
            ViewBag.IDPhongBan = new SelectList(_data.PhongBans.ToList().OrderBy(n => n.TenPhongBan), "IDPhongBan", "TenPhongBan");
            try
            {
                item.HoTen = formCollection["FullName"];
                item.Username = formCollection["Username"];
                item.Password = formCollection["Password"];
                item.Email = formCollection["Email"];
                item.NgaySinh = DateTime.Parse(formCollection["Birthday"]);
                item.SDT = formCollection["PhoneNumber"];
                item.Lock = true;
                _data.NhanViens.InsertOnSubmit(item);
                _data.SubmitChanges();

                string writeLog = "Thêm nhân viên mới: " + item.HoTen;
                NhanVien admin = (NhanVien)Session["Admin"];
                NhatKyHeThong log = new NhatKyHeThong();
                log.IDNhanVien = admin.IDNhanVien;
                log.NoiDungNhatKy = writeLog;
                log.NgayGio = DateTime.Now;
                _data.NhatKyHeThongs.InsertOnSubmit(log);
                _data.SubmitChanges();

                return RedirectToAction("ListAdmin");

            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var getAdmin = _data.NhanViens.First(c => c.IDNhanVien == id);
            getAdmin.Lock = !getAdmin.Lock;
            UpdateModel(getAdmin);
            _data.SubmitChanges();
            var result = getAdmin.Lock;
            return Json(new
            {
                status = result
            });

        }

        public ActionResult ListRole()
        {
            var getListRole = _data.BlogBusinesses.ToList();
            return View(getListRole);
        }
        [HttpGet]
        public ActionResult EditBlogBusiness(string id)
        {
            BlogBusiness blogBusiness = _data.BlogBusinesses.SingleOrDefault(n => n.BusinessId == id);
            return View(blogBusiness);
        }
        [HttpPost]
        public ActionResult EditBlogBusiness(string id, FormCollection formCollection)
        {
            try
            {
                BlogBusiness blogBusiness = _data.BlogBusinesses.SingleOrDefault(n => n.BusinessId == id);
                blogBusiness.BusinessName = formCollection["InputName"];
                UpdateModel(blogBusiness);
                _data.SubmitChanges();

                return RedirectToAction("ListRole");
            }
            catch
            {
                return View();
            }

        }

        public ActionResult ListPermission(string id)
        {
            var getListPermission = _data.BlogPermissions.Where(x => x.BusinessId == id).ToList();
            return View(getListPermission);
        }
        [HttpGet]
        public ActionResult EditBlogPermission(int id)
        {
            BlogPermission blogBusiness = _data.BlogPermissions.SingleOrDefault(n => n.PermissionId == id);
            return View(blogBusiness);
        }
        [HttpPost]
        public ActionResult EditBlogPermission(int id, FormCollection formCollection)
        {
            try
            {
                BlogPermission blogPermission = _data.BlogPermissions.SingleOrDefault(n => n.PermissionId == id);
                blogPermission.Description = formCollection["Description"];
                UpdateModel(blogPermission);
                _data.SubmitChanges();

                return RedirectToAction("ListPermission", new { id = blogPermission.BusinessId });
            }
            catch
            {
                return View();
            }

        }



        public ActionResult UpdateBusiness()
        {
            ReflectionController rc = new ReflectionController();
            List<Type> listControllerType = rc.GetControllers("ManagementDispatch.Controllers");
            List<string> listControllerOld = _data.BlogBusinesses.Select(c => c.BusinessId).ToList();
            List<string> listPermessionOld = _data.BlogPermissions.Select(c => c.PermissionName).ToList();
            foreach (var c in listControllerType)
            {
                if (!listControllerOld.Contains(c.Name))
                {
                    BlogBusiness b = new BlogBusiness() { BusinessId = c.Name, BusinessName = "Chưa có mô tả" };
                    _data.BlogBusinesses.InsertOnSubmit(b);
                    _data.SubmitChanges();
                }
                List<string> listPermission = rc.GetActions(c);
                foreach (var p in listPermission)
                {
                    if (!listPermessionOld.Contains(c.Name + "-" + p))
                    {
                        BlogPermission permission = new BlogPermission() { PermissionName = c.Name + "-" + p, Description = "Chưa có mô tả", BusinessId = c.Name };
                        _data.BlogPermissions.InsertOnSubmit(permission);
                        _data.SubmitChanges();
                    }
                }
            }

            return RedirectToAction("ListRole");
        }

    }
}