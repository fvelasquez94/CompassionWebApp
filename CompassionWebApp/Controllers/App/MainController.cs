using CompassionWebApp.Helpers.Session;
using CompassionWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CompassionWebApp.Controllers.App
{
    public class MainController : Controller
    {
        private CompassionDBEntities db = new CompassionDBEntities();
        private Cls_session cls_session = new Cls_session();
        // GET: Main
        public class ultimosBeneficiarios {
            public string LocalID { get; set; }
            public string nombre { get; set; }
            public string CDI { get; set; }
            public DateTime Fecha { get; set; }
            public string Actividad { get; set; }
            public string genero { get; set; }
        }
        public class actividadesGrafica
        {
            public int id { get; set; }
            public string nombre { get; set; }
            public string codificacion { get; set; }
            public int cantidadtotal { get; set; }
            public int cantidadfinalizadas { get; set; }
        }
        public ActionResult Inicio()
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

                List<ultimosBeneficiarios> lstultimos = new List<ultimosBeneficiarios>();
                lstultimos = (from a in db.Tb_ActividadesBeneficiarios where (a.FCPID == activeuser.CDI && a.parent==0 && (a.Fecha >=filtrostartdate && a.Fecha <=filtroenddate )) select new ultimosBeneficiarios { LocalID=a.LocalBeneficiaryID, nombre=a.Tb_Beneficiarios.AccountName, CDI=a.FCPID, Actividad=a.Tb_ActividadesSecundarias.Nombre_corto, Fecha=a.Fecha_actualizacion, genero=a.Tb_Beneficiarios.Gender }).Distinct().Take(4).ToList();
                ViewBag.lstUltimosBeneficiarios = lstultimos;

                List<actividadesGrafica> lstactividadesTotal = new List<actividadesGrafica>();
                lstactividadesTotal = (from p in db.Tb_ActividadesBeneficiarios
                                where (p.FCPID == activeuser.CDI && p.parent == 0 && (p.Fecha >= filtrostartdate && p.Fecha <= filtroenddate))
                                group p by p.ID_actividadSecundaria  into g
                                              select new actividadesGrafica { id = g.Key, nombre = g.FirstOrDefault().Tb_ActividadesSecundarias.Nombre_corto, cantidadtotal=g.Count(),cantidadfinalizadas= g.Where(c => c.ID_resultado != 1).Count(), codificacion="" }).OrderBy(c=>c.id).ToList();
                ViewBag.lstgrafica = lstactividadesTotal;


                JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                string data = jsSerializer.Serialize(lstactividadesTotal.Select(c=>c.nombre).ToArray());
                var dataActTotal = jsSerializer.Serialize(lstactividadesTotal.Select(c=>c.cantidadtotal).ToArray());
                var dataActFinal = jsSerializer.Serialize(lstactividadesTotal.Select(c=>c.cantidadfinalizadas).ToArray());
                ViewBag.data = data;
                ViewBag.dataActTotal = dataActTotal;
                ViewBag.dataActFinal = dataActFinal;
                return View();
            }
            else
            {

                return RedirectToAction("Login", "Auth", new { access = false });

            }
        }
    }
}