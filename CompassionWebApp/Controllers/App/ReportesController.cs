using CompassionWebApp.Helpers.Session;
using CompassionWebApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static CompassionWebApp.Models.Reportes.Aux_ModelsR;

namespace CompassionWebApp.Controllers.App
{
    public class ReportesController : Controller
    {
        private CompassionDBEntities db = new CompassionDBEntities();
        private Cls_session cls_session = new Cls_session();
        // GET: Reportes
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
  
                return View();

            }
            else
            {

                return RedirectToAction("Login", "Auth", new { access = false });

            }
        }


        public ActionResult ActividadesCDI()
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


                List<ReportexCDI> lstreporte = new List<ReportexCDI>();
                lstreporte = db.Database.SqlQuery<ReportexCDI>("select * from actividadesporCDI where IDCDI= {0}", activeuser.CDI).ToList();

                return View(lstreporte);

            }
            else
            {

                return RedirectToAction("Login", "Auth", new { access = false });

            }
        }

        public ActionResult ActividadesCDI_detalle (int ID_plantilla, int ID_actividad)
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

                //var r = db.Database.SqlQuery<string>("EXEC sp_generateDinamicReport {0},{1},{2}", ID_plantilla,ID_actividad, activeuser.CDI).ToList();

                DataTable table = new DataTable();
   

                //string connetionString = null;
                SqlConnection connection;
                SqlDataAdapter adapter;
                SqlCommand command = new SqlCommand();
                SqlParameter param;
                DataSet ds = new DataSet();

                int i = 0;

                //connetionString = "Data Source=servername;Initial Catalog=PUBS;User ID=sa;Password=yourpassword";
                connection = new SqlConnection(db.Database.Connection.ConnectionString);

                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_generateDinamicReport";

                param = new SqlParameter("@IDPlantilla", ID_plantilla);
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.Int32;
                command.Parameters.Add(param);

                param = new SqlParameter("@ID_ActividadSecundaria", ID_actividad);
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.Int32;
                command.Parameters.Add(param);

                param = new SqlParameter("@CDI", activeuser.CDI);
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.String;
                command.Parameters.Add(param);



                adapter = new SqlDataAdapter(command);
                adapter.Fill(ds);

                connection.Close();

                table = ds.Tables[0];

                ViewBag.idact = ID_actividad;

                return View(table);

            }
            else
            {

                return RedirectToAction("Login", "Auth", new { access = false });

            }
        }

        

        public ActionResult ActividadesCDI_det(int ID_actividadSecundaria)
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

                List<TiposPlantillas> lstTipos = new List<TiposPlantillas>();
                lstTipos = (from a in db.Tb_ActividadesBeneficiarios
                            where (a.ID_actividadSecundaria == ID_actividadSecundaria && a.FCPID == activeuser.CDI)
                            select new TiposPlantillas
                            {
                                ID_ActividadSecundaria = a.ID_actividadSecundaria,
                                Nombre = a.Tb_ActividadesSecundarias.Nombre_corto,
                                ID_plantilla = a.ID_plantilla,
                                Plantilla = a.ID_plantilla !=0 ? (from b in db.Tb_Plantillas where (b.ID_plantilla == a.ID_plantilla) select b.Nombre).FirstOrDefault() : "No asignada",
                                CDI = a.FCPID
                            }).Distinct().ToList();



                return View(lstTipos);

            }
            else
            {

                return RedirectToAction("Login", "Auth", new { access = false });

            }
        }

        public ActionResult ActividadesBeneficiario()
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


                List<ReportexBeneficiario> lstreporte = new List<ReportexBeneficiario>();
                lstreporte = db.Database.SqlQuery<ReportexBeneficiario>("select * from actividadesporBeneficiario where IDCDI= {0}", activeuser.CDI).ToList();

                return View(lstreporte);



            }
            else
            {

                return RedirectToAction("Login", "Auth", new { access = false });

            }
        }

        public ActionResult ActividadesBeneficiario_detalle(int ID_plantilla, int ID_actividad, string id_beneficiario)
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

                //var r = db.Database.SqlQuery<string>("EXEC sp_generateDinamicReport {0},{1},{2}", ID_plantilla,ID_actividad, activeuser.CDI).ToList();

                DataTable table = new DataTable();


                //string connetionString = null;
                SqlConnection connection;
                SqlDataAdapter adapter;
                SqlCommand command = new SqlCommand();
                SqlParameter param;
                DataSet ds = new DataSet();

                int i = 0;

                //connetionString = "Data Source=servername;Initial Catalog=PUBS;User ID=sa;Password=yourpassword";
            

                SqlConnectionStringBuilder strbldr = new SqlConnectionStringBuilder();
                strbldr.DataSource = db.Database.Connection.DataSource;
                strbldr.InitialCatalog = db.Database.Connection.Database;
                strbldr.IntegratedSecurity = true;
                strbldr.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled;

                connection = new SqlConnection(strbldr.ConnectionString);

                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "sp_generateDinamicReport";

                param = new SqlParameter("@IDPlantilla", ID_plantilla);
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Int;
                command.Parameters.Add(param);

                param = new SqlParameter("@ID_ActividadSecundaria", ID_actividad);
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.Int;
                command.Parameters.Add(param);

                param = new SqlParameter("@CDI", activeuser.CDI);
                param.Direction = ParameterDirection.Input;
                param.SqlDbType = SqlDbType.NVarChar;
                param.Size = 100;
                command.Parameters.Add(param);



                adapter = new SqlDataAdapter(command);
                adapter.Fill(ds);

                connection.Close();

                table = ds.Tables[0];

                ViewBag.idact = ID_actividad;
                ViewBag.idbeneficiario = id_beneficiario;

                return View(table);

            }
            else
            {

                return RedirectToAction("Login", "Auth", new { access = false });

            }
        }

        public ActionResult ActividadesBeneficiario_det(int ID_actividadSecundaria, string id_beneficiario)
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

                List<TiposPlantillas> lstTipos = new List<TiposPlantillas>();
                lstTipos = (from a in db.Tb_ActividadesBeneficiarios
                            where (a.ID_actividadSecundaria == ID_actividadSecundaria && a.FCPID == activeuser.CDI && a.LocalBeneficiaryID==id_beneficiario)
                            select new TiposPlantillas
                            {
                                ID_ActividadSecundaria = a.ID_actividadSecundaria,
                                Nombre = a.Tb_ActividadesSecundarias.Nombre_corto,
                                ID_plantilla = a.ID_plantilla,
                                Plantilla = a.ID_plantilla != 0 ? (from b in db.Tb_Plantillas where (b.ID_plantilla == a.ID_plantilla) select b.Nombre).FirstOrDefault() : "No asignada",
                                CDI = a.FCPID
                            }).Distinct().ToList();


                ViewBag.beneficiario = id_beneficiario;
                return View(lstTipos);

            }
            else
            {

                return RedirectToAction("Login", "Auth", new { access = false });

            }
        }
    }
}