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
    public class ManageTextGoController : Controller
    {
        // GET: Text
        DataBaseDataContext _data = new DataBaseDataContext();
        public ActionResult Index()
        {
            var getAllTextTo = _data.CongVanDis.Where(c => c.BaoMat == false).ToList();
            return View(getAllTextTo);
        }

        [HttpGet]
        public ActionResult AddTextGo()
        {
            ViewBag.IDLoaiCongVan = new SelectList(_data.LoaiCongVans.ToList().OrderBy(n => n.TenLoaiCongVan), "IDLoaiCongVan", "TenLoaiCongVan");
            ViewBag.IDDonViGui = new SelectList(_data.DonVis.ToList().OrderBy(n => n.TenDonVi), "IDDonVi", "TenDonVi");
            ViewBag.IDDonViNhan = new SelectList(_data.DonVis.ToList().OrderBy(n => n.TenDonVi), "IDDonVi", "TenDonVi");
            ViewBag.IDCongVanDen = new SelectList(_data.CongVanDens.ToList().OrderBy(n => n.IDCongVanDen), "IDCongVanDen", "IDCongVanDen");

            return View();
        }



        [HttpPost]
        public ActionResult AddTextGo(CongVanDi item, FormCollection formCollection, HttpPostedFileBase uploadFile)
        {
            ViewBag.IDLoaiCongVan = new SelectList(_data.LoaiCongVans.ToList().OrderBy(n => n.TenLoaiCongVan), "IDLoaiCongVan", "TenLoaiCongVan");
            ViewBag.IDDonViGui = new SelectList(_data.DonVis.ToList().OrderBy(n => n.TenDonVi), "IDDonVi", "TenDonVi");
            ViewBag.IDDonViNhan = new SelectList(_data.DonVis.ToList().OrderBy(n => n.TenDonVi), "IDDonVi", "TenDonVi");
            ViewBag.IDCongVanDen = new SelectList(_data.CongVanDens.ToList().OrderBy(n => n.IDCongVanDen), "IDCongVanDen", "IDCongVanDen");

            try
            {
                ViewBag.Validate = " ";

                var fileName = Path.GetFileName(uploadFile.FileName);
                if (fileName != null)
                {
                    var path = Path.Combine(Server.MapPath("~/FileDocument"), fileName);
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Tập tin đã tồn tại";
                    else
                        uploadFile.SaveAs(path);//Luu file vao duong dan
                    item.File = fileName;
                }
                else
                {
                    item.File = "";
                }
                //Luu duong dan File

                item.NgayGui = DateTime.Parse(formCollection["NgayGui"]);
                item.ThoiHanHoanThanh = DateTime.Parse(formCollection["ThoiHanHoanThanh"]);
                _data.CongVanDis.InsertOnSubmit(item);
                _data.SubmitChanges();

                string writeLog = "Thêm công văn đi: " + item.IDCongVanDi;
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
        public ActionResult EditTextGo(string id)
        {
            ViewBag.IDLoaiCongVan = new SelectList(_data.LoaiCongVans.ToList().OrderBy(n => n.TenLoaiCongVan), "IDLoaiCongVan", "TenLoaiCongVan");
            ViewBag.IDPhongBan = new SelectList(_data.PhongBans.ToList().OrderBy(n => n.TenPhongBan), "IDPhongBan", "TenPhongBan");
            ViewBag.IDDonViGui = new SelectList(_data.DonVis.ToList().OrderBy(n => n.TenDonVi), "IDDonVi", "TenDonVi");
            ViewBag.IDDonViNhan = new SelectList(_data.DonVis.ToList().OrderBy(n => n.TenDonVi), "IDDonVi", "TenDonVi");
            ViewBag.IDCongVanDen = new SelectList(_data.CongVanDens.ToList().OrderBy(n => n.IDCongVanDen), "IDCongVanDen", "IDCongVanDen");

            CongVanDi getTextGo = _data.CongVanDis.SingleOrDefault(c => c.IDCongVanDi == id);
            return View(getTextGo);
        }
        [HttpPost]
        public ActionResult EditTextGo(string id, FormCollection formCollection, HttpPostedFileBase uploadFile)
        {
            CongVanDi getTextGo = _data.CongVanDis.SingleOrDefault(c => c.IDCongVanDi == id);

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
            int idDonViNhan = int.Parse(formCollection["IDDonViNhan"]);
            string noiDung = formCollection["NoiDungCongViec"];
            DateTime ngayGui = DateTime.Parse(formCollection["NgayGui"]);
            DateTime ngayNhan = DateTime.Parse(formCollection["ThoiHanHoanThanh"]);
            string tenNguoiGui = formCollection["TenNguoiGui"];
            string anhScan = formCollection["AnhScan"];

            getTextGo.IDLoaiCongVan = idLoaiCongVan;
            getTextGo.NoiDungCongViec = noiDung;
            getTextGo.NgayGui = ngayGui;
            getTextGo.ThoiHanHoanThanh = ngayNhan;
            getTextGo.TenNguoiGui = tenNguoiGui;
            getTextGo.IDDonViGui = idDonViGui;
            getTextGo.IDDonViNhan = idDonViNhan;
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