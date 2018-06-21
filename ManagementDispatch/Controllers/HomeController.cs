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

        public ActionResult SuggestNumberTextNext()
        {
            int countTextTo = _data.CongVanDens.Count();
            int countTextGo = _data.CongVanDis.Count();
            var getNewTextTo = _data.CongVanDens.OrderByDescending(x => x.STT).FirstOrDefault();
            int index = 0,index2=0,number = 0, number2 = 0;
            string getStringSuggest = null;
            if (getNewTextTo != null)
            {
                index = getNewTextTo.IDCongVanDen.IndexOf("/");
                getStringSuggest = getNewTextTo.IDCongVanDen.Substring(0, index);
            }
            if (getStringSuggest != null)
                number = int.Parse(getStringSuggest);
            var getNewTextGo = _data.CongVanDis.OrderByDescending(x => x.STT).FirstOrDefault();
           
            string getStringSuggest2 = null;
            if (getNewTextGo != null)
            {
                index2 = getNewTextGo.IDCongVanDi.IndexOf("/");
                getStringSuggest2 = getNewTextGo.IDCongVanDi.Substring(0, index2);
            }
            if (getStringSuggest2 != null)
                number2 = int.Parse(getStringSuggest2);
            int selectNumber = number;
            if (number2 > number)
                selectNumber = number2;
            var statistical = new SuggestNumberText()
            {
                CountTextTo = countTextTo,
                CountTextGo = countTextGo,
                SUM = countTextTo + countTextGo,
                Number = selectNumber
            };
            

            return PartialView(statistical);
            
        }
    }
}