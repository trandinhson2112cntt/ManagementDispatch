using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManagementDispatch.Models;

namespace ManagementDispatch.Controllers
{
    public class AdminController : Controller
    {
        DataBaseDataContext _data = new DataBaseDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult LoginAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginAdmin(FormCollection collection)
        {
            var username = collection["username"];

            var password = collection["password"];
            ViewData["Loi"] = "";
            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password))
            {
                ViewData["Loi"] = "Không được bỏ trống tên đăng nhập,mật khẩu";
            }
            else
            {
                NhanVien ad = _data.NhanViens.SingleOrDefault(n => n.Username == username && n.Password == password);
                if (ad != null)
                {
                    Session["Admin"] = ad;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không chính xác";
                }
            }
            return View();
        }

        public ActionResult LogoutAdmin()
        {
            Session["Admin"] = null;
            return View("LoginAdmin");
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
            if (admin != null)
            {
                var getInfoAdmin = _data.NhanViens.First(k => k.IDNhanVien == admin.IDNhanVien);
                return View(getInfoAdmin);
            }
            else
            {
                return View("LoginAdmin");
            }
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
    }
}