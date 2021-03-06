﻿using AccountsWork.DomainModel;
using System.Collections.Generic;
using System;
using AccountsWork.DataAccessLayer;
using System.ComponentModel.Composition;

namespace AccountsWork.BusinessLayer
{
    public interface ICapexesService
    {
        IList<CapexSet> GetCapexesForYearList(int year);
        int GetCapexIdByName(string accountCapexName, int accountYear);
        IList<CapexSet> GetCapexes();
        string GetCapexNameById(int value);
    }

    [Export(typeof(ICapexesService))]
    public class CapexesService : ICapexesService
    {
        private ICapexRepository _capexRepository;

        [ImportingConstructor]
        public CapexesService(ICapexRepository capexRepository)
        {
            _capexRepository = capexRepository;
        }

        public IList<CapexSet> GetCapexesForYearList(int year)
        {
            return _capexRepository.GetList(c => c.CapexYear == year);
        }

        public int GetCapexIdByName(string accountCapexName, int accountYear)
        {
            return _capexRepository.GetSingle(c => c.CapexYear == accountYear && c.CapexName == accountCapexName).Id;
        }

        public IList<CapexSet> GetCapexes()
        {
            return _capexRepository.GetAll(c => c.AccountsCapexInfoSets);
        }

        public string GetCapexNameById(int value)
        {
            return _capexRepository.GetSingle(c => c.Id == value).CapexName;
        }
    }
}
