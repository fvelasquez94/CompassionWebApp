using CompassionWebApp.Helpers.Session;
using CompassionWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CompassionWebApp.Controllers.App
{
    public class InscripcionesController : Controller
    {
        private CompassionDBEntities db = new CompassionDBEntities();
        private Cls_session cls_session = new Cls_session();
        // GET: Inscripciones


        public class InscripcionGeneral {
            public int id_actividad { get; set; }
            public string eje { get; set; }
            public string nombreActividad { get; set; }
            public string fcpid { get; set; }
            public int countInscritas { get; set; }
        }

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

                List<InscripcionGeneral> lstactividades = new List<InscripcionGeneral>();

                var inscripcionesdistict = (from a in db.Tb_InscripcionActividad where (a.FCPID == activeuser.CDI) select a.ID_actividadSecundaria).Distinct().ToArray();

                lstactividades = (from a in db.Tb_ActividadesSecundarias where (inscripcionesdistict.Contains(a.ID_actividadSecundaria)) select new InscripcionGeneral { id_actividad = a.ID_actividadSecundaria,eje=a.Tb_ActividadesPrincipales.Tb_Categorias.Tb_ActividadesPrincipales.FirstOrDefault().Nombre, fcpid = activeuser.CDI,
                    nombreActividad = (from b in db.Tb_ActividadesSecundarias where (b.ID_actividadSecundaria == a.ID_actividadSecundaria) select b.Nombre_corto).FirstOrDefault(),
                    countInscritas = (from c in db.Tb_InscripcionActividad where (c.FCPID == activeuser.CDI && c.ID_actividadSecundaria == a.ID_actividadSecundaria) select c).Count()
                    }).ToList();

                return View(lstactividades);
            }
            else
            {

                return RedirectToAction("Login", "Auth", new { access = false });

            }
        }
        public ActionResult Nueva()
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

                List<Tb_ActividadesSecundarias> lstactividades = new List<Tb_ActividadesSecundarias>();

                var inscripcionesdistict = (from a in db.Tb_InscripcionActividad where (a.FCPID == activeuser.CDI) select a.ID_actividadSecundaria).Distinct().ToArray();

                lstactividades = (from a in db.Tb_ActividadesSecundarias
                                  where (!inscripcionesdistict.Contains(a.ID_actividadSecundaria) && a.ID_actividadSecundaria!=1)
                                  select a).ToList();
                ViewBag.actividades = lstactividades;

                List<Tb_Beneficiarios> lstBeneficiarios = new List<Tb_Beneficiarios>();
                lstBeneficiarios = (from a in db.Tb_Beneficiarios where (a.BeneficiaryICPID == activeuser.CDI) select a).ToList();

                return View(lstBeneficiarios);
            }
            else
            {

                return RedirectToAction("Login", "Auth", new { access = false });

            }
        }
        public ActionResult Editar(int id)
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


                var inscripcionesdistict = (from a in db.Tb_InscripcionActividad where (a.FCPID == activeuser.CDI && a.ID_actividadSecundaria==id) select a.LocalBeneficiaryID).Distinct().ToArray();
                List<Tb_Beneficiarios> inscritos = new List<Tb_Beneficiarios>();

                inscritos = (from a in db.Tb_Beneficiarios where (inscripcionesdistict.Contains(a.LocalBeneficiaryID)) select a).ToList();

                List<Tb_Beneficiarios> lstBeneficiarios = new List<Tb_Beneficiarios>();
                lstBeneficiarios = (from a in db.Tb_Beneficiarios where (a.BeneficiaryICPID == activeuser.CDI && !inscripcionesdistict.Contains(a.LocalBeneficiaryID)) select a).ToList();

                ViewBag.beneficiarios = lstBeneficiarios;

                var actividad = (from c in db.Tb_ActividadesSecundarias where (c.ID_actividadSecundaria == id) select c).FirstOrDefault();

                ViewBag.Id_actividad = id;
                ViewBag.actividad = actividad.Tb_ActividadesPrincipales.Tb_Categorias.Tb_EjesEstrategicos.Nombre + ": " + actividad.Nombre_corto;
                return View(inscritos);
            }
            else
            {

                return RedirectToAction("Login", "Auth", new { access = false });

            }
        }

        public class detallesInscripcion {
            public string LocalBeneficiaryID { get; set; }
            public string AccountName { get; set; }
            public string CDI { get; set; }
        }

        public ActionResult nuevaInscripcion(int ID_actividad, List<detallesInscripcion> Detalles)
        {
            try
            {
                Tb_Usuarios activeuser = Session["activeUser"] as Tb_Usuarios;
                foreach (var item in Detalles) {
                    Tb_InscripcionActividad nuevo = new Tb_InscripcionActividad();
                    nuevo.AccountName = item.AccountName;
                    nuevo.LocalBeneficiaryID = item.LocalBeneficiaryID;
                    nuevo.Fecha = DateTime.UtcNow;
                    nuevo.ID_usuario = activeuser.ID_usuario;
                    nuevo.FCPID = item.CDI;
                    nuevo.ID_actividadSecundaria = ID_actividad;
                    db.Tb_InscripcionActividad.Add(nuevo);
                }
               
                db.SaveChanges();

                var result = new { mensaje = "success" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var result = new { mensaje = "error" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult eliminarInscripcion(int ID_actividad, string localbeneficiaryID)
        {
            try
            {
                var reg = (from a in db.Tb_InscripcionActividad where (a.ID_actividadSecundaria == ID_actividad && a.LocalBeneficiaryID == localbeneficiaryID) select a).FirstOrDefault();
                db.Tb_InscripcionActividad.Remove(reg);
                db.SaveChanges();

                var result = new { mensaje = "success" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var result = new { mensaje = "error" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

    }
}