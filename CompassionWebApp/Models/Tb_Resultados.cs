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
    
    public partial class Tb_Resultados
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tb_Resultados()
        {
            this.Tb_PrecondicionesTOC = new HashSet<Tb_PrecondicionesTOC>();
            this.Tb_ActividadesBeneficiarios = new HashSet<Tb_ActividadesBeneficiarios>();
        }
    
        public int ID_resultado { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public int ID_actividadSecundaria { get; set; }
    
        public virtual Tb_ActividadesSecundarias Tb_ActividadesSecundarias { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tb_PrecondicionesTOC> Tb_PrecondicionesTOC { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tb_ActividadesBeneficiarios> Tb_ActividadesBeneficiarios { get; set; }
    }
}
