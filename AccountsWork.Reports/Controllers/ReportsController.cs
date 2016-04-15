using AccountsWork.Reports.Events;
using Microsoft.Win32;
using Prism.Events;
using System.ComponentModel.Composition;

namespace AccountsWork.Reports.Controllers
{
    [Export]
    public class ReportsController
    {
        private IEventAggregator _eventAggregator;

        public void ShowDialogWindow()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Excel documents (.xlsx)|*.xlsx";
            if (fileDialog.ShowDialog() != null)
            {
                _eventAggregator.GetEvent<OpenFileEvent>().Publish(fileDialog.FileName);
            }
        }

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
        public ReportsController(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }
    }
}
