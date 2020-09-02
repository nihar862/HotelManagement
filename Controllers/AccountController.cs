using Hotel_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Hotel_Management.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginAuth model)
        {
            using (var context = new HotelEntities())
            {
                bool isValid = context.Userdatas.Any(x=> x.Username == model.Username && x.Password == model.Password);
                if (isValid)
                {
                    FormsAuthentication.SetAuthCookie(model.Username, false);


                    if (model.Username == "Admin" && model.Password == "admin")
                    {
                        Session["id"] = "2";
                        Session["name"] = "Admin";
                        return RedirectToAction("Index", "Userdatas");
                    }
                    else
                    {
                        Userdata user = context.Userdatas.FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);
                        Session["id"] = user.Id;
                        Session["email"] = user.Email;
                        Session["name"] = user.Username;
                        return RedirectToAction("Home", "Userdatas");
                    }
                }
                ModelState.AddModelError("","Username or Passowrd is incorrect !!");
                return View();
            }
        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(Userdata model)
        {
            using (var context = new HotelEntities())
            {
                bool isValid = context.Userdatas.Any(x => x.Username == model.Username);
                if (!isValid)
                {
                    context.Userdatas.Add(model);
                    context.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("", "Username is Already Taken , Please try Different Username ");
                    return View();
                }
            }
            return RedirectToAction("Login");
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}