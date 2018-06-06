using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ManagementDispatch.Models;
namespace ManagementDispatch.Controllers
{
    [AuthorzireBusiness]
    public class BlogGrantPermissionController : Controller
    {
        DataBaseDataContext _data = new DataBaseDataContext();
        // GET: BlogGrantPermission
        public ActionResult Index(int id)
        {
            var listControl = _data.BlogBusinesses.AsEnumerable();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in listControl)
            {
                items.Add(new SelectListItem() { Text = item.BusinessName, Value = item.BusinessId });
            }
            ViewBag.items = items;

            var listgranted = from g in _data.GrantPermissions
                              join p in _data.BlogPermissions on g.PermissionId equals p.PermissionId
                              where g.IDNhanVien == id
                              select new SelectListItem() { Value = p.PermissionId.ToString(), Text = p.Description };
            ViewBag.listgranted = listgranted;

            Session["usergrant"] = id;
            var usergrant = _data.NhanViens.SingleOrDefault(x => x.IDNhanVien == id);
            ViewBag.usergrant = usergrant.Username + "(" + usergrant.HoTen + ")";
            return View();
        }
        [HttpGet]
        public JsonResult getPermission(string id, int usertemp)
        {
            var listgranted = (from g in _data.GrantPermissions
                               join p in _data.BlogPermissions on g.PermissionId equals p.PermissionId
                               where g.IDNhanVien == usertemp && p.BusinessId == id
                               select new PermissionAction { PermissionId = p.PermissionId, PermissionName = p.PermissionName, Description = p.Description, IsGranted = true }).ToList();
            var listpermission = from p in _data.BlogPermissions
                                 where p.BusinessId == id
                                 select new PermissionAction { PermissionId = p.PermissionId, PermissionName = p.PermissionName, Description = p.Description, IsGranted = false };
            var listpermissionId = listgranted.Select(p => p.PermissionId);
            foreach (var item in listpermission)
            {
                if (!listpermissionId.Contains(item.PermissionId))
                    listgranted.Add(item);
            }
            return Json(listgranted.OrderBy(x => x.Description), JsonRequestBehavior.AllowGet);
        }
        public string updatePermission(int id,int usertemp)
        {
            string msg = "";
            var grant = _data.GrantPermissions.SingleOrDefault(x => x.PermissionId == id && x.IDNhanVien == usertemp);
            if (grant == null)
            {
                GrantPermission g = new GrantPermission() { PermissionId = id, IDNhanVien = usertemp };
                _data.GrantPermissions.InsertOnSubmit(g);
                msg = "<div class='alert alert-success'>Cấp quyền thành công</div>";
            }
            else
            {
                _data.GrantPermissions.DeleteOnSubmit(grant);
                msg = "<div class='alert alert-danger'>Hủy quyền thành công</div>";
            }
            _data.SubmitChanges();
            return msg;
        }
    }
}