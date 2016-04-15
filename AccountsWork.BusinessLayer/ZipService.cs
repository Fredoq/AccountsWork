using AccountsWork.DataAccessLayer;
using AccountsWork.DomainModel;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace AccountsWork.BusinessLayer
{
    public interface IZipService
    {
        IList<ZipSet> GetEmptyZips();
        IList<ZipSet> GetZips();
        void AddNewZips(IList<ZipSet> newZipList);
        IList<string> GetMainZips();
        void UpdateMainZip(IEnumerable<ZipSet> enumerable);
    }

    [Export(typeof(IZipService))]
    public class ZipService : IZipService
    {
        private IZipRepository _zipRepository;

        [ImportingConstructor]
        public ZipService(IZipRepository zipRepository)
        {
            _zipRepository = zipRepository;
        }

        public void AddNewZips(IList<ZipSet> newZipList)
        {
            _zipRepository.Add(newZipList.ToArray());
        }

        public IList<ZipSet> GetEmptyZips()
        {
            return _zipRepository.GetList(z => string.IsNullOrWhiteSpace(z.MainZipName));
        }

        public IList<string> GetMainZips()
        {
            return _zipRepository.GetAll().Select(z => z.MainZipName).Distinct().ToList();
        }

        public IList<ZipSet> GetZips()
        {
            return _zipRepository.GetAll();
        }

        public void UpdateMainZip(IEnumerable<ZipSet> enumerable)
        {
            _zipRepository.Update(enumerable.ToArray());
        }
    }
}
