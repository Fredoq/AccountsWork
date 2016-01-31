using AccountsWork.DomainModel;
using System.Collections.Generic;

namespace AccountsWork.BusinessLayer
{
    public interface ITypesService
    {
        IList<TypeSet> GetTypes();
    }
}
