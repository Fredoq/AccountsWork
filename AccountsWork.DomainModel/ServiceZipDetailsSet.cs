//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AccountsWork.DomainModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class ServiceZipDetailsSet
    {
        public int Id { get; set; }
        public string ZipName { get; set; }
        public string ServiceMonth { get; set; }
        public System.DateTime WorkDate { get; set; }
        public string BlankNumber { get; set; }
        public Nullable<int> ZipQuantity { get; set; }
        public int StoreNumber { get; set; }
        public decimal ZipPrice { get; set; }
        public string Company { get; set; }
    
        public virtual StoresSet StoresSet { get; set; }
        public virtual ZipSet ZipSet { get; set; }
    }
}
