using ManagementDispatch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManagementDispatch.Controllers
{
    public class DepartmentController : Controller
    {
        // GET: Department
        DataBaseDataContext _data = new DataBaseDataContext();
        public ActionResult Index()
        {
            var getAllDepartment = _data.PhongBans.ToList();
            return View(getAllDepartment);
        }

        [HttpGet]
        public ActionResult AddDepartment()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddDepartment(PhongBan item, FormCollection formCollection)
        {
            try
            {
                ViewBag.Validate = " ";
                if (formCollection["InputName"] == "")
                {
                    ViewBag.Validate = "Không được bỏ trống tên phòng ban";
                    return View();
                }
                else
                {
                    item.TenPhongBan = formCollection["InputName"];
                    item.GhiChu = formCollection["InputInfo"];
                    item.isDelete = true;
                    _data.PhongBans.InsertOnSubmit(item);
                    _data.SubmitChanges();

                    string writeLog = "Thêm phòng ban mới: " + item.TenPhongBan;
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
        public ActionResult EditDepartment(int id)
        {
            PhongBan getDepartment = _data.PhongBans.SingleOrDefault(n => n.IDPhongBan == id);
            return View(getDepartment);
        }
        [HttpPost]
        public ActionResult EditDepartment(int id, FormCollection formCollection)
        {
            try
            {
                PhongBan getDepartment = _data.PhongBans.SingleOrDefault(n => n.IDPhongBan == id);
                string writeLog = "Sửa đơn vị: " + getDepartment.TenPhongBan;
                getDepartment.TenPhongBan = formCollection["InputName"];
                getDepartment.GhiChu = formCollection["InputInfo"];
                UpdateModel(getDepartment);
                _data.SubmitChanges();

                writeLog = writeLog + "  =>  " + getDepartment.TenPhongBan;
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
            var getDepartment = _data.PhongBans.First(c => c.IDPhongBan == id);
            getDepartment.isDelete = !getDepartment.isDelete;
            UpdateModel(getDepartment);
            _data.SubmitChanges();
            var result = getDepartment.isDelete;
            return Json(new
            {
                status = result
            });

        }
    }
}