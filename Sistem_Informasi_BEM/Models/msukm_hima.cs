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
    
    public partial class msukm_hima
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public msukm_hima()
        {
            this.msjabatans = new HashSet<msjabatan>();
            this.trlaporanksks = new HashSet<trlaporanksk>();
        }
    
        public int idukm_hima { get; set; }
        public string nama { get; set; }
        public Nullable<int> status { get; set; }
        public string creaby { get; set; }
        public Nullable<System.DateTime> creadate { get; set; }
        public string modiby { get; set; }
        public Nullable<System.DateTime> modidate { get; set; }
        public Nullable<int> iddepartemen { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<msjabatan> msjabatans { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<trlaporanksk> trlaporanksks { get; set; }
        public virtual msdeparteman msdeparteman { get; set; }
    }
}
