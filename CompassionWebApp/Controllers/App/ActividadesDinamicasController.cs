using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using CompassionWebApp.Helpers.Session;
using CompassionWebApp.Models;

namespace CompassionWebApp.Controllers.App
{
    public class ActividadesDinamicasController : Controller
    {
        private CompassionDBEntities db = new CompassionDBEntities();
        private Cls_session cls_session = new Cls_session();

        // GET: ActividadesDinamicas
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

                var tb_ActividadesBeneficiarios = db.Tb_ActividadesBeneficiarios.Where(c=>  c.parent==0 && (c.Fecha>=filtrostartdate && c.Fecha<=filtroenddate)).Include(t => t.Tb_ActividadesPrincipales).Include(t => t.Tb_ActividadesSecundarias).Include(t => t.Tb_Beneficiarios).Include(t => t.Tb_Categorias).Include(t => t.Tb_CDI).Include(t => t.Tb_EjesEstrategicos).Include(t => t.Tb_PrecondicionesTOC).Include(t => t.Tb_Resultados);
                return View(tb_ActividadesBeneficiarios.ToList());
            }
            else
            {

                return RedirectToAction("Login", "Auth", new { access = false });

            }
         
        }

        // GET: ActividadesDinamicas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tb_ActividadesBeneficiarios tb_ActividadesBeneficiarios = db.Tb_ActividadesBeneficiarios.Find(id);
            if (tb_ActividadesBeneficiarios == null)
            {
                return HttpNotFound();
            }
            return View(tb_ActividadesBeneficiarios);
        }

        public class SelectItemOptionString {
            public string ID { get; set; }
            public string Descripcion { get; set; }
        }
        public class SelectItemOption
        {
            public int ID { get; set; }
            public string Descripcion { get; set; }
        }
        // GET: ActividadesDinamicas/Create
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

                ViewBag.LocalBeneficiaryID = (from b in  db.Tb_Beneficiarios where (b.BeneficiaryICPID == activeuser.CDI) select new SelectItemOptionString { ID = b.LocalBeneficiaryID, Descripcion = b.AccountName }).OrderBy(b => b.Descripcion).ToList();
                //ViewBag.ID_eje = (from b in db.Tb_EjesEstrategicos where (b.Activo == true) select new SelectItemOption { ID = b.ID_eje, Descripcion = b.Nombre }).OrderBy(b => b.Descripcion).ToList();
                ViewBag.FCPID = (from b in db.Tb_CDI where (b.FCPID==activeuser.CDI) select new SelectItemOptionString { ID = b.FCPID, Descripcion = b.NombreCDI }).OrderBy(b => b.Descripcion).ToList();

                ViewBag.plantillas = (from b in db.Tb_Plantillas select new SelectItemOption { ID = b.ID_plantilla, Descripcion = b.Nombre }).OrderBy(b => b.Descripcion).ToList();

                return View();
            }
            else
            {

                return RedirectToAction("Login", "Auth", new { access = false });

            }
     
        }


        public ActionResult CreateBulk()
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

                ViewBag.LocalBeneficiaryID = (from b in db.Tb_Beneficiarios where (b.BeneficiaryICPID == activeuser.CDI) select new SelectItemOptionString { ID = b.LocalBeneficiaryID, Descripcion = b.AccountName }).OrderBy(b => b.Descripcion).ToList();
                //ViewBag.ID_eje = (from b in db.Tb_EjesEstrategicos where (b.Activo == true) select new SelectItemOption { ID = b.ID_eje, Descripcion = b.Nombre }).OrderBy(b => b.Descripcion).ToList();
                ViewBag.FCPID = (from b in db.Tb_CDI where (b.FCPID == activeuser.CDI) select new SelectItemOptionString { ID = b.FCPID, Descripcion = b.NombreCDI }).OrderBy(b => b.Descripcion).ToList();

                ViewBag.plantillas = (from b in db.Tb_Plantillas select new SelectItemOption { ID = b.ID_plantilla, Descripcion = b.Nombre }).OrderBy(b => b.Descripcion).ToList();

                var inscripcionesdistict = (from a in db.Tb_InscripcionActividad where (a.FCPID == activeuser.CDI) select a.ID_actividadSecundaria).Distinct().ToArray();

                var lstactividades = (from a in db.Tb_ActividadesSecundarias
                                  where (inscripcionesdistict.Contains(a.ID_actividadSecundaria))
                                  select a).ToList();
                ViewBag.lstactividades = lstactividades;

                return View();
            }
            else
            {

                return RedirectToAction("Login", "Auth", new { access = false });

            }

        }

        public ActionResult CrearActividad(int eje, int ID_categoria, int ID_Secundaria, int ID_Principal, string ID_beneficiario, string ID_CDI, int ID_Plantilla)
        {
            try
            {
                var actividad = db.Tb_ActividadesSecundarias.Find(ID_Secundaria);
                


                Tb_ActividadesBeneficiarios nuevaActividadBeneficiario = new Tb_ActividadesBeneficiarios();
                nuevaActividadBeneficiario.LocalBeneficiaryID = ID_beneficiario;
                nuevaActividadBeneficiario.FCPID = ID_CDI;

                nuevaActividadBeneficiario.ID_eje = actividad.Tb_ActividadesPrincipales.Tb_Categorias.ID_eje;
                nuevaActividadBeneficiario.ID_categoria = actividad.Tb_ActividadesPrincipales.ID_categoria;
                nuevaActividadBeneficiario.ID_actividadPrincipal = actividad.ID_actividadPrincipal;

                nuevaActividadBeneficiario.ID_actividadSecundaria = ID_Secundaria;
                nuevaActividadBeneficiario.ID_plantilla = ID_Plantilla;
                nuevaActividadBeneficiario.ID_resultado = 1; //por defecto
                nuevaActividadBeneficiario.ID_precondicionTOC = 1; //por defecto
                nuevaActividadBeneficiario.Fecha = DateTime.UtcNow; //por defecto
                nuevaActividadBeneficiario.Fecha_actualizacion = DateTime.UtcNow; //por defecto
                nuevaActividadBeneficiario.Comentarios = ""; //por defecto
                nuevaActividadBeneficiario.ID_usuario = 1; //por defecto
                nuevaActividadBeneficiario.parent = 0; //por defecto
                nuevaActividadBeneficiario.masiva = false; //por defecto
                db.Tb_ActividadesBeneficiarios.Add(nuevaActividadBeneficiario);
                db.SaveChanges();

                //Evaluamos si utilizara plantilla
                if (ID_Plantilla != 0) {
                    //buscamos detalles
                    List<Tb_ActividadesDetalles> nuevalista = new List<Tb_ActividadesDetalles>();
                    var detalles = (from a in db.Tb_ActividadesDetalles where (a.ID_plantilla == ID_Plantilla && a.Original == true) select a).ToList();
                    foreach (var item in detalles) {
                        Tb_ActividadesDetalles nuevoreg = new Tb_ActividadesDetalles();
                        nuevoreg = item;
                        nuevoreg.Original = false;
                        nuevoreg.ID_actividadSecundaria = ID_Secundaria;
                        nuevoreg.ID_actividadBeneficiario = nuevaActividadBeneficiario.ID_actividadBeneficiario;
                        nuevalista.Add(nuevoreg);
                    }
                    db.Tb_ActividadesDetalles.AddRange(nuevalista);
                    db.SaveChanges();
                }


                var result = new { mensaje = "success", id_actividadBeneficiario = nuevaActividadBeneficiario.ID_actividadBeneficiario };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var result = new { mensaje = "error" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CrearActividadMasiva(int ID_Secundaria, string ID_CDI, int ID_Plantilla, string beneficiarios)
        {
            try
            {

                Tb_Usuarios activeuser = Session["activeUser"] as Tb_Usuarios;
                List<string> lstbeneficiarios = beneficiarios.Split(',').ToList<string>();
                List<Tb_ActividadesBeneficiarios> lstAdd = new List<Tb_ActividadesBeneficiarios>();
                var actividad = db.Tb_ActividadesSecundarias.Find(ID_Secundaria);
                var count = 0;
                var parent = 0;
                foreach (var itembenef in lstbeneficiarios) {
                    Tb_ActividadesBeneficiarios nuevaActividadBeneficiario = new Tb_ActividadesBeneficiarios();
                    nuevaActividadBeneficiario.LocalBeneficiaryID = itembenef;
                    nuevaActividadBeneficiario.FCPID = ID_CDI;

                    nuevaActividadBeneficiario.ID_eje = actividad.Tb_ActividadesPrincipales.Tb_Categorias.ID_eje;
                    nuevaActividadBeneficiario.ID_categoria = actividad.Tb_ActividadesPrincipales.ID_categoria;
                    nuevaActividadBeneficiario.ID_actividadPrincipal = actividad.ID_actividadPrincipal;

                    nuevaActividadBeneficiario.ID_actividadSecundaria = ID_Secundaria;
                    nuevaActividadBeneficiario.ID_plantilla = ID_Plantilla;
                    nuevaActividadBeneficiario.ID_resultado = 1; //por defecto
                    nuevaActividadBeneficiario.ID_precondicionTOC = 1; //por defecto
                    nuevaActividadBeneficiario.Fecha = DateTime.UtcNow; //por defecto
                    nuevaActividadBeneficiario.Fecha_actualizacion = DateTime.UtcNow; //por defecto
                    nuevaActividadBeneficiario.Comentarios = ""; //por defecto
                    nuevaActividadBeneficiario.ID_usuario = activeuser.ID_usuario; //por defecto
                    if (count == 0)
                    {
                        nuevaActividadBeneficiario.parent = 0; //por defecto
                        nuevaActividadBeneficiario.masiva = true; //por defecto
                       
                        
                    }
                    else {
                        nuevaActividadBeneficiario.parent = parent; //por defecto
                        nuevaActividadBeneficiario.masiva = true; //por defecto
                    }

                    db.Tb_ActividadesBeneficiarios.Add(nuevaActividadBeneficiario);
                    db.SaveChanges();
                    if (count == 0)
                    {
                        parent = nuevaActividadBeneficiario.ID_actividadBeneficiario;
                        count = 1;
                    }
                       
                    //Evaluamos si utilizara plantilla
                    if (ID_Plantilla != 0)
                    {
                        //buscamos detalles
                        List<Tb_ActividadesDetalles> nuevalista = new List<Tb_ActividadesDetalles>();
                        var detalles = (from a in db.Tb_ActividadesDetalles where (a.ID_plantilla == ID_Plantilla && a.Original == true) select a).ToList();
                        foreach (var item in detalles)
                        {
                            Tb_ActividadesDetalles nuevoreg = new Tb_ActividadesDetalles();
                            nuevoreg = item;
                            nuevoreg.Original = false;
                            nuevoreg.ID_actividadSecundaria = ID_Secundaria;
                            nuevoreg.ID_actividadBeneficiario = nuevaActividadBeneficiario.ID_actividadBeneficiario;
                            nuevalista.Add(nuevoreg);
                        }
                        db.Tb_ActividadesDetalles.AddRange(nuevalista);
                        db.SaveChanges();
                    }


                    //lstAdd.Add(nuevaActividadBeneficiario);
                }

          



                var result = new { mensaje = "success", id_actividadBeneficiario = parent };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { mensaje = "error" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult GuardarActividad(int ID_Resultado, int ID_Precondicion, int ID_actividadBeneficiaro,string comentarios)
        {
            try
            {
                if (ID_Resultado == 0) { ID_Resultado = 1; }
                if (ID_Precondicion == 0) { ID_Precondicion = 1; }
                var actividad = (from a in db.Tb_ActividadesBeneficiarios where (a.ID_actividadBeneficiario == ID_actividadBeneficiaro) select a).FirstOrDefault();
                actividad.ID_resultado = ID_Resultado;
                actividad.ID_precondicionTOC = ID_Precondicion;
                actividad.Comentarios = comentarios;
                actividad.Fecha_actualizacion = DateTime.UtcNow;
                db.Entry(actividad).State = EntityState.Modified;
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



        public ActionResult GuardarParametro(int id, string value, int valueint,int valueint2, decimal valuedec, int tipo_recurso, int ID_actividadBeneficiaro)
        {
            try
            {
                var parametro = (from a in db.Tb_ActividadesDetalles where (a.ID_actividadBeneficiario == ID_actividadBeneficiaro && a.ID_detalleActividad == id) select a).FirstOrDefault();
                if (tipo_recurso == 1) {//texto
                    parametro.ValorTexto = value;
                }
                if (tipo_recurso == 2) {//numerico decimal
                    parametro.ValorNumerico = valuedec;
                }
                if (tipo_recurso == 3) {//numerico entero 
                    parametro.ValorEntero = valueint;
                }
                if (tipo_recurso == 4) {//fecha 
                    parametro.ValorTexto = value;
                }
                if (tipo_recurso == 5) {//lista
                    parametro.ValorMultipleOpciones = value;
                }
                if (tipo_recurso == 6) {//rango
                    parametro.ValorMin = valueint;
                    parametro.ValorMax = valueint2;
                }


                db.Entry(parametro).State = EntityState.Modified;
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


        public ActionResult GuardarParametroMasivo(int id, string value, int valueint, int valueint2, decimal valuedec, int tipo_recurso, int ID_actividadBeneficiaro)
        {
            try
            {
                var query = "";
                var parametro = (from a in db.Tb_ActividadesDetalles where (a.ID_actividadBeneficiario == ID_actividadBeneficiaro && a.ID_detalleActividad == id) select a).FirstOrDefault();
                var headermasivo = (from a in db.Tb_ActividadesBeneficiarios where (a.parent == parametro.ID_actividadBeneficiario) select a.ID_actividadBeneficiario).ToArray();
                var str = String.Join(",", headermasivo);
                if (tipo_recurso == 1)
                {//texto
                    parametro.ValorTexto = value;
                
                    query = "update Tb_ActividadesDetalles set ValorTexto={0} where Orden={1} and ID_actividadBeneficiario in (" + str + ")";

                    db.Entry(parametro).State = EntityState.Modified;
                    db.SaveChanges();

                    //actualizamos masivo
                    db.Database.ExecuteSqlCommand(query, value, parametro.Orden);
                }
                if (tipo_recurso == 2)
                {//numerico decimal
                    parametro.ValorNumerico = valuedec;

                    query = "update Tb_ActividadesDetalles set ValorNumerico={0} where Orden={1} and ID_actividadBeneficiario in (" + str + ")";

                    db.Entry(parametro).State = EntityState.Modified;
                    db.SaveChanges();

                    //actualizamos masivo
                    db.Database.ExecuteSqlCommand(query, valuedec, parametro.Orden);
                }
                if (tipo_recurso == 3)
                {//numerico entero 
                    parametro.ValorEntero = valueint;
                    query = "update Tb_ActividadesDetalles set ValorEntero={0} where Orden={1} and ID_actividadBeneficiario in (" + str + ")";

                    db.Entry(parametro).State = EntityState.Modified;
                    db.SaveChanges();

                    //actualizamos masivo
                    db.Database.ExecuteSqlCommand(query, valueint, parametro.Orden);
                }
                if (tipo_recurso == 4)
                {//fecha 
                    parametro.ValorTexto = value;
                    query = "update Tb_ActividadesDetalles set ValorTexto={0} where Orden={1} and ID_actividadBeneficiario in (" + str + ")";

                    db.Entry(parametro).State = EntityState.Modified;
                    db.SaveChanges();

                    //actualizamos masivo
                    db.Database.ExecuteSqlCommand(query, value, parametro.Orden);
                }
                if (tipo_recurso == 5)
                {//lista
                    parametro.ValorMultipleOpciones = value;
                    query = "update Tb_ActividadesDetalles set ValorMultipleOpciones={0} where Orden={1} and ID_actividadBeneficiario in (" + str + ")";

                    db.Entry(parametro).State = EntityState.Modified;
                    db.SaveChanges();

                    //actualizamos masivo
                    db.Database.ExecuteSqlCommand(query, value, parametro.Orden);
                }
                if (tipo_recurso == 6)
                {//rango
                    parametro.ValorMin = valueint;
                    parametro.ValorMax = valueint2;
                    query = "update Tb_ActividadesDetalles set ValorMax={0}, ValorMin={1} where Orden={2} and ID_actividadBeneficiario in (" + str + ")";

                    db.Entry(parametro).State = EntityState.Modified;
                    db.SaveChanges();

                    //actualizamos masivo
                    db.Database.ExecuteSqlCommand(query, valueint2,valueint, parametro.Orden);
                }




                var result = new { mensaje = "success" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { mensaje = "error" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult CrearPlantilla(List<Tb_ActividadesDetalles> detalles, string nombre, string descripcion)
        {
            try
            {
                
                Tb_Plantillas nuevaPlantilla = new Tb_Plantillas();
                nuevaPlantilla.Fecha_creacion = DateTime.UtcNow;
                nuevaPlantilla.Nombre = nombre;
                nuevaPlantilla.Descripcion = descripcion;
                nuevaPlantilla.ID_usuario = 1;
                db.Tb_Plantillas.Add(nuevaPlantilla);
                
                db.SaveChanges();

                if (detalles != null) {
                    foreach (var item in detalles)
                    {
                        if (item.Descripcion == null) { item.Descripcion = ""; }
                        if (item.ValorTexto == null) { item.ValorTexto = ""; }
                        if (item.ValorMultipleOpciones == null) { item.ValorMultipleOpciones = ""; }
                        item.ID_plantilla = nuevaPlantilla.ID_plantilla;
                    }
                    db.Tb_ActividadesDetalles.AddRange(detalles);
                    db.SaveChanges();
                }


                var result = new { mensaje = "success" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                var result = new { mensaje = "error" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult GuardarPlantilla(List<Tb_ActividadesDetalles> detalles, string nombre, string descripcion, int id_plantilla)
        {
            try
            {

                var nuevaPlantilla = db.Tb_Plantillas.Find(id_plantilla);
                nuevaPlantilla.Fecha_creacion = DateTime.UtcNow;
                nuevaPlantilla.Nombre = nombre;
                nuevaPlantilla.Descripcion = descripcion;
                nuevaPlantilla.ID_usuario = 1;
                db.Entry(nuevaPlantilla).State = EntityState.Modified;

                db.SaveChanges();
                //Eliminamos datos
                db.Database.ExecuteSqlCommand("delete from Tb_ActividadesDetalles where ID_plantilla={0} and Original=1", id_plantilla);

                if (detalles != null)
                {
                    foreach (var item in detalles.Where(c=>c.Parametro!=null))
                    {
                        if (item.Descripcion == null) { item.Descripcion = ""; }
                        if (item.ValorTexto == null) { item.ValorTexto = ""; }
                        if (item.ValorMultipleOpciones == null) { item.ValorMultipleOpciones = ""; }
                        item.ID_plantilla = nuevaPlantilla.ID_plantilla;
                    }
                    db.Tb_ActividadesDetalles.AddRange(detalles);
                    db.SaveChanges();
                }


                var result = new { mensaje = "success" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { mensaje = "error" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult ObtenerCategorias(int eje)
        {
            try
            {
                var categorias = (from b in db.Tb_Categorias where (b.ID_eje==eje && b.Activo==true) select new SelectItemOption { ID = b.ID_categoria, Descripcion = b.Nombre }).OrderBy(b => b.Descripcion).ToList();
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                string result = javaScriptSerializer.Serialize(categorias);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ObtenerActividadesPrincipales(int categoria)
        {
            try
            {
                var categorias = (from b in db.Tb_ActividadesPrincipales where (b.ID_categoria == categoria && b.Activo == true) select new SelectItemOption { ID = b.ID_actividadPrincipal, Descripcion = b.Nombre }).OrderBy(b => b.Descripcion).ToList();
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                string result = javaScriptSerializer.Serialize(categorias);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ObtenerActividadesSecundariasBeneficiario(string id_beneficiario)
        {
            try
            {
                var actividadesInscritas = (from a in db.Tb_InscripcionActividad where a.LocalBeneficiaryID.Contains(id_beneficiario) select a.ID_actividadSecundaria).ToArray();

                var categorias = (from b in db.Tb_ActividadesSecundarias where (actividadesInscritas.Contains(b.ID_actividadPrincipal) && b.Activo == true) select new SelectItemOption { ID = b.ID_actividadSecundaria, Descripcion = b.Nombre_corto }).OrderBy(b => b.Descripcion).ToList();
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                string result = javaScriptSerializer.Serialize(categorias);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult FiltrarInscritos(int id_actividad, string CDI)
        {
            try
            {
                var actividadesInscritas = (from a in db.Tb_InscripcionActividad where (a.FCPID==CDI && a.ID_actividadSecundaria==id_actividad) select a.LocalBeneficiaryID).ToArray();

                var categorias = (from b in db.Tb_Beneficiarios where (actividadesInscritas.Contains(b.LocalBeneficiaryID)) select new SelectItemOptionString { ID = b.LocalBeneficiaryID, Descripcion = b.AccountName }).OrderBy(b => b.Descripcion).ToList();
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                string result = javaScriptSerializer.Serialize(categorias);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ObtenerActividadesSecundarias(int actividadPrincipal)
        {
            try
            {
                var categorias = (from b in db.Tb_ActividadesSecundarias where (b.ID_actividadPrincipal == actividadPrincipal && b.Activo == true) select new SelectItemOption { ID = b.ID_actividadSecundaria, Descripcion = b.Nombre_corto }).OrderBy(b => b.Descripcion).ToList();
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                string result = javaScriptSerializer.Serialize(categorias);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ObtenerResultados(int actividadSecundaria)
        {
            try
            {
                var categorias = (from b in db.Tb_Resultados where (b.ID_actividadSecundaria == actividadSecundaria && b.Activo == true) select new SelectItemOption { ID = b.ID_resultado, Descripcion = b.Nombre }).OrderBy(b => b.Descripcion).ToList();
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                string result = javaScriptSerializer.Serialize(categorias);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ObtenerPrecondiciones(int resultado)
        {
            try
            {
                var categorias = (from b in db.Tb_PrecondicionesTOC where (b.ID_resultado == resultado && b.Activo == true) select new SelectItemOption { ID = b.ID_precondicionTOC, Descripcion = b.Nombre }).OrderBy(b => b.Descripcion).ToList();
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                string result = javaScriptSerializer.Serialize(categorias);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult CargarPlantilla(int plantilla, int actividadsecundaria, int actividadBeneficiario)
        {
            try
            {
                var detallesPlantilla = (from b in db.Tb_ActividadesDetalles where (b.ID_plantilla == plantilla && b.Original == true) select b).ToList();
                List<Tb_ActividadesDetalles> nuevaLista = new List<Tb_ActividadesDetalles>();
                foreach (var item in detallesPlantilla) {
                    Tb_ActividadesDetalles nuevoRegistro = new Tb_ActividadesDetalles();
                    nuevoRegistro = item;
                    nuevoRegistro.Original = false;
                    nuevoRegistro.ID_actividadSecundaria = actividadsecundaria;
                    nuevoRegistro.ID_actividadBeneficiario = actividadBeneficiario;
                    nuevaLista.Add(nuevoRegistro);
                }
                db.SaveChanges();
                string result = "success";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("error", JsonRequestBehavior.AllowGet);
            }
        }

        // POST: ActividadesDinamicas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_actividadBeneficiario,LocalBeneficiaryID,FCPID,ID_eje,ID_categoria,ID_actividadPrincipal,ID_actividadSecundaria,ID_resultado,ID_precondicionTOC,Fecha,Fecha_actualizacion,Comentarios")] Tb_ActividadesBeneficiarios tb_ActividadesBeneficiarios)
        {
            tb_ActividadesBeneficiarios.Fecha = DateTime.UtcNow;
            tb_ActividadesBeneficiarios.Fecha_actualizacion = DateTime.UtcNow;
            if (tb_ActividadesBeneficiarios.Comentarios == null) { tb_ActividadesBeneficiarios.Comentarios = ""; }
            try
            {
                db.Tb_ActividadesBeneficiarios.Add(tb_ActividadesBeneficiarios);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch {
                ViewBag.ID_actividadPrincipal = new SelectList(db.Tb_ActividadesPrincipales, "ID_actividadPrincipal", "Nombre");
                ViewBag.ID_actividadSecundaria = new SelectList(db.Tb_ActividadesSecundarias, "ID_actividadSecundaria", "Nombre_corto");
                ViewBag.LocalBeneficiaryID = new SelectList(db.Tb_Beneficiarios, "LocalBeneficiaryID", "AccountName");
                ViewBag.ID_categoria = new SelectList(db.Tb_Categorias, "ID_categoria", "Nombre");
                ViewBag.FCPID = new SelectList(db.Tb_CDI, "FCPID", "NombreCDI");
                ViewBag.ID_eje = new SelectList(db.Tb_EjesEstrategicos, "ID_eje", "Nombre");
                ViewBag.ID_precondicionTOC = new SelectList(db.Tb_PrecondicionesTOC, "ID_precondicionTOC", "Nombre");
                ViewBag.ID_resultado = new SelectList(db.Tb_Resultados, "ID_resultado", "Nombre");
                return View(tb_ActividadesBeneficiarios);
            }



         
        }

        // GET: ActividadesDinamicas/Edit/5
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

                Tb_ActividadesBeneficiarios tb_ActividadesBeneficiarios = db.Tb_ActividadesBeneficiarios.Find(id);
                var plantilla = "Sin asignar";
                List<Tb_ActividadesDetalles> detalles = new List<Tb_ActividadesDetalles>();
                if (tb_ActividadesBeneficiarios.ID_plantilla != 0)
                {
                    detalles = (from a in db.Tb_ActividadesDetalles where (a.ID_actividadBeneficiario == id) select a).ToList();
                    plantilla = (from a in db.Tb_Plantillas where (a.ID_plantilla == tb_ActividadesBeneficiarios.ID_plantilla) select a).FirstOrDefault().Nombre;

                }
                ViewBag.detalles = detalles;
                ViewBag.ID_resultado = (from b in db.Tb_Resultados where (b.ID_actividadSecundaria == tb_ActividadesBeneficiarios.ID_actividadSecundaria && b.Activo == true) select new SelectItemOption { ID = b.ID_resultado, Descripcion = b.Nombre }).OrderBy(b => b.Descripcion).ToList();
                ViewBag.plantilla = plantilla;

                return View(tb_ActividadesBeneficiarios);
            }
            else
            {

                return RedirectToAction("Login", "Auth", new { access = false });

            }
 
        }

        public ActionResult EditBulk(int id)
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

                Tb_ActividadesBeneficiarios tb_ActividadesBeneficiarios = db.Tb_ActividadesBeneficiarios.Find(id);
                var plantilla = "Sin asignar";
                List<Tb_ActividadesDetalles> detalles = new List<Tb_ActividadesDetalles>();
                if (tb_ActividadesBeneficiarios.ID_plantilla != 0)
                {
                    detalles = (from a in db.Tb_ActividadesDetalles where (a.ID_actividadBeneficiario == id) select a).ToList();
                    plantilla = (from a in db.Tb_Plantillas where (a.ID_plantilla == tb_ActividadesBeneficiarios.ID_plantilla) select a).FirstOrDefault().Nombre;

                }
                ViewBag.detalles = detalles;
                ViewBag.ID_resultado = (from b in db.Tb_Resultados where (b.ID_actividadSecundaria == tb_ActividadesBeneficiarios.ID_actividadSecundaria && b.Activo == true) select new SelectItemOption { ID = b.ID_resultado, Descripcion = b.Nombre }).OrderBy(b => b.Descripcion).ToList();
                ViewBag.plantilla = plantilla;

                var inscripcionesdistict = (from a in db.Tb_ActividadesBeneficiarios where (a.parent== id || a.ID_actividadBeneficiario==id) select a.LocalBeneficiaryID).Distinct().ToArray();
                List<Tb_Beneficiarios> inscritos = new List<Tb_Beneficiarios>();

                inscritos = (from a in db.Tb_Beneficiarios where (inscripcionesdistict.Contains(a.LocalBeneficiaryID)) select a).ToList();
                ViewBag.grupo = inscritos;


                return View(tb_ActividadesBeneficiarios);
            }
            else
            {

                return RedirectToAction("Login", "Auth", new { access = false });

            }

        }

        // POST: ActividadesDinamicas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_actividadBeneficiario,LocalBeneficiaryID,FCPID,ID_eje,ID_categoria,ID_actividadPrincipal,ID_actividadSecundaria,ID_resultado,ID_precondicionTOC,Fecha,Fecha_actualizacion,Comentarios")] Tb_ActividadesBeneficiarios tb_ActividadesBeneficiarios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_ActividadesBeneficiarios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_actividadPrincipal = new SelectList(db.Tb_ActividadesPrincipales, "ID_actividadPrincipal", "Codigo", tb_ActividadesBeneficiarios.ID_actividadPrincipal);
            ViewBag.ID_actividadSecundaria = new SelectList(db.Tb_ActividadesSecundarias, "ID_actividadSecundaria", "Codigo", tb_ActividadesBeneficiarios.ID_actividadSecundaria);
            ViewBag.LocalBeneficiaryID = new SelectList(db.Tb_Beneficiarios, "LocalBeneficiaryID", "BeneficiaryICPID", tb_ActividadesBeneficiarios.LocalBeneficiaryID);
            ViewBag.ID_eje = new SelectList(db.Tb_Categorias, "ID_categoria", "Codigo", tb_ActividadesBeneficiarios.ID_eje);
            ViewBag.FCPID = new SelectList(db.Tb_CDI, "FCPID", "Direccion", tb_ActividadesBeneficiarios.FCPID);
            ViewBag.ID_eje = new SelectList(db.Tb_EjesEstrategicos, "ID_eje", "Codigo", tb_ActividadesBeneficiarios.ID_eje);
            ViewBag.ID_resultado = new SelectList(db.Tb_PrecondicionesTOC, "ID_precondicionTOC", "Codigo", tb_ActividadesBeneficiarios.ID_resultado);
            ViewBag.ID_resultado = new SelectList(db.Tb_Resultados, "ID_resultado", "Codigo", tb_ActividadesBeneficiarios.ID_resultado);
            return View(tb_ActividadesBeneficiarios);
        }

        public ActionResult Delete(int id)
        {
            Tb_ActividadesBeneficiarios tb_ActividadesBeneficiarios = db.Tb_ActividadesBeneficiarios.Find(id);
            db.Tb_ActividadesBeneficiarios.Remove(tb_ActividadesBeneficiarios);
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
