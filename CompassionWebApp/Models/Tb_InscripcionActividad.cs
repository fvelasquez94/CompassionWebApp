//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CompassionWebApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tb_InscripcionActividad
    {
        public int ID_inscripcion { get; set; }
        public string LocalBeneficiaryID { get; set; }
        public string AccountName { get; set; }
        public string FCPID { get; set; }
        public System.DateTime Fecha { get; set; }
        public int ID_usuario { get; set; }
        public int ID_actividadSecundaria { get; set; }
    }
}
