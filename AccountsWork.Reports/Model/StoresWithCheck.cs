using AccountsWork.DomainModel;
using AccountsWork.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountsWork.Reports.Model
{
    public class StoresWithCheck : ValidatableBindableBase
    {
        private StoresSet _store;
        private bool _check;

        public StoresSet Store
        {
            get { return _store; }
            set { SetProperty(ref _store, value); }
        }

        public bool Check
        {
            get { return _check; }
            set { SetProperty(ref _check, value); }
        }
        public StoresWithCheck()
        {
            Check = false;
        }
    }
}
