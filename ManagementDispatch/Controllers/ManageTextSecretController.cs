using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManagementDispatch.Models;
namespace ManagementDispatch.Controllers
{
    [AuthorzireBusiness]
    public class ManageTextSecretController : Controller
    {
        DataBaseDataContext _data = new DataBaseDataContext();
        // GET: ManageTextSecret
       public ActionResult Index()
        {
            return View();
        }

        public ActionResult TextToSecret()
        {
            var getListTextToSecret = _data.CongVanDens.Where(x => x.BaoMat == true).ToList();

            return PartialView(getListTextToSecret);
        }

        public ActionResult TextGoSecret()
        {
            var getListTextGoSecret = _data.CongVanDis.Where(x => x.BaoMat == true).ToList();

            return PartialView(getListTextGoSecret);
        }
        [HttpGet]
        public ActionResult EditTextToSecret(int id)
        {
            ViewBag.IDLoaiCongVan = new SelectList(_data.LoaiCongVans.ToList().OrderBy(n => n.TenLoaiCongVan), "IDLoaiCongVan", "TenLoaiCongVan");
            ViewBag.IDPhongBan = new SelectList(_data.PhongBans.ToList().OrderBy(n => n.TenPhongBan), "IDPhongBan", "TenPhongBan");
            ViewBag.IDDonViGui = new SelectList(_data.DonVis.ToList().OrderBy(n => n.TenDonVi), "IDDonVi", "TenDonVi");
            ViewBag.IDDonViNhan = new SelectList(_data.DonVis.ToList().OrderBy(n => n.TenDonVi), "IDDonVi", "TenDonVi");

            CongVanDen getTextTo = _data.CongVanDens.SingleOrDefault(c => c.STT == id);
            return View(getTextTo);
        }
        [HttpPost]
        public ActionResult EditTextToSecret(int id, FormCollection formCollection, HttpPostedFileBase uploadFile)
        {
            CongVanDen getTextTo = _data.CongVanDens.SingleOrDefault(c => c.STT == id);

            if (ModelState.IsValid)
            {
                    if (uploadFile != null)
                    {
                        //Luu ten file
                        var fileName = Path.GetFileName(uploadFile.FileName);
                        //Luu duong dan File
                        var path = Path.Combine(Server.MapPath("~/FileDocument"), fileName);
                        //Kiem tra hinh da ton tai chua\
                        if (System.IO.File.Exists(path))
                            ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        else
                            uploadFile.SaveAs(path);//Luu file vao duong dan

                        getTextTo.File = fileName;
                    }

            }
            int idLoaiCongVan = int.Parse(formCollection["IDLoaiCongVan"]);
            int idPhongBan = int.Parse(formCollection["IDPhongBan"]);
            int idDonViGui = int.Parse(formCollection["IDDonViGui"]);
            int idDonViNhan = int.Parse(formCollection["IDDonViNhan"]);
            string noiDung = formCollection["NoiDung"];
            DateTime ngayGui = DateTime.Parse(formCollection["NgayGui"]);
            DateTime ngayNhan = DateTime.Parse(formCollection["NgayNhan"]);
            string tenNguoiGui = formCollection["TenNguoiGui"];
            string tenNguoiNhan = formCollection["TenNguoiNhan"];
            string anhScan = formCollection["AnhScan"];

            getTextTo.IDLoaiCongVan = idLoaiCongVan;
            getTextTo.NoiDung = noiDung;
            getTextTo.NgayGui = ngayGui;
            getTextTo.NgayNhan = ngayNhan;
            getTextTo.TenNguoiGui = tenNguoiGui;
            getTextTo.IDPhongBan = idPhongBan;
            getTextTo.IDDonViGui = idDonViGui;
            getTextTo.IDDonViNhan = idDonViNhan;
            getTextTo.TenNguoiNhan = tenNguoiNhan;
            getTextTo.AnhScan = anhScan;

            UpdateModel(getTextTo);
            _data.SubmitChanges();
            string writeLog = "Sửa công văn đi: " + getTextTo.IDCongVanDen;
            NhanVien admin = (NhanVien)Session["Admin"];
            NhatKyHeThong log = new NhatKyHeThong();
            log.IDNhanVien = admin.IDNhanVien;
            log.NoiDungNhatKy = writeLog;
            log.NgayGio = DateTime.Now;
            _data.NhatKyHeThongs.InsertOnSubmit(log);
            _data.SubmitChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditTextGoSecret(int id)
        {
            ViewBag.IDLoaiCongVan = new SelectList(_data.LoaiCongVans.ToList().OrderBy(n => n.TenLoaiCongVan), "IDLoaiCongVan", "TenLoaiCongVan");
            ViewBag.IDPhongBan = new SelectList(_data.PhongBans.ToList().OrderBy(n => n.TenPhongBan), "IDPhongBan", "TenPhongBan");
            ViewBag.IDDonViGui = new SelectList(_data.DonVis.ToList().OrderBy(n => n.TenDonVi), "IDDonVi", "TenDonVi");
            ViewBag.IDDonViNhan = new SelectList(_data.DonVis.ToList().OrderBy(n => n.TenDonVi), "IDDonVi", "TenDonVi");

            CongVanDi getTextGo = _data.CongVanDis.SingleOrDefault(c => c.STT == id);
            return View(getTextGo);
        }
        [HttpPost]
        public ActionResult EditTextGoSecret(int id, FormCollection formCollection, HttpPostedFileBase uploadFile)
        {
            CongVanDi getTextGo = _data.CongVanDis.SingleOrDefault(c => c.STT == id);

            if (ModelState.IsValid)
            {
                if (uploadFile != null)
                {
                    //Luu ten file
                    var fileName = Path.GetFileName(uploadFile.FileName);
                    //Luu duong dan File
                    var path = Path.Combine(Server.MapPath("~/FileDocument"), fileName);
                    //Kiem tra hinh da ton tai chua\
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                        uploadFile.SaveAs(path);//Luu file vao duong dan

                    getTextGo.File = fileName;
                }
            }
            int idLoaiCongVan = int.Parse(formCollection["IDLoaiCongVan"]);
            int idDonViGui = int.Parse(formCollection["IDDonViGui"]);
            string noiDung = formCollection["NoiDungCongViec"];
            DateTime ngayGui = DateTime.Parse(formCollection["NgayGui"]);
            string tenNguoiGui = formCollection["TenNguoiGui"];
            string anhScan = formCollection["AnhScan"];

            getTextGo.IDLoaiCongVan = idLoaiCongVan;
            getTextGo.NoiDungCongViec = noiDung;
            getTextGo.NgayGui = ngayGui;
            getTextGo.TenNguoiGui = tenNguoiGui;
            getTextGo.IDDonViGui = idDonViGui;
            getTextGo.AnhScan = anhScan;

            UpdateModel(getTextGo);
            _data.SubmitChanges();

            string writeLog = "Sửa công văn đi: " + getTextGo.IDCongVanDi;
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
}