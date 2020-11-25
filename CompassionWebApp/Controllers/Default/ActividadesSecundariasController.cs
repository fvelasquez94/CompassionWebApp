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
    public class ActividadesSecundariasController : Controller
    {
        private CompassionDBEntities db = new CompassionDBEntities();
        private Cls_session cls_session = new Cls_session();
        // GET: ActividadesSecundarias
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

                var tb_ActividadesSecundarias = db.Tb_ActividadesSecundarias.Include(t => t.Tb_ActividadesPrincipales);
                return View(tb_ActividadesSecundarias.ToList());

            }
            else
            {

                return RedirectToAction("Login", "Auth", new { access = false });

            }
        
        }

        // GET: ActividadesSecundarias/Create
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

                ViewBag.ID_actividadPrincipal = new SelectList(db.Tb_ActividadesPrincipales, "ID_actividadPrincipal", "Codigo");
                return View();

            }
            else
            {

                return RedirectToAction("Login", "Auth", new { access = false });

            }
     
        }

        // POST: ActividadesSecundarias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_actividadSecundaria,Codigo,Nombre,Nombre_corto,Descripcion,Activo,ID_actividadPrincipal,Periodicidad")] Tb_ActividadesSecundarias tb_ActividadesSecundarias)
        {
            try
            {
                if (tb_ActividadesSecundarias.Periodicidad == null || tb_ActividadesSecundarias.Periodicidad == "") { tb_ActividadesSecundarias.Periodicidad = "No asignada"; }
                tb_ActividadesSecundarias.Activo = true;
                db.Tb_ActividadesSecundarias.Add(tb_ActividadesSecundarias);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex) {

                return RedirectToAction("Index");
            }
           
        }

        // GET: ActividadesSecundarias/Edit/5
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

                Tb_ActividadesSecundarias tb_ActividadesSecundarias = db.Tb_ActividadesSecundarias.Find(id);
                ViewBag.ID_actividadPrincipal = new SelectList(db.Tb_ActividadesPrincipales, "ID_actividadPrincipal", "Codigo", tb_ActividadesSecundarias.ID_actividadPrincipal);
                return View(tb_ActividadesSecundarias);
            }
            else
            {

                return RedirectToAction("Login", "Auth", new { access = false });

            }

     
        }

        // POST: ActividadesSecundarias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_actividadSecundaria,Codigo,Nombre,Nombre_corto,Descripcion,Activo,ID_actividadPrincipal,Periodicidad")] Tb_ActividadesSecundarias tb_ActividadesSecundarias)
        {
            try
            {
                if (tb_ActividadesSecundarias.Periodicidad == null || tb_ActividadesSecundarias.Periodicidad == "") { tb_ActividadesSecundarias.Periodicidad = "No asignada"; }

                db.Entry(tb_ActividadesSecundarias).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex) {
                return RedirectToAction("Index");
            }

        }

        // GET: ActividadesSecundarias/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                Tb_ActividadesSecundarias tb_ActividadesSecundarias = db.Tb_ActividadesSecundarias.Find(id);
                db.Tb_ActividadesSecundarias.Remove(tb_ActividadesSecundarias);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch {

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
