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
            int countTextTo = _data.CongVanDens.Count();
            int countTextGo = _data.CongVanDis.Count();
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
            DateTime dateFirst =DateTime.Parse(formCollection["inputDayFirst"]);
            DateTime dateLast = DateTime.Parse(formCollection["inputDayLast"]);

            Session["dateFirst"] = dateFirst;
            Session["dateLast"] = dateLast;

            int countTextTo = _data.CongVanDens.Where(x=>x.NgayNhan >= dateFirst && x.NgayNhan<=dateLast).Count();
            int countTextGo = _data.CongVanDis.Where(x => x.NgayGui >= dateFirst && x.NgayGui <= dateLast).Count();

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
            DateTime dateFirst = DateTime.Parse(Session["dateFirst"].ToString());
            DateTime dateLast = DateTime.Parse(Session["dateLast"].ToString());

            var getListTextTo = _data.CongVanDens.Where(x => x.NgayNhan >= dateFirst && x.NgayNhan <= dateLast).ToList();

            return PartialView(getListTextTo);
        }

        public ActionResult CountTextGo()
        {
            DateTime dateFirst = DateTime.Parse(Session["dateFirst"].ToString());
            DateTime dateLast = DateTime.Parse(Session["dateLast"].ToString());

            var getListTextGo = _data.CongVanDis.Where(x => x.NgayGui >= dateFirst && x.NgayGui <= dateLast).ToList();

            return PartialView(getListTextGo);
        }
    }
}