using AccountsWork.DomainModel;
using System.ComponentModel.Composition;

namespace AccountsWork.DataAccessLayer
{
    public interface IExpenseRepository : IGenericDataRepository<AccountsExpenseSet>
    { }

    [Export(typeof(IExpenseRepository))]
    public class ExpenseRepository : GenericDataRepository<AccountsExpenseSet>, IExpenseRepository
    {
    }
}
