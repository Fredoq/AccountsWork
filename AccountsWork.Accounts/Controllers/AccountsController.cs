using AccountsWork.Accounts.Events;
using Microsoft.Win32;
using Prism.Events;
using System.ComponentModel.Composition;

namespace AccountsWork.Accounts.Controllers
{
    [Export]
    public class AccountsController
    {
        private IEventAggregator _eventAggregator;

        public void SaveDialogWindow()
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.DefaultExt = ".xlsx";
            fileDialog.Filter = "Excel documents (.xlsx)|*.xlsx";
            if (fileDialog.ShowDialog() != null)
            {
                _eventAggregator.GetEvent<SaveFileEvent>().Publish(fileDialog.FileName);
            }
        }

        [ImportingConstructor]
        public AccountsController(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }
    }
}
