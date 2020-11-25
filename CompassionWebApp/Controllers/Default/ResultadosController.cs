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
    public class ResultadosController : Controller
    {
        private CompassionDBEntities db = new CompassionDBEntities();
        private Cls_session cls_session = new Cls_session();
        // GET: Resultados
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

                var tb_Resultados = db.Tb_Resultados.Include(t => t.Tb_ActividadesSecundarias);
                return View(tb_Resultados.ToList());

            }
            else
            {

                return RedirectToAction("Login", "Auth", new { access = false });

            }
   
        }


        // GET: Resultados/Create
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

                ViewBag.ID_actividadSecundaria = new SelectList(db.Tb_ActividadesSecundarias, "ID_actividadSecundaria", "Codigo");
                return View();

            }
            else
            {

                return RedirectToAction("Login", "Auth", new { access = false });

            }
         
        }

        // POST: Resultados/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_resultado,Codigo,Nombre,Descripcion,Activo,ID_actividadSecundaria")] Tb_Resultados tb_Resultados)
        {
            tb_Resultados.Activo = true;
            try {
                db.Tb_Resultados.Add(tb_Resultados);
                db.SaveChanges();
                return RedirectToAction("Index");
            }catch(Exception ex){
                return RedirectToAction("Index");
            }


        }

        // GET: Resultados/Edit/5
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

                Tb_Resultados tb_Resultados = db.Tb_Resultados.Find(id);
                ViewBag.ID_actividadSecundaria = new SelectList(db.Tb_ActividadesSecundarias, "ID_actividadSecundaria", "Codigo", tb_Resultados.ID_actividadSecundaria);
                return View(tb_Resultados);
            }
            else
            {

                return RedirectToAction("Login", "Auth", new { access = false });

            }

        }

        // POST: Resultados/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_resultado,Codigo,Nombre,Descripcion,Activo,ID_actividadSecundaria")] Tb_Resultados tb_Resultados)
        {
            try
            {
                db.Entry(tb_Resultados).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex) {
                return RedirectToAction("Index");
            }
         }

        // GET: Resultados/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                Tb_Resultados tb_Resultados = db.Tb_Resultados.Find(id);
                db.Tb_Resultados.Remove(tb_Resultados);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex) {
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
