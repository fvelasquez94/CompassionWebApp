using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CompassionWebApp.Helpers.Session;
using CompassionWebApp.Models;

namespace CompassionWebApp.Controllers.Default
{
    public class EjesEstrategicosController : Controller
    {
        private CompassionDBEntities db = new CompassionDBEntities();
        private Cls_session cls_session = new Cls_session();
        // GET: EjesEstrategicos
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

                return View(db.Tb_EjesEstrategicos.ToList());
    
            }
            else
            {

                return RedirectToAction("Login", "Auth", new { access = false });

            }

        }


        // GET: EjesEstrategicos/Create
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

        // POST: EjesEstrategicos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_eje,Codigo,Nombre,Descripcion,Activo")] Tb_EjesEstrategicos tb_EjesEstrategicos)
        {
            try
            {
                tb_EjesEstrategicos.Activo = true;
                    db.Tb_EjesEstrategicos.Add(tb_EjesEstrategicos);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                
            }
            catch (Exception ex) {
                return RedirectToAction("Index");
            }

        }

        // GET: EjesEstrategicos/Edit/5
        public ActionResult Edit(int? id)
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
                Tb_EjesEstrategicos tb_EjesEstrategicos = db.Tb_EjesEstrategicos.Find(id);
                return View(tb_EjesEstrategicos);

            }
            else
            {

                return RedirectToAction("Login", "Auth", new { access = false });

            }

        
        }

        // POST: EjesEstrategicos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_eje,Codigo,Nombre,Descripcion,Activo")] Tb_EjesEstrategicos tb_EjesEstrategicos)
        {
            try
            {
                db.Entry(tb_EjesEstrategicos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex) {
                return RedirectToAction("Index");
            }

        }



        public ActionResult Delete(int id)
        {
            Tb_EjesEstrategicos tb_EjesEstrategicos = db.Tb_EjesEstrategicos.Find(id);
            db.Tb_EjesEstrategicos.Remove(tb_EjesEstrategicos);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
