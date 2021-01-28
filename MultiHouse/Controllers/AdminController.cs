using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MultiHouse.Database;
using MultiHouse.Models;

namespace MultiHouse.Controllers
{
    public class AdminController : Controller
    {
        

        public IActionResult Index()
        {

            return View();
        }


        public IActionResult PasswordSetView()
        {
            return View();
        }
        
        public IActionResult PasswordSet([FromForm]PasswordData data)
        {
            if (PersonalData.AdminPassword == data.OldPassword)
            {
                PersonalData.AdminPassword = data.NewPassword;
            }

            ViewData["status"] = "setted new password";
            
            return View("PasswordSetView");
        }

        public IActionResult EmailSetView()
        {
            return View();
        }
        
        public IActionResult EmailSet([FromForm]EmailData data)
        {
            
            
            
            if (data.AdminPassword == PersonalData.AdminPassword)
            {
                PersonalData.AuthToken = Guid.NewGuid().ToString();
                PersonalData.EmailAddress = data.Email;
                PersonalData.EMailPassword = data.EmailPasword;

                ViewData["status"] = "email setted";
            }
            else
            {
                ViewData["status"] = "invalid admin password";
            }
            
            
            return View("EmailSetView");
        }
        
        
        
        
        
        
        public IActionResult Panel()
        {
            string password = Request.Form["password"];
            
            

            if (password == PersonalData.AdminPassword)
            {
                HttpContext.Response.Cookies.Append("auth_token",PersonalData.AuthToken);
                return View();
            }
            else
            {
                return View("Index");
            }
            
        }
        
    }
}