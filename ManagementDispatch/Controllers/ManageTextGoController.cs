using ManagementDispatch.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManagementDispatch.Controllers
{
    public class ManageTextGoController : Controller
    {
        // GET: Text
        DataBaseDataContext _data = new DataBaseDataContext();
        public ActionResult Index()
        {
            var getAllTextTo = _data.CongVanDis.Where(c => c.IDLoaiCongVan != 1).ToList();
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
                //Luu duong dan File
                var path = Path.Combine(Server.MapPath("~/FileDocument"), fileName);
                if (System.IO.File.Exists(path))
                    ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                else
                    uploadFile.SaveAs(path);//Luu file vao duong dan
                item.File = fileName;
                item.NgayGui = DateTime.Parse(formCollection["NgayGui"]);
                item.ThoiHanHoanThanh = DateTime.Parse(formCollection["ThoiHanHoanThanh"]);
                _data.CongVanDis.InsertOnSubmit(item);
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
            }else if (Path.GetExtension(fileName) == ".doc")
            {
                contentType = "application/msword";
            }else if (Path.GetExtension(fileName) == ".docx")
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

            CongVanDi getTextTo = _data.CongVanDis.SingleOrDefault(c => c.IDCongVanDi == id);
            return View(getTextTo);
        }
        [HttpPost]
        public ActionResult EditTextGo(string id, FormCollection formCollection, HttpPostedFileBase uploadFile)
        {
            CongVanDi getTextTo = _data.CongVanDis.SingleOrDefault(c => c.IDCongVanDi == id);


            if (ModelState.IsValid)
            {

                try
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
                catch (Exception)
                {
                    ViewBag.Thongbao = "Vui lòng chọn ảnh cho sản phẩm";
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

            getTextTo.IDLoaiCongVan = idLoaiCongVan;
            getTextTo.NoiDungCongViec = noiDung;
            getTextTo.NgayGui = ngayGui;
            getTextTo.ThoiHanHoanThanh = ngayNhan;
            getTextTo.TenNguoiGui = tenNguoiGui;
            getTextTo.IDDonViGui = idDonViGui;
            getTextTo.IDDonViNhan = idDonViNhan;
            getTextTo.AnhScan = anhScan;

            UpdateModel(getTextTo);
            _data.SubmitChanges();

            return RedirectToAction("Index");
        }
    }
}