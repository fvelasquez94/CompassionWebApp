using CompassionWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompassionWebApp.Helpers.Session
{
    public class Cls_session
    {
        private CompassionDBEntities db = new CompassionDBEntities();
        public bool checkSession()
        {
            var flag = false;
            Tb_Usuarios activeuser = HttpContext.Current.Session["activeUser"] as Tb_Usuarios;
            if (activeuser != null)
            {
                flag = true;
            }
            else
            {
                if (HttpContext.Current.Request.Cookies["correo"] != null)
                {
                    //COMO YA EXISTE NO NECESITAMOS RECREARLA Y SOLO VOLVEMOS A INICIAR SESION
                    flag = true;
                    var email = HttpContext.Current.Request.Cookies["correo"].Value;
                    var password = HttpContext.Current.Request.Cookies["pass"].Value;
                    HttpContext.Current.Session["activeUser"] = (from a in db.Tb_Usuarios where (a.Email == email && a.Password == password && a.Active == true) select a).FirstOrDefault();
                    Tb_Usuarios activeuserAgain = HttpContext.Current.Session["activeUser"] as Tb_Usuarios;
                    if (activeuserAgain != null)
                    {
                        flag = true;
                    }
                    else { flag = false; }


                }
                else
                {
                    flag = false;
                }
            }
            return flag;
        }
    }
}