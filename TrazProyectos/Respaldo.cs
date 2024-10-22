//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TrazProyectosDiverscan
{
    using System;
    using System.Collections.Generic;
    
    public partial class Respaldo
    {
        public int IdRespaldo { get; set; }
        public int IdSistema { get; set; }
        public int IdComponenteSistema { get; set; }
        public int IdDesarrollador { get; set; }
        public Nullable<System.DateTime> FechaLiberacion { get; set; }
        public Nullable<System.DateTime> FechaUltimaModificacion { get; set; }
        public System.DateTime FechaUltimoRespaldo { get; set; }
        public System.DateTime FechaPruebaRespaldo { get; set; }
        public bool RespaldoGIT { get; set; }
        public bool RespaldoNube { get; set; }
        public bool RespaldoFisico { get; set; }
        public string Comentarios { get; set; }
        public string Version { get; set; }
        public Nullable<int> IdDesarrolladorCertifica { get; set; }
        public Nullable<bool> RespaldoDB { get; set; }
    
        public virtual ComponenteSistema ComponenteSistema { get; set; }
        public virtual Desarrolladores Desarrolladores { get; set; }
        public virtual Sistema Sistema { get; set; }
    }
}
