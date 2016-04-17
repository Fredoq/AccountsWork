using System.Collections.ObjectModel;

namespace AccountsWork.Reports.Model
{
    public class StoreZip
    {
        public string ZipName { get; set; }
        public decimal Summ { get; set; }
        public ObservableCollection<ZipPrice> ZipList { get; set; }
    }
}
