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
    
    public partial class FASet
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FASet()
        {
            this.AccountsBudgetDetailsSets = new HashSet<AccountsBudgetDetailsSet>();
        }
    
        public string FAName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccountsBudgetDetailsSet> AccountsBudgetDetailsSets { get; set; }
    }
}
