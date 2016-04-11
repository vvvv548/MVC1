using MVC1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVC1.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {        
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginViewModel data)
        {
            // 登入時清空所有 Session 資料
            Session.RemoveAll();
            string roles;
            int customerId;
            // 登入的密碼（以 SHA1 加密）
            
            if (CheckLogin(data,out roles, out customerId))
            {
                // 將管理者登入的 Cookie 設定成 Session Cookie
                bool isPersistent = false;

                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                  data.Account,
                  DateTime.Now,
                  DateTime.Now.AddMinutes(30),
                  isPersistent,
                  roles,
                  FormsAuthentication.FormsCookiePath);

                string encTicket = FormsAuthentication.Encrypt(ticket);

                // Create the cookie.
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                if (roles.Contains("sysadmin"))
                {
                    return RedirectToAction("Index", "Home");
                }

                var repo = RepositoryHelper.Get客戶資料Repository();

                return RedirectToAction("Edit", "客戶資料", new { id = customerId });
            }
            return View();
        }

        private bool CheckLogin(LoginViewModel data,out string roles, out int customerId)
        {
            customerId = -1;
            roles = string.Empty;

            // 驗證.
            客戶資料 客戶資料 = repoCustInfo.All(false).Where(c => c.帳號 == data.Account).SingleOrDefault();
            
            if (data.Account == "admin" && FormsAuthentication.HashPasswordForStoringInConfigFile(data.Password, "SHA1") == 客戶資料?.密碼)
            {
                roles = "sysadmin";
                return true;
            }

            if (FormsAuthentication.HashPasswordForStoringInConfigFile(data.Password, "SHA1") == 客戶資料?.密碼)
            {
                customerId = 客戶資料.Id;
                roles = "customer";
                return true;
            }
            return false;
        }

        [AllowAnonymous]

        public ActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(RegisterViewModel data)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", "客戶資料");

            }
            return View();
        }

        public ActionResult EditProfile()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EditProfile(EditProfileViewModel data)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index", "客戶資料");

            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "客戶資料");
        }

    }
}