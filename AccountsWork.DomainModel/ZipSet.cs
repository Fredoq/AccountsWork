//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AccountsWork.DomainModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class ZipSet
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ZipSet()
        {
            this.ServiceZipDetailsSets = new HashSet<ServiceZipDetailsSet>();
        }
    
        public string ZipName { get; set; }
        public string MainZipName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceZipDetailsSet> ServiceZipDetailsSets { get; set; }
    }
}
