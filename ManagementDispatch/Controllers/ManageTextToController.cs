using ManagementDispatch.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManagementDispatch.Controllers
{
    [AuthorzireBusiness]
    public class ManageTextToController : Controller
    {
        // GET: Text
        DataBaseDataContext _data = new DataBaseDataContext();
        public ActionResult Index()
        {
            var getAllTextTo = _data.CongVanDens.Where(c => c.BaoMat==false).ToList();
            return View(getAllTextTo);
        }

        [HttpGet]
        public ActionResult AddTextTo()
        {
            ViewBag.IDLoaiCongVan = new SelectList(_data.LoaiCongVans.ToList().OrderBy(n => n.TenLoaiCongVan), "IDLoaiCongVan", "TenLoaiCongVan");
            ViewBag.IDPhongBan = new SelectList(_data.PhongBans.ToList().OrderBy(n => n.TenPhongBan), "IDPhongBan", "TenPhongBan");
            ViewBag.IDDonViGui = new SelectList(_data.DonVis.ToList().OrderBy(n => n.TenDonVi), "IDDonVi", "TenDonVi");
            ViewBag.IDDonViNhan = new SelectList(_data.DonVis.ToList().OrderBy(n => n.TenDonVi), "IDDonVi", "TenDonVi");

            return View();
        }



        [HttpPost]
        public ActionResult AddTextTo(CongVanDen item, FormCollection formCollection, HttpPostedFileBase uploadFile)
        {
            ViewBag.IDLoaiCongVan = new SelectList(_data.LoaiCongVans.ToList().OrderBy(n => n.TenLoaiCongVan), "IDLoaiCongVan", "TenLoaiCongVan");
            ViewBag.IDPhongBan = new SelectList(_data.PhongBans.ToList().OrderBy(n => n.TenPhongBan), "IDPhongBan", "TenPhongBan");
            ViewBag.IDDonViGui = new SelectList(_data.DonVis.ToList().OrderBy(n => n.TenDonVi), "IDDonVi", "TenDonVi");
            ViewBag.IDDonViNhan = new SelectList(_data.DonVis.ToList().OrderBy(n => n.TenDonVi), "IDDonVi", "TenDonVi");

            try
            {
                ViewBag.Validate = " ";

                var fileName = Path.GetFileName(uploadFile.FileName);
                //Luu duong dan File
                var path = Path.Combine(Server.MapPath("~/FileDocument"), fileName);
                if (System.IO.File.Exists(path))
                    ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                else
                    uploadFile.SaveAs(path);//Luu file vao duong dan
                item.File = fileName;
                item.NgayGui = DateTime.Parse(formCollection["NgayGui"]);
                item.NgayNhan = DateTime.Parse(formCollection["NgayNhan"]);

                _data.CongVanDens.InsertOnSubmit(item);
                _data.SubmitChanges();

                string writeLog = "Thêm công văn đến: " + item.IDCongVanDen;
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
        public FileResult Download(string fileName)
        {

            string contentType = string.Empty;
            if (Path.GetExtension(fileName) == ".txt")
            {
                contentType = "application/txt";
            }

            else if (Path.GetExtension(fileName) == ".pdf")
            {
                contentType = "application/pdf";
            }
            else if (Path.GetExtension(fileName) == ".doc")
            {
                contentType = "application/msword";
            }
            else if (Path.GetExtension(fileName) == ".docx")
            {
                contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            }
            else
            {
                contentType = "unknown/unknown";
            }
            string fullPath = Path.Combine(Server.MapPath("~/FileDocument/"), fileName);
            return File(fullPath, contentType, fileName);
        }

        [HttpGet]
        public ActionResult EditTextTo(string id)
        {
            ViewBag.IDLoaiCongVan = new SelectList(_data.LoaiCongVans.ToList().OrderBy(n => n.TenLoaiCongVan), "IDLoaiCongVan", "TenLoaiCongVan");
            ViewBag.IDPhongBan = new SelectList(_data.PhongBans.ToList().OrderBy(n => n.TenPhongBan), "IDPhongBan", "TenPhongBan");
            ViewBag.IDDonViGui = new SelectList(_data.DonVis.ToList().OrderBy(n => n.TenDonVi), "IDDonVi", "TenDonVi");
            ViewBag.IDDonViNhan = new SelectList(_data.DonVis.ToList().OrderBy(n => n.TenDonVi), "IDDonVi", "TenDonVi");

            CongVanDen getTextTo = _data.CongVanDens.SingleOrDefault(c => c.IDCongVanDen == id);
            return View(getTextTo);
        }
        [HttpPost]
        public ActionResult EditTextTo(string id, FormCollection formCollection, HttpPostedFileBase uploadFile)
        {
            CongVanDen getTextTo = _data.CongVanDens.SingleOrDefault(c => c.IDCongVanDen == id);
           
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

        public string ChangeImage(string id,string picture)
        {
            if(id==null)
            {
                return "Mã không tồn tại";
            }
            CongVanDen textTo = _data.CongVanDens.FirstOrDefault(x => x.IDCongVanDen == id);
            if (textTo == null) return "Mã không tồn tại";
            textTo.AnhScan = picture;
            UpdateModel(textTo);
            _data.SubmitChanges();
            return "";
        }
    }
}