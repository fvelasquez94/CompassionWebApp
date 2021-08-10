using CompassionWebApp.Helpers.Session;
using CompassionWebApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace CompassionWebApp.Controllers
{
    public class AuthController : Controller
    {
        private CompassionDBEntities db = new CompassionDBEntities();
        private Cls_cookies cls_cookies = new Cls_cookies();
        public ActionResult Log_out()
        {
            Session.RemoveAll();
            //Global_variables.active_user.Name = null;
            //Global_variables.active_Departments = null;
            //Global_variables.active_Roles = null;
            if (Request.Cookies["correo"] != null)
            {
                var c = new HttpCookie("correo");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }
            if (Request.Cookies["pass"] != null)
            {
                var c = new HttpCookie("pass");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }
            if (Request.Cookies["rememberme"] != null)
            {
                var c = new HttpCookie("rememberme");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }



            return RedirectToAction("Login", "Auth", new { access = true, logpage = 2 });
        }

        public ActionResult Login(bool access = true, int? logpage = 0)
        {
            if (access == false)
            {
                if (logpage == 0)
                {
                    TempData["advertencia"] = "Sesion expirada.";
                }
                else if (logpage == 1)
                {
                    TempData["advertencia"] = "Correo o contrasena incorrectas.";
                }

            }

            HttpCookie aCookieCorreo = Request.Cookies["correo"];
            HttpCookie aCookiePassword = Request.Cookies["pass"];
            HttpCookie aCookieRememberme = Request.Cookies["rememberme"];

            try
            {
                var correo = Server.HtmlEncode(aCookieCorreo.Value).ToString();
                var pass = Server.HtmlEncode(aCookiePassword.Value).ToString();
                int remember = Convert.ToInt32(Server.HtmlEncode(aCookieRememberme.Value));

                if (remember == 1) { ViewBag.remember = true; } else { ViewBag.remember = false; }
                ViewBag.correo = correo;
                ViewBag.pass = pass;

            }
            catch
            {
                ViewBag.remember = false;

            }
            return View();
        }

        public ActionResult Log_in(string email, string password, bool? rememberme = true)
        {
            if (rememberme == null) { rememberme = false; }
            Session["activeUser"] = (from a in db.Tb_Usuarios where (a.Email == email && a.Password == password && a.Active == true) select a).FirstOrDefault();
            if (Session["activeUser"] != null)
            {
                //Validamos cookies
                cls_cookies.Check_cookies(email, password, rememberme);


                //encriptamos conexion
                try
                {
                    //Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
                    //ConfigurationSection section = config.GetSection("connectionStrings");
                    //if (!section.SectionInformation.IsProtected)
                    //{
                    //    section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                    //    config.Save();
                    //}



                    //para desencriptar
                    Configuration config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
                    ConfigurationSection section = config.GetSection("connectionStrings");
                    if (section.SectionInformation.IsProtected)
                    {
                        section.SectionInformation.UnprotectSection();
                        config.Save();
                    }


                    //obtenemos informacion de loggin
                    //host name
                    var s = Request.ServerVariables["REMOTE_HOST"]; 
                    //computer name
                    var ss = Request.ServerVariables["SERVER_NAME"];
                    //Windows account del usuario.
                     var sss = Request.ServerVariables["AUTH_USER"];

                    //IP
                    string ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    var ipcliente = GetIp();
                }
                catch (Exception ex){
                }
      

                return RedirectToAction("Inicio", "Main", null);


            }

            return RedirectToAction("Login", "Auth", new { access = false, logpage = 1 });
        }

        public string GetIp()
        {
            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }
    }

    
}