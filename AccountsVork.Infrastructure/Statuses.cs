using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountsVork.Infrastructure
{
    public static class Statuses
    {
        public const string InWork = "В обработке";
        public const string InPO = "В ПО";
        public const string InAcc = "В бухгалтерии";
        public const string InReturn = "Возврат";
        public const string InCancel = "Отмена";
        public const string InPayed = "Оплачен";

        public static List<string> GetStatusesList()
        {
            return new List<string>()
            {
                InWork, InPO, InAcc, InReturn, InCancel, InPayed
            };
        }
    }
}
