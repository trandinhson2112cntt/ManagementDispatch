using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManagementDispatch.Models;
namespace ManagementDispatch.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        DataBaseDataContext _data = new DataBaseDataContext();
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
                    Session["userId"] = ad.IDNhanVien;

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
        public ActionResult NotificationAuthorize()
        {
            return View();
        }
    }
}