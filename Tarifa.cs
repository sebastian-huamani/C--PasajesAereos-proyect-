//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace T11A
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tarifa
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tarifa()
        {
            this.Vuelo = new HashSet<Vuelo>();
        }
    
        public int Id { get; set; }
        public Nullable<int> IdCiudadOrigen { get; set; }
        public Nullable<int> IdCiudadDestino { get; set; }
        public Nullable<int> IdTipoServicio { get; set; }
        public Nullable<decimal> Precio { get; set; }
        public Nullable<bool> Activo { get; set; }
    
        public virtual Ciudad Ciudad { get; set; }
        public virtual Ciudad Ciudad1 { get; set; }
        public virtual TipoServicio TipoServicio { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Vuelo> Vuelo { get; set; }
    }
}
