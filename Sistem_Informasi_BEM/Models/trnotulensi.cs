//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sistem_Informasi_BEM.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class trnotulensi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public trnotulensi()
        {
            this.dtlnotulensis = new HashSet<dtlnotulensi>();
        }
    
        public int idnotulensi { get; set; }
        public Nullable<int> idrapat { get; set; }
        public Nullable<int> jmlhadir { get; set; }
        public string notulensi { get; set; }
        public string creaby { get; set; }
        public Nullable<System.DateTime> creadate { get; set; }
        public string modiby { get; set; }
        public Nullable<System.DateTime> modidate { get; set; }
        public Nullable<int> status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<dtlnotulensi> dtlnotulensis { get; set; }
        public virtual trrapat trrapat { get; set; }
    }
}