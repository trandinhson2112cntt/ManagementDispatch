using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManagementDispatch.Models;
namespace ManagementDispatch.Controllers
{
    [AuthorzireBusiness]
    public class StatisticalController : Controller
    {
        DataBaseDataContext _data = new DataBaseDataContext();
        // GET: Statistical
        public ActionResult Index()
        {
            ViewBag.IDLoaiCongVan = new SelectList(_data.LoaiCongVans.ToList().OrderBy(n => n.IDLoaiCongVan), "IDLoaiCongVan", "TenLoaiCongVan");
            ViewBag.IDPhongBan = new SelectList(_data.PhongBans.ToList().OrderBy(n => n.IDPhongBan), "IDPhongBan", "TenPhongBan");
            ViewBag.idDonViGui = new SelectList(_data.DonVis.ToList().OrderBy(n => n.IDDonVi), "IDDonVi", "TenDonVi");

            int countTextTo = 0, countTextGo = 0;
            countTextTo = _data.CongVanDens.Count();
            countTextGo = _data.CongVanDis.Count();
            var statistical = new Statistical()
            {
                CountTextTo = countTextTo,
                CountTextGo = countTextGo,
                SUM = countTextTo + countTextGo
            };
            return View(statistical);
        }
        [HttpPost]
        public ActionResult CountText(FormCollection formCollection)
        {
            string dateFirst = null;
            if (formCollection["inputDayFirst"] != "")
                dateFirst = formCollection["inputDayFirst"];
            string dateLast = null;
            if (formCollection["inputDayLast"] != "")
                dateLast = formCollection["inputDayLast"];
            Session["dateFirst"] = dateFirst;
            Session["dateLast"] = dateLast;

            string idLoaiCongVan = null;
            if (formCollection["IDLoaiCongVan"] != "")
                idLoaiCongVan  = formCollection["IDLoaiCongVan"];
            string idPhongBan = null;
            if (formCollection["IDPhongBan"] != "")
                idPhongBan = formCollection["IDPhongBan"];
            string idDonViGui = null;
            if (formCollection["IDDonViGui"] != "")
                idDonViGui = formCollection["IDDonViGui"];
            Session["IDLoaiCongVan"] = idLoaiCongVan;
            Session["IDPhongBan"] = idPhongBan;
            Session["IDDonViGui"] = idDonViGui;

            int countTextGo, countTextTo;
            if((dateFirst==null && dateLast !=null) || (dateFirst != null && dateLast == null))
            {
                var script = @"alert(""Vui lòng chọn đầy đủ thông tin ngày!!!"");";
                return JavaScript(script);
            }
            if (dateFirst == null && dateLast == null)
            {
                countTextGo = _data.TimKiemCongVanDi_2(idPhongBan, idDonViGui, idLoaiCongVan).Count();
                countTextTo = _data.TimKiemCongVanDen_2(idPhongBan, idDonViGui, idLoaiCongVan).Count();
            }
            else
            {
                countTextGo = _data.TimKiemCongVanDi(DateTime.Parse(dateFirst), DateTime.Parse(dateLast), idPhongBan, idDonViGui, idLoaiCongVan).Count();
                countTextTo = _data.TimKiemCongVanDen(DateTime.Parse(dateFirst), DateTime.Parse(dateLast), idPhongBan, idDonViGui, idLoaiCongVan).Count();
            }
           
            var statistical = new Statistical()
            {
                CountTextTo = countTextTo,
                CountTextGo = countTextGo,
                SUM = countTextTo + countTextGo
            };
            return View(statistical);
        }

        public ActionResult CountTextTo()
        {
            string dateFirst = null;
            if (Session["inputDayFirst"] != null)
                dateFirst = Session["inputDayFirst"].ToString();
            string dateLast = null;
            if (Session["inputDayFirst"] != null)
                dateLast = Session["inputDayLast"].ToString();
            string idPhongBan = null;
            string idLoaiCongVan = null;
            string idDonViGui = null;
            if(Session["IDPhongBan"] != null)
                idPhongBan =Session["IDPhongBan"].ToString();
            if (Session["IDLoaiCongVan"] != null)
                idLoaiCongVan = Session["IDLoaiCongVan"].ToString();
            if (Session["IDDonViGui"] != null)
                idDonViGui = Session["IDDonViGui"].ToString();
            
            if (dateFirst == null && dateLast == null)
            {
               var getListTextTo = _data.TimKiemCongVanDen_2(idPhongBan, idDonViGui, idLoaiCongVan).ToList();
              
               return PartialView(getListTextTo);
            }
            else
            {
               var getListTextTo = _data.TimKiemCongVanDen(DateTime.Parse(dateFirst), DateTime.Parse(dateLast), idPhongBan, idDonViGui, idLoaiCongVan).ToList();
               return PartialView(getListTextTo);
            }

           // var getListTextTo = _data.TimKiemCongVanDen(dateFirst, dateLast, idPhongBan, idDonViGui, idLoaiCongVan);
            
        }

        public ActionResult CountTextGo()
        {
            string dateFirst = null;
            if (Session["inputDayFirst"] != null)
                dateFirst = Session["inputDayFirst"].ToString();
            string dateLast = null;
            if (Session["inputDayFirst"] != null)
                dateLast = Session["inputDayLast"].ToString();
            string idPhongBan = null;
            string idLoaiCongVan = null;
            string idDonViGui = null;
            if (Session["IDPhongBan"] != null)
                idPhongBan = Session["IDPhongBan"].ToString();
            if (Session["IDLoaiCongVan"] != null)
                idLoaiCongVan = Session["IDLoaiCongVan"].ToString();
            if (Session["IDDonViGui"] != null)
                idDonViGui = Session["IDDonViGui"].ToString();

            if (dateFirst == null && dateLast == null)
            {
                var getListTextGo = _data.TimKiemCongVanDi_2(idPhongBan, idDonViGui, idLoaiCongVan).ToList();
                return PartialView(getListTextGo);
            }
            else
            {
                var getListTextGo = _data.TimKiemCongVanDi(DateTime.Parse(dateFirst), DateTime.Parse(dateLast), idPhongBan, idDonViGui, idLoaiCongVan).ToList();
                return PartialView(getListTextGo);
            }
        }
    }
}