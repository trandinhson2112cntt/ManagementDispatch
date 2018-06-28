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
            ViewBag.IDLoaiCongVan = new SelectList(_data.LoaiCongVans.ToList().OrderBy(n => n.IDLoaiCongVan), "IDLoaiCongVan", "TenLoaiCongVan");
            ViewBag.IDDonViGui = new SelectList(_data.DonVis.ToList().OrderBy(n => n.IDDonVi ), "IDDonVi", "TenDonVi");
            ViewBag.IDDonViNhan = new SelectList(_data.DonVis.ToList().OrderBy(n => n.IDDonVi ), "IDDonVi", "TenDonVi");
            ViewBag.IDPhongBan = new SelectList(_data.PhongBans.ToList().OrderBy(n => n.IDPhongBan ), "IDPhongBan", "TenPhongBan");
            return View();
        }



        [HttpPost]
        public ActionResult AddTextGo(CongVanDi item, FormCollection formCollection, HttpPostedFileBase uploadFile)
        {
            ViewBag.IDLoaiCongVan = new SelectList(_data.LoaiCongVans.ToList().OrderBy(n => n.IDLoaiCongVan ), "IDLoaiCongVan", "TenLoaiCongVan");
            ViewBag.IDDonViGui = new SelectList(_data.DonVis.ToList().OrderBy(n => n.IDDonVi ), "IDDonVi", "TenDonVi");
            ViewBag.IDDonViNhan = new SelectList(_data.DonVis.ToList().OrderBy(n => n.IDDonVi ), "IDDonVi", "TenDonVi");
            ViewBag.IDPhongBan = new SelectList(_data.PhongBans.ToList().OrderBy(n => n.IDPhongBan ), "IDPhongBan", "TenPhongBan");
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
        public ActionResult EditTextGo(int id)
        {
            ViewBag.IDLoaiCongVan = new SelectList(_data.LoaiCongVans.ToList().OrderBy(n => n.TenLoaiCongVan), "IDLoaiCongVan", "TenLoaiCongVan");
            ViewBag.IDPhongBan = new SelectList(_data.PhongBans.ToList().OrderBy(n => n.TenPhongBan), "IDPhongBan", "TenPhongBan");
            ViewBag.IDDonViGui = new SelectList(_data.DonVis.ToList().OrderBy(n => n.TenDonVi), "IDDonVi", "TenDonVi");
            ViewBag.IDDonViNhan = new SelectList(_data.DonVis.ToList().OrderBy(n => n.TenDonVi), "IDDonVi", "TenDonVi");

            CongVanDi getTextGo = _data.CongVanDis.SingleOrDefault(c => c.STT == id);
            return View(getTextGo);
        }
        [HttpPost]
        public ActionResult EditTextGo(int id, FormCollection formCollection, HttpPostedFileBase uploadFile)
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
        public bool DeleteText(int id)
        {
            try
            {
                var getTextGo = _data.CongVanDis.First(x => x.STT == id);
                string fullPath = Request.MapPath(getTextGo.File);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                string fullPath2 = Request.MapPath(getTextGo.AnhScan);
                if (System.IO.File.Exists(fullPath2))
                {
                    System.IO.File.Delete(fullPath2);
                }
                _data.CongVanDis.DeleteOnSubmit(getTextGo);
                _data.SubmitChanges();

                string writeLog = "Xoá công văn đi: " + getTextGo.IDCongVanDi;
                NhanVien admin = (NhanVien)Session["Admin"];
                NhatKyHeThong log = new NhatKyHeThong();
                log.IDNhanVien = admin.IDNhanVien;
                log.NoiDungNhatKy = writeLog;
                log.NgayGio = DateTime.Now;

                _data.NhatKyHeThongs.InsertOnSubmit(log);
                _data.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public ActionResult Delete(int id)
        {
            DeleteText(id);
            return RedirectToAction("Index");
        }
    }
}