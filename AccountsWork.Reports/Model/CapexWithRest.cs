using AccountsWork.DomainModel;

namespace AccountsWork.Reports.Model
{
    public class CapexWithRest
    {
        public CapexSet Capex { get; set; }
        public decimal Rest { get; set; }
    }
}
