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
    
    public partial class DetallePartido
    {
        public int idDetallePartido { get; set; }
        public int minuto { get; set; }
        public int Sucesos_idSuceso { get; set; }
        public Nullable<int> Jugadores_idJugador { get; set; }
        public int Partido_idPartido { get; set; }
    
        public virtual Jugadores Jugadores { get; set; }
        public virtual Partidos Partidos { get; set; }
        public virtual Sucesos Sucesos { get; set; }
    }
}
