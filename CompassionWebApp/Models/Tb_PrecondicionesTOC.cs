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
    
    public partial class Tb_PrecondicionesTOC
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tb_PrecondicionesTOC()
        {
            this.Tb_ActividadesBeneficiarios = new HashSet<Tb_ActividadesBeneficiarios>();
            this.Tb_ActividadesBeneficiarios1 = new HashSet<Tb_ActividadesBeneficiarios>();
        }
    
        public int ID_precondicionTOC { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public int ID_resultado { get; set; }
    
        public virtual Tb_Resultados Tb_Resultados { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tb_ActividadesBeneficiarios> Tb_ActividadesBeneficiarios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tb_ActividadesBeneficiarios> Tb_ActividadesBeneficiarios1 { get; set; }
    }
}
