using AccountsWork.DomainModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountsWork.DataAccessLayer
{
    public interface IZipRepository : IGenericDataRepository<ZipSet>
    { }

    [Export(typeof(IZipRepository))]
    public class ZipRepository : GenericDataRepository<ZipSet>, IZipRepository
    {
    }
}
