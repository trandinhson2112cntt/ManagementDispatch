using ManagementDispatch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManagementDispatch.Controllers
{
    public class AgencyController : Controller
    {
        DataBaseDataContext _data = new DataBaseDataContext();
        // GET All: Agency
        public ActionResult Index()
        {
            var getAllAgency = _data.DonVis.ToList();
            return View(getAllAgency);
        }

        [HttpGet]
        public ActionResult AddAgency()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddAgency(DonVi item, FormCollection formCollection)
        {
            try
            {
                ViewBag.Validate = " ";
                if (formCollection["InputName"] == "")
                {
                    ViewBag.Validate = "Không được bỏ trống tên đơn vị";
                    return View();
                }
                else
                {
                    item.TenDonVi = formCollection["InputName"];
                    item.ThongTin = formCollection["InputInfo"];
                    item.isDelete = true;
                    _data.DonVis.InsertOnSubmit(item);
                    _data.SubmitChanges();

                    string writeLog = "Thêm đơn vị mới: " + item.TenDonVi;
                    NhanVien admin = (NhanVien)Session["Admin"];
                    NhatKyHeThong log = new NhatKyHeThong();
                    log.IDNhanVien = admin.IDNhanVien;
                    log.NoiDungNhatKy = writeLog;
                    log.NgayGio = DateTime.Now;
                    _data.NhatKyHeThongs.InsertOnSubmit(log);
                    _data.SubmitChanges();

                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult EditAgency(int id)
        {
            DonVi getAgency = _data.DonVis.SingleOrDefault(n => n.IDDonVi == id);
            return View(getAgency);
        }
        [HttpPost]
        public ActionResult EditAgency(int id, FormCollection formCollection)
        {
            try
            {
                DonVi getAgency = _data.DonVis.SingleOrDefault(n => n.IDDonVi == id);
                string writeLog = "Sửa đơn vị: " + getAgency.TenDonVi;

                getAgency.TenDonVi = formCollection["InputName"];
                getAgency.ThongTin = formCollection["InputInfo"];
                UpdateModel(getAgency);
                _data.SubmitChanges();

                writeLog = writeLog + "  =>  " + getAgency.TenDonVi;
                NhanVien admin = (NhanVien)Session["Admin"];
                NhatKyHeThong log = new NhatKyHeThong();
                log.IDNhanVien = admin.IDNhanVien;
                log.NoiDungNhatKy = writeLog;
                log.NgayGio = DateTime.Now;
                _data.NhatKyHeThongs.InsertOnSubmit(log);
                _data.SubmitChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }

        }

        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var getAgency = _data.DonVis.First(c => c.IDDonVi == id);
            getAgency.isDelete = !getAgency.isDelete;
            UpdateModel(getAgency);
            _data.SubmitChanges();
            var result = getAgency.isDelete;
            return Json(new
            {
                status = result
            });

        }
    }
}