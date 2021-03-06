﻿using AccountsWork.BusinessLayer;
using AccountsWork.DomainModel;
using OfficeOpenXml;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;

namespace AccountsWork.ExcelReports
{
    public interface IExcelReportService
    {
        byte[] CreateNewStatusesReport(ObservableCollection<AccountsMainSet> accountsList);
        byte[] CreateAccountWithStatusesReport(ObservableCollection<AccountsMainSet> accountsList);
        void SaveReport(string filename, byte[] report);
    }

    [Export(typeof(IExcelReportService))]
    public class ExcelReportService : IExcelReportService
    {
        private IStoresService _storeService;

        public byte[] CreateNewStatusesReport(ObservableCollection<AccountsMainSet> accountsList)
        {
            byte[] resultPackage;
            using (ExcelPackage p = new ExcelPackage())
            {
                p.Workbook.Worksheets.Add("Report");
                var ws = p.Workbook.Worksheets[1];

                ws.Cells[1, 1].Value = @"Номер п\п";
                ws.Cells[1, 2].Value = "Объект";
                ws.Cells[1, 3].Value = "Поставщик";
                ws.Cells[1, 4].Value = "Номер счета";
                ws.Cells[1, 5].Value = "Дата счета";
                ws.Cells[1, 6].Value = "Сумма счета";
                ws.Cells[1, 1, 1, 6].Style.Font.Bold = true;
                var i = 1;
                foreach(var account in accountsList)
                {
                    ws.Cells[i + 1, 1].Value = i;
                    ws.Cells[i + 1, 2].Value = (account.AccountsStoreDetailsSets.Count > 1) ? "По списку" : ((account.AccountsStoreDetailsSets.Count == 0) ? "Не задан" : _storeService.GetStoreName(account.AccountsStoreDetailsSets.FirstOrDefault().AccountStore));
                    ws.Cells[i + 1, 3].Value = account.AccountCompany;
                    ws.Cells[i + 1, 4].Value = account.AccountNumber;
                    ws.Cells[i + 1, 5].Value = account.AccountDate.ToShortDateString();
                    ws.Cells[i + 1, 6].Value = account.AccountAmount;
                    i++;
                }
                for (int k = 1; k <= 6; k++)
                {
                    ws.Column(k).AutoFit(k);
                }
                resultPackage = p.GetAsByteArray();
                return resultPackage;
            }
            
        }
        public byte[] CreateAccountWithStatusesReport(ObservableCollection<AccountsMainSet> accountsList)
        {
            byte[] resultPackage;
            using (ExcelPackage p = new ExcelPackage())
            {
                p.Workbook.Worksheets.Add("Report");
                var ws = p.Workbook.Worksheets[1];

                ws.Cells[1, 1].Value = "Номер счета";
                ws.Cells[1, 2].Value = "Поставщик";
                ws.Cells[1, 3].Value = "Дата счета";
                ws.Cells[1, 4].Value = "Сумма счета";
                ws.Cells[1, 5].Value = "Статус";
                ws.Cells[1, 6].Value = "Дата статуса";
                ws.Cells[1, 7].Value = "Комментарий";
                ws.Cells[1, 8].Value = "Описание";
                ws.Cells[1, 9].Value = "Статья";
                ws.Cells[1, 1, 1, 9].Style.Font.Bold = true;
                var i = 1;
                foreach (var account in accountsList)
                {
                    ws.Cells[i + 1, 1].Value = account.AccountNumber;
                    ws.Cells[i + 1, 2].Value = account.AccountCompany;
                    ws.Cells[i + 1, 3].Value = account.AccountDate.ToShortDateString();
                    ws.Cells[i + 1, 4].Value = account.AccountAmount;
                    ws.Cells[i + 1, 5].Value = account.AccountsStatusDetailsSets.LastOrDefault().AccountStatus;
                    ws.Cells[i + 1, 6].Value = account.AccountsStatusDetailsSets.LastOrDefault().AccountStatusDate;
                    ws.Cells[i + 1, 7].Value = account.AccountsStatusDetailsSets.LastOrDefault().Commentary;
                    ws.Cells[i + 1, 8].Value = account.AccountDescription;
                    foreach (var cap in account.AccountsCapexInfoSets)
                    {
                        ws.Cells[i + 1, 9].Value += cap.AccountExpense + ";";
                    }
                    i++;
                }
                for (int k = 1; k <= 8; k++)
                {
                    ws.Column(k).AutoFit(k);
                }
                resultPackage = p.GetAsByteArray();
                return resultPackage;
            }
        }
        public void SaveReport(string filename, byte[] report)
        {
            if (File.Exists(filename))
                File.Delete(filename);
            File.WriteAllBytes(filename, report);
        }

        [ImportingConstructor]
        public ExcelReportService(IStoresService storeService)
        {
            _storeService = storeService;
        }
    }
}
