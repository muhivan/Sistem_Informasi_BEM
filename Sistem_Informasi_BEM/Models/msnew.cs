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
    using System.ComponentModel.DataAnnotations;

    public partial class msnew
    {
        public int idnews { get; set; }
        [Required(ErrorMessage = "Judul tidak boleh kosong")]
        [StringLength(50)]
        public string judul { get; set; }
        [Required(ErrorMessage = "Preview tidak boleh kosong")]
        [StringLength(50)]
        public string preview { get; set; }
        [Required(ErrorMessage = "Deskripsi tidak boleh kosong")]
        public string deskripsi { get; set; }
        public string gambar { get; set; }
        public Nullable<int> status { get; set; }
        public string creaby { get; set; }
        public Nullable<System.DateTime> creadate { get; set; }
        public string modiby { get; set; }
        public Nullable<System.DateTime> modidate { get; set; }
    }
}