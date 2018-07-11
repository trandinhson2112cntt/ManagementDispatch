using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
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
                if (ad != null && ad.Lock == true)
                {
                    Session["Admin"] = ad;
                    Session["userId"] = ad.IDNhanVien;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    if (ad != null && ad.Lock == false)
                        ViewBag.Thongbao = "Tài khoản đã bị khóa bởi Admin";
                    else
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
        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        public string GetPassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(FormCollection formCollection)
        {
            string username = formCollection["username"];

            var getAdmin = _data.NhanViens.First(x => x.Username == username);
            if (getAdmin == null)
            {
                ViewBag.ThongBao = "Không tìm thấy Username!!!";
                return View();
            }
            SmtpClient smtp = new SmtpClient();
            try
            {
                string newPassword = GetPassword();
                getAdmin.Password = newPassword;
                UpdateModel(getAdmin);
                _data.SubmitChanges();
                //ĐỊA CHỈ SMTP Server
                smtp.Host = "smtp.gmail.com";
                //Cổng SMTP
                smtp.Port = 587;
                //SMTP yêu cầu mã hóa dữ liệu theo SSL
                smtp.EnableSsl = true;
                //UserName và Password của mail
                smtp.Credentials = new NetworkCredential("chandinhnui@gmail.com", "dknight2112");

                //Tham số lần lượt là địa chỉ người gửi, người nhận, tiêu đề và nội dung thư
                smtp.Send("chandinhnui@gmail.com", getAdmin.Email, "Cấp lại mật khẩu", "Mật khẩu mới của tài khoản là: " + newPassword );
                ViewBag.ThongBao = "Gửi thành công!!!";
            }
            catch (Exception ex)
            {
               // lbStatus.Text = ex.Message;
            }
            return View();
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
            int selectNumberTo = number;
            int selectNumberGo = number2;
            var statistical = new SuggestNumberText()
            {
                CountTextTo = countTextTo,
                CountTextGo = countTextGo,
                SUM = countTextTo + countTextGo,
                NumberTo = selectNumberTo,
                NumberGo = selectNumberGo
            };
            

            return PartialView(statistical);
            
        }
    }
}