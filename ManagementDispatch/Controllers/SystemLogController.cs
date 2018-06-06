using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManagementDispatch.Models;
namespace ManagementDispatch.Controllers
{

    [AuthorzireBusiness]
    public class SystemLogController : Controller
    {
        DataBaseDataContext _data = new DataBaseDataContext();
        // GET: SystemLog
        public ActionResult Index()
        {
            var getAllLog = _data.NhatKyHeThongs.OrderByDescending(x=>x.NgayGio).ToList();
            return View(getAllLog);
        }

    }
}