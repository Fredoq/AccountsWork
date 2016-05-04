using AccountsWork.DomainModel;
using System.Collections.Generic;

namespace AccountsWork.Reports.Model
{
    public class StackedStoreInfo
    {
        public StoresSet Store { get; set; }
        public decimal EquipmentSum { get; set; }
        public decimal RepairSum { get; set; }
        public IEnumerable<ServiceZipDetailsSet> ServiceZipList { get; set; }
    }
}
