using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManagementDispatch.Models
{
    public class AuthorzireBusiness : ActionFilterAttribute
    {
        
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (HttpContext.Current.Session["Admin"] == null)
            {
                filterContext.Result = new RedirectResult("/Home/LoginAdmin");
                return;
            }
            int userId = int.Parse(HttpContext.Current.Session["userId"].ToString());
            string actionName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + "Controller-" + filterContext.ActionDescriptor.ActionName;
            DataBaseDataContext _data = new DataBaseDataContext();
            string username = "admin";
            var admin = _data.NhanViens.SingleOrDefault(a => a.IDNhanVien == userId && a.Username == username);
            if(admin != null)
            {
                return;
            }
            var listpermission = from p in _data.BlogPermissions
                                 join g in _data.GrantPermissions on p.PermissionId equals g.PermissionId
                                 where g.IDNhanVien == userId
                                 select p.PermissionName;
            if (!listpermission.Contains(actionName))
            {
                filterContext.Result = new RedirectResult("/Home/NotificationAuthorize/");
                return;
            }


            base.OnActionExecuted(filterContext);
        }
    }
}