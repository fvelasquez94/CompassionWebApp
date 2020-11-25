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
    public class ActividadesPrincipalesController : Controller
    {
        private CompassionDBEntities db = new CompassionDBEntities();
        private Cls_session cls_session = new Cls_session();
        // GET: ActividadesPrincipales
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

                var tb_ActividadesPrincipales = db.Tb_ActividadesPrincipales.Include(t => t.Tb_Categorias);
                return View(tb_ActividadesPrincipales.ToList());

            }
            else
            {

                return RedirectToAction("Login", "Auth", new { access = false });

            }
     
        }


        // GET: ActividadesPrincipales/Create
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

                ViewBag.ID_categoria = new SelectList(db.Tb_Categorias, "ID_categoria", "Codigo");
                return View();

            }
            else
            {

                return RedirectToAction("Login", "Auth", new { access = false });

            }
           
        }

        // POST: ActividadesPrincipales/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_actividadPrincipal,Codigo,Nombre,Descripcion,Activo,ID_categoria")] Tb_ActividadesPrincipales tb_ActividadesPrincipales)
        {
            try
            {
                tb_ActividadesPrincipales.Activo = true;
                db.Tb_ActividadesPrincipales.Add(tb_ActividadesPrincipales);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex) {
    
                return RedirectToAction("Index");
            }

        }

        // GET: ActividadesPrincipales/Edit/5
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

                Tb_ActividadesPrincipales tb_ActividadesPrincipales = db.Tb_ActividadesPrincipales.Find(id);

                ViewBag.ID_categoria = new SelectList(db.Tb_Categorias, "ID_categoria", "Codigo", tb_ActividadesPrincipales.ID_categoria);
                return View(tb_ActividadesPrincipales);

            }
            else
            {

                return RedirectToAction("Login", "Auth", new { access = false });

            }

          
        }

        // POST: ActividadesPrincipales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_actividadPrincipal,Codigo,Nombre,Descripcion,Activo,ID_categoria")] Tb_ActividadesPrincipales tb_ActividadesPrincipales)
        {
            try
            {
                db.Entry(tb_ActividadesPrincipales).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex) {

                return RedirectToAction("Index");
            }

        }

        // GET: ActividadesPrincipales/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                Tb_ActividadesPrincipales tb_ActividadesPrincipales = db.Tb_ActividadesPrincipales.Find(id);
                db.Tb_ActividadesPrincipales.Remove(tb_ActividadesPrincipales);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception ex) {
                return RedirectToAction("Index");
            }
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
