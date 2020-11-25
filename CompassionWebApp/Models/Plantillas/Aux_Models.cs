using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompassionWebApp.Models.Plantillas
{
    public class Aux_Models
    {
        public class Tb_PlantillasIndex
        {
            public int ID_plantilla { get; set; }
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
            public System.DateTime Fecha_creacion { get; set; }
            public int ID_usuario { get; set; }
            public int count { get; set; }
        }
    }
}