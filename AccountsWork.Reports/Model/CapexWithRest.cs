using AccountsWork.DomainModel;

namespace AccountsWork.Reports.Model
{
    public class CapexWithRest
    {
        public CapexSet Capex { get; set; }
        public decimal Rest { get; set; }
        public int Pay { get; set; }
        public decimal PaySum { get; set; }
    }
}
