using ManagementDispatch.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManagementDispatch.Controllers
{
    public class ManageTextToController : Controller
    {
        // GET: Text
        DataBaseDataContext _data = new DataBaseDataContext();
        public ActionResult Index()
        {
            var getAllTextTo = _data.CongVanDens.Where(c=>c.IDLoaiCongVan != 1).ToList();
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
            string fullPath = Path.Combine(Server.MapPath("~/FileDocument/"), fileName);
            return File(fullPath,contentType,fileName);
        }
    }
}