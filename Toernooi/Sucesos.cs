//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Toernooi
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sucesos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sucesos()
        {
            this.DetallePartido = new HashSet<DetallePartido>();
        }
    
        public int idSuceso { get; set; }
        public string nomSuceso { get; set; }
        public bool afectaJug { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetallePartido> DetallePartido { get; set; }
    }
}
