using AccountsWork.DataAccessLayer;
using AccountsWork.DomainModel;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace AccountsWork.BusinessLayer
{
    public interface IServiceZipsService
    {
        void AddServiceZips(IList<ServiceZipDetailsSet> serviceZipList);
    }

    [Export(typeof(IServiceZipsService))]
    public class ServiceZipsService : IServiceZipsService
    {
        private IServiceZipsRepository _serviceZipsRepository;

        [ImportingConstructor]
        public ServiceZipsService(IServiceZipsRepository serviceZipsRepository)
        {
            _serviceZipsRepository = serviceZipsRepository;
        }

        public void AddServiceZips(IList<ServiceZipDetailsSet> serviceZipList)
        {
            _serviceZipsRepository.Add(serviceZipList.ToArray());
        }
    }
}
