using CompassionWebApp.Helpers.Session;
using CompassionWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static CompassionWebApp.Models.Plantillas.Aux_Models;

namespace CompassionWebApp.Controllers.App
{
    public class PlantillasController : Controller
    {
        private CompassionDBEntities db = new CompassionDBEntities();
        private Cls_session cls_session = new Cls_session();
        // GET: Plantillas
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


                var plantillas = (from a in db.Tb_Plantillas select new Tb_PlantillasIndex { ID_plantilla=a.ID_plantilla, Nombre=a.Nombre, Descripcion=a.Descripcion, Fecha_creacion=a.Fecha_creacion,ID_usuario=a.ID_usuario, count=(from c in db.Tb_ActividadesBeneficiarios where(c.ID_plantilla==a.ID_plantilla) select c).Count()  }).ToList();

                return View(plantillas);
            }
            else
            {

                return RedirectToAction("Login", "Auth", new { access = false });

            }
            
        }
        public ActionResult Create()
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

                return View();
            }
            else
            {

                return RedirectToAction("Login", "Auth", new { access = false });

            }
          
        }
        public ActionResult Edit(int id)
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

                var plantilla = (from a in db.Tb_Plantillas where (a.ID_plantilla == id) select a).FirstOrDefault();
                var lstDetalles = (from a in db.Tb_ActividadesDetalles where (a.ID_plantilla == id && a.Original == true) select a).ToList();
                int count = lstDetalles.Count();
                ViewBag.count = count;
                ViewBag.lstDetalles = lstDetalles;
                return View(plantilla);
            }
            else
            {

                return RedirectToAction("Login", "Auth", new { access = false });

            }
 
        }


        public ActionResult Delete(int id)
        {
            Tb_Plantillas tb_plantilla = db.Tb_Plantillas.Find(id);
            db.Tb_Plantillas.Remove(tb_plantilla);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}