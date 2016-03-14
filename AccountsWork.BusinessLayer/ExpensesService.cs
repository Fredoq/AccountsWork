using AccountsWork.DataAccessLayer;
using AccountsWork.DomainModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace AccountsWork.BusinessLayer
{
    public interface IExpensesService
    {
        IList<AccountsExpenseSet> GetExpensesList();
    }

    [Export(typeof(IExpensesService))]
    public class ExpensesService : IExpensesService
    {
        private IExpenseRepository _expenseRepository;

        [ImportingConstructor]
        public ExpensesService(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public IList<AccountsExpenseSet> GetExpensesList()
        {
            return _expenseRepository.GetAll();
        }
    }
}
