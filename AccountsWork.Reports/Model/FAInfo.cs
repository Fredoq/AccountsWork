using AccountsWork.DomainModel;
using System;
using System.Collections.ObjectModel;

namespace AccountsWork.Reports.Model
{
    public class FAInfo
    {
        public string Store { get; set; }
        public decimal FAPrice { get; set; }
        public string Company { get; set; }
        public DateTime DateAccount { get; set; }
        public ObservableCollection<AccountsMainSet> Accounts { get; set; }
        public int Quantity { get; set; }
        public string FAName { get; set; }
    }
}
