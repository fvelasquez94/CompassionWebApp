using CompassionWebApp.Helpers.Session;
using CompassionWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CompassionWebApp.Controllers.App
{
    public class CDIController : Controller
    {
        private CompassionDBEntities db = new CompassionDBEntities();
        private Cls_session cls_session = new Cls_session();
        // GET: CDI
        public ActionResult Index()
        {
            if (cls_session.checkSession())
            {
                Tb_Usuarios activeuser = Session["activeUser"] as Tb_Usuarios;
                ViewBag.usuario = activeuser;
                //NOTIFICATIONS
                //List<Tb_Alerts> lstAlerts = (from a in db.Tb_Alerts where (a.ID_user == activeuser.ID_User && a.Active == true) select a).OrderByDescending(x => x.Date).Take(4).ToList();
                //ViewBag.notifications = lstAlerts;
                //FILTROS VARIABLES
                DateTime now = DateTime.Today;
                DateTime filtrostartdate;
                DateTime filtroenddate;
                ////filtros de fecha (MENSUAL)
                var sunday = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                var saturday = sunday.AddMonths(1).AddDays(-1);

                if (activeuser.FechaInicioPeriodos == null) { filtrostartdate = sunday; } else { filtrostartdate = Convert.ToDateTime(activeuser.FechaInicioPeriodos); }
                if (activeuser.FechaFinalPeriodos == null) { filtroenddate = saturday; } else { filtroenddate = Convert.ToDateTime(activeuser.FechaFinalPeriodos); }

                ViewBag.filtrofechastart = filtrostartdate.ToShortDateString();
                ViewBag.filtrofechaend = filtroenddate.ToShortDateString();

                List<Tb_CDI> lstCDI = new List<Tb_CDI>();

                if (activeuser.Roles.Contains("CDI"))
                {
                    return RedirectToAction("CDI", "CDI");
                }
                else {

                        lstCDI = (from a in db.Tb_CDI select a).ToList();

                    return View(lstCDI);
                }
         
            }
            else
            {

                return RedirectToAction("Login", "Auth", new { access = false });

            }
           
        }
        public ActionResult CDI(string id)
        {
            if (cls_session.checkSession())
            {
                Tb_Usuarios activeuser = Session["activeUser"] as Tb_Usuarios;
                ViewBag.usuario = activeuser;
                //NOTIFICATIONS
                //List<Tb_Alerts> lstAlerts = (from a in db.Tb_Alerts where (a.ID_user == activeuser.ID_User && a.Active == true) select a).OrderByDescending(x => x.Date).Take(4).ToList();
                //ViewBag.notifications = lstAlerts;
                //FILTROS VARIABLES
                DateTime now = DateTime.Today;
                DateTime filtrostartdate;
                DateTime filtroenddate;
                ////filtros de fecha (MENSUAL)
                var sunday = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                var saturday = sunday.AddMonths(1).AddDays(-1);

                if (activeuser.FechaInicioPeriodos == null) { filtrostartdate = sunday; } else { filtrostartdate = Convert.ToDateTime(activeuser.FechaInicioPeriodos); }
                if (activeuser.FechaFinalPeriodos == null) { filtroenddate = saturday; } else { filtroenddate = Convert.ToDateTime(activeuser.FechaFinalPeriodos); }

                ViewBag.filtrofechastart = filtrostartdate.ToShortDateString();
                ViewBag.filtrofechaend = filtroenddate.ToShortDateString();

                Tb_CDI lstCDI = new Tb_CDI();
                lstCDI = (from a in db.Tb_CDI where (a.FCPID == activeuser.CDI) select a).FirstOrDefault();


                var beneficiarios = (from a in db.Tb_Beneficiarios where (a.BeneficiaryICPID == activeuser.CDI) select a).Count();
                var actividades = (from a in db.Tb_ActividadesBeneficiarios where (a.FCPID == activeuser.CDI) select a).Count();
                var ultimaactividad = (from a in db.Tb_ActividadesBeneficiarios where (a.FCPID == activeuser.CDI) select a.Fecha).OrderByDescending(c=>c).Take(1).FirstOrDefault();

                ViewBag.beneficiarios = beneficiarios;
                ViewBag.actividades = actividades;
                ViewBag.ultimaactividad = ultimaactividad;
                return View(lstCDI);
               
            }
            else
            {

                return RedirectToAction("Login", "Auth", new { access = false });

            }

        }
    }
}