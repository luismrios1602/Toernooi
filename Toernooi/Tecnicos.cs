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
    
    public partial class Tecnicos
    {
        public int idTecnico { get; set; }
        public string nomTecnico { get; set; }
        public string documentoTec { get; set; }
        public Nullable<System.DateTime> fechaNacimiento { get; set; }
        public Nullable<System.DateTime> fechaInicio { get; set; }
        public Nullable<System.DateTime> fechaFin { get; set; }
        public int Equipos_idEquipo { get; set; }
        public int Cargos_idCargo { get; set; }
    
        public virtual Equipos Equipos { get; set; }
        public virtual Cargos Cargos { get; set; }
    }
}
