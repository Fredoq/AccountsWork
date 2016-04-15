using AccountsWork.DomainModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountsWork.DataAccessLayer
{
    public interface IServiceZipsRepository : IGenericDataRepository<ServiceZipDetailsSet>
    { }

    [Export(typeof(IServiceZipsRepository))]
    public class ServiceZipsRepository : GenericDataRepository<ServiceZipDetailsSet>, IServiceZipsRepository
    {
    }
}
