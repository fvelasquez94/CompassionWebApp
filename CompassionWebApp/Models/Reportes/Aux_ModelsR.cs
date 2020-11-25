using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompassionWebApp.Models.Reportes
{
    public class Aux_ModelsR
    {
        public class ReportexCDI {
            public Int64 ID { get; set; }
            public string IDCDI { get; set; }
            public string NombreCDI { get; set; }
            public string Tipo { get; set; }
            public int IDinterno_eje { get; set; }
            public string NombreEje { get; set; }
            public int IDinterno_actividad { get; set; }
            public string NombreActividad { get; set; }
            public int BeneficiariosInscritos { get; set; }
            public int BeneficiariosUnicosconActividades { get; set; }
            public int BeneficiariosUnicosconActividadesFinalizadas { get; set; }
            public decimal Porcentaje { get; set; }
            public int ActividadesTotales { get; set; }
        }

        public class TiposPlantillas {
            public int ID_ActividadSecundaria { get; set; }
            public string Nombre { get; set; }
            public int ID_plantilla { get; set; }
            public string Plantilla { get; set; }
            public string CDI { get; set; }
        }

        public class ReportexBeneficiario
        {
            public Int64 ID { get; set; }
            public string IDBeneficiario { get; set; }
            public string Nombre { get; set; }
            public string IDCDI { get; set; }
            public string NombreCDI { get; set; }
            public string Tipo { get; set; }
            public int IDinterno_eje { get; set; }
            public string NombreEje { get; set; }
            public int IDinterno_actividad { get; set; }
            public string NombreActividad { get; set; }
            public int ActividadesTotales { get; set; }
            public int PlantillasUtilizadas { get; set; }
            public int ActividadesFinalizadas { get; set; }
     
        }
    }
}