using ManagementDispatch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManagementDispatch.Controllers
{
    public class CategoryController : Controller
    {
        DataBaseDataContext _data = new DataBaseDataContext();
        // GET All: Category
        public ActionResult Index()
        {
            var getAllCategory = _data.LoaiCongVans.ToList();
            return View(getAllCategory);
        }

        [HttpGet]
        public ActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddCategory(LoaiCongVan item, FormCollection formCollection)
        {
            try
            {
                ViewBag.Validate = " ";
                if (formCollection["InputName"] == "")
                {
                    ViewBag.Validate = "Không được bỏ trống tên loaị công văn";
                    return View();
                }
                else
                {
                    item.TenLoaiCongVan = formCollection["InputName"];
                    item.isDelete = true;
                    _data.LoaiCongVans.InsertOnSubmit(item);
                    _data.SubmitChanges();


                    string writeLog = "Thêm loại văn bản mới: " + item.TenLoaiCongVan;
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
        public ActionResult EditCategory(int id)
        {
            LoaiCongVan getCategory = _data.LoaiCongVans.SingleOrDefault(n => n.IDLoaiCongVan == id);
            return View(getCategory);
        }
        [HttpPost]
        public ActionResult EditCategory(int id, FormCollection formCollection)
        {
            try
            {
                LoaiCongVan getCategory = _data.LoaiCongVans.SingleOrDefault(n => n.IDLoaiCongVan == id);
                string writeLog = "Sửa loại văn bản: " + getCategory.TenLoaiCongVan;
                getCategory.TenLoaiCongVan = formCollection["InputName"];
                UpdateModel(getCategory);
                _data.SubmitChanges();

                writeLog = writeLog + "  =>  " + getCategory.TenLoaiCongVan;
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
            var getCategory = _data.LoaiCongVans.First(c => c.IDLoaiCongVan == id);
            getCategory.isDelete = !getCategory.isDelete;
            UpdateModel(getCategory);
            _data.SubmitChanges();
            var result = getCategory.isDelete;
            return Json(new
            {
                status = result
            });

        }
    }
}